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
    public partial class ContentBlockView : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(vm.ContentBlockView info)
        {
            info = info ?? new vm.ContentBlockView();
            
            info.Item = await ContentBlock.FindByKey(info.Key);
            
            return View(info);
        }
    }
}

namespace ViewModel
{
    [EscapeGCop("Auto generated code.")]
    public partial class ContentBlockView : IViewModel
    {
        public String Key { get; set; }
        
        public String Output
        {
            get
            {
                if (Item == null) return "No content found for key: '" + Key + "'";
                return Item.Content;
            }
        }
        
        [ReadOnly(true)]
        [ValidateNever]
        public ContentBlock Item { get; set; }
    }
}