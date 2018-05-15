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
    [ViewData("Title", "Admin > Settings > Administrators > View")]
    [Authorize(Roles = "Admin")]
    [EscapeGCop("Auto generated code.")]
    public partial class AdminSettingsAdministratorsViewController : BaseController
    {
        [Route("admin/settings/administrators/view/{item:Guid}")]
        public async Task<ActionResult> Index(vm.ViewAdmin info)
        {
            return View(info);
        }
        
        [HttpPost, Route("ViewAdmin/Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(vm.ViewAdmin info)
        {
            try
            {
                await Database.Delete(info.Item);
            }
            catch (Exception ex)
            {
                return Notify(ex.Message, "error");
            }
            
            return AjaxRedirect(Url.ReturnUrl());
        }
    }
}

namespace ViewModel
{
    [EscapeGCop("Auto generated code.")]
    public partial class ViewAdmin : IViewModel
    {
        [ValidateNever]
        public Administrator Item { get; set; }
    }
}