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
    [ViewData("Title", "Admin > Settings > Administrators")]
    [Authorize(Roles = "Admin")]
    [EscapeGCop("Auto generated code.")]
    public partial class AdminSettingsAdministratorsController : BaseController
    {
        [Route("admin/settings/administrators")]
        public async Task<ActionResult> Index(vm.AdministratorsList info)
        {
            return View(info);
        }
        
        [HttpPost, Route("AdministratorsList/Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(vm.AdministratorsList info, [Bind(Prefix="list.item")] Administrator item)
        {
            try
            {
                await Database.Delete(item);
            }
            catch (Exception ex)
            {
                return Notify(ex.Message, "error");
            }
            
            await TryUpdateModelAsync(info);
            return View(info);
        }
        
        [NonAction, OnBound]
        public async Task OnBound(vm.AdministratorsList info)
        {
            info.Items = await GetSource(info)
                .Select(item => new vm.AdministratorsList.ListItem(item)).ToList();
        }
        
        [NonAction]
        async Task<IEnumerable<Administrator>> GetSource(vm.AdministratorsList info, bool all = false)
        {
            var query = Database.Of<Administrator>();
            
            var result = await query.GetList();
            
            if (info.FullSearch.HasValue())
            {
                var keywords = info.FullSearch.Split(' ').Trim().ToArray();
                result = result.Where(item => item.ToString("F").ContainsAll(keywords, caseSensitive: false));
            }
            
            result = info.Sort.Apply(result);
            
            result = result.ToList();
            info.Paging.TotalItemsCount = result.Count();
            
            if (all) return result;
            else return result.TakePage(info.Paging);
            return result;
        }
    }
}

namespace ViewModel
{
    [EscapeGCop("Auto generated code.")]
    public partial class AdministratorsList : IViewModel
    {
        public AdministratorsList()
        {
            Paging = new ListPagination(this, 10) { UseAjaxGet = true };
            Sort = new ListSortExpression(this);
        }
        
        [FromQuery(Name = "s")]
        public ListSortExpression Sort { get; set; }
        
        [FromQuery(Name = "p")]
        public ListPagination Paging { get; set; }
        
        // Search filters
        public string FullSearch { get; set; }
        
        [ReadOnly(true)]
        public List<ListItem> Items = new List<ListItem>();
        
        public partial class ListItem : IViewModel
        {
            public ListItem(Administrator item) => Item = item;
            
            [ValidateNever]
            public Administrator Item { get; set; }
        }
    }
}