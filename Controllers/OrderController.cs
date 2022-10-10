using TBENesys_Orderflow_Core.DataModel;
using TBENesys_Orderflow_Core.Models;
using TBENesys_Orderflow_Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using System.Data;
using System.Net.Mail;

namespace TBENesys_Orderflow_Core.Controllers
{
    public class OrderController : Controller
    {
        private readonly IDbContextFactory<ApplicationDbContext> _DbContextFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;

        public OrderController(IDbContextFactory<ApplicationDbContext> contextFactor, IHttpContextAccessor httpContextAccessor)
        {
            _DbContextFactory = contextFactor;
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
        }

        public IActionResult Index(string Success, string Details)
        {
            OrderViewModel model = new OrderViewModel();
            if (!String.IsNullOrEmpty(Success))
            {
                if (Convert.ToBoolean(Success))
                {
                    model.Response = new ResponseMessage() { User = User.Identity.Name, IsSuccessful = true, MessageDateTime = DateTime.UtcNow, Message = "Operation was successful.", MessageType = "success", Details = Details };
                }
                else
                {
                    model.Response = new ResponseMessage() { User = User.Identity.Name, IsSuccessful = false, MessageDateTime = DateTime.UtcNow, Message = "Operation was unsuccessful.", MessageType = "error", Details = Details };
                }
            }

            model.PageTitle = "Place Order";
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            string SessionKey = "UOD" + UserNameFromSession;
            UserOrderDetails UODNews = session.GetComplexData(SessionKey);

            string SessionTerms = session.getSessionOrderTerms();
            if (String.IsNullOrEmpty(SessionTerms))
                SessionTerms = "Not Yet";
            model.TermsAccepted = SessionTerms;

            if (UODNews != null)
            {
                model.DLs = UODNews.DLs;
                model.SelectedEnvelope = UODNews.EnvelopeSelected;
                model.MLs = UODNews.MLs;
                if (UODNews.MLs != null && UODNews.MLs.Count > 0)
                    model.MLPresent = true;
                else
                    model.MLPresent = false;

                model.RushJob = "No";
                model.EnvelopeSize = "#10 Regular";
                model.JobPrints = "COLOR";

                if (UODNews.JobDetailsFilled)
                {
                    model.JobName = UODNews.JobName;
                    model.JobPrints = UODNews.JobPrints;
                    model.EnvelopeSize = UODNews.EnvelopeSize;
                    model.Printquantity = UODNews.Printquantity;
                    model.OrderNeededBy = UODNews.OrderNeededBy;
                    model.MailDropDate = UODNews.MailDropDate;
                    model.RushJob = UODNews.RushJob;
                    model.SpecialInstructions = UODNews.SpecialInstructions;
                    model.JobDetailsFilled = true;
                }
                model.ClearedActionAppliedDLs = UODNews.ClearedActionAppliedDLs;
                model.ClearedActionAppliedMLs = UODNews.ClearedActionAppliedMLs;
                model.ClearedActionAppliedEnvelope = UODNews.ClearedActionAppliedEnvelope;
            }
            else
            {
                model.DLs = new List<DocumentLists>();
                model.SelectedEnvelope = null;
                model.MLs = new List<DocumentLists>();
                model.JobDetailsFilled = false;



                model.RushJob = "No";
                model.EnvelopeSize = "#10 Regular";
                model.JobPrints = "COLOR";

                model.ClearedActionAppliedDLs = false;
                model.ClearedActionAppliedMLs = false;
                model.ClearedActionAppliedEnvelope = false;
                model.MLPresent = false;

                UODNews = new UserOrderDetails();
                UODNews.UserID = UserNameFromSession;
                UODNews.ClearedActionAppliedDLs = false;
                UODNews.ClearedActionAppliedMLs = false;
                UODNews.ClearedActionAppliedEnvelope = false;
                session.SetComplexData(SessionKey, UODNews);
            }

            model.MailDropDate = DateTime.Now;
            model.OrderNeededBy = DateTime.Now;

            if (UODNews != null && UODNews.DLs != null && UODNews.DLs.Count > 0 )
                model.OrderDetailsFilled = true;
            else
                model.OrderDetailsFilled = false;
            return View(model);
        }

        public string SubmitTermsAccepted()
        {
            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    SessionTest session = new SessionTest(_httpContextAccessor);
                    session.setSessionOrderTerms("Accepted");
                    return "Success";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ResultMessage = "Error! " + ex.Message;
                return "Error";
            }
        }

        public IActionResult SubmitTermsRejected()
        {
            return RedirectToAction("Logout", "Login");
        }

