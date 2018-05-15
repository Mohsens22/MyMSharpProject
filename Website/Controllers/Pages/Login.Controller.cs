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
using Olive.Security;
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
    [ViewData("Title", "Login")]
    [EscapeGCop("Auto generated code.")]
    public partial class LoginController : BaseController
    {
        [Route("login")]
        [Route("")]
        public async Task<ActionResult> Index(vm.LoginForm loginForm, vm.SocialMediaLogin socialMediaLogin)
        {
            if (Request.IsAjaxPost())
            {
                return Redirect(Url.CurrentUri().OriginalString);
            }
            
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(Url.Index("LoginDispatch"));
            }
            
            if (Url.ReturnUrl().IsEmpty())
            {
                return Redirect("/login" + "?ReturnUrl=" + "/login");
            }
            
            ModelState.Clear(); // Remove initial validation messages
            
            ViewBag.LoginForm = loginForm;
            ViewBag.SocialMediaLogin = socialMediaLogin;
            
            return View(ViewBag);
        }
        
        [HttpPost, Route("LoginForm/Login")]
        public async Task<ActionResult> Login(vm.LoginForm info)
        {
            var authenticationResult = await Olive.Security.Auth0.Authenticate(info.Email, info.Password);
            
            if (!authenticationResult.Success)
            {
                Notify(authenticationResult.Message, "error");
                return View(info);
            }
            
            await info.Item.LogOn();
            
            if (Url.ReturnUrl().HasValue())
            {
                return Redirect(Url.ReturnUrl());
            }
            
            return AjaxRedirect(Url.Index("LoginDispatch"));
        }
        
        [NonAction, OnBound]
        public async Task OnBound(vm.LoginForm info)
        {
            info.Item = await Domain.User.FindByEmail(info.Email);
        }
        
        [HttpPost, Route("SocialMediaLogin/LoginByGoogle")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginByGoogle(vm.SocialMediaLogin info)
        {
            await OAuth.Instance.LoginBy("Google");
            
            return JsonActions(info);
        }
        
        [HttpPost, Route("SocialMediaLogin/LoginByFacebook")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginByFacebook(vm.SocialMediaLogin info)
        {
            await OAuth.Instance.LoginBy("Facebook");
            
            return JsonActions(info);
        }
        
        [HttpPost, Route("SocialMediaLogin/LoginByMicrosoft")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginByMicrosoft(vm.SocialMediaLogin info)
        {
            await OAuth.Instance.LoginBy("Microsoft");
            
            return JsonActions(info);
        }
    }
}

namespace ViewModel
{
    [EscapeGCop("Auto generated code.")]
    public partial class LoginForm : IViewModel
    {
        [ReadOnly(true)]
        public User Item { get; set; }
        
        [Required]
        [EmailAddress(ErrorMessage = "Email should be a valid Email address.")]
        [StringLength(100, ErrorMessage = "Email should not exceed 100 characters.")]
        public string Email { get; set; }
        
        [Required, DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password should not exceed 100 characters.")]
        public string Password { get; set; }
    }
}

namespace ViewModel
{
    [EscapeGCop("Auto generated code.")]
    public partial class SocialMediaLogin : IViewModel
    {
        public String Provider { get; set; }
        
        public String Error { get; set; }
        
        [ReadOnly(true)]
        public User Item { get; set; }
    }
}