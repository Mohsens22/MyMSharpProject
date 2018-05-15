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
    [ViewData("Title", "Login > Reset password > Confirm")]
    [EscapeGCop("Auto generated code.")]
    public partial class LoginResetPasswordConfirmController : BaseController
    {
        [Route("login/reset-password/confirm/{item:Guid}")]
        public async Task<ActionResult> Index(vm.ConfirmPasswordReset info)
        {
            return View(info);
        }
    }
}

namespace ViewModel
{
    [EscapeGCop("Auto generated code.")]
    public partial class ConfirmPasswordReset : IViewModel
    {
        [ValidateNever]
        public User Item { get; set; }
    }
}