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
    [ViewData("Title", "Admin > Settings > Content blocks")]
    [Authorize(Roles = "Admin")]
    [EscapeGCop("Auto generated code.")]
    public partial class AdminSettingsContentBlocksController : BaseController
    {
        [Route("admin/settings/content-blocks")]
        public async Task<ActionResult> Index(vm.ContentBlocksList info)
        {
            return View(info);
        }
        
        [NonAction, OnBound]
        public async Task OnBound(vm.ContentBlocksList info)
        {
            info.Items = await GetSource(info)
                .Select(item => new vm.ContentBlocksList.ListItem(item)).ToList();
        }
        
        [NonAction]
        async Task<IEnumerable<ContentBlock>> GetSource(vm.ContentBlocksList info)
        {
            var query = Database.Of<ContentBlock>();
            
            var result = await query.GetList();
            
            if (info.Sort.Expression == "Content")
                result = info.Sort.Apply(result, item => item.Content);
            else
                result = info.Sort.Apply(result);
            
            return result;
        }
    }
}

namespace ViewModel
{
    [EscapeGCop("Auto generated code.")]
    public partial class ContentBlocksList : IViewModel
    {
        public ContentBlocksList() => Sort = new ListSortExpression(this);
        
        [FromQuery(Name = "s")]
        public ListSortExpression Sort { get; set; }
        
        [ReadOnly(true)]
        public List<ListItem> Items = new List<ListItem>();
        
        public partial class ListItem : IViewModel
        {
            public ListItem(ContentBlock item) => Item = item;
            
            [ValidateNever]
            public ContentBlock Item { get; set; }
        }
    }
}