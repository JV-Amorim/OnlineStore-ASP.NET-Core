using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Libraries.Email;
using OnlineStore.Libraries.LogSystem;
using OnlineStore.Database;

namespace OnlineStore.Controllers
{
    public class HomeController : Controller
    {
        private OnlineStoreContext _database;

        public HomeController(OnlineStoreContext database) => _database = database;

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Index([FromForm]NewsletterEmail newsletter)
        {
            if( ModelState.IsValid)
            {
                _database.NewsletterEmails.Add(newsletter);
                _database.SaveChanges();

                TempData["MSG_OK"] = 
                    "Your email has been successfully registered to the newsletter!";

                return RedirectToAction(nameof(Index));
            }
            else
                return View();
        }

        public IActionResult Contact() => View();

        public IActionResult ContactAction()
        {
            try
            {
                Contact newContact = new Contact()
                {
                    Name = HttpContext.Request.Form["username"],
                    Email = HttpContext.Request.Form["email"],
                    Text = HttpContext.Request.Form["text"]
                };
                
                var listOfMessages = new List<ValidationResult>();
                var context = new ValidationContext(newContact);
                bool isValid = Validator.TryValidateObject(newContact, context, listOfMessages, true);

                if (isValid)
                {
                    EmailContact.SendContactMessageByEmail(newContact);
                    
                    ViewData["MSG_OK"] = "Your contact message has been successfully sent!";
                }
                else
                {
                    StringBuilder completeErrorMessage = new StringBuilder();

                    foreach (var text in listOfMessages)
                        completeErrorMessage.Append(text.ErrorMessage + "<br/>");

                    ViewData["MSG_ERROR"] = completeErrorMessage.ToString();
                    ViewData["CONTACT"] = newContact;
                }
            }
            catch (Exception e)
            {
                ViewData["MSG_ERROR"] = "Oops, something went wrong. Try again!";
            
                LogWriter.WriteNewDataInLogFile("./Logs/ContactErrorLog.log", e.Message);
            }
            
            return View("Contact");
        }

        public IActionResult Login() => View();

        public IActionResult SignUp() => View();

        public IActionResult Cart() => View();
    }
}