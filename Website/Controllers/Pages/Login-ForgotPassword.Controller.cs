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
    [ViewData("Title", "Login > Forgot password")]
    [EscapeGCop("Auto generated code.")]
    public partial class LoginForgotPasswordController : BaseController
    {
        [Route("login/forgot-password")]
        public async Task<ActionResult> Index(vm.RequestUserPasswordResetTicket info)
        {
            ModelState.Clear(); // Remove initial validation messages
            
            return View(info);
        }
        
        [HttpPost, Route("RequestUserPasswordResetTicket/Send")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Send(vm.RequestUserPasswordResetTicket info)
        {
            using (var scope = Database.CreateTransactionScope())
            {
                var user = await Domain.User.FindByEmail(info.Email.Trim());
                
                if (user == null)
                {
                    Notify("Invalid email address. Please try again.");
                    return JsonActions(info);
                }
                
                await PasswordResetService.RequestTicket(user);
                
                ReplaceView("<h2> Forgot Your Password? </h2>\n                <p> An email containing instructions to change your password has been sent to your email address. </p>", htmlEncode: false);
                
                scope.Complete();
            }
            
            return JsonActions(info);
        }
    }
}

namespace ViewModel
{
    [EscapeGCop("Auto generated code.")]
    public partial class RequestUserPasswordResetTicket : IViewModel
    {
        [ReadOnly(true)]
        public User Item { get; set; }
        
        [Required]
        [EmailAddress(ErrorMessage = "Email should be a valid Email address.")]
        [StringLength(100, ErrorMessage = "Email should not exceed 100 characters.")]
        public string Email { get; set; }
    }
}