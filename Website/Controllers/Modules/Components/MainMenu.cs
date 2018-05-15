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
    public partial class MainMenu : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(vm.MainMenu info)
        {
            info = info ?? new vm.MainMenu();
            
            info.ActiveItem = GetActiveItem(info);
            
            return View(info);
        }
        
        [NonAction]
        public string GetActiveItem(vm.MainMenu info)
        {
            var items = new[]
            {
                new MenuItem ("Login", Url.Index("Login")),
                new MenuItem ("Settings", Url.Index("AdminSettings")),
                new MenuItem ("Apartments", Url.Index("Apartment"))
            };
            
            return items.Where(i => i.MatchesCurrentUrl()).WithMax(x => x.Url.Split('?').First().Length)?.Key;
        }
    }
}

namespace ViewModel
{
    [EscapeGCop("Auto generated code.")]
    public partial class MainMenu : IViewModel
    {
        public string ActiveItem { get; set; }
    }
}