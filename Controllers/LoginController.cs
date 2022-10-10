using TBENesys_Orderflow_Core.DataModel;
using TBENesys_Orderflow_Core.Models;
using TBENesys_Orderflow_Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;

namespace TBENesys_Orderflow_Core.Controllers
{
    public class LoginController : Controller
    {
        private readonly IDbContextFactory<ApplicationDbContext> _DbContextFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;

        public LoginController(IDbContextFactory<ApplicationDbContext> contextFactor, IHttpContextAccessor httpContextAccessor)
        {
            _DbContextFactory = contextFactor;
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            session.setSession("");
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                try
                {
                    using (var db = _DbContextFactory.CreateDbContext())
                    {
                        Account obj = db.Account.FirstOrDefault(d => d.UserID == model.username && d.Pwd == model.pass);
                        if (obj == null)
                        {
                            ViewBag.ResultMessage = "User name and password are incorrent!";
                            return View(model);
                        }
                        else
                        {
                            SessionTest session = new SessionTest(_httpContextAccessor);
                            session.setSession(model.username);
                            string UserNameFromSession = session.getSession();

                            Account objAcc = db.Account.FirstOrDefault(d => d.UserID == model.username);
                            if (!String.IsNullOrEmpty(objAcc.AccountRoleID))
                                session.setSessionRoles(objAcc.AccountRoleID);

                            if (objAcc.AccountRoleID.Contains("23") || objAcc.AccountRoleID.Contains("32"))
                            {
                                return RedirectToAction("InvoiceListing", "Admin");
                            }
                            else
                            {
                                return RedirectToAction("Index", "Order");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ResultMessage = "Error! " + ex.Message;
                }
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            session.setSession("");
            session.setSessionRoles("");
            session.setSessionTerms("Not Yet");
            session.setSessionOrderTerms("Not Yet");

            return RedirectToAction("Login", "Login");
        }

        public IActionResult ForgotLogin()
        {
            ForgotViewModel model = new ForgotViewModel();
            SessionTest session = new SessionTest(_httpContextAccessor);
            session.setSession("");
            return View(model);
        }

        [HttpPost]
        public IActionResult ForgotLogin(ForgotViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                try
                {
                    using (var db = _DbContextFactory.CreateDbContext())
                    {
                        Account obj = db.Account.FirstOrDefault(d => d.Email == model.Email);
                        if (obj == null)
                        {
                            ViewBag.ResultMessage = "Error! Please provide a valid email!";
                            return View(model);
                        }
                        else
                        {
                            string Subject = "Smart Source Orderflow - Forgot password!";
                            string Body = "Dear " + obj.Name + ", <br/><br/> Please click on this link to reset your password. <br/><br/>";
                            List<Configuration> Emailconfs = db.Configurations.Where(x => x.Module == "Email Server Configuration").ToList<Configuration>();
                            Body = Body + ((Configuration)Emailconfs.Find(y => y.Name == "URL")).Value + obj.UserID;
                            Body = Body + "<br/><br/Regards,<br/>Smart Source Orderflow Team";
                            SendEMail(obj.Email, Subject, Body);
                            ViewBag.ResultMessage = "A link has been sent to your email address. Please follow the link to reset your password.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ResultMessage = "Error! " + ex.Message;
                }
            }
            return View(model);
        }

        public IActionResult RecoverPassword(string UserDetail)
        {
            RecoverPasswordViewModel model = new RecoverPasswordViewModel();
            SessionTest session = new SessionTest(_httpContextAccessor);
            session.setSession("");
            model.UserID = UserDetail;
            return View(model);
        }

        [HttpPost]
        public IActionResult RecoverPassword(RecoverPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                try
                {
                    using (var db = _DbContextFactory.CreateDbContext())
                    {
                        Account obj = db.Account.FirstOrDefault(d => d.UserID == model.UserID);
                        if (obj == null)
                        {
                            ViewBag.ResultMessage = "Error! Please provide a valid user!";
                            return View(model);
                        }
                        else
                        {
                            if (model.NewPassword != model.ConfirmPassword)
                            {
                                ViewBag.ResultMessage = "Error! New password does not match with the confirm password!";
                                return View(model);
                            }
                            obj.Pwd = model.NewPassword;
                            obj.LastPwdChange = DateTime.Now;
                            db.SaveChanges();
                            ViewBag.ResultMessage = "You can now login with the new password provided.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ResultMessage = "Error! " + ex.Message;
                }
            }
            return View(model);
        }

        public void SendEMail(string emailid, string subject, string body)
        {
            string Host = "";
            string AuthUsername = "";
            string AuthPassword = "";
            string BCC = "";
            string Port = "";
            bool SSLEnabled = false;

            using (var db = _DbContextFactory.CreateDbContext())
            {
                List<Configuration> Emailconfs = db.Configurations.Where(x => x.Module == "Email Server Configuration").ToList<Configuration>();
                if (Emailconfs != null && Emailconfs.Count > 0)
                {
                    AuthUsername = ((Configuration)Emailconfs.Find(y => y.Name == "Username")).Value;
                    BCC = ((Configuration)Emailconfs.Find(y => y.Name == "BCC")).Value;
                    Host = ((Configuration)Emailconfs.Find(y => y.Name == "Server")).Value;
                    AuthPassword = ((Configuration)Emailconfs.Find(y => y.Name == "Password")).Value;
                    string EnableSsl = ((Configuration)Emailconfs.Find(y => y.Name == "EnableSsl")).Value;
                    if (!String.IsNullOrEmpty(EnableSsl) && EnableSsl == "true")
                        SSLEnabled = true;
                    else if (!String.IsNullOrEmpty(EnableSsl) && EnableSsl == "false")
                        SSLEnabled = false;
                    Port = ((Configuration)Emailconfs.Find(y => y.Name == "Port")).Value;
                }
            }

            SmtpClient client = new SmtpClient(Host);
            client.EnableSsl = SSLEnabled; // check if your ISP supports SSL
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Host = Host;
            client.Port = Convert.ToInt32(Port);
            string Username = AuthUsername;
            string Password = AuthPassword;
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(Username, Password);
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(Username);
            if (emailid.Contains(';'))
            {
                string[] emails = emailid.Split(';');
                foreach (string email in emails)
                    msg.To.Add(new MailAddress(email));
            }
            else
                msg.To.Add(new MailAddress(emailid));
            if (!String.IsNullOrEmpty(BCC))
                msg.Bcc.Add(new MailAddress(BCC));
            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = body;

            client.Send(msg);
        }

    }
}
