using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Utility
{
    public class EmailSender : IEmailSender    //Implementing Fake EMail Sender Just For Now ger rid of exception on 'IEmailSender emailSender' at Register.cshtml.cs
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //throw new NotImplementedException();
            //TODO- Logic For Sending Email...
            return Task.CompletedTask;
        }
    }
}
