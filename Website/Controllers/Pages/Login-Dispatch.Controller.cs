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
    [ViewData("Title", "Login > Dispatch")]
    [EscapeGCop("Auto generated code.")]
    public partial class LoginDispatchController : BaseController
    {
        [Route("login/dispatch")]
        public async Task<ActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                return Redirect(Url.Index("Admin"));
            }
            
            Notify("TODO: Add redirect logic here and then delete this activity!", obstruct: false);
            
            return new EmptyResult();
        }
    }
}