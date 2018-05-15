using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using Olive;
using Olive.Entities;
using Olive.Mvc;
using Olive.Web;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using vm = ViewModel;

namespace Controllers
{
    [ViewData("LeftMenu", "AdminSettingsMenu")]
    [ViewData("Title", "Admin > Settings > Administrators > Enter")]
    [Authorize(Roles = "Admin")]
    [EscapeGCop("Auto generated code.")]
    public partial class AdminSettingsAdministratorsEnterController : BaseController
    {
        [Route("admin/settings/administrators/enter/{item:Guid?}")]
        public async Task<ActionResult> Index(vm.AdministratorForm info)
        {
            ModelState.Clear(); // Remove initial validation messages
            
            return View(info);
        }
        
        [HttpPost, Route("AdministratorForm/Save")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(vm.AdministratorForm info)
        {
            try
            {
                if (!await SaveInDatabase(info)) return JsonActions(info);
            }
            catch (Olive.Entities.ValidationException ex)
            {
                return Notify(ex.Message, "error");
            }
            
            Notify("Saved successfully.", obstruct: false);
            
            Do(WindowAction.CloseModalRefreshParent);
            
            return JsonActions(info);
        }
        
        [NonAction, OnBound]
        public async Task OnBound(vm.AdministratorForm info)
        {
            info.Item = info.Item ?? new Administrator();
            
            if (Request.IsGet()) await info.Item.CopyDataTo(info);
            
            info.IsDeactivated_Options.Clear();
            info.IsDeactivated_Options.Add("Yes", true);
            info.IsDeactivated_Options.Add("No", false);
        }
        
        [NonAction]
        public async Task<bool> SaveInDatabase(vm.AdministratorForm info)
        {
            if (!ModelState.IsValid(info))
            {
                Notify(ModelState.GetErrors().ToLinesString(), "error");
                return false;
            }
            
            var item = info.Item.Clone();
            
            await info.CopyDataTo(item); // Read the ViewModel data into the domain object.
            
            using (var scope = Database.CreateTransactionScope())
            {
                try
                {
                    info.Item = await Database.Save(item);
                    
                    scope.Complete();
                    return true;
                }
                catch (Olive.Entities.ValidationException ex)
                {
                    Notify(ex.ToFullMessage(), "error");
                }
                
                return false;
            }
        }
    }
}

namespace ViewModel
{
    [EscapeGCop("Auto generated code.")]
    public partial class AdministratorForm : IViewModel
    {
        [ValidateNever]
        public Administrator Item { get; set; }
        
        [Required, DisplayName("First name")]
        [StringLength(200, ErrorMessage = "First name should not exceed 200 characters.")]
        public string FirstName { get; set; }
        
        [Required, DisplayName("Last name")]
        [StringLength(200, ErrorMessage = "Last name should not exceed 200 characters.")]
        public string LastName { get; set; }
        
        [Required]
        [EmailAddress(ErrorMessage = "Email should be a valid Email address.")]
        [StringLength(100, ErrorMessage = "Email should not exceed 100 characters.")]
        public string Email { get; set; }
        
        [Required, DisplayName("Is deactivated")]
        public bool IsDeactivated { get; set; }
        
        [ReadOnly(true)]
        public List<SelectListItem> IsDeactivated_Options = new List<SelectListItem>();
    }
}