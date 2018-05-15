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

namespace ViewComponents
{
    [EscapeGCop("Auto generated code.")]
    public partial class AdminSettingsMenu : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(vm.AdminSettingsMenu info)
        {
            info = info ?? new vm.AdminSettingsMenu();
            
            info.ActiveItem = GetActiveItem(info);
            
            return View(info);
        }
        
        [NonAction]
        public string GetActiveItem(vm.AdminSettingsMenu info)
        {
            var items = new[]
            {
                new MenuItem ("GeneralSettings", Url.Index("AdminSettingsGeneral")),
                new MenuItem ("Administrators", Url.Index("AdminSettingsAdministrators")),
                new MenuItem ("ContentBlocks", Url.Index("AdminSettingsContentBlocks"))
            };
            
            return items.Where(i => i.MatchesCurrentUrl()).WithMax(x => x.Url.Split('?').First().Length)?.Key;
        }
    }
}

namespace ViewModel
{
    [EscapeGCop("Auto generated code.")]
    public partial class AdminSettingsMenu : IViewModel
    {
        public string ActiveItem { get; set; }
    }
}