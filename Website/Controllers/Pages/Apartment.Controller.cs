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
    [ViewData("Title", "Apartment")]
    [EscapeGCop("Auto generated code.")]
    public partial class ApartmentController : BaseController
    {
        [Route("apartment")]
        public async Task<ActionResult> Index(vm.ApartmentList info)
        {
            return View(info);
        }
        
        [HttpPost, Route("ApartmentList/Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(vm.ApartmentList info, [Bind(Prefix="list.item")] Apartment item)
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
        public async Task OnBound(vm.ApartmentList info)
        {
            info.Items = await GetSource(info)
                .Select(item => new vm.ApartmentList.ListItem(item)).ToList();
        }
        
        [NonAction]
        async Task<IEnumerable<Apartment>> GetSource(vm.ApartmentList info)
        {
            var query = Database.Of<Apartment>();
            
            var result = await query.GetList();
            
            return result;
        }
    }
}

namespace ViewModel
{
    [EscapeGCop("Auto generated code.")]
    public partial class ApartmentList : IViewModel
    {
        [ReadOnly(true)]
        public List<ListItem> Items = new List<ListItem>();
        
        public partial class ListItem : IViewModel
        {
            public ListItem(Apartment item) => Item = item;
            
            [ValidateNever]
            public Apartment Item { get; set; }
        }
    }
}