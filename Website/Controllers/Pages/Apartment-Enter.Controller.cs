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
    [ViewData("Title", "Apartment > Enter")]
    [EscapeGCop("Auto generated code.")]
    public partial class ApartmentEnterController : BaseController
    {
        [Route("apartment/enter/{item:Guid?}")]
        public async Task<ActionResult> Index(vm.ApartmentForm info)
        {
            ModelState.Clear(); // Remove initial validation messages
            
            return View(info);
        }
        
        [HttpPost, Route("ApartmentForm/Save")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(vm.ApartmentForm info)
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
            
            return AjaxRedirect(Url.ReturnUrl());
        }
        
        [NonAction, OnBound]
        public async Task OnBound(vm.ApartmentForm info)
        {
            info.Item = info.Item ?? new Apartment();
            
            if (Request.IsGet()) await info.Item.CopyDataTo(info);
        }
        
        [NonAction]
        public async Task<bool> SaveInDatabase(vm.ApartmentForm info)
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
    public partial class ApartmentForm : IViewModel
    {
        [ValidateNever]
        public Apartment Item { get; set; }
        
        [Required]
        [StringLength(200, ErrorMessage = "Name should not exceed 200 characters.")]
        public string Name { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime? BuildTime { get; set; }
    }
}