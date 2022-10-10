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
    public class LoginViewModel
    {
        public string username { set; get; }

        public string pass { set; get; }

    }

    public class ForgotViewModel
    {
        public string Email { set; get; }
    }

    public class RecoverPasswordViewModel
    {
        public string UserID { set; get; }
        public string NewPassword { set; get; }
        public string ConfirmPassword { set; get; }
    }

    public class AccountRolesListingViewModel
    {
        public AccountRolesListingViewModel()
        {
            PageSize = 10;
            Records = new List<AccountRole>();
        }
        public string PageTitle { set; get; }

        public string UserRole { set; get; }

        public string Sort { set; get; }
        public string Filter { set; get; }
        public DateTime? DateFilter { set; get; }
        public DateTime? DateFilter2 { set; get; }

        public int PageSize { set; get; }
        public int PageNumber { set; get; }

        public string SortTitle { set; get; }

        public string SortClassTitle { set; get; }

        public ResponseMessage Response { set; get; }
        public List<AccountRole> Records { set; get; }
        public List<AccountRole> RecordsPaged { set; get; }

    }

    public class AccountRoleHandlerViewModel
    {
        [Display(Name = "ID")]
        public int? ID { set; get; }
        [Display(Name = "Title")]
        public string Title { set; get; }

    }

    public class AccountListingViewModel
    {
        public string PageTitle { set; get; }

        public string UserRole { set; get; }

        public ResponseMessage Response { set; get; }
        public List<Account> Records { set; get; }
        public List<Account> RecordsPaged { set; get; }

    }

    public class AccountHandlerViewModel
    {
        [Display(Name = "ID")]
        public int? lngID { get; set; }
        [Display(Name = "Access Mask")]
        public int AccessMask { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "User ID")]
        public string UserID { get; set; }
        [Display(Name = "Password")]
        public string Pwd { get; set; }
        [Display(Name = "Active")]
        public bool Active { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Location")]
        public string Location { get; set; }
        public List<Itemlist> AllLocations { get; set; }

        [Display(Name = "Locked")]
        public bool LOCKED { get; set; }
        [Display(Name = "Last Password Change")]
        public DateTime? LastPwdChange { get; set; }

        [Display(Name = "Account Role")]
        public string AccountRoleID { get; set; }
        public List<SelectListItem> AllAcountRoles { get; set; }
        public string[] SelectedAccountRoles { get; set; }
        public string ViewSelectedAccountRoles { get; set; }

    }

    public class MyAccountViewModel
    {
        public ResponseMessage Response { set; get; }

        [Display(Name = "ID")]
        public int? lngID { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "User ID")]
        public string UserID { get; set; }
        [Display(Name = " Current Password")]
        public string Pwd { get; set; }
        [Display(Name = "New Password")]
        public string NewPwd { get; set; }
        [Display(Name = "Confirm Password")]
        public string ConfirmPwd { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Role(s)")]
        public string UserRole { set; get; }
    }

    public class LetterheadListingViewModel
    {
        public string PageTitle { set; get; }

        public string UserRole { set; get; }

        public ResponseMessage Response { set; get; }
        public List<Letterhead> Records { set; get; }
    }
    public class LetterheadHandlerViewModel
    {
        [Display(Name = "ID")]
        public int? lngID { get; set; }
        [Display(Name = "Name")]
        public string FileName { get; set; }
        [Display(Name = "File")]
        public string FilePath { get; set; }
    }


    public class Itemlist
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }

    public class EnvelopesListingViewModel
    {
        public EnvelopesListingViewModel()
        {
        }
        public string PageTitle { set; get; }

        public string UserRole { set; get; }

        public ResponseMessage Response { set; get; }
        public List<Contents> Records { set; get; }

    }

    public class EnvelopeHandlerViewModel
    {
        [Display(Name = "ID")]
        public int? lngID { set; get; }
        [Display(Name = "Cost Center")]
        public string MainContent { set; get; }
        [Display(Name = "Group Name 1")]
        public string SubContent2 { set; get; }
        [Display(Name = "Group Name 2")]
        public string SubContent3 { set; get; }
        [Display(Name = "Group Name 3")]
        public string SubContent4 { set; get; }
        [Display(Name = "Address")]
        public string SubContent5 { set; get; }
        [Display(Name = "City, State Zip")]
        public string SubContent6 { set; get; }
    }


    public class ChargeToCCsListingViewModel
    {
        public string PageTitle { set; get; }

        public string UserRole { set; get; }

        public ResponseMessage Response { set; get; }
        public List<Contents> Records { set; get; }

    }

    public class ChargeToCCsHandlerViewModel
    {
        [Display(Name = "ID")]
        public int? lngID { set; get; }
        [Display(Name = "Cost Center")]
        public string MainContent { set; get; }
        [Display(Name = "Active")]
        public bool SubContent2 { set; get; }
    }

    public class ShipToListingViewModel
    {
        public string PageTitle { set; get; }

        public string UserRole { set; get; }

        public ResponseMessage Response { set; get; }
        public List<Contents> Records { set; get; }

    }

    public class ShipToHandlerViewModel
    {
        [Display(Name = "ID")]
        public int? lngID { set; get; }
        [Display(Name = "ShipToLocation")]
        public string SubContent1 { set; get; }
        [Display(Name = "Company")]
        public string SubContent2 { set; get; }
        [Display(Name = "Address 1")]
        public string SubContent3 { set; get; }
        [Display(Name = "Address 2")]
        public string SubContent4 { set; get; }
        [Display(Name = "City")]
        public string SubContent5 { set; get; }
        [Display(Name = "State")]
        public string SubContent6 { set; get; }
        [Display(Name = "Zip")]
        public string SubContent7 { set; get; }
        [Display(Name = "Active")]
        public bool SubContent8 { set; get; }
    }

    public class OrderListingViewModel
    {
        public string PageTitle { set; get; }

        public string UserRole { set; get; }

        public ResponseMessage Response { set; get; }
        public List<Order> Records { set; get; }
        public List<OrderCheckoutField> OCFRecords { set; get; }
        public string TermsAccepted { set; get; }

    }

    public class InvoiceList 
    {
        public int OrderID { set; get; }
        public string OrderDate { set; get; }
        public string DateMailed { set; get; }
        public string JobName { set; get; }
        public string Status { set; get; }
        public int? InvoiceID { set; get; }
        public string InvoiceDate { set; get; }
        public string ClientInvoiceUploadDate { set; get; }
        public string ClientInvoiceName { set; get; }
        public string ClientInvoiceDate { set; get; }
        public string InvoiceRecipient { set; get; }
        public string ClientPaymentDate { set; get; }

    }

    public class InvoiceListingViewModel
    {
        public string DTClientOrderNo { get; set; }
        public string DTClientChargeTo { get; set; }
        public string DTClientJobName { get; set; }
        public string DTVendorInvoiceNo { get; set; }
        public string DTVendorInvoiceDate { get; set; }

        public string PageTitle { set; get; }

        public string UserRole { set; get; }

        public ResponseMessage Response { set; get; }
        public List<InvoiceList> Records { set; get; }
        public List<OrderCheckoutField> OCFRecords { set; get; }

        [Display(Name = "ID")]
        public int? lngID { set; get; }
        [Display(Name = "Order ID")]
        public int orderID { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }
        [Display(Name = "VendorInvoiceNo")]

        public string vendorInvoiceNo { get; set; }
        [Display(Name = "Comment")]

        public string comment { get; set; }
        [Display(Name = "Date Bill Submitted")]

        public DateTime? datBillSubmitted { get; set; }
        [Display(Name = "Date Bill Revised")]

        public DateTime? datBillRevised { get; set; }
        [Display(Name = "Date Client Invoiced")]

        public DateTime? datClientInvoiced { get; set; }
        [Display(Name = "Date Client Invoice Uploaded")]

        public DateTime? datClientInvoiceUploaded { get; set; }
        [Display(Name = "Date Client Payment")]

        public DateTime? datClientPayment { get; set; }
        [Display(Name = "LastTouch")]

        public string lastTouch { get; set; }
        [Display(Name = "Client Invoice File")]

        public string clientInvoiceFile { get; set; }
        [Display(Name = "Vendor Invoice File")]

        public string vendorInvoiceFile { get; set; }
        [Display(Name = "Internal Invoice File")]

        public string internalInvoiceFile { get; set; }

        public Order InvoiceOrder { set; get; }
        public int ilInvoiceID { set; get; }

        public string ilStatusType { set; get; }

        public int ilOrderID { set; get; }
        public string il1linetype { set; get; }
        public int il1display { set; get; }
        public string il1name { set; get; }
        public string il1size { set; get; }
        public double il1qty { set; get; }
        public string il1UOM { set; get; }
        public double il1unitCost { set; get; }
        public double il1extCost { set; get; }
        public double il1unitSell { set; get; }
        public double il1extSell { set; get; }
        public double il1minExtSell { set; get; }

        public string il2linetype { set; get; }
        public int il2display { set; get; }
        public string il2name { set; get; }
        public string il2size { set; get; }
        public double il2qty { set; get; }
        public string il2UOM { set; get; }
        public double il2unitCost { set; get; }
        public double il2extCost { set; get; }
        public double il2unitSell { set; get; }
        public double il2extSell { set; get; }
        public double il2minExtSell { set; get; }

        public string il3linetype { set; get; }
        public int il3display { set; get; }
        public string il3name { set; get; }
        public string il3size { set; get; }
        public double il3qty { set; get; }
        public string il3UOM { set; get; }
        public double il3unitCost { set; get; }
        public double il3extCost { set; get; }
        public double il3unitSell { set; get; }
        public double il3extSell { set; get; }
        public double il3minExtSell { set; get; }

        public string il4linetype { set; get; }
        public int il4display { set; get; }
        public string il4name { set; get; }
        public string il4size { set; get; }
        public double il4qty { set; get; }
        public string il4UOM { set; get; }
        public double il4unitCost { set; get; }
        public double il4extCost { set; get; }
        public double il4unitSell { set; get; }
        public double il4extSell { set; get; }
        public double il4minExtSell { set; get; }

        public string il5linetype { set; get; }
        public int il5display { set; get; }
        public string il5name { set; get; }
        public string il5size { set; get; }
        public double il5qty { set; get; }
        public string il5UOM { set; get; }
        public double il5unitCost { set; get; }
        public double il5extCost { set; get; }
        public double il5unitSell { set; get; }
        public double il5extSell { set; get; }
        public double il5minExtSell { set; get; }

        public string il6linetype { set; get; }
        public int il6display { set; get; }
        public string il6name { set; get; }
        public string il6size { set; get; }
        public double il6qty { set; get; }
        public string il6UOM { set; get; }
        public double il6unitCost { set; get; }
        public double il6extCost { set; get; }
        public double il6unitSell { set; get; }
        public double il6extSell { set; get; }
        public double il6minExtSell { set; get; }

        public string il7linetype { set; get; }
        public int il7display { set; get; }
        public string il7name { set; get; }
        public string il7size { set; get; }
        public double il7qty { set; get; }
        public string il7UOM { set; get; }
        public double il7unitCost { set; get; }
        public double il7extCost { set; get; }
        public double il7unitSell { set; get; }
        public double il7extSell { set; get; }
        public double il7minExtSell { set; get; }

        public string il8linetype { set; get; }
        public int il8display { set; get; }
        public string il8name { set; get; }
        public string il8size { set; get; }
        public double il8qty { set; get; }
        public string il8UOM { set; get; }
        public double il8unitCost { set; get; }
        public double il8extCost { set; get; }
        public double il8unitSell { set; get; }
        public double il8extSell { set; get; }
        public double il8minExtSell { set; get; }

        public string il9linetype { set; get; }
        public int il9display { set; get; }
        public string il9name { set; get; }
        public string il9size { set; get; }
        public double il9qty { set; get; }
        public string il9UOM { set; get; }
        public double il9unitCost { set; get; }
        public double il9extCost { set; get; }
        public double il9unitSell { set; get; }
        public double il9extSell { set; get; }
        public double il9minExtSell { set; get; }

        public string il10linetype { set; get; }
        public int il10display { set; get; }
        public string il10name { set; get; }
        public string il10size { set; get; }
        public double il10qty { set; get; }
        public string il10UOM { set; get; }
        public double il10unitCost { set; get; }
        public double il10extCost { set; get; }
        public double il10unitSell { set; get; }
        public double il10extSell { set; get; }
        public double il10minExtSell { set; get; }

        public string il11linetype { set; get; }
        public int il11display { set; get; }
        public string il11name { set; get; }
        public string il11size { set; get; }
        public double il11qty { set; get; }
        public string il11UOM { set; get; }
        public double il11unitCost { set; get; }
        public double il11extCost { set; get; }
        public double il11unitSell { set; get; }
        public double il11extSell { set; get; }
        public double il11minExtSell { set; get; }

        public string il20linetype { set; get; }
        public int il20display { set; get; }
        public string il20name { set; get; }
        public string il20size { set; get; }
        public double il20qty { set; get; }
        public string il20UOM { set; get; }
        public double il20unitCost { set; get; }
        public double il20extCost { set; get; }
        public double il20unitSell { set; get; }
        public double il20extSell { set; get; }
        public double il20minExtSell { set; get; }

        public string il21linetype { set; get; }
        public int il21display { set; get; }
        public string il21name { set; get; }
        public string il21size { set; get; }
        public double il21qty { set; get; }
        public string il21UOM { set; get; }
        public double il21unitCost { set; get; }
        public double il21extCost { set; get; }
        public double il21unitSell { set; get; }
        public double il21extSell { set; get; }
        public double il21minExtSell { set; get; }

        public string il22linetype { set; get; }
        public int il22display { set; get; }
        public string il22name { set; get; }
        public string il22size { set; get; }
        public double il22qty { set; get; }
        public string il22UOM { set; get; }
        public double il22unitCost { set; get; }
        public double il22extCost { set; get; }
        public double il22unitSell { set; get; }
        public double il22extSell { set; get; }
        public double il22minExtSell { set; get; }

        public string il23linetype { set; get; }
        public int il23display { set; get; }
        public string il23name { set; get; }
        public string il23size { set; get; }
        public double il23qty { set; get; }
        public string il23UOM { set; get; }
        public double il23unitCost { set; get; }
        public double il23extCost { set; get; }
        public double il23unitSell { set; get; }
        public double il23extSell { set; get; }
        public double il23minExtSell { set; get; }

        public string il24linetype { set; get; }
        public int il24display { set; get; }
        public string il24name { set; get; }
        public string il24size { set; get; }
        public double il24qty { set; get; }
        public string il24UOM { set; get; }
        public double il24unitCost { set; get; }
        public double il24extCost { set; get; }
        public double il24unitSell { set; get; }
        public double il24extSell { set; get; }
        public double il24minExtSell { set; get; }

        public string il25linetype { set; get; }
        public int il25display { set; get; }
        public string il25name { set; get; }
        public string il25size { set; get; }
        public double il25qty { set; get; }
        public string il25UOM { set; get; }
        public double il25unitCost { set; get; }
        public double il25extCost { set; get; }
        public double il25unitSell { set; get; }
        public double il25extSell { set; get; }
        public double il25minExtSell { set; get; }

        public string il26linetype { set; get; }
        public int il26display { set; get; }
        public string il26name { set; get; }
        public string il26size { set; get; }
        public double il26qty { set; get; }
        public string il26UOM { set; get; }
        public double il26unitCost { set; get; }
        public double il26extCost { set; get; }
        public double il26unitSell { set; get; }
        public double il26extSell { set; get; }
        public double il26minExtSell { set; get; }

        public string il27linetype { set; get; }
        public int il27display { set; get; }
        public string il27name { set; get; }
        public string il27size { set; get; }
        public double il27qty { set; get; }
        public string il27UOM { set; get; }
        public double il27unitCost { set; get; }
        public double il27extCost { set; get; }
        public double il27unitSell { set; get; }
        public double il27extSell { set; get; }
        public double il27minExtSell { set; get; }

        public string il28linetype { set; get; }
        public int il28display { set; get; }
        public string il28name { set; get; }
        public string il28size { set; get; }
        public double il28qty { set; get; }
        public string il28UOM { set; get; }
        public double il28unitCost { set; get; }
        public double il28extCost { set; get; }
        public double il28unitSell { set; get; }
        public double il28extSell { set; get; }
        public double il28minExtSell { set; get; }

        public string il29linetype { set; get; }
        public int il29display { set; get; }
        public string il29name { set; get; }
        public string il29size { set; get; }
        public double il29qty { set; get; }
        public string il29UOM { set; get; }
        public double il29unitCost { set; get; }
        public double il29extCost { set; get; }
        public double il29unitSell { set; get; }
        public double il29extSell { set; get; }
        public double il29minExtSell { set; get; }

        public string il30linetype { set; get; }
        public int il30display { set; get; }
        public string il30name { set; get; }
        public string il30size { set; get; }
        public double il30qty { set; get; }
        public string il30UOM { set; get; }
        public double il30unitCost { set; get; }
        public double il30extCost { set; get; }
        public double il30unitSell { set; get; }
        public double il30extSell { set; get; }
        public double il30minExtSell { set; get; }

        public string il31linetype { set; get; }
        public int il31display { set; get; }
        public string il31name { set; get; }
        public string il31size { set; get; }
        public double il31qty { set; get; }
        public string il31UOM { set; get; }
        public double il31unitCost { set; get; }
        public double il31extCost { set; get; }
        public double il31unitSell { set; get; }
        public double il31extSell { set; get; }
        public double il31minExtSell { set; get; }

        public string il32linetype { set; get; }
        public int il32display { set; get; }
        public string il32name { set; get; }
        public string il32size { set; get; }
        public double il32qty { set; get; }
        public string il32UOM { set; get; }
        public double il32unitCost { set; get; }
        public double il32extCost { set; get; }
        public double il32unitSell { set; get; }
        public double il32extSell { set; get; }
        public double il32minExtSell { set; get; }

        public string il33linetype { set; get; }
        public int il33display { set; get; }
        public string il33name { set; get; }
        public string il33size { set; get; }
        public double il33qty { set; get; }
        public string il33UOM { set; get; }
        public double il33unitCost { set; get; }
        public double il33extCost { set; get; }
        public double il33unitSell { set; get; }
        public double il33extSell { set; get; }
        public double il33minExtSell { set; get; }

        public string il34linetype { set; get; }
        public int il34display { set; get; }
        public string il34name { set; get; }
        public string il34size { set; get; }
        public double il34qty { set; get; }
        public string il34UOM { set; get; }
        public double il34unitCost { set; get; }
        public double il34extCost { set; get; }
        public double il34unitSell { set; get; }
        public double il34extSell { set; get; }
        public double il34minExtSell { set; get; }

        public string il35linetype { set; get; }
        public int il35display { set; get; }
        public string il35name { set; get; }
        public string il35size { set; get; }
        public double il35qty { set; get; }
        public string il35UOM { set; get; }
        public double il35unitCost { set; get; }
        public double il35extCost { set; get; }
        public double il35unitSell { set; get; }
        public double il35extSell { set; get; }
        public double il35minExtSell { set; get; }

        public string il36linetype { set; get; }
        public int il36display { set; get; }
        public string il36name { set; get; }
        public string il36size { set; get; }
        public double il36qty { set; get; }
        public string il36UOM { set; get; }
        public double il36unitCost { set; get; }
        public double il36extCost { set; get; }
        public double il36unitSell { set; get; }
        public double il36extSell { set; get; }
        public double il36minExtSell { set; get; }

        public string il40linetype { set; get; }
        public int il40display { set; get; }
        public string il40name { set; get; }
        public string il40size { set; get; }
        public double il40qty { set; get; }
        public string il40UOM { set; get; }
        public double il40unitCost { set; get; }
        public double il40extCost { set; get; }
        public double il40unitSell { set; get; }
        public double il40extSell { set; get; }
        public double il40minExtSell { set; get; }

    }

    public class InvoiceHandlerViewModel
    {
        public string DTClientOrderNo { get; set; }
        public string DTClientChargeTo { get; set; }
        public string DTClientJobName { get; set; }
        public string DTVendorInvoiceNo { get; set; }
        public string DTVendorInvoiceDate { get; set; }

        [Display(Name = "ID")]
        public int? lngID { set; get; }
        [Display(Name = "Order ID")]
        public int orderID { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }
        [Display(Name = "VendorInvoiceNo")]

        public string vendorInvoiceNo { get; set; }
        [Display(Name = "Comment")]

        public string comment { get; set; }
        [Display(Name = "Date Bill Submitted")]

        public DateTime? datBillSubmitted { get; set; }
        [Display(Name = "Date Bill Revised")]

        public DateTime? datBillRevised { get; set; }
        [Display(Name = "Date Client Invoiced")]

        public DateTime? datClientInvoiced { get; set; }
        [Display(Name = "Date Client Invoice Uploaded")]

        public DateTime? datClientInvoiceUploaded { get; set; }
        [Display(Name = "Date Client Payment")]

        public DateTime? datClientPayment { get; set; }
        [Display(Name = "LastTouch")]

        public string lastTouch { get; set; }
        [Display(Name = "Client Invoice File")]

        public string clientInvoiceFile { get; set; }
        [Display(Name = "Vendor Invoice File")]

        public string vendorInvoiceFile { get; set; }
        [Display(Name = "Internal Invoice File")]

        public string internalInvoiceFile { get; set; }

        public Order InvoiceOrder { set; get; }
        public string ilStatusType { set; get; }

        public int ilOrderID { set; get; }
        public int ilInvoiceID { set; get; }
        public string il1linetype { set; get; }
        public int il1display { set; get; }
        public string il1name { set; get; }
        public string il1size { set; get; }
        public double il1qty { set; get; }
        public string il1UOM { set; get; }
        public double il1unitCost { set; get; }
        public double il1extCost { set; get; }
        public double il1unitSell { set; get; }
        public double il1extSell { set; get; }
        public double il1minExtSell { set; get; }

        public string il2linetype { set; get; }
        public int    il2display { set; get; }
        public string il2name { set; get; }
        public string il2size { set; get; }
        public double il2qty { set; get; }
        public string il2UOM { set; get; }
        public double il2unitCost { set; get; }
        public double il2extCost { set; get; }
        public double il2unitSell { set; get; }
        public double il2extSell { set; get; }
        public double il2minExtSell { set; get; }

        public string il3linetype { set; get; }
        public int    il3display { set; get; }
        public string il3name { set; get; }
        public string il3size { set; get; }
        public double il3qty { set; get; }
        public string il3UOM { set; get; }
        public double il3unitCost { set; get; }
        public double il3extCost { set; get; }
        public double il3unitSell { set; get; }
        public double il3extSell { set; get; }
        public double il3minExtSell { set; get; }

        public string il4linetype { set; get; }
        public int    il4display { set; get; }
        public string il4name { set; get; }
        public string il4size { set; get; }
        public double il4qty { set; get; }
        public string il4UOM { set; get; }
        public double il4unitCost { set; get; }
        public double il4extCost { set; get; }
        public double il4unitSell { set; get; }
        public double il4extSell { set; get; }
        public double il4minExtSell { set; get; }

        public string il5linetype { set; get; }
        public int    il5display { set; get; }
        public string il5name { set; get; }
        public string il5size { set; get; }
        public double il5qty { set; get; }
        public string il5UOM { set; get; }
        public double il5unitCost { set; get; }
        public double il5extCost { set; get; }
        public double il5unitSell { set; get; }
        public double il5extSell { set; get; }
        public double il5minExtSell { set; get; }

        public string il6linetype { set; get; }
        public int    il6display { set; get; }
        public string il6name { set; get; }
        public string il6size { set; get; }
        public double il6qty { set; get; }
        public string il6UOM { set; get; }
        public double il6unitCost { set; get; }
        public double il6extCost { set; get; }
        public double il6unitSell { set; get; }
        public double il6extSell { set; get; }
        public double il6minExtSell { set; get; }

        public string il7linetype { set; get; }
        public int    il7display { set; get; }
        public string il7name { set; get; }
        public string il7size { set; get; }
        public double il7qty { set; get; }
        public string il7UOM { set; get; }
        public double il7unitCost { set; get; }
        public double il7extCost { set; get; }
        public double il7unitSell { set; get; }
        public double il7extSell { set; get; }
        public double il7minExtSell { set; get; }

        public string il8linetype { set; get; }
        public int    il8display { set; get; }
        public string il8name { set; get; }
        public string il8size { set; get; }
        public double il8qty { set; get; }
        public string il8UOM { set; get; }
        public double il8unitCost { set; get; }
        public double il8extCost { set; get; }
        public double il8unitSell { set; get; }
        public double il8extSell { set; get; }
        public double il8minExtSell { set; get; }

        public string il9linetype { set; get; }
        public int    il9display { set; get; }
        public string il9name { set; get; }
        public string il9size { set; get; }
        public double il9qty { set; get; }
        public string il9UOM { set; get; }
        public double il9unitCost { set; get; }
        public double il9extCost { set; get; }
        public double il9unitSell { set; get; }
        public double il9extSell { set; get; }
        public double il9minExtSell { set; get; }

        public string il10linetype { set; get; }
        public int    il10display { set; get; }
        public string il10name { set; get; }
        public string il10size { set; get; }
        public double il10qty { set; get; }
        public string il10UOM { set; get; }
        public double il10unitCost { set; get; }
        public double il10extCost { set; get; }
        public double il10unitSell { set; get; }
        public double il10extSell { set; get; }
        public double il10minExtSell { set; get; }

        public string il11linetype { set; get; }
        public int    il11display { set; get; }
        public string il11name { set; get; }
        public string il11size { set; get; }
        public double il11qty { set; get; }
        public string il11UOM { set; get; }
        public double il11unitCost { set; get; }
        public double il11extCost { set; get; }
        public double il11unitSell { set; get; }
        public double il11extSell { set; get; }
        public double il11minExtSell { set; get; }

        public string il20linetype { set; get; }
        public int    il20display { set; get; }
        public string il20name { set; get; }
        public string il20size { set; get; }
        public double il20qty { set; get; }
        public string il20UOM { set; get; }
        public double il20unitCost { set; get; }
        public double il20extCost { set; get; }
        public double il20unitSell { set; get; }
        public double il20extSell { set; get; }
        public double il20minExtSell { set; get; }

        public string il21linetype { set; get; }
        public int    il21display { set; get; }
        public string il21name { set; get; }
        public string il21size { set; get; }
        public double il21qty { set; get; }
        public string il21UOM { set; get; }
        public double il21unitCost { set; get; }
        public double il21extCost { set; get; }
        public double il21unitSell { set; get; }
        public double il21extSell { set; get; }
        public double il21minExtSell { set; get; }

        public string il22linetype { set; get; }
        public int    il22display { set; get; }
        public string il22name { set; get; }
        public string il22size { set; get; }
        public double il22qty { set; get; }
        public string il22UOM { set; get; }
        public double il22unitCost { set; get; }
        public double il22extCost { set; get; }
        public double il22unitSell { set; get; }
        public double il22extSell { set; get; }
        public double il22minExtSell { set; get; }

        public string il23linetype { set; get; }
        public int    il23display { set; get; }
        public string il23name { set; get; }
        public string il23size { set; get; }
        public double il23qty { set; get; }
        public string il23UOM { set; get; }
        public double il23unitCost { set; get; }
        public double il23extCost { set; get; }
        public double il23unitSell { set; get; }
        public double il23extSell { set; get; }
        public double il23minExtSell { set; get; }

        public string il24linetype { set; get; }
        public int    il24display { set; get; }
        public string il24name { set; get; }
        public string il24size { set; get; }
        public double il24qty { set; get; }
        public string il24UOM { set; get; }
        public double il24unitCost { set; get; }
        public double il24extCost { set; get; }
        public double il24unitSell { set; get; }
        public double il24extSell { set; get; }
        public double il24minExtSell { set; get; }

        public string il25linetype { set; get; }
        public int    il25display { set; get; }
        public string il25name { set; get; }
        public string il25size { set; get; }
        public double il25qty { set; get; }
        public string il25UOM { set; get; }
        public double il25unitCost { set; get; }
        public double il25extCost { set; get; }
        public double il25unitSell { set; get; }
        public double il25extSell { set; get; }
        public double il25minExtSell { set; get; }

        public string il26linetype { set; get; }
        public int    il26display { set; get; }
        public string il26name { set; get; }
        public string il26size { set; get; }
        public double il26qty { set; get; }
        public string il26UOM { set; get; }
        public double il26unitCost { set; get; }
        public double il26extCost { set; get; }
        public double il26unitSell { set; get; }
        public double il26extSell { set; get; }
        public double il26minExtSell { set; get; }

        public string il27linetype { set; get; }
        public int    il27display { set; get; }
        public string il27name { set; get; }
        public string il27size { set; get; }
        public double il27qty { set; get; }
        public string il27UOM { set; get; }
        public double il27unitCost { set; get; }
        public double il27extCost { set; get; }
        public double il27unitSell { set; get; }
        public double il27extSell { set; get; }
        public double il27minExtSell { set; get; }

        public string il28linetype { set; get; }
        public int    il28display { set; get; }
        public string il28name { set; get; }
        public string il28size { set; get; }
        public double il28qty { set; get; }
        public string il28UOM { set; get; }
        public double il28unitCost { set; get; }
        public double il28extCost { set; get; }
        public double il28unitSell { set; get; }
        public double il28extSell { set; get; }
        public double il28minExtSell { set; get; }

        public string il29linetype { set; get; }
        public int    il29display { set; get; }
        public string il29name { set; get; }
        public string il29size { set; get; }
        public double il29qty { set; get; }
        public string il29UOM { set; get; }
        public double il29unitCost { set; get; }
        public double il29extCost { set; get; }
        public double il29unitSell { set; get; }
        public double il29extSell { set; get; }
        public double il29minExtSell { set; get; }

        public string il30linetype { set; get; }
        public int    il30display { set; get; }
        public string il30name { set; get; }
        public string il30size { set; get; }
        public double il30qty { set; get; }
        public string il30UOM { set; get; }
        public double il30unitCost { set; get; }
        public double il30extCost { set; get; }
        public double il30unitSell { set; get; }
        public double il30extSell { set; get; }
        public double il30minExtSell { set; get; }

        public string il31linetype { set; get; }
        public int    il31display { set; get; }
        public string il31name { set; get; }
        public string il31size { set; get; }
        public double il31qty { set; get; }
        public string il31UOM { set; get; }
        public double il31unitCost { set; get; }
        public double il31extCost { set; get; }
        public double il31unitSell { set; get; }
        public double il31extSell { set; get; }
        public double il31minExtSell { set; get; }

        public string il32linetype { set; get; }
        public int    il32display { set; get; }
        public string il32name { set; get; }
        public string il32size { set; get; }
        public double il32qty { set; get; }
        public string il32UOM { set; get; }
        public double il32unitCost { set; get; }
        public double il32extCost { set; get; }
        public double il32unitSell { set; get; }
        public double il32extSell { set; get; }
        public double il32minExtSell { set; get; }

        public string il33linetype { set; get; }
        public int    il33display { set; get; }
        public string il33name { set; get; }
        public string il33size { set; get; }
        public double il33qty { set; get; }
        public string il33UOM { set; get; }
        public double il33unitCost { set; get; }
        public double il33extCost { set; get; }
        public double il33unitSell { set; get; }
        public double il33extSell { set; get; }
        public double il33minExtSell { set; get; }

        public string il34linetype { set; get; }
        public int    il34display { set; get; }
        public string il34name { set; get; }
        public string il34size { set; get; }
        public double il34qty { set; get; }
        public string il34UOM { set; get; }
        public double il34unitCost { set; get; }
        public double il34extCost { set; get; }
        public double il34unitSell { set; get; }
        public double il34extSell { set; get; }
        public double il34minExtSell { set; get; }

        public string il35linetype { set; get; }
        public int    il35display { set; get; }
        public string il35name { set; get; }
        public string il35size { set; get; }
        public double il35qty { set; get; }
        public string il35UOM { set; get; }
        public double il35unitCost { set; get; }
        public double il35extCost { set; get; }
        public double il35unitSell { set; get; }
        public double il35extSell { set; get; }
        public double il35minExtSell { set; get; }

        public string il36linetype { set; get; }
        public int    il36display { set; get; }
        public string il36name { set; get; }
        public string il36size { set; get; }
        public double il36qty { set; get; }
        public string il36UOM { set; get; }
        public double il36unitCost { set; get; }
        public double il36extCost { set; get; }
        public double il36unitSell { set; get; }
        public double il36extSell { set; get; }
        public double il36minExtSell { set; get; }

        public string il40linetype { set; get; }
        public int    il40display { set; get; }
        public string il40name { set; get; }
        public string il40size { set; get; }
        public double il40qty { set; get; }
        public string il40UOM { set; get; }
        public double il40unitCost { set; get; }
        public double il40extCost { set; get; }
        public double il40unitSell { set; get; }
        public double il40extSell { set; get; }
        public double il40minExtSell { set; get; }
    }

    public class InvoiceDetails
    {
        public int orderID { get; set; }
        public string vendorInvoiceNo { get; set; }
        public string comment { get; set; }

        public string ilStatusType { set; get; }
        public int ilInvoiceID { set; get; }
        public int ilOrderID { set; get; }
        public string il1linetype { set; get; }
        public int il1display { set; get; }
        public string il1name { set; get; }
        public string il1size { set; get; }
        public double il1qty { set; get; }
        public string il1UOM { set; get; }
        public double il1unitCost { set; get; }
        public double il1extCost { set; get; }
        public double il1unitSell { set; get; }
        public double il1extSell { set; get; }
        public double il1minExtSell { set; get; }

        public string il2linetype { set; get; }
        public int il2display { set; get; }
        public string il2name { set; get; }
        public string il2size { set; get; }
        public double il2qty { set; get; }
        public string il2UOM { set; get; }
        public double il2unitCost { set; get; }
        public double il2extCost { set; get; }
        public double il2unitSell { set; get; }
        public double il2extSell { set; get; }
        public double il2minExtSell { set; get; }

        public string il3linetype { set; get; }
        public int il3display { set; get; }
        public string il3name { set; get; }
        public string il3size { set; get; }
        public double il3qty { set; get; }
        public string il3UOM { set; get; }
        public double il3unitCost { set; get; }
        public double il3extCost { set; get; }
        public double il3unitSell { set; get; }
        public double il3extSell { set; get; }
        public double il3minExtSell { set; get; }

        public string il4linetype { set; get; }
        public int il4display { set; get; }
        public string il4name { set; get; }
        public string il4size { set; get; }
        public double il4qty { set; get; }
        public string il4UOM { set; get; }
        public double il4unitCost { set; get; }
        public double il4extCost { set; get; }
        public double il4unitSell { set; get; }
        public double il4extSell { set; get; }
        public double il4minExtSell { set; get; }

        public string il5linetype { set; get; }
        public int il5display { set; get; }
        public string il5name { set; get; }
        public string il5size { set; get; }
        public double il5qty { set; get; }
        public string il5UOM { set; get; }
        public double il5unitCost { set; get; }
        public double il5extCost { set; get; }
        public double il5unitSell { set; get; }
        public double il5extSell { set; get; }
        public double il5minExtSell { set; get; }

        public string il6linetype { set; get; }
        public int il6display { set; get; }
        public string il6name { set; get; }
        public string il6size { set; get; }
        public double il6qty { set; get; }
        public string il6UOM { set; get; }
        public double il6unitCost { set; get; }
        public double il6extCost { set; get; }
        public double il6unitSell { set; get; }
        public double il6extSell { set; get; }
        public double il6minExtSell { set; get; }

        public string il7linetype { set; get; }
        public int il7display { set; get; }
        public string il7name { set; get; }
        public string il7size { set; get; }
        public double il7qty { set; get; }
        public string il7UOM { set; get; }
        public double il7unitCost { set; get; }
        public double il7extCost { set; get; }
        public double il7unitSell { set; get; }
        public double il7extSell { set; get; }
        public double il7minExtSell { set; get; }

        public string il8linetype { set; get; }
        public int il8display { set; get; }
        public string il8name { set; get; }
        public string il8size { set; get; }
        public double il8qty { set; get; }
        public string il8UOM { set; get; }
        public double il8unitCost { set; get; }
        public double il8extCost { set; get; }
        public double il8unitSell { set; get; }
        public double il8extSell { set; get; }
        public double il8minExtSell { set; get; }

        public string il9linetype { set; get; }
        public int il9display { set; get; }
        public string il9name { set; get; }
        public string il9size { set; get; }
        public double il9qty { set; get; }
        public string il9UOM { set; get; }
        public double il9unitCost { set; get; }
        public double il9extCost { set; get; }
        public double il9unitSell { set; get; }
        public double il9extSell { set; get; }
        public double il9minExtSell { set; get; }

        public string il10linetype { set; get; }
        public int il10display { set; get; }
        public string il10name { set; get; }
        public string il10size { set; get; }
        public double il10qty { set; get; }
        public string il10UOM { set; get; }
        public double il10unitCost { set; get; }
        public double il10extCost { set; get; }
        public double il10unitSell { set; get; }
        public double il10extSell { set; get; }
        public double il10minExtSell { set; get; }

        public string il11linetype { set; get; }
        public int il11display { set; get; }
        public string il11name { set; get; }
        public string il11size { set; get; }
        public double il11qty { set; get; }
        public string il11UOM { set; get; }
        public double il11unitCost { set; get; }
        public double il11extCost { set; get; }
        public double il11unitSell { set; get; }
        public double il11extSell { set; get; }
        public double il11minExtSell { set; get; }

        public string il20linetype { set; get; }
        public int il20display { set; get; }
        public string il20name { set; get; }
        public string il20size { set; get; }
        public double il20qty { set; get; }
        public string il20UOM { set; get; }
        public double il20unitCost { set; get; }
        public double il20extCost { set; get; }
        public double il20unitSell { set; get; }
        public double il20extSell { set; get; }
        public double il20minExtSell { set; get; }

        public string il21linetype { set; get; }
        public int il21display { set; get; }
        public string il21name { set; get; }
        public string il21size { set; get; }
        public double il21qty { set; get; }
        public string il21UOM { set; get; }
        public double il21unitCost { set; get; }
        public double il21extCost { set; get; }
        public double il21unitSell { set; get; }
        public double il21extSell { set; get; }
        public double il21minExtSell { set; get; }

        public string il22linetype { set; get; }
        public int il22display { set; get; }
        public string il22name { set; get; }
        public string il22size { set; get; }
        public double il22qty { set; get; }
        public string il22UOM { set; get; }
        public double il22unitCost { set; get; }
        public double il22extCost { set; get; }
        public double il22unitSell { set; get; }
        public double il22extSell { set; get; }
        public double il22minExtSell { set; get; }

        public string il23linetype { set; get; }
        public int il23display { set; get; }
        public string il23name { set; get; }
        public string il23size { set; get; }
        public double il23qty { set; get; }
        public string il23UOM { set; get; }
        public double il23unitCost { set; get; }
        public double il23extCost { set; get; }
        public double il23unitSell { set; get; }
        public double il23extSell { set; get; }
        public double il23minExtSell { set; get; }

        public string il24linetype { set; get; }
        public int il24display { set; get; }
        public string il24name { set; get; }
        public string il24size { set; get; }
        public double il24qty { set; get; }
        public string il24UOM { set; get; }
        public double il24unitCost { set; get; }
        public double il24extCost { set; get; }
        public double il24unitSell { set; get; }
        public double il24extSell { set; get; }
        public double il24minExtSell { set; get; }

        public string il25linetype { set; get; }
        public int il25display { set; get; }
        public string il25name { set; get; }
        public string il25size { set; get; }
        public double il25qty { set; get; }
        public string il25UOM { set; get; }
        public double il25unitCost { set; get; }
        public double il25extCost { set; get; }
        public double il25unitSell { set; get; }
        public double il25extSell { set; get; }
        public double il25minExtSell { set; get; }

        public string il26linetype { set; get; }
        public int il26display { set; get; }
        public string il26name { set; get; }
        public string il26size { set; get; }
        public double il26qty { set; get; }
        public string il26UOM { set; get; }
        public double il26unitCost { set; get; }
        public double il26extCost { set; get; }
        public double il26unitSell { set; get; }
        public double il26extSell { set; get; }
        public double il26minExtSell { set; get; }

        public string il27linetype { set; get; }
        public int il27display { set; get; }
        public string il27name { set; get; }
        public string il27size { set; get; }
        public double il27qty { set; get; }
        public string il27UOM { set; get; }
        public double il27unitCost { set; get; }
        public double il27extCost { set; get; }
        public double il27unitSell { set; get; }
        public double il27extSell { set; get; }
        public double il27minExtSell { set; get; }

        public string il28linetype { set; get; }
        public int il28display { set; get; }
        public string il28name { set; get; }
        public string il28size { set; get; }
        public double il28qty { set; get; }
        public string il28UOM { set; get; }
        public double il28unitCost { set; get; }
        public double il28extCost { set; get; }
        public double il28unitSell { set; get; }
        public double il28extSell { set; get; }
        public double il28minExtSell { set; get; }

        public string il29linetype { set; get; }
        public int il29display { set; get; }
        public string il29name { set; get; }
        public string il29size { set; get; }
        public double il29qty { set; get; }
        public string il29UOM { set; get; }
        public double il29unitCost { set; get; }
        public double il29extCost { set; get; }
        public double il29unitSell { set; get; }
        public double il29extSell { set; get; }
        public double il29minExtSell { set; get; }

        public string il30linetype { set; get; }
        public int il30display { set; get; }
        public string il30name { set; get; }
        public string il30size { set; get; }
        public double il30qty { set; get; }
        public string il30UOM { set; get; }
        public double il30unitCost { set; get; }
        public double il30extCost { set; get; }
        public double il30unitSell { set; get; }
        public double il30extSell { set; get; }
        public double il30minExtSell { set; get; }

        public string il31linetype { set; get; }
        public int il31display { set; get; }
        public string il31name { set; get; }
        public string il31size { set; get; }
        public double il31qty { set; get; }
        public string il31UOM { set; get; }
        public double il31unitCost { set; get; }
        public double il31extCost { set; get; }
        public double il31unitSell { set; get; }
        public double il31extSell { set; get; }
        public double il31minExtSell { set; get; }

        public string il32linetype { set; get; }
        public int il32display { set; get; }
        public string il32name { set; get; }
        public string il32size { set; get; }
        public double il32qty { set; get; }
        public string il32UOM { set; get; }
        public double il32unitCost { set; get; }
        public double il32extCost { set; get; }
        public double il32unitSell { set; get; }
        public double il32extSell { set; get; }
        public double il32minExtSell { set; get; }

        public string il33linetype { set; get; }
        public int il33display { set; get; }
        public string il33name { set; get; }
        public string il33size { set; get; }
        public double il33qty { set; get; }
        public string il33UOM { set; get; }
        public double il33unitCost { set; get; }
        public double il33extCost { set; get; }
        public double il33unitSell { set; get; }
        public double il33extSell { set; get; }
        public double il33minExtSell { set; get; }

        public string il34linetype { set; get; }
        public int il34display { set; get; }
        public string il34name { set; get; }
        public string il34size { set; get; }
        public double il34qty { set; get; }
        public string il34UOM { set; get; }
        public double il34unitCost { set; get; }
        public double il34extCost { set; get; }
        public double il34unitSell { set; get; }
        public double il34extSell { set; get; }
        public double il34minExtSell { set; get; }

        public string il35linetype { set; get; }
        public int il35display { set; get; }
        public string il35name { set; get; }
        public string il35size { set; get; }
        public double il35qty { set; get; }
        public string il35UOM { set; get; }
        public double il35unitCost { set; get; }
        public double il35extCost { set; get; }
        public double il35unitSell { set; get; }
        public double il35extSell { set; get; }
        public double il35minExtSell { set; get; }

        public string il36linetype { set; get; }
        public int il36display { set; get; }
        public string il36name { set; get; }
        public string il36size { set; get; }
        public double il36qty { set; get; }
        public string il36UOM { set; get; }
        public double il36unitCost { set; get; }
        public double il36extCost { set; get; }
        public double il36unitSell { set; get; }
        public double il36extSell { set; get; }
        public double il36minExtSell { set; get; }

        public string il40linetype { set; get; }
        public int il40display { set; get; }
        public string il40name { set; get; }
        public string il40size { set; get; }
        public double il40qty { set; get; }
        public string il40UOM { set; get; }
        public double il40unitCost { set; get; }
        public double il40extCost { set; get; }
        public double il40unitSell { set; get; }
        public double il40extSell { set; get; }
        public double il40minExtSell { set; get; }

    }

    public class InvoiceDisplay
    { 
        public string  pdftemplate { set; get; }

    }

    public class UploadClientInvoiceViewModel
    {
        public string PageTitle { set; get; }

        public ResponseMessage Response { set; get; }
        public int OrderID { set; get; }
    }
    public class ChargeToCCRecords
    {
        public Contents charge2CC { set; get; }
    }

    public class postageQty
    {
        public InvoiceLine invoiceLines { set; get; }
    }
}
