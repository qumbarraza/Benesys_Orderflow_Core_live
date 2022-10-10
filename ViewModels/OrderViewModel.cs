using TBENesys_Orderflow_Core.DataModel;
using TBENesys_Orderflow_Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TBENesys_Orderflow_Core.ViewModels
{

    public class OrderViewModel
    {
        public string TermsAccepted { set; get; }
        public bool MLPresent { set; get; }
        public bool ClearedActionAppliedDLs { set; get; }
        public bool ClearedActionAppliedMLs { set; get; }
        public bool ClearedActionAppliedEnvelope { set; get; }

        public string PageTitle { set; get; }

        public ResponseMessage Response { set; get; }

        public List<DocumentLists> DLs { set; get; }
        public List<DocumentLists> MLs { set; get; }

        public Contents SelectedEnvelope { set; get; }

        public bool OrderDetailsFilled { set; get; }

        public bool JobDetailsFilled { set; get; }

        
        [Display(Name = "Job Name")]
        [StringLength(35)]
        public string JobName { get; set; }
        [Display(Name = "Job Prints")]
        public string JobPrints { get; set; }
        [Display(Name = "Envelope Size")]
        public string EnvelopeSize { get; set; }

        [Display(Name = "Print quantity")]
        public int Printquantity { get; set; }
        [Display(Name = "Order needed by")]
        public DateTime OrderNeededBy { get; set; }
        [Display(Name = "Mail Drop Date")]
        public DateTime MailDropDate { get; set; }
        [Display(Name = "RUSH Job")]
        public string RushJob { get; set; }
        [Display(Name = "Special Instructions")]
        public string SpecialInstructions { get; set; }
    }

    public class OrderCheckOutViewModel
    {
        [Required]
        [Display(Name = "Requestor Name*")]
        public string RequestorName { get; set; }
        [Required]
        [Display(Name = "Requestor Email*")]
        public string RequestorEmail { get; set; }
        [Required]
        [Display(Name = "Requestor Phone*")]
        public string RequestorPhone { get; set; }
        
        [Display(Name = "Fringe")]
        public string Fringe { get; set; }
        [Required]
        [Display(Name = "Charge Order To*")]
        public string ChargeOrderTo { get; set; }
        public List<Itemlist> AllChargeOrderTos { get; set; }
        [Required]
        [Display(Name = "Approval Manager*")]
        public string ApprovalManager { get; set; }
        public List<Itemlist> AllApprovalManagers { get; set; }
        
        public string PageTitle { set; get; }

        public ResponseMessage Response { set; get; }

    }

    public class OrderPreviewViewModel
    {
        public string PageTitle { set; get; }
        public ResponseMessage Response { set; get; }

        public UserOrderDetails UODs { set; get; }


        [Display(Name = "Additional Emails")]
        public string AdditionalEmail { get; set; }
        public List<SelectListItem> AllEmails { get; set; }
        public string[] SelectedEmails { get; set; }
        public string ViewSelectedEmails { get; set; }
    }

    public class OrderReceiptViewModel
    {
        public string PageTitle { set; get; }
        public ResponseMessage Response { set; get; }

        public string OrderReceiptDetails { set; get; }

        public int OrderNumber { set; get; }

    }

    public class OrderUploadDocumentsViewModel
    {
        public List<DocumentLists> DLs { set; get; }
        public int TotalDocuments { set; get; }

        public string PageTitle { set; get; }

        public ResponseMessage Response { set; get; }

    }
    public class UserOrderDetails
    {
        public bool ClearedActionAppliedDLs { set; get; }
        public bool ClearedActionAppliedMLs { set; get; }
        public bool ClearedActionAppliedEnvelope { set; get; }

        public string UserID { set; get; }
        public List<DocumentLists> DLs { set; get; }
        public Contents EnvelopeSelected { set; get; }
        public List<DocumentLists> MLs { set; get; }

        public bool JobDetailsFilled { set; get; }
        public string JobName { get; set; }
        public string JobPrints { get; set; }
        public string EnvelopeSize { get; set; }
        public int Printquantity { get; set; }
        public DateTime OrderNeededBy { get; set; }
        public DateTime MailDropDate { get; set; }
        public string RushJob { get; set; }
        public string SpecialInstructions { get; set; }

        public bool OrderContactShippingFilled { get; set; }
        public string RequestorName { get; set; }
        public string RequestorEmail { get; set; }
        public string RequestorPhone { get; set; }
        public string Fringe { get; set; }
        public string ChargeOrderTo { get; set; }
        public string ApprovalManager { get; set; }
        public string ApprovalManagerName { get; set; }
        public string ShipToLocation { get; set; }
        public string Company { get; set; }
        public string Attention { get; set; }
        public string Street { get; set; }
        public string Suite { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
    public class DocumentLists
    {
        public int Sequence { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }

    public class OrderSelectEnvelopViewModel
    {
        public List<Contents> Envelops { set; get; }

        public string PageTitle { set; get; }

        public ResponseMessage Response { set; get; }

    }

    public class OrderUploadMailingViewModel
    {
        public List<DocumentLists> MLs { set; get; }
        public int TotalDocuments { set; get; }

        public string PageTitle { set; get; }

        public ResponseMessage Response { set; get; }

    }
}
