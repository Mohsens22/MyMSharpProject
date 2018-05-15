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
    [ViewData("Title", "Admin > Settings > General")]
    [Authorize(Roles = "Admin")]
    [EscapeGCop("Auto generated code.")]
    public partial class AdminSettingsGeneralController : BaseController
    {
        [Route("admin/settings/general")]
        public async Task<ActionResult> Index(vm.GeneralSettingsForm info)
        {
            ModelState.Clear(); // Remove initial validation messages
            
            return View(info);
        }
        
        [HttpPost, Route("GeneralSettingsForm/Save")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(vm.GeneralSettingsForm info)
        {
            try
            {
                if (!await SaveInDatabase(info)) return JsonActions(info);
            }
            catch (Olive.Entities.ValidationException ex)
            {
                return Notify(ex.Message, "error");
            }
            
            Notify("Updated", obstruct: false);
            
            return JsonActions(info);
        }
        
        [NonAction, OnBound]
        public async Task OnBound(vm.GeneralSettingsForm info)
        {
            info.Item = Domain.Settings.Current;
            
            if (Request.IsGet()) await info.Item.CopyDataTo(info);
        }
        
        [NonAction]
        public async Task<bool> SaveInDatabase(vm.GeneralSettingsForm info)
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
    public partial class GeneralSettingsForm : IViewModel
    {
        [ReadOnly(true)]
        public Settings Item { get; set; }
        
        [Required]
        [DisplayName("Password reset ticket expiry minutes")]
        [Range(0, int.MaxValue, ErrorMessage = "Password reset ticket expiry minutes should be 0 or more.")]
        public int? PasswordResetTicketExpiryMinutes { get; set; }
    }
}