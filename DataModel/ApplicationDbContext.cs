using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TBENesys_Orderflow_Core.DataModel
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<AccountRole> AccountRole { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Contents> Contents { get; set; }
        public DbSet<Letterhead> Letterheads { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderCheckoutField> OrderCheckoutFields { get; set; }
        public DbSet<InvoiceLinesTemplate> InvoiceLinesTemplates { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceLine> InvoiceLines { get; set; }
    }

    [Table("tblAccountRole")]
    public class AccountRole
    {
        [Key]
        public int ID { get; set; }
        public string Title { get; set; }
    }

    [Table("tblAccount")]
    public class Account
    {
        [Key]
        public int lngID { get; set; }
        public int AccessMask { get; set; }
        public string Name { get; set; }
        public string UserID { get; set; }
        public string Pwd { get; set; }
        public bool? Active { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public bool LOCKED { get; set; }
        public DateTime? LastPwdChange { get; set; }
        public string AccountRoleID { get; set; }

    }

    [Table("tblContents")]
    public class Contents
    {
        [Key]
        public int lngID { get; set; }
        public string MainType { get; set; }
        public string SubType1 { get; set; }
        public string SubType2 { get; set; }
        public string SubType3 { get; set; }
        public int Parent_ContentID { get; set; }
        public int MainOrdinal { get; set; }
        public int SubOrdinal1 { get; set; }
        public int SubOrdinal2 { get; set; }
        public int SubOrdinal3 { get; set; }
        public string MainContent { get; set; }
        public string SubContent1 { get; set; }
        public string SubContent2 { get; set; }
        public string SubContent3 { get; set; }
        public string SubContent4 { get; set; }
        public string SubContent5 { get; set; }
        public string SubContent6 { get; set; }
        public string SubContent7 { get; set; }
        public string SubContent8 { get; set; }
        public string SubContent9 { get; set; }
        public string SubContent10 { get; set; }


    }

    [Table("tblLetterhead")]
    public class Letterhead
    {
        [Key]
        public int lngID { get; set; }
        public string strName { get; set; }
        public string strFile { get; set; }
        public string strPreviewFile { get; set; }
        public string strCategory { get; set; }



    }

    [Table("tblConfiguration")]
    public class Configuration
    {
        [Key]
        public long Id { get; set; }
        public string Module { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Encrypted { get; set; }
    }

    [Table("tblOrders")]
    public class Order
    {
        [Key]
        public int lngID { get; set; }
        public int lngAccount { get; set; }
        public DateTime? datSubmitted { get; set; }
        public string status { get; set; }
        public DateTime? datApproved { get; set; }
        public int? approver_acctID { get; set; }
        public string approver_comment { get; set; }
        public string document_uploadFiles { get; set; }
        public string envelope_productType { get; set; }
        public string envelope_product_Cost_Center { get; set; }
        public string envelope_product_Group_Name_Line_1 { get; set; }
        public string envelope_product_Group_Name_Line_2 { get; set; }
        public string envelope_product_Group_Name_Line_3 { get; set; }
        public string envelope_product_Address_Line_4 { get; set; }
        public string envelope_product_City_State_Zip_Line_5 { get; set; }
        public string mailing_list_uploadFiles { get; set; }
        public string job_details_jobName { get; set; }
        public string job_details_jobPrints { get; set; }
        public string job_details_envSize { get; set; }
        public string job_details_includesMailList { get; set; }
        public DateTime? job_details_mailDropDate { get; set; }
        public string job_details_printQuantity { get; set; }
        public DateTime? job_details_neededByDate { get; set; }
        public string job_details_rushJob { get; set; }
        public string job_details_specInstr { get; set; }
        public string vendor_status { get; set; }
        public string vendor_invoiceNo { get; set; }
        public DateTime? vendor_submittedDate { get; set; }
        public string json_extras { get; set; }
    }

    [Table("tblOrderCheckoutFields")]
    public class OrderCheckoutField
    {
        [Key]
        public int lngID { get; set; }
        public int? lngOrder { get; set; }
        public string form_name { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public int? display_order { get; set; }
    }

    [Table("tblInvoiceLinesTemplate")]
    public class InvoiceLinesTemplate
    {
        [Key]
        public int lngID { get; set; }
        public string lineType { get; set; }
        public int? displayOrder { get; set; }
        public string name { get; set; }
        public string name_vendor { get; set; }
        public string size { get; set; }
        public string UOM { get; set; }
        public double? unitCost { get; set; }
        public double? unitSell { get; set; }
        public double? minExtSell { get; set; }
        public double? minExtCost { get; set; }

    }

    [Table("tblInvoices")]
    public class Invoice
    {
        [Key]
        public int lngID { get; set; }
        public int orderID { get; set; }
        public string status { get; set; }
        public string vendorInvoiceNo { get; set; }
        public string comment { get; set; }
        public DateTime? datBillSubmitted { get; set; }
        public DateTime? datBillRevised { get; set; }
        public DateTime? datClientInvoiced { get; set; }
        public DateTime? datClientInvoiceUploaded { get; set; }
        public DateTime? datClientPayment { get; set; }
        public string lastTouch { get; set; }
        public string clientInvoiceFile { get; set; }
        public string vendorInvoiceFile { get; set; }
        public string internalInvoiceFile { get; set; }

    }

    [Table("tblInvoiceLines")]
    public class InvoiceLine
    {
        [Key]
        public int lngID { get; set; }
        public int invoiceID { get; set; }
        public string lineType { get; set; }
        public int? displayOrder { get; set; }
        public string name { get; set; }
        public string size { get; set; }
        public double qty { get; set; }
        public string UOM { get; set; }
        public double? unitCost { get; set; }
        public double? extCost { get; set; }
        public double? unitSell { get; set; }
        public double? extSell { get; set; }
        public double? minExtSell { get; set; }
        public double? minExtCost { get; set; }

    }
}