        [HttpPost]
        public IActionResult Index(OrderViewModel model)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");


            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                try
                {
                    string SessionKey = "UOD" + UserNameFromSession;
                    UserOrderDetails UOD = session.GetComplexData(SessionKey);

                    UserOrderDetails UODNews = session.GetComplexData(SessionKey);

                    UOD.JobName = model.JobName;
                    UOD.JobPrints = model.JobPrints;
                    UOD.EnvelopeSize = model.EnvelopeSize;
                    UOD.Printquantity = model.Printquantity;
                    UOD.OrderNeededBy = model.OrderNeededBy;
                    UOD.MailDropDate = model.MailDropDate;
                    UOD.RushJob = model.RushJob;
                    UOD.SpecialInstructions = model.SpecialInstructions;
                    UOD.JobDetailsFilled = true;

                    session.SetComplexData(SessionKey, UOD);

                    return RedirectToAction("OrderCheckOut");
                }
                catch (Exception ex)
                {
                    ViewBag.ResultMessage = "Error! " + ex.Message;
                    return RedirectToAction("Index", new { Success = true, Details = ViewBag.ResultMessage });
                }

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult OrderClearDocuments()
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            bool IsSuccessful = false;
            string DetailMessage = "";
            try
            {
                string SessionKey = "UOD" + UserNameFromSession;
                UserOrderDetails UODNews = session.GetComplexData(SessionKey);
                if (UODNews.DLs != null)
                {
                    int Count = UODNews.DLs.Count();
                    for (int i = 0; i < Count; i++)
                    {
                        DocumentLists DLTobeDeleted = UODNews.DLs[i];
                        string path = DLTobeDeleted.FilePath;
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)//check file exsit or not  
                        {
                            file.Delete();
                        }
                        DetailMessage = "Document removed.";
                        IsSuccessful = true;
                    }
                }
                UODNews.DLs = new List<DocumentLists>();
                UODNews.ClearedActionAppliedDLs = true;
                session.SetComplexData(SessionKey, UODNews);
            }
            catch (Exception ex)
            {
                IsSuccessful = false;
                DetailMessage = ex.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult OrderClearEnvelope(int id)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            bool IsSuccessful = false;
            string DetailMessage = "";
            try
            {
                string SessionKey = "UOD" + UserNameFromSession;
                UserOrderDetails UODNews = session.GetComplexData(SessionKey);
                if (UODNews.EnvelopeSelected != null)
                {
                    UODNews.EnvelopeSelected = null;
                    UODNews.ClearedActionAppliedEnvelope = true;
                    session.SetComplexData(SessionKey, UODNews);
                    DetailMessage = "Envelop removed.";
                    IsSuccessful = true;
                }
                UODNews.EnvelopeSelected = null;
                UODNews.ClearedActionAppliedEnvelope = true;
                session.SetComplexData(SessionKey, UODNews);
            }
            catch (Exception ex)
            {
                IsSuccessful = false;
                DetailMessage = ex.Message;
            }

            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult OrderClearMailing()
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            bool IsSuccessful = false;
            string DetailMessage = "";
            try
            {
                string SessionKey = "UOD" + UserNameFromSession;
                UserOrderDetails UODNews = session.GetComplexData(SessionKey);
                if (UODNews.MLs != null)
                {
                    int Count = UODNews.MLs.Count();
                    for (int i = 0; i < Count; i++)
                    {
                        DocumentLists DLTobeDeleted = UODNews.MLs[i];
                        string path = DLTobeDeleted.FilePath;
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)//check file exsit or not  
                        {
                            file.Delete();
                        }
                        DetailMessage = "Mailing list removed.";
                        IsSuccessful = true;
                    }
                }
                UODNews.MLs = new List<DocumentLists>();
                UODNews.ClearedActionAppliedMLs = true;
                session.SetComplexData(SessionKey, UODNews);
            }
            catch (Exception ex)
            {
                IsSuccessful = false;
                DetailMessage = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult OrderClearAll()
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            bool IsSuccessful = false;
            string DetailMessage = "";
            try
            {
                string SessionKey = "UOD" + UserNameFromSession;
                UserOrderDetails UODNews = session.GetComplexData(SessionKey);

                if (UODNews.DLs != null)
                {
                    int Count = UODNews.DLs.Count();
                    for (int i = 0; i < Count; i++)
                    {
                        DocumentLists DLTobeDeleted = UODNews.DLs[i];
                        string path = DLTobeDeleted.FilePath;
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)//check file exsit or not  
                        {
                            file.Delete();
                        }
                        DetailMessage = "Document removed.";
                        IsSuccessful = true;
                    }
                }
                UODNews.DLs = new List<DocumentLists>();

                if (UODNews.EnvelopeSelected != null)
                {
                    UODNews.EnvelopeSelected = null;
                    session.SetComplexData(SessionKey, UODNews);
                    DetailMessage = "Envelop removed.";
                    IsSuccessful = true;
                }

                if (UODNews.MLs != null)
                {
                    int Count = UODNews.MLs.Count();
                    for (int i = 0; i < Count; i++)
                    {
                        DocumentLists DLTobeDeleted = UODNews.MLs[i];
                        string path = DLTobeDeleted.FilePath;
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)//check file exsit or not  
                        {
                            file.Delete();
                        }
                        DetailMessage = "Mailing list removed.";
                        IsSuccessful = true;
                    }
                    UODNews.MLs = new List<DocumentLists>();
                }

                UserOrderDetails UOD = new UserOrderDetails();
                UOD.UserID = UserNameFromSession;
                UOD.ClearedActionAppliedDLs = false;
                UOD.ClearedActionAppliedMLs = false;
                UOD.ClearedActionAppliedEnvelope = false;
                session.SetComplexData(SessionKey, UOD);
            }
            catch (Exception ex)
            {
                IsSuccessful = false;
                DetailMessage = ex.Message;
            }

            return RedirectToAction("Index");
        }

        public IActionResult OrderCheckOut(string Success, string Details)
        {
            OrderCheckOutViewModel model = new OrderCheckOutViewModel();
            if (!String.IsNullOrEmpty(Success))
            {
                if (Convert.ToBoolean(Success))
                {
                    model.Response = new ResponseMessage() { User = User.Identity.Name, IsSuccessful = true, MessageDateTime = DateTime.UtcNow, Message = "Operation was successful.", MessageType = "success", Details = Details };
                }
                else
                {
                    model.Response = new ResponseMessage() { User = User.Identity.Name, IsSuccessful = false, MessageDateTime = DateTime.UtcNow, Message = "Operation was unsuccessful.", MessageType = "error", Details = Details };
                }
            }

            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            model.PageTitle = "Order CheckOut";

            string SessionKey = "UOD" + UserNameFromSession;
            UserOrderDetails UODNews = session.GetComplexData(SessionKey);

            #region Load Last filled details if any
            using (var db = _DbContextFactory.CreateDbContext())
            {
                model.AllChargeOrderTos = db.Contents.Where(d => d.MainType == "ChargeToCC").OrderBy(d => Convert.ToString(d.MainContent)).Select(x => new Itemlist { Value = x.MainContent, Text = x.MainContent }).ToList();
                model.AllApprovalManagers = db.Account.Where(d => d.AccountRoleID.Contains("24")).Select(x => new Itemlist { Value = x.lngID.ToString(), Text = x.Email }).ToList();

                //model.AllShipToLocations = db.Contents.Where(d => d.MainType == "ShipToCC").Select(x => new Itemlist { Value = x.lngID.ToString(), Text = x.MainContent }).ToList();
                Account ACT = db.Account.First(d => d.UserID == UserNameFromSession);

                model.RequestorName = ACT.Name;
                model.RequestorEmail = ACT.Email;

                Order UserOrder = db.Orders.Where(d => d.lngAccount == ACT.lngID).OrderByDescending(d => d.lngID).FirstOrDefault();
                if (UserOrder != null)
                {
                    OrderCheckoutField OCF = db.OrderCheckoutFields.FirstOrDefault(d => d.lngOrder == UserOrder.lngID && d.name == "Contact_Requestor Phone");
                    if (OCF != null)
                        model.RequestorPhone = OCF.value;

                    OCF = new OrderCheckoutField();
                    OCF = db.OrderCheckoutFields.FirstOrDefault(d => d.lngOrder == UserOrder.lngID && d.name == "Contact_Fringe");
                    if (OCF != null)
                        model.Fringe = OCF.value;

                    OCF = new OrderCheckoutField();
                    OCF = db.OrderCheckoutFields.FirstOrDefault(d => d.lngOrder == UserOrder.lngID && d.name == "Contact_Charge Order To");
                    if (OCF != null)
                        model.ChargeOrderTo = OCF.value;

                    OCF = new OrderCheckoutField();
                    OCF = db.OrderCheckoutFields.FirstOrDefault(d => d.lngOrder == UserOrder.lngID && d.name == "Contact_Approval Manager");
                    if (OCF != null)
                        model.ApprovalManager = OCF.value;
                }
            }
            #endregion
            return View(model);
        }

        [HttpPost]
        public IActionResult OrderCheckOut(OrderCheckOutViewModel model,string approval_manager)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");


            if (!ModelState.IsValid)
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    model.AllChargeOrderTos = db.Contents.Where(d => d.MainType == "ChargeToCC").Select(x => new Itemlist { Value = x.MainContent, Text = x.MainContent }).ToList();
                    model.AllApprovalManagers = db.Account.Where(d => d.AccountRoleID.Contains("24")).Select(x => new Itemlist { Value = x.lngID.ToString(), Text = x.Email }).ToList();
                    //model.AllShipToLocations = db.Contents.Where(d => d.MainType == "ShipToCC").Select(x => new Itemlist { Value = x.lngID.ToString(), Text = x.MainContent }).ToList();

                }
                return View(model);
            }
            else
            {
                try
                {
                    string SessionKey = "UOD" + UserNameFromSession;
                    UserOrderDetails UOD = session.GetComplexData(SessionKey);

                    if (UOD == null)
                    {
                        ViewBag.ResultMessage = "Your session has expired! Please place order again.";
                        return RedirectToAction("Index", new { Success = false, Details = ViewBag.ResultMessage });

                    }
                    else
                    {
                        using (var db = _DbContextFactory.CreateDbContext())
                        {
                            Account ACT = db.Account.First(d => d.UserID == UserNameFromSession);
                            Order UserOrder = db.Orders.Where(d => d.lngAccount == ACT.lngID).OrderByDescending(d => d.lngID).FirstOrDefault();
                            UserOrderDetails UODNews = session.GetComplexData(SessionKey);
                            OrderCheckoutField OCF = db.OrderCheckoutFields.FirstOrDefault(d => d.lngOrder == UserOrder.lngID && d.name == "Contact_Requestor Phone");

                            UOD.RequestorName = model.RequestorName;
                            UOD.RequestorEmail = model.RequestorEmail;
                            UOD.RequestorPhone = model.RequestorPhone;
                            UOD.Fringe = model.Fringe;
                            UOD.ChargeOrderTo = model.ChargeOrderTo;
                            UOD.ApprovalManager = model.ApprovalManager;
                            var Id = Convert.ToInt32(model.ApprovalManager);
                            var ApprovalManagerName= db.Account.Where(x => x.lngID == Id).SingleOrDefault()?.Name;
                            UOD.ApprovalManagerName = ApprovalManagerName;
                            

                            //if (db.OrderCheckoutFields.FirstOrDefault(d => d.lngOrder == UserOrder.lngID && d.name == "Contact_Approval Manager") != null) {
                            //    UOD.ApprovalManagerName = db.OrderCheckoutFields.FirstOrDefault(d => d.lngOrder == UserOrder.lngID && d.name == "Contact_Approval Manager").value;
                            //}
                            
                            //if (model.ApprovalManager== "1996443")
                            //{
                            //    UOD.ApprovalManagerName = "fahad.hafeez@hotmail.com";
                            //}
                            //else if (model.ApprovalManager== "1904319")
                            //{
                            //    UOD.ApprovalManagerName = "Rikki.Perez@TBENesys.com";
                            //}
                            //else if (model.ApprovalManager== "1904319")
                            //{
                            //    UOD.ApprovalManagerName = "";
                            //}
                            //else if (model.ApprovalManager== "1904319")
                            //{
                            //    UOD.ApprovalManagerName = "Rikki.Perez@TBENesys.com";
                            //}

                            //UOD.ShipToLocation = model.ShipToLocation;
                            //UOD.Company = model.Company;
                            //UOD.Attention = model.Attention;
                            //UOD.Street = model.Street;
                            //UOD.Suite = model.Suite;
                            //UOD.City = model.City;
                            //UOD.State = model.State;
                            //UOD.Zip = model.Zip;

                            UOD.OrderContactShippingFilled = true;
                            session.SetComplexData(SessionKey, UOD);
                        }
                    }

                    return RedirectToAction("OrderPreview");
                }
                catch (Exception ex)
                {
                    ViewBag.ResultMessage = "Error! " + ex.Message;
                    using (var db = _DbContextFactory.CreateDbContext())
                    {
                        model.AllChargeOrderTos = db.Contents.Where(d => d.MainType == "ChargeToCC").Select(x => new Itemlist { Value = x.MainContent, Text = x.MainContent }).ToList();
                        model.AllApprovalManagers = db.Account.Where(d => d.AccountRoleID.Contains("24")).Select(x => new Itemlist { Value = x.lngID.ToString(), Text = x.Email }).ToList();
                        //model.AllShipToLocations = db.Contents.Where(d => d.MainType == "ShipToCC").Select(x => new Itemlist { Value = x.lngID.ToString(), Text = x.MainContent }).ToList();
                    }
                }

            }
            return View(model);
        }

        public IActionResult OrderPreview(string Success, string Details)
        {
            OrderPreviewViewModel model = new OrderPreviewViewModel();
            if (!String.IsNullOrEmpty(Success))
            {
                if (Convert.ToBoolean(Success))
                {
                    model.Response = new ResponseMessage() { User = User.Identity.Name, IsSuccessful = true, MessageDateTime = DateTime.UtcNow, Message = "Operation was successful.", MessageType = "success", Details = Details };
                }
                else
                {
                    model.Response = new ResponseMessage() { User = User.Identity.Name, IsSuccessful = false, MessageDateTime = DateTime.UtcNow, Message = "Operation was unsuccessful.", MessageType = "error", Details = Details };
                }
            }

            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            model.PageTitle = "Upload Document";

            string SessionKey = "UOD" + UserNameFromSession;
            UserOrderDetails UODNews = session.GetComplexData(SessionKey);

            #region Load Last filled details if any

            model.UODs = UODNews;
            using (var db = _DbContextFactory.CreateDbContext())
            {
                model.AllEmails = db.Account.Select(x => new SelectListItem { Value = x.Email.ToString(), Text = x.Email }).ToList();
            }
            #endregion

            return View(model);
        }

        [HttpPost]
        public IActionResult OrderPreview(OrderPreviewViewModel model)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            try
            {
                string SessionKey = "UOD" + UserNameFromSession;

                UserOrderDetails UOD = session.GetComplexData(SessionKey);

                using (var db = _DbContextFactory.CreateDbContext())
                {
                    
                    Order NewOrder = new Order();
                    Account AR = db.Account.FirstOrDefault(d => d.UserID == UserNameFromSession);
                    NewOrder.lngAccount = AR.lngID;
                    NewOrder.datSubmitted = DateTime.Now;
                    NewOrder.status = "pending";
                    NewOrder.datApproved = null;

                    NewOrder.approver_acctID = Convert.ToInt32(UOD.ApprovalManager);
                    NewOrder.approver_comment = "";
                    string DocumentUploadFiles = "";
                    foreach (DocumentLists DL in UOD.DLs)
                    {
                        if (String.IsNullOrEmpty(DocumentUploadFiles))
                            DocumentUploadFiles = DL.FileName;
                        else
                            DocumentUploadFiles = DocumentUploadFiles + "," + DL.FileName;
                    }
                    NewOrder.document_uploadFiles = DocumentUploadFiles;

                    if (UOD.EnvelopeSelected != null)
                    {
                        NewOrder.envelope_productType = "Envelope";
                        NewOrder.envelope_product_Cost_Center = UOD.EnvelopeSelected.MainContent;
                        NewOrder.envelope_product_Group_Name_Line_1 = UOD.EnvelopeSelected.SubContent2;
                        NewOrder.envelope_product_Group_Name_Line_2 = UOD.EnvelopeSelected.SubContent3;
                        NewOrder.envelope_product_Group_Name_Line_3 = UOD.EnvelopeSelected.SubContent4;
                        NewOrder.envelope_product_Address_Line_4 = UOD.EnvelopeSelected.SubContent5;
                        NewOrder.envelope_product_City_State_Zip_Line_5 = UOD.EnvelopeSelected.SubContent6;
                    }
                    else
                    {
                        NewOrder.envelope_productType = "none";
                        NewOrder.envelope_product_Cost_Center = "";
                        NewOrder.envelope_product_Group_Name_Line_1 = "";
                        NewOrder.envelope_product_Group_Name_Line_2 = "";
                        NewOrder.envelope_product_Group_Name_Line_3 = "";
                        NewOrder.envelope_product_Address_Line_4 = "";
                        NewOrder.envelope_product_City_State_Zip_Line_5 = "";
                    }

                    string MailingListUploadFiles = "";
                    if(!String.IsNullOrEmpty(MailingListUploadFiles))
                    {
                        foreach (DocumentLists ML in UOD.MLs)
                        {
                            if (String.IsNullOrEmpty(MailingListUploadFiles))
                                MailingListUploadFiles = ML.FileName;
                            else
                                MailingListUploadFiles = MailingListUploadFiles + "," + ML.FileName;
                        }
                    }
                    
                    
                    NewOrder.mailing_list_uploadFiles = MailingListUploadFiles;

                    NewOrder.job_details_jobName = UOD.JobName;
                    NewOrder.job_details_jobPrints = UOD.JobPrints;
                    NewOrder.job_details_envSize = UOD.EnvelopeSize;
                    NewOrder.job_details_includesMailList = "";
                    if (UOD.MLs != null && UOD.MLs.Count > 0)
                        NewOrder.job_details_mailDropDate = UOD.MailDropDate;
                    else
                        NewOrder.job_details_neededByDate = UOD.OrderNeededBy;

                    NewOrder.job_details_printQuantity = UOD.Printquantity.ToString();
                    NewOrder.job_details_rushJob = UOD.RushJob;
                    NewOrder.job_details_specInstr = UOD.SpecialInstructions;

                    NewOrder.vendor_status = "";
                    NewOrder.vendor_invoiceNo = "";
                    NewOrder.vendor_submittedDate = null;
                    if (model.SelectedEmails != null && model.SelectedEmails.Length > 0)
                        NewOrder.json_extras = String.Join(",", model.SelectedEmails.ToList());
                    else
                        NewOrder.json_extras = "";

                    db.Orders.Add(NewOrder);
                    db.SaveChanges();

                    #region Requestor Details 

                    OrderCheckoutField OCF = new OrderCheckoutField();
                    OCF.lngOrder = NewOrder.lngID;
                    OCF.name = "Contact_Requestor Name";
                    OCF.value = UOD.RequestorName;
                    OCF.display_order = 0;
                    db.OrderCheckoutFields.Add(OCF);
                    db.SaveChanges();

                    OCF = new OrderCheckoutField();
                    OCF.lngOrder = NewOrder.lngID;
                    OCF.name = "Contact_Requestor Email";
                    OCF.value = UOD.RequestorEmail;
                    OCF.display_order = 0;
                    db.OrderCheckoutFields.Add(OCF);
                    db.SaveChanges();

                    OCF = new OrderCheckoutField();
                    OCF.lngOrder = NewOrder.lngID;
                    OCF.name = "Contact_Requestor Phone";
                    OCF.value = UOD.RequestorPhone;
                    OCF.display_order = 0;
                    db.OrderCheckoutFields.Add(OCF);
                    db.SaveChanges();

                    OCF = new OrderCheckoutField();
                    OCF.lngOrder = NewOrder.lngID;
                    OCF.name = "Contact_Fringe";
                    OCF.value = UOD.Fringe;
                    OCF.display_order = 0;
                    db.OrderCheckoutFields.Add(OCF);
                    db.SaveChanges();

                    OCF = new OrderCheckoutField();
                    OCF.lngOrder = NewOrder.lngID;
                    OCF.name = "Contact_Charge Order To";
                    OCF.value = UOD.ChargeOrderTo;
                    OCF.display_order = 0;
                    db.OrderCheckoutFields.Add(OCF);
                    db.SaveChanges();

                    Account AM = db.Account.FirstOrDefault(d => d.lngID == Convert.ToInt32(UOD.ApprovalManager));
                    OCF = new OrderCheckoutField();
                    OCF.lngOrder = NewOrder.lngID;
                    OCF.name = "Contact_Approval Manager";
                    OCF.value = AM.Email;
                    OCF.display_order = 0;
                    db.OrderCheckoutFields.Add(OCF);
                    db.SaveChanges();

                    #endregion

                    #region Shipping Details
                    OCF = new OrderCheckoutField();
                    OCF.lngOrder = NewOrder.lngID;
                    OCF.name = "Shipping_Ship To Location";
                    OCF.value = "";
                    OCF.display_order = 0;
                    db.OrderCheckoutFields.Add(OCF);
                    db.SaveChanges();

                    OCF = new OrderCheckoutField();
                    OCF.lngOrder = NewOrder.lngID;
                    OCF.name = "Shipping_Company";
                    OCF.value = "";
                    OCF.display_order = 0;
                    db.OrderCheckoutFields.Add(OCF);
                    db.SaveChanges();

                    OCF = new OrderCheckoutField();
                    OCF.lngOrder = NewOrder.lngID;
                    OCF.name = "Shipping_Attention";
                    OCF.value = "";
                    OCF.display_order = 0;
                    db.OrderCheckoutFields.Add(OCF);
                    db.SaveChanges();

                    OCF = new OrderCheckoutField();
                    OCF.lngOrder = NewOrder.lngID;
                    OCF.name = "Shipping_Street";
                    OCF.value = "";
                    OCF.display_order = 0;
                    db.OrderCheckoutFields.Add(OCF);
                    db.SaveChanges();

                    OCF = new OrderCheckoutField();
                    OCF.lngOrder = NewOrder.lngID;
                    OCF.name = "Shipping_Suite";
                    OCF.value = "";
                    OCF.display_order = 0;
                    db.OrderCheckoutFields.Add(OCF);
                    db.SaveChanges();

                    OCF = new OrderCheckoutField();
                    OCF.lngOrder = NewOrder.lngID;
                    OCF.name = "Shipping_City";
                    OCF.value = "";
                    OCF.display_order = 0;
                    db.OrderCheckoutFields.Add(OCF);
                    db.SaveChanges();

                    OCF = new OrderCheckoutField();
                    OCF.lngOrder = NewOrder.lngID;
                    OCF.name = "Shipping_State";
                    OCF.value = "";
                    OCF.display_order = 0;
                    db.OrderCheckoutFields.Add(OCF);
                    db.SaveChanges();

                    OCF = new OrderCheckoutField();
                    OCF.lngOrder = NewOrder.lngID;
                    OCF.name = "Shipping_Zip";
                    OCF.value = "";
                    OCF.display_order = 0;
                    db.OrderCheckoutFields.Add(OCF);
                    db.SaveChanges();

                    #endregion

                    //Generate email to requestor, approver & additional emails
                    string SubjectRequester = "Orderflow TBEN-" + NewOrder.lngID + " Pending Manager Approval [Orderer Copy]";

                    string SubjectApprover = "Orderflow TBEN-" + NewOrder.lngID + " Pending Manager Approval [Approver Copy]";

                    List<Configuration> Emailconfs = db.Configurations.Where(x => x.Module == "Order Template").ToList<Configuration>();
                    string Body = ((Configuration)Emailconfs.Find(y => y.Name == "Order Template")).Value;
                    string EmailBody = Body;

                    Body = Body.Replace("@OrderNumber", "TBEN-" + NewOrder.lngID.ToString());
                    Body = Body.Replace("@OrderDate", NewOrder.datSubmitted.Value.ToString("MM/dd/yyyy h:mm tt"));

                    List<OrderCheckoutField> OCFs = db.OrderCheckoutFields.Where(d => d.lngOrder == NewOrder.lngID).ToList();
                    
                    if (OCFs != null && OCFs.Count > 0)
                    {
                        if (OCFs.FirstOrDefault(d => d.name == "Contact_Requestor Name") != null)
                            Body = Body.Replace("@RequestorName", OCFs.First(d => d.name == "Contact_Requestor Name").value);
                        else
                            Body = Body.Replace("@RequestorName", "");

                        if (OCFs.FirstOrDefault(d => d.name == "Contact_Requestor Email") != null)
                            Body = Body.Replace("@RequestorEmail", OCFs.First(d => d.name == "Contact_Requestor Email").value);
                        else
                            Body = Body.Replace("@RequestorEmail", "");

                        if (OCFs.FirstOrDefault(d => d.name == "Contact_Requestor Phone") != null)
                            Body = Body.Replace("@RequestorPhone", OCFs.First(d => d.name == "Contact_Requestor Phone").value);
                        else
                            Body = Body.Replace("@RequestorPhone", "");

                        if (OCFs.FirstOrDefault(d => d.name == "Contact_Charge Order To") != null)
                            Body = Body.Replace("@ChargeOrder", OCFs.First(d => d.name == "Contact_Charge Order To").value);
                        else
                            Body = Body.Replace("@ChargeOrder", "");

                        if (OCFs.FirstOrDefault(d => d.name == "Contact_Approval Manager") != null)
                            Body = Body.Replace("@ApprovalManager", OCFs.First(d => d.name == "Contact_Approval Manager").value);
                        else
                            Body = Body.Replace("@ApprovalManager", OCFs.First(d => d.name == "Contact_Approval Manager").value);
                    }
                    else
                    {
                        Body = Body.Replace("@RequestorName", "");
                        Body = Body.Replace("@RequestorEmail", "");
                        Body = Body.Replace("@RequestorPhone", "");
                        Body = Body.Replace("@ChargeOrder", "");
                        Body = Body.Replace("@ApprovalManager", "");
                    }

                    Body = Body.Replace("@JobName", NewOrder.job_details_jobName);

                    if (UOD.EnvelopeSelected!=null)
                    {
                        Body = Body.Replace("@Envelope", UOD.EnvelopeSelected.SubContent1);
                    }
                    else
                    {
                        Body = Body.Replace("@Envelope", "None");
                    }
                    if (UOD.EnvelopeSize!=null)
                    {
                        var size = UOD.EnvelopeSize;
                        Body = Body.Replace("@EnvelopeSize", size);
                    }
                    else
                    {
                        Body = Body.Replace("@EnvelopeSize", "No ");
                    }
                    if (UOD.MLs.Count!=0)
                    {
                        Body = Body.Replace("@MailingList", UOD.MLs.Select(file=>file.FileName).FirstOrDefault());
                    }
                    else
                    {
                        Body = Body.Replace("@MailingList", "No Mail List");
                    }
                    Body = Body.Replace("@JobPrint", NewOrder.job_details_jobPrints);
                    Body = Body.Replace("@MailDropDate", NewOrder.job_details_mailDropDate.HasValue ? NewOrder.job_details_mailDropDate.Value.ToString("MM/dd/yyyy") : "");
                    Body = Body.Replace("@RushJob", NewOrder.job_details_rushJob);
                    Body = Body.Replace("@SpecialInstructions", NewOrder.job_details_specInstr);

                    EmailBody = Body;

                    string DocumentListBuilder = "";
                    if (NewOrder.document_uploadFiles.Split(',').Length > 0)
                    {
                        Account Requester = db.Account.FirstOrDefault(d => d.lngID == NewOrder.lngAccount);
                        var folderName = Path.Combine("Repository", "Orders", "UOD" + Requester.UserID, "Document");
                        Configuration SiteURL = db.Configurations.First(x => x.Module == "SiteURL");

                        if (Requester != null)
                        {
                            DocumentListBuilder = "<span><b>" + NewOrder.document_uploadFiles.Split(',').Length + "</b> file(s)</span> <ol>";
                            foreach (string fileselected in NewOrder.document_uploadFiles.Split(','))
                            {
                                DocumentListBuilder = DocumentListBuilder + "<li><a href='" + SiteURL.Value + "/Admin/DownloadFile?filePath=" + folderName + "/" + fileselected + "' target='_blank'>" + fileselected + "</a></li>";
                            }
                            DocumentListBuilder = DocumentListBuilder + "</ol>";
                        }
                    }
                    Body = Body.Replace("@DocumentList", DocumentListBuilder);

                    string EmailDocumentListBuilder = "";
                    if (NewOrder.document_uploadFiles.Split(',').Length > 0)
                    {
                        Account Requester = db.Account.FirstOrDefault(d => d.lngID == NewOrder.lngAccount);
                        var folderName = Path.Combine("Repository", "Orders", "UOD" + Requester.UserID, "Document");
                        Configuration SiteURL = db.Configurations.First(x => x.Module == "SiteURL");

                        if (Requester != null)
                        {
                            EmailDocumentListBuilder = "<span><b>" + NewOrder.document_uploadFiles.Split(',').Length + "</b> file(s)</span> <ol>";
                            foreach (string fileselected in NewOrder.document_uploadFiles.Split(','))
                            {
                                EmailDocumentListBuilder = EmailDocumentListBuilder + "<li>" + fileselected + "</li>";
                            }
                            EmailDocumentListBuilder = EmailDocumentListBuilder + "</ol>";
                        }
                    }
                    EmailBody = EmailBody.Replace("@DocumentList", EmailDocumentListBuilder);

                    DocumentListBuilder = "";
                    if (!String.IsNullOrEmpty(NewOrder.mailing_list_uploadFiles) && NewOrder.mailing_list_uploadFiles.Split(',').Length > 0)
                    {
                        Account Requester = db.Account.FirstOrDefault(d => d.lngID == NewOrder.lngAccount);
                        var folderName = Path.Combine("Repository", "Orders", "UOD" + Requester.UserID, "MailingList");
                        Configuration SiteURL = db.Configurations.First(x => x.Module == "SiteURL");

                        DocumentListBuilder = "<span><b>" + NewOrder.mailing_list_uploadFiles.Split(',').Length + "</b> file(s)</span> <ol>";
                        foreach (string fileselected in NewOrder.mailing_list_uploadFiles.Split(','))
                        {
                            DocumentListBuilder = DocumentListBuilder + "<li><a href='" + SiteURL.Value + "/Admin/DownloadFile?filePath=" + folderName + "/" + fileselected + "' target='_blank'>" + fileselected + "</a></li>";
                        }
                        DocumentListBuilder = DocumentListBuilder + "</ol>";
                    }
                    Body = Body.Replace("@MailingList", DocumentListBuilder);

                    EmailDocumentListBuilder = "";
                    if (!String.IsNullOrEmpty(NewOrder.mailing_list_uploadFiles) && NewOrder.mailing_list_uploadFiles.Split(',').Length > 0)
                    {
                        Account Requester = db.Account.FirstOrDefault(d => d.lngID == NewOrder.lngAccount);
                        var folderName = Path.Combine("Repository", "Orders", "UOD" + Requester.UserID, "MailingList");
                        Configuration SiteURL = db.Configurations.First(x => x.Module == "SiteURL");

                        EmailDocumentListBuilder = "<span><b>" + NewOrder.mailing_list_uploadFiles.Split(',').Length + "</b> file(s)</span> <ol>";
                        foreach (string fileselected in NewOrder.mailing_list_uploadFiles.Split(','))
                        {
                            EmailDocumentListBuilder = EmailDocumentListBuilder + "<li>" + fileselected + "</li>";
                        }
                        EmailDocumentListBuilder = EmailDocumentListBuilder + "</ol>";
                    }
                    EmailBody = EmailBody.Replace("@MailingList", EmailDocumentListBuilder);

                    session.SetEmailData(NewOrder.lngID.ToString(), Body);
                    //1. Requestor
                    SendEMail(UOD.RequestorEmail, SubjectRequester, EmailBody);

                    Account AMEmail = db.Account.FirstOrDefault(d => d.lngID == Convert.ToInt32(UOD.ApprovalManager));
                    //2. Approver
                    SendEMail(AMEmail.Email, SubjectApprover, EmailBody);

                    //3. Additional Emails
                    if (model.SelectedEmails != null)
                    {
                        foreach (string receipient in model.SelectedEmails.ToList())
                        {
                            SendEMail(receipient, SubjectRequester, EmailBody);
                        }
                    }

                    ViewBag.ResultMessage = "An email receipt has been sent to you. Your order is TBEN-" + NewOrder.lngID;
                    UOD = new UserOrderDetails();
                    session.SetComplexData(SessionKey, UOD);
                    return RedirectToAction("OrderReceipt", new { OrderId = NewOrder.lngID });
                }
            }
            catch (Exception ex)
            {
                ViewBag.ResultMessage = "Error! " + ex.Message;
                return RedirectToAction("OrderPreview", new { Success = false, Details = ViewBag.ResultMessage });
            }
        }

        public IActionResult OrderReceipt(int OrderId, string Success, string Details)
        {
            OrderReceiptViewModel model = new OrderReceiptViewModel();
            if (!String.IsNullOrEmpty(Success))
            {
                if (Convert.ToBoolean(Success))
                {
                    model.Response = new ResponseMessage() { User = User.Identity.Name, IsSuccessful = true, MessageDateTime = DateTime.UtcNow, Message = "Operation was successful.", MessageType = "success", Details = Details };
                }
                else
                {
                    model.Response = new ResponseMessage() { User = User.Identity.Name, IsSuccessful = false, MessageDateTime = DateTime.UtcNow, Message = "Operation was unsuccessful.", MessageType = "error", Details = Details };
                }
            }

            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            model.PageTitle = "Order Check Out";

            model.OrderNumber = OrderId;
            model.OrderReceiptDetails = session.GetEmailData(OrderId.ToString());

            return View(model);
        }

        public IActionResult OrderUploadDocuments(string Success, string Details)
        {
            OrderUploadDocumentsViewModel model = new OrderUploadDocumentsViewModel();
            if (!String.IsNullOrEmpty(Success))
            {
                if (Convert.ToBoolean(Success))
                {
                    model.Response = new ResponseMessage() { User = User.Identity.Name, IsSuccessful = true, MessageDateTime = DateTime.UtcNow, Message = "Operation was successful.", MessageType = "success", Details = Details };
                }
                else
                {
                    model.Response = new ResponseMessage() { User = User.Identity.Name, IsSuccessful = false, MessageDateTime = DateTime.UtcNow, Message = "Operation was unsuccessful.", MessageType = "error", Details = Details };
                }
            }

            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            model.PageTitle = "Upload Document";

            string SessionKey = "UOD" + UserNameFromSession;
            UserOrderDetails UODNews = session.GetComplexData(SessionKey);
            if (UODNews != null)
            {
                model.DLs = UODNews.DLs;
                model.TotalDocuments = UODNews.DLs != null ? UODNews.DLs.Count : 0;

            }
            else
            {
                model.DLs = new List<DocumentLists>();
                model.TotalDocuments = 0;
            }

            return View(model);
        }

        [HttpPost, DisableRequestSizeLimit]
        public IActionResult OrderUploadDocuments(OrderUploadDocumentsViewModel model)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");


            if (!ModelState.IsValid)
            {
                string SessionKey = "UOD" + UserNameFromSession;
                UserOrderDetails UODNews = session.GetComplexData(SessionKey);
                if (UODNews != null)
                {
                    model.DLs = UODNews.DLs;
                    model.TotalDocuments = UODNews.DLs.Count;

                }
                else
                {
                    model.DLs = new List<DocumentLists>();
                    model.TotalDocuments = 0;
                }
                return View(model);
            }
            else
            {
                try
                {
                    using (var db = _DbContextFactory.CreateDbContext())
                    {
                        string SessionKey = "UOD" + UserNameFromSession;

                        string FileNameforDB = "";
                        if (Request.Form.Files.Count > 0)
                        {
                            var file = Request.Form.Files[0];
                            var folderName = Path.Combine("Repository", "Orders", SessionKey, "Document");
                            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                            bool exists = System.IO.Directory.Exists(pathToSave);
                            if (!exists)
                                System.IO.Directory.CreateDirectory(pathToSave);

                            if (file.Length > 0)
                            {
                                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                                FileNameforDB = fileName;
                                string fileType = fileName.Split('.')[1];

                                if (fileType == "pdf")
                                {
                                    UserOrderDetails UOD = session.GetComplexData(SessionKey);
                                    if (UOD != null && UOD.DLs != null)
                                    {
                                        foreach (DocumentLists DL in UOD.DLs)
                                        {
                                            if (DL.FileName == fileName)
                                            {
                                                ViewBag.ResultMessage = "File name already exists!";
                                                return RedirectToAction("OrderUploadDocuments", new { Success = false, Details = ViewBag.ResultMessage });
                                            }
                                        }
                                    }

                                    var fullPath = Path.Combine(pathToSave, fileName);
                                    var dbPath = Path.Combine(folderName, fileName);
                                    using (var stream = new FileStream(fullPath, FileMode.Create))
                                    {
                                        file.CopyTo(stream);
                                    }

                                    if (UOD == null)
                                    {
                                        UOD = new UserOrderDetails();
                                        UOD.UserID = UserNameFromSession;
                                        UOD.DLs = new List<DocumentLists>();

                                        DocumentLists DL = new DocumentLists();
                                        DL.FileName = FileNameforDB;
                                        DL.FilePath = fullPath;
                                        DL.Sequence = 1;
                                        UOD.DLs.Add(DL);
                                        UOD.ClearedActionAppliedDLs = false;
                                        session.SetComplexData(SessionKey, UOD);
                                    }
                                    else
                                    {
                                        UserOrderDetails UODNews = session.GetComplexData(SessionKey);

                                        if (UODNews.DLs != null && UODNews.DLs.Count > 0)
                                        {
                                            DocumentLists DL = new DocumentLists();
                                            DL.FileName = FileNameforDB;
                                            DL.FilePath = fullPath;
                                            DL.Sequence = UOD.DLs.Last().Sequence + 1; ;
                                            UOD.DLs.Add(DL);
                                            session.SetComplexData(SessionKey, UOD);
                                        }
                                        else
                                        {
                                            UOD.DLs = new List<DocumentLists>();
                                            DocumentLists DL = new DocumentLists();
                                            DL.FileName = FileNameforDB;
                                            DL.FilePath = fullPath;
                                            DL.Sequence = 1; ;
                                            UOD.DLs.Add(DL);
                                            UOD.ClearedActionAppliedDLs = false;
                                            session.SetComplexData(SessionKey, UOD);
                                        }
                                    }

                                    model.TotalDocuments = UOD.DLs.Count;
                                    ViewBag.ResultMessage = "Document uploaded successfully!";
                                    return RedirectToAction("OrderUploadDocuments", new { Success = true, Details = ViewBag.ResultMessage });
                                }
                                else
                                {
                                    ViewBag.ResultMessage = "Only .pdf extentions allowed for upload!";
                                    return RedirectToAction("OrderUploadDocuments", new { Success = false, Details = ViewBag.ResultMessage });
                                }
                            }
                            else
                            {
                                ViewBag.ResultMessage = "Please select a file for upload!";
                                return RedirectToAction("OrderUploadDocuments", new { Success = false, Details = ViewBag.ResultMessage });
                            }
                        }
                        else
                        {
                            ViewBag.ResultMessage = "Please select a file for upload!";
                            return RedirectToAction("OrderUploadDocuments", new { Success = false, Details = ViewBag.ResultMessage });
                        }

                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ResultMessage = "Error! " + ex.Message;
                    string SessionKey = "UOD" + UserNameFromSession;
                    UserOrderDetails UODNews = session.GetComplexData(SessionKey);
                    if (UODNews != null)
                    {
                        model.DLs = UODNews.DLs;
                        model.TotalDocuments = UODNews.DLs.Count;

                    }
                    else
                    {
                        model.DLs = new List<DocumentLists>();
                        model.TotalDocuments = 0;
                    }
                }

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult UpSequenceDocument(int id)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            bool IsSuccessful = false;
            string DetailMessage = "";
            try
            {
                string SessionKey = "UOD" + UserNameFromSession;
                UserOrderDetails UODNews = session.GetComplexData(SessionKey);
                if (id == 1)
                {

                }
                else
                {

                    DocumentLists DLTobeMovedUp = UODNews.DLs.First(d => d.Sequence == id);
                    DocumentLists DLTobeRemoved = UODNews.DLs.First(d => d.Sequence == id);

                    DocumentLists DLTobeMovedDown = UODNews.DLs.First(d => d.Sequence == id - 1);
                    DocumentLists DLTobeRemovedDown = UODNews.DLs.First(d => d.Sequence == id - 1);

                    DLTobeMovedUp.Sequence = DLTobeMovedUp.Sequence - 1;
                    UODNews.DLs.Remove(DLTobeRemoved);
                    UODNews.DLs.Add(DLTobeMovedUp);


                    DLTobeMovedDown.Sequence = DLTobeMovedDown.Sequence + 1; ;
                    UODNews.DLs.Remove(DLTobeRemovedDown);
                    UODNews.DLs.Add(DLTobeMovedDown);

                    UODNews.DLs = UODNews.DLs.OrderBy(d => d.Sequence).ToList();

                    session.SetComplexData(SessionKey, UODNews);

                    IsSuccessful = true;
                    DetailMessage = "Document sequence updated.";
                }
            }
            catch (Exception ex)
            {
                IsSuccessful = false;
                DetailMessage = ex.Message;
            }

            return RedirectToAction("OrderUploadDocuments", new { Success = IsSuccessful, Details = DetailMessage });
        }

        [HttpGet]
        public IActionResult DownSequenceDocument(int id)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            bool IsSuccessful = false;
            string DetailMessage = "";
            try
            {
                string SessionKey = "UOD" + UserNameFromSession;
                UserOrderDetails UODNews = session.GetComplexData(SessionKey);
                if (id == UODNews.DLs.Last().Sequence)
                {

                }
                else
                {

                    DocumentLists DLTobeMovedDown = UODNews.DLs.First(d => d.Sequence == id);
                    DocumentLists DLTobeRemovedDown = UODNews.DLs.First(d => d.Sequence == id);

                    DocumentLists DLTobeMovedUp = UODNews.DLs.First(d => d.Sequence == id + 1);
                    DocumentLists DLTobeRemoved = UODNews.DLs.First(d => d.Sequence == id + 1);

                    DLTobeMovedDown.Sequence = DLTobeMovedDown.Sequence + 1;
                    UODNews.DLs.Remove(DLTobeRemovedDown);
                    UODNews.DLs.Add(DLTobeMovedDown);

                    DLTobeMovedUp.Sequence = DLTobeMovedUp.Sequence - 1;
                    UODNews.DLs.Remove(DLTobeRemoved);
                    UODNews.DLs.Add(DLTobeMovedUp);

                    UODNews.DLs = UODNews.DLs.OrderBy(d => d.Sequence).ToList();

                    session.SetComplexData(SessionKey, UODNews);

                    IsSuccessful = true;
                    DetailMessage = "Document sequence updated.";
                }
            }
            catch (Exception ex)
            {
                IsSuccessful = false;
                DetailMessage = ex.Message;
            }

            return RedirectToAction("OrderUploadDocuments", new { Success = IsSuccessful, Details = DetailMessage });
        }

        [HttpGet]
        public IActionResult DeleteDocument(int id)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            bool IsSuccessful = false;
            string DetailMessage = "";
            try
            {
                string SessionKey = "UOD" + UserNameFromSession;
                UserOrderDetails UODNews = session.GetComplexData(SessionKey);
                DocumentLists DLTobeDeleted = UODNews.DLs.FirstOrDefault(d => d.Sequence == id);
                if (DLTobeDeleted != null)
                {
                    string path = DLTobeDeleted.FilePath;
                    FileInfo file = new FileInfo(path);
                    if (file.Exists)//check file exsit or not  
                    {
                        file.Delete();
                    }

                    UODNews.DLs.Remove(DLTobeDeleted);
                    session.SetComplexData(SessionKey, UODNews);
                    DetailMessage = "Document removed.";
                    IsSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                IsSuccessful = false;
                DetailMessage = ex.Message;
            }

            return RedirectToAction("OrderUploadDocuments", new { Success = IsSuccessful, Details = DetailMessage });
        }

        public IActionResult OrderSelectEnvelope(string Success, string Details )
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            OrderSelectEnvelopViewModel model = new OrderSelectEnvelopViewModel();
            if (!String.IsNullOrEmpty(Success))
            {
                if (Convert.ToBoolean(Success))
                {
                    model.Response = new ResponseMessage() { User = User.Identity.Name, IsSuccessful = true, MessageDateTime = DateTime.UtcNow, Message = "Operation was successful.", MessageType = "success", Details = Details };
                }
                else
                {
                    model.Response = new ResponseMessage() { User = User.Identity.Name, IsSuccessful = false, MessageDateTime = DateTime.UtcNow, Message = "Operation was unsuccessful.", MessageType = "error", Details = Details };
                }
            }

            model.PageTitle = "Select Address";

            using (var db = _DbContextFactory.CreateDbContext())
            {
                model.Envelops = db.Contents.Where(d => d.MainType == "Envelope").OrderBy(d => Convert.ToString(d.MainContent)).ToList();
            }
            return View(model);

        }

        [HttpGet]
        public IActionResult EnvelopeSelected(int id)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            bool IsSuccessful = true;
            string DetailMessage = "";
            try
            {
                string SessionKey = "UOD" + UserNameFromSession;
                UserOrderDetails UODNews = session.GetComplexData(SessionKey);

                if (UODNews == null)
                {
                    UODNews = new UserOrderDetails();
                    UODNews.UserID = UserNameFromSession;
                    UODNews.DLs = new List<DocumentLists>();
                    UODNews.EnvelopeSelected = new Contents();
                    UODNews.ClearedActionAppliedEnvelope = false;
                    session.SetComplexData(SessionKey, UODNews);
                }

                UODNews.EnvelopeSelected = new Contents();
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    Contents x = db.Contents.FirstOrDefault(d => d.lngID == id);
                    if (x != null)
                    {
                        UODNews.EnvelopeSelected = x;
                    }
                }
                session.SetComplexData(SessionKey, UODNews);
            }
            catch (Exception ex)
            {
                IsSuccessful = false;
                DetailMessage = ex.Message;
            }

            return RedirectToAction("Index", new { Success = IsSuccessful, Details = DetailMessage });
        }

        public IActionResult OrderUploadMailing(string Success, string Details)
        {
            OrderUploadMailingViewModel model = new OrderUploadMailingViewModel();
            if (!String.IsNullOrEmpty(Success))
            {
                if (Convert.ToBoolean(Success))
                {
                    model.Response = new ResponseMessage() { User = User.Identity.Name, IsSuccessful = true, MessageDateTime = DateTime.UtcNow, Message = "Operation was successful.", MessageType = "success", Details = Details };
                }
                else
                {
                    model.Response = new ResponseMessage() { User = User.Identity.Name, IsSuccessful = false, MessageDateTime = DateTime.UtcNow, Message = "Operation was unsuccessful.", MessageType = "error", Details = Details };
                }
            }

            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            model.PageTitle = "Upload Mailing List";

            string SessionKey = "UOD" + UserNameFromSession;
            UserOrderDetails UODNews = session.GetComplexData(SessionKey);
            if (UODNews != null)
            {
                model.MLs = UODNews.MLs;
                model.TotalDocuments = UODNews.DLs != null ? UODNews.DLs.Count : 0;

            }
            else
            {
                model.MLs = new List<DocumentLists>();
                model.TotalDocuments = 0;
            }

            return View(model);
        }

        [HttpPost, DisableRequestSizeLimit]
        public IActionResult OrderUploadMailing(OrderUploadMailingViewModel model)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");


            if (!ModelState.IsValid)
            {
                string SessionKey = "UOD" + UserNameFromSession;
                UserOrderDetails UODNews = session.GetComplexData(SessionKey);
                if (UODNews != null)
                {
                    model.MLs = UODNews.MLs;
                    model.TotalDocuments = UODNews.DLs != null ? UODNews.DLs.Count : 0;

                }
                else
                {
                    model.MLs = new List<DocumentLists>();
                    model.TotalDocuments = 0;
                }
                return View(model);
            }
            else
            {
                try
                {
                    using (var db = _DbContextFactory.CreateDbContext())
                    {
                        string SessionKey = "UOD" + UserNameFromSession;

                        string FileNameforDB = "";
                        if (Request.Form.Files.Count > 0)
                        {
                            var file = Request.Form.Files[0];
                            var folderName = Path.Combine("Repository", "Orders", SessionKey, "MailingList");
                            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                            bool exists = System.IO.Directory.Exists(pathToSave);
                            if (!exists)
                                System.IO.Directory.CreateDirectory(pathToSave);

                            if (file.Length > 0)
                            {
                                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                                FileNameforDB = fileName;
                                string fileType = fileName.Split('.')[1];

                                if (fileType == "xls" || fileType == "xlsx" || fileType == "csv" || fileType == "zip")
                                {
                                    UserOrderDetails UOD = session.GetComplexData(SessionKey);
                                    if (UOD != null && UOD.MLs != null)
                                    {
                                        foreach (DocumentLists DL in UOD.MLs)
                                        {
                                            if (DL.FileName == fileName)
                                            {
                                                ViewBag.ResultMessage = "File name already exists!";
                                                return RedirectToAction("OrderUploadMailing", new { Success = false, Details = ViewBag.ResultMessage });
                                            }
                                        }
                                    }

                                    var fullPath = Path.Combine(pathToSave, fileName);
                                    var dbPath = Path.Combine(folderName, fileName);
                                    using (var stream = new FileStream(fullPath, FileMode.Create))
                                    {
                                        file.CopyTo(stream);
                                    }

                                    if (UOD == null)
                                    {
                                        UOD = new UserOrderDetails();
                                        UOD.UserID = UserNameFromSession;
                                        UOD.MLs = new List<DocumentLists>();

                                        DocumentLists DL = new DocumentLists();
                                        DL.FileName = FileNameforDB;
                                        DL.FilePath = fullPath;
                                        DL.Sequence = 1;
                                        UOD.MLs.Add(DL);
                                        UOD.ClearedActionAppliedMLs = false;
                                        session.SetComplexData(SessionKey, UOD);
                                    }
                                    else
                                    {
                                        UserOrderDetails UODNews = session.GetComplexData(SessionKey);
                                        if (UOD.MLs != null && UOD.MLs.Count > 0)
                                        {
                                            DocumentLists DL = new DocumentLists();
                                            DL.FileName = FileNameforDB;
                                            DL.FilePath = fullPath;
                                            DL.Sequence = UOD.MLs.Last().Sequence + 1; ;
                                            UOD.MLs.Add(DL);
                                            session.SetComplexData(SessionKey, UOD);
                                        }
                                        else
                                        {
                                            UOD.MLs = new List<DocumentLists>();
                                            DocumentLists DL = new DocumentLists();
                                            DL.FileName = FileNameforDB;
                                            DL.FilePath = fullPath;
                                            DL.Sequence = 1; ;
                                            UOD.MLs.Add(DL);
                                            UOD.ClearedActionAppliedMLs = false;
                                            session.SetComplexData(SessionKey, UOD);
                                        }
                                    }

                                    model.TotalDocuments = UOD.MLs.Count;
                                    ViewBag.ResultMessage = "Mailing list uploaded successfully!";
                                    return RedirectToAction("OrderUploadMailing", new { Success = true, Details = ViewBag.ResultMessage });
                                }
                                else
                                {
                                    ViewBag.ResultMessage = "Only .xls, .xlsx, .csv, .zip extentions allowed for upload";
                                    return RedirectToAction("OrderUploadMailing", new { Success = false, Details = ViewBag.ResultMessage });
                                }
                            }
                            else
                            {
                                ViewBag.ResultMessage = "Please select a file for upload";
                                return RedirectToAction("OrderUploadMailing", new { Success = false, Details = ViewBag.ResultMessage });
                            }
                        }
                        else
                        {
                            ViewBag.ResultMessage = "Please select a file for upload";
                            return RedirectToAction("OrderUploadMailing", new { Success = false, Details = ViewBag.ResultMessage });
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ResultMessage = ex.Message;
                    return RedirectToAction("OrderUploadMailing", new { Success = false, Details = ViewBag.ResultMessage });
                }

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult DeleteMailing(int id)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            bool IsSuccessful = false;
            string DetailMessage = "";
            try
            {
                string SessionKey = "UOD" + UserNameFromSession;
                UserOrderDetails UODNews = session.GetComplexData(SessionKey);
                DocumentLists MLTobeDeleted = UODNews.MLs.FirstOrDefault(d => d.Sequence == id);
                if (MLTobeDeleted != null)
                {
                    string path = MLTobeDeleted.FilePath;
                    FileInfo file = new FileInfo(path);
                    if (file.Exists)//check file exsit or not  
                    {
                        file.Delete();
                    }

                    UODNews.MLs.Remove(MLTobeDeleted);
                    session.SetComplexData(SessionKey, UODNews);
                    DetailMessage = "Mailing list removed.";
                    IsSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                IsSuccessful = false;
                DetailMessage = ex.Message;
            }

            return RedirectToAction("OrderUploadMailing", new { Success = IsSuccessful, Details = DetailMessage });
        }

        public ActionResult GetShippingDetails(string maincontent)
        {
            using (var db = _DbContextFactory.CreateDbContext())
            {
                if (!String.IsNullOrEmpty(maincontent))
                {
                    int lngID = Convert.ToInt32(maincontent);
                    Contents obj = db.Contents.First(d => d.lngID == lngID && d.MainType == "ShipToCC");
                    return Json(obj);
                }
                else
                {
                    Contents obj = new Contents();
                    return Json(obj);
                }
            }
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

        public ActionResult DownloadFile(string filePath)
        {
            string fullName = Directory.GetCurrentDirectory() + "/" + filePath;

            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filePath);
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }

    }
}
