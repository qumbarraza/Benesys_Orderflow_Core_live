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
using Rotativa.AspNetCore;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Collections.Specialized.BitVector32;
using Newtonsoft.Json.Linq;

namespace TBENesys_Orderflow_Core.Controllers
{
    public class AdminController : Controller
    {
        private readonly IDbContextFactory<ApplicationDbContext> _DbContextFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;
        public AdminController(IDbContextFactory<ApplicationDbContext> contextFactor, IHttpContextAccessor httpContextAccessor)
        {
            _DbContextFactory = contextFactor;
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
        }

        public IActionResult Index()
        {
            using (var db = _DbContextFactory.CreateDbContext())
            {
                SessionTest session = new SessionTest(_httpContextAccessor);
                string UserNameFromSession = session.getSession();
                if (String.IsNullOrEmpty(UserNameFromSession))
                    return RedirectToAction("Login", "Login");
            }
            //string PDFTemplate = "<table width="100%" border="0" cellspacing="0" cellpadding="0" align="center" style="max-width:1300px; margin:auto;"><tbody><tr><td><table width="100%" border="0" cellspacing="0" cellpadding="0"><tbody><tr><td valign="middle" align="center" width="33%"><img src="https://localhost:44394/Rotativa/prestige-logo.png" width="175" alt="" /></td><td valign="middle" align="center" width="33%" style="text-align:center; width:30%; vertical-align:middle; font-size:30px; font-weight:bold; font-family:'Arial'; color: #000;">TBENeSys Billing</td><td valign="middle" align="center" width="33%"><img src="https://localhost:44394/Rotativa/smart-source-logo.png" width="150" alt="" /></td></tr></tbody></table></td></tr><tr><td><table width="100%" border="1" cellspacing="0" cellpadding="0"><tbody><tr><td><table width="100%" border="0" cellspacing="0" cellpadding="5"><tbody><tr><th valign="middle" align="left" style="text-align:left; vertical-align:middle; font-size:14px; font-weight:bold; font-family:'Arial'; color: #000;">Client Order No.</th><td valign="middle" align="left" style="text-align:left; vertical-align:middle; font-size:14px; font-weight:normal; font-family:'Arial'; color: #000;">@ClientOrderNo</td><th valign="middle" align="left" style="text-align:left; vertical-align:middle; font-size:14px; font-weight:bold; font-family:'Arial'; color: #000;">Client Charge To.</th><td valign="middle" align="left" style="text-align:left; vertical-align:middle; font-size:14px; font-weight:normal; font-family:'Arial'; color: #000;">@ClientChargeTo</td><th valign="middle" align="left" style="text-align:left; vertical-align:middle; font-size:14px; font-weight:bold; font-family:'Arial'; color: #000;">Client Job Name.</th><td valign="middle" align="left" style="text-align:left; vertical-align:middle; font-size:14px; font-weight:normal; font-family:'Arial'; color: #000;">@ClientJobName</td></tr><tr><th valign="middle" align="left" style="text-align:left; vertical-align:middle; font-size:14px; font-weight:bold; font-family:'Arial'; color: #000;">Vendor Invoice No.</th><td valign="middle" align="left" style="text-align:left; vertical-align:middle; font-size:14px; font-weight:normal; font-family:'Arial'; color: #000;">@VendorInvoiceNo</td><th valign="middle" align="left" style="text-align:left; vertical-align:middle; font-size:14px; font-weight:bold; font-family:'Arial'; color: #000;">Invoice Date.</th><td valign="middle" align="left" style="text-align:left; vertical-align:middle; font-size:14px; font-weight:normal; font-family:'Arial'; color: #000;">@InvoiceDate</td><th valign="middle" align="left" style="text-align:left; vertical-align:middle; font-size:14px; font-weight:bold; font-family:'Arial'; color: #000;"></th><td valign="middle" align="left" style="text-align:left; vertical-align:middle; font-size:14px; font-weight:normal; font-family:'Arial'; color: #000;"></td></tr></tbody></table></td></tr><tr><td style="font-family: 'Arial'; font-size:14px; color: #000000;"><table width="100%" border="1" cellspacing="0" cellpadding="2" style="font-family: 'Arial'; font-size:14px; color: #000000;"><thead><th height="28" bgcolor="#3365ff" align="left" style="color:#FFFFFF; background-color:#3365ff; text-align:left; height:35px; font-family:'Arial'">TBENeSys Printing Services</th><th height="28" bgcolor="#3365ff" align="center" style="color:#FFFFFF; background-color:#3365ff; text-align:center; height:35px; font-family:'Arial'">Size</th><th height="28" bgcolor="#3365ff" align="center" style="color: #FFFFFF; background-color:#3365ff; text-align:center; height: 35px; font-family: 'Arial'">QTY</th><th height="28" bgcolor="#3365ff" align="center" style="color: #FFFFFF; background-color:#3365ff; text-align:center; height: 35px; font-family: 'Arial'">UOM</th><th height="28" nowrap bgcolor="#3365ff" align="center" style="color:#FFFFFF; background-color:#3365ff; white-space:nowrap; text-align:center; height: 35px; font-family: 'Arial'">Unit Cost</th><th height="28" nowrap bgcolor="#3365ff" align="center" style="color: #FFFFFF; background-color:#3365ff; white-space:nowrap; text-align:center; height: 35px; font-family: 'Arial'">Ext Cost</th><th height="28" nowrap bgcolor="#3365ff" align="center" style="color:#FFFFFF; background-color:#3365ff; white-space:nowrap; text-align:center; height: 35px; font-family: 'Arial'">Unit Sell</th><th height="28" nowrap bgcolor="#3365ff" align="center" style="color: #FFFFFF; background-color:#3365ff; white-space:nowrap; text-align:center; height: 35px; font-family: 'Arial'">Ext Sell</th></thead><tbody><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Black & White Imaging</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">8.5x11</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@Q1</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">per img</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.015</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PEC1</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PUS1</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PES1</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Black & White Imaging</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">17x11</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@Q2</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">per img</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.04</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PEC2</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PUS2</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PES2</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Color Imaging</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">8.5x11</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@Q3</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">per img</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.09</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PEC3</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PUS3</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PES3</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Color Imaging</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">17x11</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@Q4</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">per img</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.18</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PEC4</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PUS4</td>                                                <td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PES4</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Stapling in left hand corner</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">8.5x11</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@Q5</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">per img</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.02</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PEC5</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PUS5</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PES5</td></tr><tr>                                                <td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Binding and Stapling from 17x11 to an 8.5 x 11 booklet (8 pages & above)</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">17x11</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@Q6</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">per img</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.04</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PEC6</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PUS6</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PES6</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">#10 and #9 Envelopes</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">#10/#9</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@Q7</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.01997</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PEC7</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PUS7</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PES7</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">#10 Window Safety Tint</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">#10</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@Q8</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.01997</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PEC8</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PUS8</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PES8</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">9x12 Envelopes</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">9x12</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@Q9</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffffff; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.07175</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PEC9</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PUS9</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PES9</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;"><strong>Additional Charge</strong> @PACName</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@PACNameSize</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@W1</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@PACUOM</td><td height="28" align="right" bgcolor="#ffffff" style="background-color:#ffffff; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">@PACUC</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">@PACExtCost</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PUSAEC</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PESAEC</td></tr><tr><th height="28" align="left" bgcolor="#cdcdcd" style="background-color:#cdcdcd; font-family:'Arial'; font-weight:bold; color:#000000; text-align:left;">Total Printing Services</th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th height="28" bgcolor="#cdcdcd" align="right" style="background-color:#cdcdcd; font-family: 'Arial'; font-weight: bold; color: #000000; text-align: right;">$@PTotalCost</th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th height="28" bgcolor="#cdcdcd" align="right" style="background-color:#cdcdcd; font-family: 'Arial'; font-weight: bold; color: #000000; text-align: right;">$@PTotalExtSell</th></tr><tr><th height="28" bgcolor="#3365ff" align="left" style="color:#FFFFFF; background-color:#3365ff; text-align:left; height:35px; font-family:'Arial'">TBENeSys Printing Services</th><th height="28" bgcolor="#3365ff" align="center" style="color:#FFFFFF; background-color:#3365ff; text-align:center; height:35px; font-family:'Arial'"></th><th height="28" bgcolor="#3365ff" align="center" style="color: #FFFFFF; background-color:#3365ff; text-align:center; height: 35px; font-family: 'Arial'">QTY</th><th height="28" bgcolor="#3365ff" align="center" style="color: #FFFFFF; background-color:#3365ff; text-align:center; height: 35px; font-family: 'Arial'">UOM</th><th height="28" nowrap bgcolor="#3365ff" align="center" style="color:#FFFFFF; background-color:#3365ff; white-space:nowrap; text-align:center; height: 35px; font-family: 'Arial'">Unit Cost</th><th height="28" nowrap bgcolor="#3365ff" align="center" style="color: #FFFFFF; background-color:#3365ff; white-space:nowrap; text-align:center; height: 35px; font-family: 'Arial'">Ext Cost</th><th height="28" nowrap bgcolor="#3365ff" align="center" style="color:#FFFFFF; background-color:#3365ff; white-space:nowrap; text-align:center; height: 35px; font-family: 'Arial'">Unit Sell</th><th height="28" nowrap bgcolor="#3365ff" align="center" style="color: #FFFFFF; background-color:#3365ff; white-space:nowrap; text-align:center; height: 35px; font-family: 'Arial'">Ext Sell</th></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">NCOA</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@W2</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">lot</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">45.00</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MEC1</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MUS1</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MES1</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">New Data Base File Set Up</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td>                                                <td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@W3</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">35.00</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MEC2</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MUS2</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MES2</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Cass Certification</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@W4</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.003</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MEC3</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MUS3</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MES3</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Set Up Injet (# 10, 9x12, 10x13 or larger)</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@W5</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">lot</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">40.00</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MEC4</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MUS4</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MES4</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Sort / MOCR / Letters</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@W6</td>                                                <td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.015</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MEC5</td>                                                <td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MUS5</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MES5</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Inkjet Address (# 10 ) includes Indicia & Return Address</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@W7</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.01</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MEC6</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MUS6</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MES6</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Inkjet Address (9x12, 10x13 and larger)</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@W8</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td>                                                <td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.018</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MEC7</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MUS7</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MES7</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Machine Tri-Fold (8.5x11) 1 sheet</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@W9</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.01</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MEC8</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MUS8</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MES8</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Each additional fold piece</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@E1</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.02</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MEC9</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MUS9</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MES9</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Machine Insert into (# 10 )1 sheet</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@E2</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.08</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MECM1</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNUS1</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNES1</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Machine Insert Into (9x12, 10x13 or larger) per sheet</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@E3</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.034</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MECM2</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNUS2</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNES2</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Hand Inserting into (#10, 9x12, 10x13 and larger) <em>- minimum 1 hour</em></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@E4</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">per hour</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">24.00</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MECM3</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNUS3</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNES3</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Each additional insert</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@E5</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.002</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MECM4</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNUS4</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNES4</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Postage Affixing</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@E6</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.012</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MECM5</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNUS5</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNES5</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Postage</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@E7</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#ffffff" style="background-color:#ffffff; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">1.36</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MECM6</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNUS6</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNES6</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: bold; color: #000000; text-align: left;">Additional Charge</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@E8</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#ffffff" style="background-color:#ffffff; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.00</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MECM7</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNUS7</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNES7</td></tr><tr><th height="28" align="left" bgcolor="#cdcdcd" style="background-color:#cdcdcd; font-family:'Arial'; font-weight:bold; color:#000000; text-align:left;">Total Mailing Services</th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th height="28" bgcolor="#cdcdcd" align="right" style="background-color:#cdcdcd; font-family: 'Arial'; font-weight: bold; color: #000000; text-align: right;">$@MTotalCost</th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th height="28" bgcolor="#cdcdcd" align="right" style="background-color:#cdcdcd; font-family: 'Arial'; font-weight: bold; color: #000000; text-align: right;">$@MTotalCostSell</th></tr><tr><th height="28" align="left" bgcolor="#cdcdcd" style="background-color:#cdcdcd; font-family:'Arial'; font-weight:bold; color:#000000; text-align:left;">Freight</th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;">@Freight</th><th height="28" bgcolor="#cdcdcd" align="right" style="background-color:#cdcdcd; font-family: 'Arial'; font-weight: bold; color: #000000; text-align: right;">$@FCost</th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th></tr></tbody></table></td></tr><tr><td height="28" align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left; padding-left:10px;"><strong>Memo/Comment</strong> &nbsp; <em>@Comments</em></td></tr></tbody></table></td></tr></tbody></table>";
            //"<table width="100 % " border="0" cellspacing="0" cellpadding="0" align="center" style="max - width:1300px; margin: auto; "><tbody><tr><td><table width="100 % " border="0" cellspacing="0" cellpadding="0"><tbody><tr><td valign="middle" align="center" width="33 % "><img src="https://localhost:44394/Rotativa/prestige-logo.png" width="175" alt="" /></td><td valign="middle" align="center" width="33%" style="text-align:center; width:30%; vertical-align:middle; font-size:30px; font-weight:bold; font-family:'Arial'; color: #000;">TBENeSys Billing</td><td valign="middle" align="center" width="33%"><img src="https://localhost:44394/Rotativa/smart-source-logo.png" width="150" alt="" /></td></tr></tbody></table></td></tr><tr><td><table width="100%" border="1" cellspacing="0" cellpadding="0"><tbody><tr><td><table width="100%" border="0" cellspacing="0" cellpadding="5"><tbody><tr><th valign="middle" align="left" style="text-align:left; vertical-align:middle; font-size:14px; font-weight:bold; font-family:'Arial'; color: #000;">Client Order No.</th><td valign="middle" align="left" style="text-align:left; vertical-align:middle; font-size:14px; font-weight:normal; font-family:'Arial'; color: #000;">@ClientOrderNo</td><th valign="middle" align="left" style="text-align:left; vertical-align:middle; font-size:14px; font-weight:bold; font-family:'Arial'; color: #000;">Client Charge To.</th><td valign="middle" align="left" style="text-align:left; vertical-align:middle; font-size:14px; font-weight:normal; font-family:'Arial'; color: #000;">@ClientChargeTo</td><th valign="middle" align="left" style="text-align:left; vertical-align:middle; font-size:14px; font-weight:bold; font-family:'Arial'; color: #000;">Client Job Name.</th><td valign="middle" align="left" style="text-align:left; vertical-align:middle; font-size:14px; font-weight:normal; font-family:'Arial'; color: #000;">@ClientJobName</td></tr><tr><th valign="middle" align="left" style="text-align:left; vertical-align:middle; font-size:14px; font-weight:bold; font-family:'Arial'; color: #000;">Vendor Invoice No.</th><td valign="middle" align="left" style="text-align:left; vertical-align:middle; font-size:14px; font-weight:normal; font-family:'Arial'; color: #000;">@VendorInvoiceNo</td><th valign="middle" align="left" style="text-align:left; vertical-align:middle; font-size:14px; font-weight:bold; font-family:'Arial'; color: #000;">Invoice Date.</th><td valign="middle" align="left" style="text-align:left; vertical-align:middle; font-size:14px; font-weight:normal; font-family:'Arial'; color: #000;">@InvoiceDate</td><th valign="middle" align="left" style="text-align:left; vertical-align:middle; font-size:14px; font-weight:bold; font-family:'Arial'; color: #000;"></th><td valign="middle" align="left" style="text-align:left; vertical-align:middle; font-size:14px; font-weight:normal; font-family:'Arial'; color: #000;"></td></tr></tbody></table></td></tr><tr><td style="font-family: 'Arial'; font-size:14px; color: #000000;"><table width="100%" border="1" cellspacing="0" cellpadding="2" style="font-family: 'Arial'; font-size:14px; color: #000000;"><thead><th height="28" bgcolor="#3365ff" align="left" style="color:#FFFFFF; background-color:#3365ff; text-align:left; height:35px; font-family:'Arial'">TBENeSys Printing Services</th><th height="28" bgcolor="#3365ff" align="center" style="color:#FFFFFF; background-color:#3365ff; text-align:center; height:35px; font-family:'Arial'">Size</th><th height="28" bgcolor="#3365ff" align="center" style="color: #FFFFFF; background-color:#3365ff; text-align:center; height: 35px; font-family: 'Arial'">QTY</th><th height="28" bgcolor="#3365ff" align="center" style="color: #FFFFFF; background-color:#3365ff; text-align:center; height: 35px; font-family: 'Arial'">UOM</th><th height="28" nowrap bgcolor="#3365ff" align="center" style="color:#FFFFFF; background-color:#3365ff; white-space:nowrap; text-align:center; height: 35px; font-family: 'Arial'">Unit Cost</th><th height="28" nowrap bgcolor="#3365ff" align="center" style="color: #FFFFFF; background-color:#3365ff; white-space:nowrap; text-align:center; height: 35px; font-family: 'Arial'">Ext Cost</th><th height="28" nowrap bgcolor="#3365ff" align="center" style="color:#FFFFFF; background-color:#3365ff; white-space:nowrap; text-align:center; height: 35px; font-family: 'Arial'">Unit Sell</th><th height="28" nowrap bgcolor="#3365ff" align="center" style="color: #FFFFFF; background-color:#3365ff; white-space:nowrap; text-align:center; height: 35px; font-family: 'Arial'">Ext Sell</th></thead><tbody><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Black & White Imaging</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">8.5x11</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@Q1</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">per img</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.015</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PEC1</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PUS1</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PES1</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Black & White Imaging</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">17x11</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@Q2</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">per img</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.04</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PEC2</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PUS2</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PES2</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Color Imaging</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">8.5x11</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@Q3</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">per img</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.09</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PEC3</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PUS3</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PES3</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Color Imaging</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">17x11</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@Q4</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">per img</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.18</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PEC4</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PUS4</td>                                                <td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PES4</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Stapling in left hand corner</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">8.5x11</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@Q5</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">per img</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.02</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PEC5</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PUS5</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PES5</td></tr><tr>                                                <td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Binding and Stapling from 17x11 to an 8.5 x 11 booklet (8 pages & above)</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">17x11</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@Q6</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">per img</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.04</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PEC6</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PUS6</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PES6</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">#10 and #9 Envelopes</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">#10/#9</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@Q7</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.01997</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PEC7</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PUS7</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PES7</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">#10 Window Safety Tint</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">#10</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@Q8</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.01997</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PEC8</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PUS8</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PES8</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">9x12 Envelopes</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">9x12</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@Q9</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffffff; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.07175</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PEC9</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PUS9</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PES9</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;"><strong>Additional Charge</strong> @PACName</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@PACNameSize</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@W1</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@PACUOM</td><td height="28" align="right" bgcolor="#ffffff" style="background-color:#ffffff; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">@PACUC</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">@PACExtCost</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PUSAEC</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@PESAEC</td></tr><tr><th height="28" align="left" bgcolor="#cdcdcd" style="background-color:#cdcdcd; font-family:'Arial'; font-weight:bold; color:#000000; text-align:left;">Total Printing Services</th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th height="28" bgcolor="#cdcdcd" align="right" style="background-color:#cdcdcd; font-family: 'Arial'; font-weight: bold; color: #000000; text-align: right;">$@PTotalCost</th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th height="28" bgcolor="#cdcdcd" align="right" style="background-color:#cdcdcd; font-family: 'Arial'; font-weight: bold; color: #000000; text-align: right;"></th></tr><tr><th height="28" bgcolor="#3365ff" align="left" style="color:#FFFFFF; background-color:#3365ff; text-align:left; height:35px; font-family:'Arial'">TBENeSys Printing Services</th><th height="28" bgcolor="#3365ff" align="center" style="color:#FFFFFF; background-color:#3365ff; text-align:center; height:35px; font-family:'Arial'"></th><th height="28" bgcolor="#3365ff" align="center" style="color: #FFFFFF; background-color:#3365ff; text-align:center; height: 35px; font-family: 'Arial'">QTY</th><th height="28" bgcolor="#3365ff" align="center" style="color: #FFFFFF; background-color:#3365ff; text-align:center; height: 35px; font-family: 'Arial'">UOM</th><th height="28" nowrap bgcolor="#3365ff" align="center" style="color:#FFFFFF; background-color:#3365ff; white-space:nowrap; text-align:center; height: 35px; font-family: 'Arial'">Unit Cost</th><th height="28" nowrap bgcolor="#3365ff" align="center" style="color: #FFFFFF; background-color:#3365ff; white-space:nowrap; text-align:center; height: 35px; font-family: 'Arial'">Ext Cost</th><th height="28" nowrap bgcolor="#3365ff" align="center" style="color:#FFFFFF; background-color:#3365ff; white-space:nowrap; text-align:center; height: 35px; font-family: 'Arial'">Unit Sell</th><th height="28" nowrap bgcolor="#3365ff" align="center" style="color: #FFFFFF; background-color:#3365ff; white-space:nowrap; text-align:center; height: 35px; font-family: 'Arial'">Ext Sell</th></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">NCOA</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@W2</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">lot</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">45.00</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MEC1</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MUS1</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MES1</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">New Data Base File Set Up</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td>                                                <td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@W3</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">35.00</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MEC2</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MUS2</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MES2</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Cass Certification</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@W4</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.003</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MEC3</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MUS3</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MES3</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Set Up Injet (# 10, 9x12, 10x13 or larger)</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@W5</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">lot</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">40.00</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MEC4</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MUS4</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MES4</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Sort / MOCR / Letters</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@W6</td>                                                <td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.015</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MEC5</td>                                                <td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MUS5</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MES5</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Inkjet Address (# 10 ) includes Indicia & Return Address</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@W7</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.01</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MEC6</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MUS6</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MES6</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Inkjet Address (9x12, 10x13 and larger)</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@W8</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td>                                                <td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.018</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MEC7</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MUS7</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MES7</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Machine Tri-Fold (8.5x11) 1 sheet</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@W9</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.01</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MEC8</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MUS8</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MES8</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Each additional fold piece</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@E1</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.02</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MEC9</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MUS9</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MES9</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Machine Insert into (# 10 )1 sheet</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@E2</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.08</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MECM1</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNUS1</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNES1</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Machine Insert Into (9x12, 10x13 or larger) per sheet</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@E3</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.034</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MECM2</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNUS2</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNES2</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Hand Inserting into (#10, 9x12, 10x13 and larger) <em>- minimum 1 hour</em></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@E4</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">per hour</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">24.00</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MECM3</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNUS3</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNES3</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Each additional insert</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@E5</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.002</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MECM4</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNUS4</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNES4</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Postage Affixing</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@E6</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#3365ff" style="background-color:#ffcc9a; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.012</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MECM5</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNUS5</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNES5</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left;">Postage</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@E7</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#ffffff" style="background-color:#ffffff; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">1.36</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MECM6</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNUS6</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNES6</td></tr><tr><td align="left" style="font-family: 'Arial'; font-weight: bold; color: #000000; text-align: left;">Additional Charge</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;"></td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">@E8</td><td height="28" align="center" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: center;">ea</td><td height="28" align="right" bgcolor="#ffffff" style="background-color:#ffffff; font-family:'Arial'; font-weight: normal; color: #000000; text-align: right;">0.00</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MECM7</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNUS7</td><td height="28" align="right" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: right;">$@MNES7</td></tr><tr><th height="28" align="left" bgcolor="#cdcdcd" style="background-color:#cdcdcd; font-family:'Arial'; font-weight:bold; color:#000000; text-align:left;">Total Mailing Services</th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th height="28" bgcolor="#cdcdcd" align="right" style="background-color:#cdcdcd; font-family: 'Arial'; font-weight: bold; color: #000000; text-align: right;">$@MTotalCost</th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th height="28" bgcolor="#cdcdcd" align="right" style="background-color:#cdcdcd; font-family: 'Arial'; font-weight: bold; color: #000000; text-align: right;"></th></tr><tr><th height="28" align="left" bgcolor="#cdcdcd" style="background-color:#cdcdcd; font-family:'Arial'; font-weight:bold; color:#000000; text-align:left;">Freight</th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;">@Freight</th><th height="28" bgcolor="#cdcdcd" align="right" style="background-color:#cdcdcd; font-family: 'Arial'; font-weight: bold; color: #000000; text-align: right;">$@FCost</th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th><th align="center" bgcolor="#cdcdcd" style="background-color:#cdcdcd; text-align:center; font-family:'Arial'; font-weight:bold; color:#000000; text-align:center;"></th></tr></tbody></table></td></tr><tr><td height="28" align="left" style="font-family: 'Arial'; font-weight: normal; color: #000000; text-align: left; padding-left:10px;"><strong>Memo/Comment</strong> &nbsp; <em>@Comments</em></td></tr></tbody></table></td></tr></tbody></table>"
            return View();
        }

        public IActionResult AccountRolesListing(string Success, string Details, string Sort, string Filter, int? Page, int? PageSize)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            AccountRolesListingViewModel model = new AccountRolesListingViewModel();
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

            model.PageTitle = "Account Role Listing";

            model.SortTitle = Sort == "Title" ? "Title_Desc" : "Title";


            switch (Sort)
            {
                case "Title":
                    model.SortClassTitle = "glyphicon glyphicon-arrow-down";
                    break;
                case "Title_Desc":
                    model.SortClassTitle = "glyphicon glyphicon-arrow-up";
                    break;

                default:
                    model.SortClassTitle = "glyphicon glyphicon-arrow-up";
                    break;
            }

            model.Sort = Sort;
            model.Filter = Filter;
            model.PageSize = PageSize.HasValue ? PageSize.Value : 10;
            model.PageNumber = (Page ?? 1);

            using (var db = _DbContextFactory.CreateDbContext())
            {

                model.Records = db.AccountRole.ToList();

                if (!String.IsNullOrEmpty(Filter))
                {
                    model.Records = model.Records.Where(s => s.Title.ToLower().Contains(Filter.ToLower())
                                                            ).ToList();
                }

                switch (Sort)
                {
                    case "Title":
                        model.Records = model.Records.OrderBy(s => s.Title).ToList();
                        break;
                    case "Title_Desc":
                        model.Records = model.Records.OrderByDescending(s => s.Title).ToList();
                        break;

                    default:  // Name ascending 
                        model.Records = model.Records.OrderByDescending(s => s.Title).ToList(); ;
                        break;
                }

                model.RecordsPaged = model.Records.ToList();//.ToPagedList(model.PageNumber, model.PageSize);

            }
            return View(model);

        }

        [HttpGet]
        public IActionResult DeleteAccountRole(int ID)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            bool IsSuccessful = false;
            string DetailMessage = "";
            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    AccountRole x = db.AccountRole.FirstOrDefault(d => d.ID == ID);
                    if (x != null)
                    {
                        db.AccountRole.Remove(x);
                        db.SaveChanges();
                        IsSuccessful = true;
                    }
                }

            }
            catch (Exception ex)
            {
                IsSuccessful = false;
                DetailMessage = ex.Message;
            }

            return RedirectToAction("AccountRolesListing", new { Success = IsSuccessful, Details = DetailMessage });
        }

        public IActionResult AccountRoleHandler(string Id)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            AccountRoleHandlerViewModel model = new AccountRoleHandlerViewModel();
            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    #region Load Existing Details

                    if (Id != null && Id.Length > 0)
                    {
                        int ID = Convert.ToInt32(Id);
                        AccountRole obj = db.AccountRole.First(d => d.ID == ID);
                        model.ID = obj.ID;
                        model.Title = obj.Title;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                ViewBag.ResultMessage = "Error: " + ex.Message;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult AccountRoleHandler(AccountRoleHandlerViewModel model)
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
                    using (var db = _DbContextFactory.CreateDbContext())
                    {
                        bool IsNewRecord = false;
                        long ID = Convert.ToInt64(model.ID);
                        AccountRole obj = db.AccountRole.FirstOrDefault(d => d.ID == ID);
                        if (obj == null)
                        {
                            obj = new AccountRole();
                            IsNewRecord = true;
                        }

                        obj.Title = model.Title;

                        if (IsNewRecord)
                        {

                            db.AccountRole.Add(obj);
                            db.SaveChanges();
                            ViewBag.ResultMessage = "Account role created by the title: " + obj.Title + " successfully!";
                            return RedirectToAction("AccountRolesListing", new { Success = true, Details = ViewBag.ResultMessage });
                        }
                        else
                        {
                            db.SaveChanges();
                            ViewBag.ResultMessage = "Account role updated with new title: " + obj.Title + " successfully!";
                            return RedirectToAction("AccountRolesListing", new { Success = true, Details = ViewBag.ResultMessage });
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

        public IActionResult AccountListing(string Success, string Details, string Sort, string Filter, int? Page, int? PageSize)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            AccountListingViewModel model = new AccountListingViewModel();
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

            model.PageTitle = "Account  Listing";

            using (var db = _DbContextFactory.CreateDbContext())
            {
                model.Records = db.Account.ToList();
                model.Records = model.Records.OrderBy(s => s.Name).ToList(); ;
            }
            return View(model);

        }

        [HttpGet]
        public IActionResult DeleteAccount(int ID)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            bool IsSuccessful = false;
            string DetailMessage = "";
            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    Account x = db.Account.FirstOrDefault(d => d.lngID == ID);
                    if (x != null)
                    {
                        db.Account.Remove(x);
                        db.SaveChanges();
                        IsSuccessful = true;
                    }
                }

            }
            catch (Exception ex)
            {
                IsSuccessful = false;
                DetailMessage = ex.Message;
            }

            return RedirectToAction("AccountListing", new { Success = IsSuccessful, Details = DetailMessage });
        }

        public IActionResult AccountHandler(string Id)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            AccountHandlerViewModel model = new AccountHandlerViewModel();
            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    #region Load Pre Details

                    model.AllLocations = db.Contents.Where(d => d.MainType == "ShipToCC").Select(x => new Itemlist { Value = x.MainContent, Text = x.MainContent }).ToList();
                    model.AllAcountRoles = db.AccountRole.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.Title }).ToList();

                    #endregion

                    #region Load Existing Details

                    if (Id != null && Id.Length > 0)
                    {
                        int ID = Convert.ToInt32(Id);
                        Account obj = db.Account.First(d => d.lngID == ID);

                        model.lngID = obj.lngID;
                        model.AccessMask = obj.AccessMask;
                        model.Name = obj.Name;
                        model.UserID = obj.UserID;
                        model.Pwd = obj.Pwd;
                        model.Active = obj.Active.HasValue ? obj.Active.Value : true;
                        model.Email = obj.Email;
                        model.Location = obj.Location;
                        model.LOCKED = obj.LOCKED;
                        model.LastPwdChange = obj.LastPwdChange;
                        model.AccountRoleID = obj.AccountRoleID;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                ViewBag.ResultMessage = "Error: " + ex.Message;
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    model.AllLocations = db.Contents.Where(d => d.MainType == "ShipToCC").Select(x => new Itemlist { Value = x.MainContent, Text = x.MainContent }).ToList();
                    model.AllAcountRoles = db.AccountRole.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.Title }).ToList();
                }
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult AccountHandler(AccountHandlerViewModel model)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            if (!ModelState.IsValid)
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    model.AllLocations = db.Contents.Where(d => d.MainType == "ShipToCC").Select(x => new Itemlist { Value = x.MainContent, Text = x.MainContent }).ToList();
                    model.AllAcountRoles = db.AccountRole.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.Title }).ToList();
                }
                return View(model);
            }
            else
            {
                try
                {
                    using (var db = _DbContextFactory.CreateDbContext())
                    {
                        bool IsNewRecord = false;
                        long ID = Convert.ToInt64(model.lngID);
                        Account obj = db.Account.FirstOrDefault(d => d.lngID == ID);
                        if (obj == null)
                        {
                            obj = new Account();
                            IsNewRecord = true;
                        }

                        obj.AccessMask = model.AccessMask;
                        obj.Name = model.Name;
                        obj.UserID = model.UserID;
                        obj.Pwd = model.Pwd;
                        obj.Active = model.Active;
                        obj.Email = model.Email;
                        obj.Location = model.Location;
                        obj.LOCKED = model.LOCKED;
                        obj.LastPwdChange = model.LastPwdChange;
                        obj.AccountRoleID = String.Join(",", model.SelectedAccountRoles.ToList());

                        if (IsNewRecord)
                        {
                            obj.LOCKED = false;
                            obj.LastPwdChange = DateTime.Now;
                            db.Account.Add(obj);
                            db.SaveChanges();
                            ViewBag.ResultMessage = "Account created for " + obj.Name + " successfully!";
                            return RedirectToAction("AccountListing", new { Success = true, Details = ViewBag.ResultMessage });
                        }
                        else
                        {
                            obj.LastPwdChange = DateTime.Now;
                            db.SaveChanges();
                            ViewBag.ResultMessage = "Account information updated for " + obj.Name + " successfully!";
                            return RedirectToAction("AccountListing", new { Success = true, Details = ViewBag.ResultMessage });
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ResultMessage = "Error! " + ex.Message;
                    using (var db = _DbContextFactory.CreateDbContext())
                    {
                        model.AllLocations = db.Contents.Where(d => d.MainType == "ShipToCC").Select(x => new Itemlist { Value = x.MainContent, Text = x.MainContent }).ToList();
                        model.AllAcountRoles = db.AccountRole.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.Title }).ToList();
                    }
                }

            }
            return View(model);
        }

        public IActionResult MyAccount(string Success, string Details)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            MyAccountViewModel model = new MyAccountViewModel();
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

            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    #region Load Existing Details

                    if (UserNameFromSession != null && UserNameFromSession.Length > 0)
                    {
                        Account obj = db.Account.First(d => d.UserID == UserNameFromSession);
                        model.lngID = obj.lngID;
                        model.Name = obj.Name;
                        model.UserID = obj.UserID;
                        model.Email = obj.Email;

                        string UserRoleIDs = session.getSessionRoles();
                        string FinalName = "";
                        string[] AllRoles = UserRoleIDs.Split(',');
                        foreach (string SpeRole in AllRoles)
                        {
                            int MyRoleID = Convert.ToInt32(SpeRole);
                            AccountRole AR = db.AccountRole.FirstOrDefault(d => d.ID == MyRoleID);
                            if (AR != null)
                            {
                                if (!String.IsNullOrEmpty(FinalName))
                                    FinalName = FinalName + "," + AR.Title;
                                else
                                    FinalName = AR.Title;
                            }
                        }
                        model.UserRole = FinalName;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                ViewBag.ResultMessage = "Error: " + ex.Message;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult MyAccount(MyAccountViewModel model)
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
                    using (var db = _DbContextFactory.CreateDbContext())
                    {
                        Account obj = db.Account.FirstOrDefault(d => d.UserID == UserNameFromSession);
                        if (obj == null)
                        {
                            ViewBag.ResultMessage = "Error: Account information not found!";
                            return View(model);
                        }

                        if (obj != null)
                        {
                            if (model.Pwd != obj.Pwd)
                            {
                                ViewBag.ResultMessage = "Error: Current password is not correct!";
                                return View(model);
                            }

                            if (model.NewPwd != model.ConfirmPwd)
                            {
                                ViewBag.ResultMessage = "Error: New password & confirm password do not match!";
                                return View(model);
                            }
                        }

                        obj.Name = model.Name;
                        obj.Pwd = model.NewPwd;
                        obj.Email = model.Email;
                        obj.LastPwdChange = DateTime.Now;
                        db.SaveChanges();
                        ViewBag.ResultMessage = "Your account information has been updated!";
                        return RedirectToAction("MyAccount", new { Success = true, Details = ViewBag.ResultMessage });
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ResultMessage = "Error! " + ex.Message;
                }

            }
            return View(model);
        }

        public IActionResult LetterheadsListing(string Success, string Details)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            LetterheadListingViewModel model = new LetterheadListingViewModel();
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

            model.PageTitle = "Letterhead Listing";


            using (var db = _DbContextFactory.CreateDbContext())
            {
                model.Records = db.Letterheads.ToList();
                model.Records = model.Records.OrderByDescending(s => s.strName).ToList(); ;
            }
            return View(model);

        }

        [HttpGet]
        public IActionResult DeleteLetterhead(int ID)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            bool IsSuccessful = false;
            string DetailMessage = "";
            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    Letterhead x = db.Letterheads.FirstOrDefault(d => d.lngID == ID);
                    if (x != null)
                    {
                        db.Letterheads.Remove(x);
                        db.SaveChanges();
                        IsSuccessful = true;
                    }
                }

            }
            catch (Exception ex)
            {
                IsSuccessful = false;
                DetailMessage = ex.Message;
            }

            return RedirectToAction("LetterheadsListing", new { Success = IsSuccessful, Details = DetailMessage });
        }

        public IActionResult LetterheadHandler(string Id)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            LetterheadHandlerViewModel model = new LetterheadHandlerViewModel();
            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    #region Load Existing Details

                    if (Id != null && Id.Length > 0)
                    {
                        int ID = Convert.ToInt32(Id);
                        Letterhead obj = db.Letterheads.First(d => d.lngID == ID);
                        model.lngID = obj.lngID;
                        model.FileName = obj.strName;
                        model.FilePath = obj.strFile;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                ViewBag.ResultMessage = "Error: " + ex.Message;
            }
            return View(model);
        }

        [HttpPost, DisableRequestSizeLimit]
        public IActionResult LetterheadHandler(LetterheadHandlerViewModel model)
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
                    string FileNameforDB = "";
                    var file = Request.Form.Files[0];
                    var folderName = Path.Combine("Repository", "Letterheads");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        FileNameforDB = fileName;
                        string fileType = fileName.Split('.')[1];

                        if (fileType == "docx")
                        {
                            var fullPath = Path.Combine(pathToSave, fileName);
                            var dbPath = Path.Combine(folderName, fileName);
                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                        }
                        else
                        {
                            ViewBag.ResultMessage = "Error! Only .docx extentions allowed for upload";
                            return View(model);
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }

                    using (var db = _DbContextFactory.CreateDbContext())
                    {
                        bool IsNewRecord = false;
                        long ID = Convert.ToInt64(model.lngID);
                        Letterhead obj = db.Letterheads.FirstOrDefault(d => d.lngID == ID);
                        if (obj == null)
                        {
                            obj = new Letterhead();
                            IsNewRecord = true;
                        }

                        obj.strName = model.FileName;
                        obj.strFile = FileNameforDB;

                        if (IsNewRecord)
                        {
                            db.Letterheads.Add(obj);
                            db.SaveChanges();

                            ViewBag.ResultMessage = "Letterhead uploaded successfully!";
                            return RedirectToAction("LetterheadsListing", new { Success = true, Details = ViewBag.ResultMessage });
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

        public FileResult DownloadLetterhead(string fileName)
        {
            //Build the File Path.
            var folderName = Path.Combine("Repository", "Letterheads");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            string path = pathToSave + "\\" + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }

        public IActionResult EnvelopesListing(string Success, string Details)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            EnvelopesListingViewModel model = new EnvelopesListingViewModel();
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

            model.PageTitle = "Envelopes Listing";

            using (var db = _DbContextFactory.CreateDbContext())
            {
                model.Records = db.Contents.Where(d => d.MainType == "Envelope").ToList();

            }
            return View(model);

        }

        public IActionResult DeleteEnvelope(int ID)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            bool IsSuccessful = false;
            string DetailMessage = "";
            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    Contents x = db.Contents.FirstOrDefault(d => d.lngID == ID);
                    if (x != null)
                    {
                        db.Contents.Remove(x);
                        db.SaveChanges();
                        IsSuccessful = true;
                    }
                }

            }
            catch (Exception ex)
            {
                IsSuccessful = false;
                DetailMessage = ex.Message;
            }

            return RedirectToAction("EnvelopesListing", new { Success = IsSuccessful, Details = DetailMessage });
        }

        public IActionResult EnvelopeHandler(string Id)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            EnvelopeHandlerViewModel model = new EnvelopeHandlerViewModel();
            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    #region Load Existing Details

                    if (Id != null && Id.Length > 0)
                    {
                        int ID = Convert.ToInt32(Id);
                        Contents obj = db.Contents.First(d => d.lngID == ID);
                        model.lngID = obj.lngID;
                        model.MainContent = obj.MainContent;
                        model.SubContent2 = obj.SubContent2;
                        model.SubContent3 = obj.SubContent3;
                        model.SubContent4 = obj.SubContent4;
                        model.SubContent5 = obj.SubContent5;
                        model.SubContent6 = obj.SubContent6;

                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                ViewBag.ResultMessage = "Error: " + ex.Message;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult EnvelopeHandler(EnvelopeHandlerViewModel model)
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
                    using (var db = _DbContextFactory.CreateDbContext())
                    {
                        bool IsNewRecord = false;
                        long ID = Convert.ToInt64(model.lngID);
                        Contents obj = db.Contents.FirstOrDefault(d => d.lngID == ID);
                        if (obj == null)
                        {
                            obj = new Contents();
                            IsNewRecord = true;
                        }

                        obj.MainContent = obj.SubContent1 = model.MainContent;
                        obj.MainType = "Envelope";
                        obj.Parent_ContentID = 0;
                        obj.MainOrdinal = 0;
                        obj.SubOrdinal1 = 0;
                        obj.SubOrdinal2 = 0;
                        obj.SubOrdinal3 = 0;
                        obj.SubContent2 = model.SubContent2;
                        obj.SubContent3 = model.SubContent3;
                        obj.SubContent4 = model.SubContent4;
                        obj.SubContent5 = model.SubContent5;
                        obj.SubContent6 = model.SubContent6;

                        if (IsNewRecord)
                        {

                            db.Contents.Add(obj);
                            db.SaveChanges();
                            ViewBag.ResultMessage = "Envelope created by the Cost Center: " + obj.MainContent + " successfully!";
                            return RedirectToAction("EnvelopesListing", new { Success = true, Details = ViewBag.ResultMessage });
                        }
                        else
                        {
                            db.SaveChanges();
                            ViewBag.ResultMessage = "Envelope information updated for the Cost Center: " + obj.MainContent + " successfully!";
                            return RedirectToAction("EnvelopesListing", new { Success = true, Details = ViewBag.ResultMessage });
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

        public IActionResult UploadEnvelope()
        {
            return View();
        }

        [HttpPost, DisableRequestSizeLimit]
        public IActionResult UploadEnvelope(IFormFile postedFile)
        {
            string FileNameforDB = "";
            var file = Request.Form.Files[0];
            var folderName = Path.Combine("Repository", "Envelopes");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                FileNameforDB = fileName;
                string fileType = fileName.Split('.')[1];

                if (fileType == "csv")
                {
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    string csvData = System.IO.File.ReadAllText(fullPath);
                    DataTable dt = new DataTable();
                    bool firstRow = true;
                    foreach (string row in csvData.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            if (!string.IsNullOrEmpty(row))
                            {
                                if (firstRow)
                                {
                                    foreach (string cell in row.Split(','))
                                    {
                                        dt.Columns.Add(cell.Trim());
                                    }
                                    firstRow = false;
                                }
                                else
                                {
                                    dt.Rows.Add();
                                    int i = 0;
                                    foreach (string cell in row.Split(','))
                                    {
                                        dt.Rows[dt.Rows.Count - 1][i] = cell.Trim();
                                        i++;
                                    }
                                }
                            }
                        }
                    }

                    if (dt.Rows.Count > 0)
                    {
                        using (var db = _DbContextFactory.CreateDbContext())
                        {
                            foreach (DataRow rw in dt.Rows)
                            {
                                Contents obj = new Contents();
                                obj.MainContent = obj.SubContent1 = rw[0].ToString();
                                obj.MainType = "Envelope";
                                obj.Parent_ContentID = 0;
                                obj.MainOrdinal = 0;
                                obj.SubOrdinal1 = 0;
                                obj.SubOrdinal2 = 0;
                                obj.SubOrdinal3 = 0;
                                obj.SubContent2 = rw[1].ToString();
                                obj.SubContent3 = rw[2].ToString();
                                obj.SubContent4 = rw[3].ToString();
                                obj.SubContent5 = rw[4].ToString();
                                obj.SubContent6 = rw[5].ToString();
                                db.Contents.Add(obj);
                                db.SaveChanges();
                            }
                        }
                        ViewBag.ResultMessage = "Envelopes uploaded successfully!";
                        return RedirectToAction("EnvelopesListing", new { Success = true, Details = ViewBag.ResultMessage });
                    }
                }
                else
                {
                    ViewBag.ResultMessage = "Error! Only .csv extentions allowed for upload";
                    return View();
                }
            }
            else
            {
                ViewBag.ResultMessage = "Error! Bad request for upload";
                return View();
            }

            ViewBag.ResultMessage = "Something went wrong";
            return View();
        }

        public FileResult DownloadSampleEnvelope()
        {
            //Build the File Path.
            var folderName = Path.Combine("Repository", "Envelopes");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            string path = pathToSave + "\\sample-envelopes.csv";

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", "sample-envelopes.csv");
        }

        public IActionResult ChargeToCCsListing(string Success, string Details)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            ChargeToCCsListingViewModel model = new ChargeToCCsListingViewModel();
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

            model.PageTitle = "Charge To CCs Listing";

            using (var db = _DbContextFactory.CreateDbContext())
            {
                model.Records = db.Contents.Where(d => d.MainType == "ChargeToCC").ToList();

            }
            return View(model);

        }

        public IActionResult DeleteChargeToCC(int ID)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            bool IsSuccessful = false;
            string DetailMessage = "";
            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    Contents x = db.Contents.FirstOrDefault(d => d.lngID == ID);
                    if (x != null)
                    {
                        db.Contents.Remove(x);
                        db.SaveChanges();
                        IsSuccessful = true;
                    }
                }

            }
            catch (Exception ex)
            {
                IsSuccessful = false;
                DetailMessage = ex.Message;
            }

            return RedirectToAction("ChargeToCCsListing", new { Success = IsSuccessful, Details = DetailMessage });
        }

        public IActionResult ChargeToCCHandler(string Id)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            ChargeToCCsHandlerViewModel model = new ChargeToCCsHandlerViewModel();
            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    #region Load Existing Details

                    if (Id != null && Id.Length > 0)
                    {
                        int ID = Convert.ToInt32(Id);
                        Contents obj = db.Contents.First(d => d.lngID == ID);
                        model.lngID = obj.lngID;
                        model.MainContent = obj.MainContent;
                        model.SubContent2 = obj.SubContent2 == "active" ? true : false;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                ViewBag.ResultMessage = "Error: " + ex.Message;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult ChargeToCCHandler(ChargeToCCsHandlerViewModel model)
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
                    using (var db = _DbContextFactory.CreateDbContext())
                    {
                        bool IsNewRecord = false;
                        long ID = Convert.ToInt64(model.lngID);
                        Contents obj = db.Contents.FirstOrDefault(d => d.lngID == ID);
                        if (obj == null)
                        {
                            obj = new Contents();
                            IsNewRecord = true;
                        }

                        obj.MainContent = obj.SubContent1 = model.MainContent;
                        obj.MainType = "ChargeToCC";
                        obj.Parent_ContentID = 0;
                        obj.MainOrdinal = 0;
                        obj.SubOrdinal1 = 0;
                        obj.SubOrdinal2 = 0;
                        obj.SubOrdinal3 = 0;
                        obj.SubContent2 = model.SubContent2 ? "active" : "inactive";

                        if (IsNewRecord)
                        {

                            db.Contents.Add(obj);
                            db.SaveChanges();
                            ViewBag.ResultMessage = "Charge To CC created for the Cost Center: " + obj.MainContent + " successfully!";
                            return RedirectToAction("ChargeToCCsListing", new { Success = true, Details = ViewBag.ResultMessage });
                        }
                        else
                        {
                            db.SaveChanges();
                            ViewBag.ResultMessage = "Charge To CC information updated for the Cost Center: " + obj.MainContent + " successfully!";
                            return RedirectToAction("ChargeToCCsListing", new { Success = true, Details = ViewBag.ResultMessage });
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

        public IActionResult ShipToListing(string Success, string Details)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            ShipToListingViewModel model = new ShipToListingViewModel();
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

            model.PageTitle = "Ship Tos Listing";

            using (var db = _DbContextFactory.CreateDbContext())
            {
                model.Records = db.Contents.Where(d => d.MainType == "ShipToCC").ToList();
            }
            return View(model);
        }

        public IActionResult DeleteShipTo(int ID)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            bool IsSuccessful = false;
            string DetailMessage = "";
            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    Contents x = db.Contents.FirstOrDefault(d => d.lngID == ID);
                    if (x != null)
                    {
                        db.Contents.Remove(x);
                        db.SaveChanges();
                        IsSuccessful = true;
                    }
                }

            }
            catch (Exception ex)
            {
                IsSuccessful = false;
                DetailMessage = ex.Message;
            }

            return RedirectToAction("ShipToListing", new { Success = IsSuccessful, Details = DetailMessage });
        }

        public IActionResult ShipToHandler(string Id)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            ShipToHandlerViewModel model = new ShipToHandlerViewModel();
            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    #region Load Existing Details

                    if (Id != null && Id.Length > 0)
                    {
                        int ID = Convert.ToInt32(Id);
                        Contents obj = db.Contents.First(d => d.lngID == ID);
                        model.lngID = obj.lngID;
                        model.SubContent1 = obj.SubContent1;
                        model.SubContent2 = obj.SubContent2;
                        model.SubContent3 = obj.SubContent3;
                        model.SubContent4 = obj.SubContent4;
                        model.SubContent5 = obj.SubContent5;
                        model.SubContent6 = obj.SubContent6;
                        model.SubContent7 = obj.SubContent7;
                        model.SubContent8 = obj.SubContent8 == "active" ? true : false;

                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                ViewBag.ResultMessage = "Error: " + ex.Message;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult ShipToHandler(ShipToHandlerViewModel model)
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
                    using (var db = _DbContextFactory.CreateDbContext())
                    {
                        bool IsNewRecord = false;
                        long ID = Convert.ToInt64(model.lngID);
                        Contents obj = db.Contents.FirstOrDefault(d => d.lngID == ID);
                        if (obj == null)
                        {
                            obj = new Contents();
                            IsNewRecord = true;
                        }

                        obj.MainContent = obj.SubContent1 = model.SubContent1;
                        obj.MainType = "ShipToCC";
                        obj.Parent_ContentID = 0;
                        obj.MainOrdinal = 0;
                        obj.SubOrdinal1 = 0;
                        obj.SubOrdinal2 = 0;
                        obj.SubOrdinal3 = 0;
                        obj.SubContent2 = model.SubContent2;
                        obj.SubContent3 = model.SubContent3;
                        obj.SubContent4 = model.SubContent4;
                        obj.SubContent5 = model.SubContent5;
                        obj.SubContent6 = model.SubContent6;
                        obj.SubContent7 = model.SubContent7;
                        obj.SubContent8 = model.SubContent8 ? "active" : "inactive";

                        if (IsNewRecord)
                        {

                            db.Contents.Add(obj);
                            db.SaveChanges();
                            ViewBag.ResultMessage = "Ship To created for the Company: " + obj.MainContent + " successfully!";
                            return RedirectToAction("ShipToListing", new { Success = true, Details = ViewBag.ResultMessage });
                        }
                        else
                        {
                            db.SaveChanges();
                            ViewBag.ResultMessage = "Ship To information updated for the Company: " + obj.MainContent + " successfully!";
                            return RedirectToAction("ShipToListing", new { Success = true, Details = ViewBag.ResultMessage });
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

        public IActionResult OrderListing(string Success, string Details)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            OrderListingViewModel model = new OrderListingViewModel();
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

            model.PageTitle = "Orders Listing";

            using (var db = _DbContextFactory.CreateDbContext())
            {
                model.Records = db.Orders.OrderByDescending(d => d.lngID).ToList();
                model.OCFRecords = db.OrderCheckoutFields.ToList();
            }
            return View(model);
        }

        public IActionResult DeleteOrder(int ID)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            bool IsSuccessful = false;
            string DetailMessage = "";
            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    Order x = db.Orders.FirstOrDefault(d => d.lngID == ID);
                    if (x != null)
                    {
                        List<OrderCheckoutField> OCFs = db.OrderCheckoutFields.Where(d => d.lngOrder == x.lngID).ToList();
                        db.OrderCheckoutFields.RemoveRange(OCFs);
                        db.SaveChanges();

                        db.Orders.Remove(x);
                        db.SaveChanges();
                        IsSuccessful = true;
                    }
                }

            }
            catch (Exception ex)
            {
                IsSuccessful = false;
                DetailMessage = ex.Message;
            }
            ViewBag.ResultMessage = "Order has been deleted successfully!";
            return RedirectToAction("OrderListing", new { Success = IsSuccessful, Details = DetailMessage });
        }

        public IActionResult ApprovalOrders(string Success, string Details)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();

            string SessionTerms = session.getSessionTerms();

            if (String.IsNullOrEmpty(SessionTerms))
                SessionTerms = "Not Yet";

            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            OrderListingViewModel model = new OrderListingViewModel();
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
            model.TermsAccepted = SessionTerms;

            model.PageTitle = "Approve Orders";

            using (var db = _DbContextFactory.CreateDbContext())
            {
                Account AA = db.Account.First(d => d.UserID == UserNameFromSession);

                DateTime CurrentDateTime = DateTime.Now.AddHours(-1);
                model.Records = db.Orders.Where(d => d.approver_acctID == AA.lngID && (d.status == "pending" || d.status== "approved")).OrderByDescending(d => d.lngID).ToList();
                model.OCFRecords = db.OrderCheckoutFields.ToList();
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult OrderApproved(int ID, string approveComment)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            bool IsSuccessful = false;
            string DetailMessage = "";
            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    Order x = db.Orders.FirstOrDefault(d => d.lngID == ID);
                    if (x != null)
                    {
                        x.datApproved = System.DateTime.Now;
                        x.status = "approved";

                        db.SaveChanges();
                        IsSuccessful = true;

                       
                        //send out email
                        //send out email to requester
                        //if (x.status=="approved")
                        //{
                        //    Account Requester = db.Account.FirstOrDefault(d => d.lngID == x.lngAccount);
                        //    string Subject = "";
                        //    string Body = "";

                        //    if (!String.IsNullOrEmpty(approveComment))
                        //    {
                        //        Subject = "Orderflow TBEN-" + x.lngID + " APPROVED/COMMENTED";
                        //        Body = "<p style='color:red;'>Approval Manager's Comments: <br/>" + approveComment + "</p><br/>" + GetOrderReceiptDetailsForEmail(ID.ToString());
                        //    }
                        //    else
                        //    {
                        //        Subject = "Orderflow TBEN-" + x.lngID + " APPROVED";
                        //        Body = "<p style='color:red;'>Approval Manager's Comments: <br/>" + approveComment + "</p><br/>" + GetOrderReceiptDetailsForEmail(ID.ToString());
                        //    }
                        //    //Body = "Dear " + Requester.Name + ", <br/><br/> Your order # TBEN-" + x.lngID + " submitted on " + x.datSubmitted.Value.ToShortDateString() + " has been approved. <br/><br/>";
                        //    List<Configuration> Emailconfs = db.Configurations.Where(x => x.Module == "Email Server Configuration").ToList<Configuration>();
                        //    //Body = Body + "<br/><br/Regards,<br/>Smart Source Orderflow Team";
                        //    SendEMail(Requester.Email, Subject, Body);

                        //    //send out email to approval manager
                        //    Account ApprovalManager = db.Account.FirstOrDefault(d => d.lngID == x.approver_acctID);
                        //    Subject = "";
                        //    Body = "";

                        //    if (!String.IsNullOrEmpty(approveComment))
                        //    {
                        //        Subject = "Orderflow TBEN-" + x.lngID + " APPROVED/COMMENTED [Approver Copy]";
                        //        Body = "<p style='color:red;'>Approval Manager's Comments: <br/>" + approveComment + "</p><br/>" + GetOrderReceiptDetailsForEmail(ID.ToString());
                        //    }
                        //    else
                        //    {
                        //        Subject = "Orderflow TBEN-" + x.lngID + " APPROVED [Approver Copy]";
                        //        Body = "<p style='color:red;'>Approval Manager's Comments: <br/>" + approveComment + "</p><br/>" + GetOrderReceiptDetailsForEmail(ID.ToString());
                        //    }
                        //    Emailconfs = db.Configurations.Where(x => x.Module == "Email Server Configuration").ToList<Configuration>();
                        //    //Body = Body + "<br/><br/Regards,<br/>Smart Source Orderflow Team";
                        //    SendEMail(ApprovalManager.Email, Subject, Body);

                        //    //send out email to vendor
                        //    Account Vendor = db.Account.FirstOrDefault(d => d.lngID == x.approver_acctID);
                        //    //Subject = "Smart Source Orderflow - Order # TBEN-" + x.lngID + " approved for processing!";
                        //    //Body = "Dear " + Vendor.Name + ", <br/><br/> Order # TBEN-" + x.lngID + " submitted by requester on " + x.datSubmitted.Value.ToShortDateString() + " has been approved for processing.<br/><br/>";
                        //    if (!String.IsNullOrEmpty(approveComment))
                        //    {
                        //        Subject = "Orderflow TBEN-" + x.lngID + " APPROVED/COMMENTED";
                        //        Body = "<p style='color:red;'>Approval Manager's Comments: <br/>" + approveComment + "</p><br/>" + GetOrderReceiptDetailsForEmail(ID.ToString());
                        //    }
                        //    else
                        //    {
                        //        Subject = "Orderflow TBEN-" + x.lngID + " APPROVED";
                        //        Body = "<p style='color:red;'>Approval Manager's Comments: <br/>" + approveComment + "</p><br/>" + GetOrderReceiptDetailsForEmail(ID.ToString());
                        //    }
                        //    Emailconfs = db.Configurations.Where(x => x.Module == "Email Server Configuration").ToList<Configuration>();
                        //    Body = Body + "<br/><br/Regards,<br/>Smart Source Orderflow Team";
                        //    SendEMail(ApprovalManager.Email, Subject, Body);

                            
                            
                            
                            
                        //}
                        
                    }
                }

            }
            catch (Exception ex)
            {
                IsSuccessful = false;
                DetailMessage = ex.Message;
            }
            return Json(IsSuccessful);
        }
        [HttpPost]
        public IActionResult SendApproveEmail(int ID, string approveComment)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            bool IsSuccessful = false;
            string DetailMessage = "";
            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    Order x = db.Orders.FirstOrDefault(d => d.lngID == ID);
                    if (x != null)
                    {
                        //send out email
                        //send out email to requester
                        if (x.status == "approved")
                        {
                            Account Requester = db.Account.FirstOrDefault(d => d.lngID == x.lngAccount);
                            string Subject = "";
                            string Body = "";

                            if (!String.IsNullOrEmpty(approveComment))
                            {
                                Subject = "Orderflow TBEN-" + x.lngID + " APPROVED/COMMENTED";
                                Body = "<p style='color:red;'>Approval Manager's Comments: <br/>" + approveComment + "</p><br/>" + GetOrderReceiptDetailsForEmail(ID.ToString());
                            }
                            else
                            {
                                Subject = "Orderflow TBEN-" + x.lngID + " APPROVED";
                                Body = "<p style='color:red;'>Approval Manager's Comments: <br/>" + approveComment + "</p><br/>" + GetOrderReceiptDetailsForEmail(ID.ToString());
                            }
                            //Body = "Dear " + Requester.Name + ", <br/><br/> Your order # TBEN-" + x.lngID + " submitted on " + x.datSubmitted.Value.ToShortDateString() + " has been approved. <br/><br/>";
                            List<Configuration> Emailconfs = db.Configurations.Where(x => x.Module == "Email Server Configuration").ToList<Configuration>();
                            //Body = Body + "<br/><br/Regards,<br/>Smart Source Orderflow Team";
                            SendEMail(Requester.Email, Subject, Body);

                            //send out email to approval manager
                            Account ApprovalManager = db.Account.FirstOrDefault(d => d.lngID == x.approver_acctID);
                            Subject = "";
                            Body = "";

                            if (!String.IsNullOrEmpty(approveComment))
                            {
                                Subject = "Orderflow TBEN-" + x.lngID + " APPROVED/COMMENTED [Approver Copy]";
                                Body = "<p style='color:red;'>Approval Manager's Comments: <br/>" + approveComment + "</p><br/>" + GetOrderReceiptDetailsForEmail(ID.ToString());
                            }
                            else
                            {
                                Subject = "Orderflow TBEN-" + x.lngID + " APPROVED [Approver Copy]";
                                Body = "<p style='color:red;'>Approval Manager's Comments: <br/>" + approveComment + "</p><br/>" + GetOrderReceiptDetailsForEmail(ID.ToString());
                            }
                            Emailconfs = db.Configurations.Where(x => x.Module == "Email Server Configuration").ToList<Configuration>();
                            //Body = Body + "<br/><br/Regards,<br/>Smart Source Orderflow Team";
                            SendEMail(ApprovalManager.Email, Subject, Body);

                            //send out email to vendor
                            Account Vendor = db.Account.FirstOrDefault(d => d.lngID == x.approver_acctID);
                            //Subject = "Smart Source Orderflow - Order # TBEN-" + x.lngID + " approved for processing!";
                            //Body = "Dear " + Vendor.Name + ", <br/><br/> Order # TBEN-" + x.lngID + " submitted by requester on " + x.datSubmitted.Value.ToShortDateString() + " has been approved for processing.<br/><br/>";
                            if (!String.IsNullOrEmpty(approveComment))
                            {
                                Subject = "Orderflow TBEN-" + x.lngID + " APPROVED/COMMENTED";
                                Body = "<p style='color:red;'>Approval Manager's Comments: <br/>" + approveComment + "</p><br/>" + GetOrderReceiptDetailsForEmail(ID.ToString());
                            }
                            else
                            {
                                Subject = "Orderflow TBEN-" + x.lngID + " APPROVED";
                                Body = "<p style='color:red;'>Approval Manager's Comments: <br/>" + approveComment + "</p><br/>" + GetOrderReceiptDetailsForEmail(ID.ToString());
                            }
                            Emailconfs = db.Configurations.Where(x => x.Module == "Email Server Configuration").ToList<Configuration>();
                            Body = Body + "<br/><br/Regards,<br/>Smart Source Orderflow Team";
                            SendEMail(ApprovalManager.Email, Subject, Body);


                            IsSuccessful=true;


                        }

                    }
                }

            }
            catch (Exception ex)
            {
                IsSuccessful = false;
                DetailMessage = ex.Message;
            }
            return Json(IsSuccessful);
        }


        public IActionResult OrderDenied(int ID, string comment)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            bool IsSuccessful = false;
            string DetailMessage = "";
            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    Order x = db.Orders.FirstOrDefault(d => d.lngID == ID);
                    if (x != null)
                    {
                        x.status = "denied";

                        //send out email to requester
                        Account Requester = db.Account.FirstOrDefault(d => d.lngID == x.lngAccount);

                        string Subject = "";
                        string Body = "";
                        if (!String.IsNullOrEmpty(comment))
                        {
                             Subject = "Orderflow TBEN-" + x.lngID + " DENIED/COMMENTED";
                             Body = "<p style='color:red;'>Approval Manager's Comments: <br/>" + comment + "</p><br/>" + GetOrderReceiptDetailsForEmail(ID.ToString());
                        }
                        else
                        {
                             Subject = "Orderflow TBEN-" + x.lngID + " DENIED";
                             Body = "<br/>" + GetOrderReceiptDetailsForEmail(ID.ToString());
                        }
                         //string Body = "Dear " + Requester.Name + ", <br/><br/> Your order # TBEN-" + x.lngID + " submitted on " + x.datSubmitted.Value.ToShortDateString() + " has been denied. <br/><br/>";
                        List<Configuration> Emailconfs = db.Configurations.Where(x => x.Module == "Email Server Configuration").ToList<Configuration>();
                        SendEMail(Requester.Email, Subject, Body);

                        //send out email to approval manager
                        Account ApprovalManager = db.Account.FirstOrDefault(d => d.lngID == x.approver_acctID);

                        //Subject = "Orderflow TBEN-" + x.lngID + " DENIED/COMMENTED [Approver Copy]";
                        //Body = "Dear " + ApprovalManager.Name + ", <br/><br/> Your approval order # TBEN-" + x.lngID + " submitted by requester on " + x.datSubmitted.Value.ToShortDateString() + " has been denied. Requester has been informed.<br/><br/>";
                         Subject = "";
                         Body = "";
                        if (!String.IsNullOrEmpty(comment))
                        {
                            Subject = "Orderflow TBEN-" + x.lngID + " DENIED/COMMENTED [Approver Copy]";
                            Body = "<p style='color:red;'>Approval Manager's Comments: <br/>" + comment + "</p><br/>" + GetOrderReceiptDetailsForEmail(ID.ToString());
                        }
                        else
                        {
                            Subject = "Orderflow TBEN-" + x.lngID + " DENIED [Approver Copy]";
                            Body = "<br/>" + GetOrderReceiptDetailsForEmail(ID.ToString());
                        }

                        Emailconfs = db.Configurations.Where(x => x.Module == "Email Server Configuration").ToList<Configuration>();
                        SendEMail(ApprovalManager.Email, Subject, Body);

                        db.SaveChanges();
                        IsSuccessful = true;
                    }
                }

            }
            catch (Exception ex)
            {
                IsSuccessful = false;
                DetailMessage = ex.Message;
            }
            DetailMessage = "Order has been denied! Email sent to requester and approveral manager for status change.";
            return RedirectToAction("ApprovalOrders", new { Success = IsSuccessful, Details = DetailMessage });
        }

        public IActionResult OrderCancelled(int ID, string comment)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            bool IsSuccessful = false;
            string DetailMessage = "";
            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    Order x = db.Orders.FirstOrDefault(d => d.lngID == ID);
                    if (x != null)
                    {
                        x.status = "canceled";

                        //send out email to requester
                        Account Requester = db.Account.FirstOrDefault(d => d.lngID == x.lngAccount);

                        string Subject = "";
                        string Body = "";
                        
                        Subject = "Orderflow TBEN-" + x.lngID + " CANCELED";
                        Body = "<p style='color:red;'>Approval Manager's Comments: <br/>" + comment + "</p><br/>" + GetOrderReceiptDetailsForEmail(ID.ToString());
                        //string Body = "Dear " + Requester.Name + ", <br/><br/> Your order # TBEN-" + x.lngID + " submitted on " + x.datSubmitted.Value.ToShortDateString() + " has been denied. <br/><br/>";
                        List<Configuration> Emailconfs = db.Configurations.Where(x => x.Module == "Email Server Configuration").ToList<Configuration>();
                        SendEMail(Requester.Email, Subject, Body);

                        //send out email to approval manager
                        Account ApprovalManager = db.Account.FirstOrDefault(d => d.lngID == x.approver_acctID);

                        //Subject = "Orderflow TBEN-" + x.lngID + " DENIED/COMMENTED [Approver Copy]";
                        //Body = "Dear " + ApprovalManager.Name + ", <br/><br/> Your approval order # TBEN-" + x.lngID + " submitted by requester on " + x.datSubmitted.Value.ToShortDateString() + " has been denied. Requester has been informed.<br/><br/>";
                        Subject = "";
                        Body = "";
   
                        Subject = "Orderflow TBEN-" + x.lngID + " CANCELED";
                        Body = "<p style='color:red;'>Approval Manager's Comments: <br/>" + comment + "</p><br/>" + GetOrderReceiptDetailsForEmail(ID.ToString());
                        
                        Emailconfs = db.Configurations.Where(x => x.Module == "Email Server Configuration").ToList<Configuration>();
                        SendEMail(ApprovalManager.Email, Subject, Body);

                        db.SaveChanges();
                        IsSuccessful = true;
                    }
                }

            }
            catch (Exception ex)
            {
                IsSuccessful = false;
                DetailMessage = ex.Message;
            }
            DetailMessage = "Order has been canceled! Email sent to requester and approveral manager for status change.";
            return RedirectToAction("ApprovalOrders", new { Success = IsSuccessful, Details = DetailMessage });
        }

        public ActionResult GetOrderReceiptDetails(string maincontent)
        {
            using (var db = _DbContextFactory.CreateDbContext())
            {
                if (!String.IsNullOrEmpty(maincontent))
                {
                    int lngID = Convert.ToInt32(maincontent);
                    Order obj = db.Orders.First(d => d.lngID == lngID);

                    List<Configuration> Emailconfs = db.Configurations.Where(x => x.Module == "Order Template").ToList<Configuration>();
                    string Body = ((Configuration)Emailconfs.Find(y => y.Name == "Order Template")).Value;

                    Body = Body.Replace("@OrderNumber", "TBEN-" + obj.lngID.ToString());
                    Body = Body.Replace("@OrderDate", obj.datSubmitted.Value.ToString("MM/dd/yyyy h:mm tt"));

                    List<OrderCheckoutField> OCFs = db.OrderCheckoutFields.Where(d => d.lngOrder == obj.lngID).ToList();
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

                    Body = Body.Replace("@JobName", obj.job_details_jobName);

                    string DocumentListBuilder = "";
                    if (obj.document_uploadFiles.Split(',').Length > 0)
                    {
                        Account Requester = db.Account.FirstOrDefault(d => d.lngID == obj.lngAccount);
                        var folderName = Path.Combine("Repository", "Orders", "UOD" + Requester.UserID, "Document");
                        Configuration SiteURL = db.Configurations.First(x => x.Module == "SiteURL");

                        if (Requester != null)
                        {
                            DocumentListBuilder = "<span><b>" + obj.document_uploadFiles.Split(',').Length + "</b> file(s)</span> <ol>";
                            foreach (string fileselected in obj.document_uploadFiles.Split(','))
                            {
                                DocumentListBuilder = DocumentListBuilder + "<li><a href='" + SiteURL.Value + "/Admin/DownloadFile?filePath=" + folderName + "/" + fileselected + "' target='_blank'>" + fileselected + "</a></li>";
                            }
                            DocumentListBuilder = DocumentListBuilder + "</ol>";
                        }
                    }
                    Body = Body.Replace("@DocumentList", DocumentListBuilder);

                    Body = Body.Replace("@Envelope", "Envelope");
                    Body = Body.Replace("@EnvelopeSize", obj.job_details_envSize);

                    DocumentListBuilder = "";
                    if (!String.IsNullOrEmpty(obj.mailing_list_uploadFiles) && obj.mailing_list_uploadFiles.Split(',').Length > 0)
                    {
                        Account Requester = db.Account.FirstOrDefault(d => d.lngID == obj.lngAccount);
                        var folderName = Path.Combine("Repository", "Orders", "UOD" + Requester.UserID, "MailingList");
                        Configuration SiteURL = db.Configurations.First(x => x.Module == "SiteURL");

                        DocumentListBuilder = "<span><b>" + obj.mailing_list_uploadFiles.Split(',').Length + "</b> file(s)</span> <ol>";
                        foreach (string fileselected in obj.mailing_list_uploadFiles.Split(','))
                        {
                            DocumentListBuilder = DocumentListBuilder + "<li><a href='" + SiteURL.Value + "/Admin/DownloadFile?filePath=" + folderName + "/" + fileselected + "' target='_blank'>" + fileselected + "</a></li>";
                        }
                        DocumentListBuilder = DocumentListBuilder + "</ol>";
                    }
                    Body = Body.Replace("@MailingList", DocumentListBuilder);

                    Body = Body.Replace("@JobPrint", obj.job_details_jobPrints);
                    Body = Body.Replace("@MailDropDate", obj.job_details_mailDropDate.HasValue ? obj.job_details_mailDropDate.Value.ToString("MM/dd/yyyy") : "");
                    Body = Body.Replace("@RushJob", obj.job_details_rushJob);
                    Body = Body.Replace("@SpecialInstructions", obj.job_details_specInstr);

                    return Json(Body);
                }
                else
                {
                    string obj = "";
                    return Json(obj);
                }
            }
        }

        



        public string GetOrderReceiptDetailsForEmail(string maincontent)
        {
            using (var db = _DbContextFactory.CreateDbContext())
            {
                if (!String.IsNullOrEmpty(maincontent))
                {
                    int lngID = Convert.ToInt32(maincontent);
                    Order obj = db.Orders.First(d => d.lngID == lngID);

                    List<Configuration> Emailconfs = db.Configurations.Where(x => x.Module == "Order Template").ToList<Configuration>();
                    string Body = ((Configuration)Emailconfs.Find(y => y.Name == "Order Template")).Value;

                    Body = Body.Replace("@OrderNumber", "TBEN-" + obj.lngID.ToString());
                    Body = Body.Replace("@OrderDate", obj.datSubmitted.Value.ToString("MM/dd/yyyy h:mm tt"));

                    List<OrderCheckoutField> OCFs = db.OrderCheckoutFields.Where(d => d.lngOrder == obj.lngID).ToList();
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

                    Body = Body.Replace("@JobName", obj.job_details_jobName);

                    string DocumentListBuilder = "";
                    if (obj.document_uploadFiles.Split(',').Length > 0)
                    {
                        Account Requester = db.Account.FirstOrDefault(d => d.lngID == obj.lngAccount);
                        var folderName = Path.Combine("Repository", "Orders", "UOD" + Requester.UserID, "Document");
                        Configuration SiteURL = db.Configurations.First(x => x.Module == "SiteURL");

                        if (Requester != null)
                        {
                            DocumentListBuilder = "<span><b>" + obj.document_uploadFiles.Split(',').Length + "</b> file(s)</span> <ol>";
                            foreach (string fileselected in obj.document_uploadFiles.Split(','))
                            {
                                DocumentListBuilder = DocumentListBuilder + "<li><a href='" + SiteURL.Value + "/Admin/DownloadFile?filePath=" + folderName + "/" + fileselected + "' target='_blank'>" + fileselected + "</a></li>";
                            }
                            DocumentListBuilder = DocumentListBuilder + "</ol>";
                        }
                    }

                    Body = Body.Replace("@DocumentList", DocumentListBuilder);
                    if (obj.envelope_product_City_State_Zip_Line_5!=null)
                    {
                        Body = Body.Replace("@Envelope", obj.envelope_product_City_State_Zip_Line_5);
                    }
                    else
                    {
                        Body = Body.Replace("@Envelope", "None");
                    }
                    if (obj.job_details_envSize!=null)
                    {
                        Body = Body.Replace("@EnvelopeSize", obj.job_details_envSize);
                    }
                    else
                    {
                        Body = Body.Replace("@EnvelopeSize", "None");
                    }
                    

                    var DocumentListBuilder1 = "";
                    if (!String.IsNullOrEmpty(obj.mailing_list_uploadFiles) && obj.mailing_list_uploadFiles.Split(',').Length > 0)
                    {
                        Account Requester = db.Account.FirstOrDefault(d => d.lngID == obj.lngAccount);
                        var folderName = Path.Combine("Repository", "Orders", "UOD" + Requester.UserID, "MailingList");
                        Configuration SiteURL = db.Configurations.First(x => x.Module == "SiteURL");

                        DocumentListBuilder1 = "<span><b>" + obj.mailing_list_uploadFiles.Split(',').Length + "</b> file(s)</span> <ol>";
                        foreach (string fileselected in obj.mailing_list_uploadFiles.Split(','))
                        {
                            DocumentListBuilder1 = DocumentListBuilder1 + "<li><a href='" + SiteURL.Value + "/Admin/DownloadFile?filePath=" + folderName + "/" + fileselected + "' target='_blank'>" + fileselected + "</a></li>";
                        }
                        DocumentListBuilder1 = DocumentListBuilder1 + "</ol>";
                    }

                    if (obj.job_details_includesMailList!=null)
                    {
                        Body = Body.Replace("@MailingList", DocumentListBuilder1);
                    }
                    else
                    {
                        Body = Body.Replace("@MailingList", "No Mailing List");
                    }
                   

                    Body = Body.Replace("@JobPrint", obj.job_details_jobPrints);
                    Body = Body.Replace("@MailDropDate", obj.job_details_mailDropDate.HasValue ? obj.job_details_mailDropDate.Value.ToString("MM/dd/yyyy") : "");
                    Body = Body.Replace("@RushJob", obj.job_details_rushJob);
                    Body = Body.Replace("@SpecialInstructions", obj.job_details_specInstr);

                    return Body;
                }
                else
                {
                    string obj = "";
                    return obj;
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

        public void SendEMailWithAttachments(string emailid, string subject, string body, string AttachmentPath)
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
            msg.Attachments.Add(new Attachment(@AttachmentPath));

            client.Send(msg);
        }

        public void SendEMailWithTwoAttachments(string emailid, string subject, string body, string AttachmentPath, string SecondAttachmentPath)
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
            msg.Attachments.Add(new Attachment(@AttachmentPath));
            msg.Attachments.Add(new Attachment(SecondAttachmentPath));

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

        public IActionResult InvoiceListing(string Success, string Details)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            InvoiceListingViewModel model = new InvoiceListingViewModel();
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

            model.PageTitle = "Invoice Listing";

            using (var db = _DbContextFactory.CreateDbContext())
            {
                List<InvoiceList> ILS = new List<InvoiceList>();
                List<Order> ORDs = db.Orders.Where(d => d.status == "approved").OrderByDescending(d => d.lngID).ToList();
                List<OrderCheckoutField> ORDCOFs = db.OrderCheckoutFields.ToList();

                List<Invoice> INVs = db.Invoices.ToList();
                Configuration SiteURL = db.Configurations.First(x => x.Module == "SiteURL");
                var folderName = Path.Combine("Repository", "Invoices", "ClientInvoices");

                foreach (Order ORD in ORDs)
                {
                    InvoiceList IL = new InvoiceList();
                    IL.OrderID = ORD.lngID;
                    IL.OrderDate = ORD.datSubmitted.HasValue ? ORD.datSubmitted.Value.ToString("MM/dd/yyyy") : "";
                    IL.DateMailed = ORD.job_details_mailDropDate.HasValue ? ORD.job_details_mailDropDate.Value.ToString("MM/dd/yyyy") : "";
                    IL.JobName = String.IsNullOrEmpty(ORD.job_details_jobName) ? "" : (ORD.job_details_jobName.Length <= 10 ? ORD.job_details_jobName : ORD.job_details_jobName.Substring(0, 10) + "..."); ;
                    IL.Status = ORD.vendor_status;
                    if (INVs.FirstOrDefault(d => d.orderID == ORD.lngID) != null)
                    {
                        IL.Status = INVs.First(d => d.orderID == ORD.lngID).status;
                        IL.InvoiceID = INVs.First(d => d.orderID == ORD.lngID).lngID;
                        IL.InvoiceDate = INVs.First(d => d.orderID == ORD.lngID).datBillSubmitted.HasValue ? INVs.First(d => d.orderID == ORD.lngID).datBillSubmitted.Value.ToString("MM/dd/yyyy") : ""; ;
                        IL.ClientInvoiceUploadDate = INVs.First(d => d.orderID == ORD.lngID).datClientInvoiceUploaded.HasValue ? INVs.First(d => d.orderID == ORD.lngID).datClientInvoiceUploaded.Value.ToString("MM/dd/yyyy") : ""; ;
                        if (!String.IsNullOrEmpty(INVs.First(d => d.orderID == ORD.lngID).clientInvoiceFile))
                            IL.ClientInvoiceName = SiteURL.Value + "/Admin/DownloadFile?filePath=" + folderName + "/" + INVs.First(d => d.orderID == ORD.lngID).clientInvoiceFile;
                        else
                            IL.ClientInvoiceName = "";

                        IL.ClientInvoiceDate = INVs.First(d => d.orderID == ORD.lngID).datClientInvoiced.HasValue ? INVs.First(d => d.orderID == ORD.lngID).datClientInvoiced.Value.ToString("MM/dd/yyyy") : "";
                        IL.ClientPaymentDate = INVs.First(d => d.orderID == ORD.lngID).datClientPayment.HasValue ? INVs.First(d => d.orderID == ORD.lngID).datClientPayment.Value.ToString("MM/dd/yyyy") : "N/A";
                        IL.InvoiceRecipient = ORDCOFs.FirstOrDefault(d => d.lngOrder == ORD.lngID && d.name == "Contact_Requestor Email") != null ? ORDCOFs.FirstOrDefault(d => d.lngOrder == ORD.lngID && d.name == "Contact_Requestor Email").value : "";
                    }

                    ILS.Add(IL);
                }
                model.Records = ILS.ToList();
                model.OCFRecords = db.OrderCheckoutFields.ToList();
                model.UserRole = session.getSessionRoles();
            }
            return View(model);
        }

        public IActionResult CreateInvoice(int OrderId)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            InvoiceHandlerViewModel model = new InvoiceHandlerViewModel();
            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {

                    if (OrderId > 0)
                    {
                        Order ORD = db.Orders.FirstOrDefault(d => d.lngID == OrderId);
                        model.InvoiceOrder = ORD;

                        List<InvoiceLinesTemplate> ILTs = db.InvoiceLinesTemplates.ToList();
                        List<InvoiceLine> LIs = new List<InvoiceLine>();

                        List<InvoiceLinesTemplate> ILTsPrinting = ILTs.Where(d => d.lineType.Contains("Printing")).OrderBy(d => d.displayOrder).ToList();
                        int index = 0;

                        #region Printing Line Items

                        model.il1linetype = ILTsPrinting[index].lineType;
                        model.il1display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                        model.il1name = ILTsPrinting[index].name;
                        model.il1size = ILTsPrinting[index].size;
                        model.il1qty = 0;
                        model.il1UOM = ILTsPrinting[index].UOM;
                        model.il1unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                        model.il1extCost = 0;
                        model.il1unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                        model.il1extSell = 0;
                        model.il1minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                        index = index + 1;
                        model.il2linetype = ILTsPrinting[index].lineType;
                        model.il2display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                        model.il2name = ILTsPrinting[index].name;
                        model.il2size = ILTsPrinting[index].size;
                        model.il2qty = 0;
                        model.il2UOM = ILTsPrinting[index].UOM;
                        model.il2unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                        model.il2extCost = 0;
                        model.il2unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                        model.il2extSell = 0;
                        model.il2minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                        index = index + 1;
                        model.il3linetype = ILTsPrinting[index].lineType;
                        model.il3display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                        model.il3name = ILTsPrinting[index].name;
                        model.il3size = ILTsPrinting[index].size;
                        model.il3qty = 0;
                        model.il3UOM = ILTsPrinting[index].UOM;
                        model.il3unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                        model.il3extCost = 0;
                        model.il3unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                        model.il3extSell = 0;
                        model.il3minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                        index = index + 1;
                        model.il4linetype = ILTsPrinting[index].lineType;
                        model.il4display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                        model.il4name = ILTsPrinting[index].name;
                        model.il4size = ILTsPrinting[index].size;
                        model.il4qty = 0;
                        model.il4UOM = ILTsPrinting[index].UOM;
                        model.il4unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                        model.il4extCost = 0;
                        model.il4unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                        model.il4extSell = 0;
                        model.il4minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                        index = index + 1;
                        model.il5linetype = ILTsPrinting[index].lineType;
                        model.il5display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                        model.il5name = ILTsPrinting[index].name;
                        model.il5size = ILTsPrinting[index].size;
                        model.il5qty = 0;
                        model.il5UOM = ILTsPrinting[index].UOM;
                        model.il5unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                        model.il5extCost = 0;
                        model.il5unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                        model.il5extSell = 0;
                        model.il5minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                        index = index + 1;
                        model.il6linetype = ILTsPrinting[index].lineType;
                        model.il6display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                        model.il6name = ILTsPrinting[index].name;
                        model.il6size = ILTsPrinting[index].size;
                        model.il6qty = 0;
                        model.il6UOM = ILTsPrinting[index].UOM;
                        model.il6unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                        model.il6extCost = 0;
                        model.il6unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                        model.il6extSell = 0;
                        model.il6minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                        index = index + 1;
                        model.il7linetype = ILTsPrinting[index].lineType;
                        model.il7display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                        model.il7name = ILTsPrinting[index].name;
                        model.il7size = ILTsPrinting[index].size;
                        model.il7qty = 0;
                        model.il7UOM = ILTsPrinting[index].UOM;
                        model.il7unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                        model.il7extCost = 0;
                        model.il7unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                        model.il7extSell = 0;
                        model.il7minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                        index = index + 1;
                        model.il8linetype = ILTsPrinting[index].lineType;
                        model.il8display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                        model.il8name = ILTsPrinting[index].name;
                        model.il8size = ILTsPrinting[index].size;
                        model.il8qty = 0;
                        model.il8UOM = ILTsPrinting[index].UOM;
                        model.il8unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                        model.il8extCost = 0;
                        model.il8unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                        model.il8extSell = 0;
                        model.il8minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                        index = index + 1;
                        model.il9linetype = ILTsPrinting[index].lineType;
                        model.il9display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                        model.il9name = ILTsPrinting[index].name;
                        model.il9size = ILTsPrinting[index].size;
                        model.il9qty = 0;
                        model.il9UOM = ILTsPrinting[index].UOM;
                        model.il9unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                        model.il9extCost = 0;
                        model.il9unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                        model.il9extSell = 0;
                        model.il9minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                        index = index + 1;
                        model.il10linetype = ILTsPrinting[index].lineType;
                        model.il10display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                        model.il10name = ILTsPrinting[index].name;
                        model.il10size = ILTsPrinting[index].size;
                        model.il10qty = 0;
                        model.il10UOM = ILTsPrinting[index].UOM;
                        model.il10unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                        model.il10extCost = 0;
                        model.il10unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                        model.il10extSell = 0;
                        model.il10minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                        index = index + 1;
                        model.il11linetype = ILTsPrinting[index].lineType;
                        model.il11display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                        model.il11name = ILTsPrinting[index].name;
                        model.il11size = ILTsPrinting[index].size;
                        model.il11qty = 0;
                        model.il11UOM = ILTsPrinting[index].UOM;
                        model.il11unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                        model.il11extCost = 0;
                        model.il11unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                        model.il11extSell = 0;
                        model.il11minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                        #endregion

                        List<InvoiceLinesTemplate> ILTsMailing = ILTs.Where(d => d.lineType.Contains("Mailing")).OrderBy(d => d.displayOrder).ToList();
                        int indexNew = 0;

                        #region Mailing Line Items

                        model.il20linetype = ILTsMailing[indexNew].lineType;
                        model.il20display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il20name = ILTsMailing[indexNew].name;
                        model.il20size = ILTsMailing[indexNew].size;
                        model.il20qty = 0;
                        model.il20UOM = ILTsMailing[indexNew].UOM;
                        model.il20unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il20extCost = 0;
                        model.il20unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il20extSell = 0;
                        model.il20minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il21linetype = ILTsMailing[indexNew].lineType;
                        model.il21display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il21name = ILTsMailing[indexNew].name;
                        model.il21size = ILTsMailing[indexNew].size;
                        model.il21qty = 0;
                        model.il21UOM = ILTsMailing[indexNew].UOM;
                        model.il21unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il21extCost = 0;
                        model.il21unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il21extSell = 0;
                        model.il21minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il22linetype = ILTsMailing[indexNew].lineType;
                        model.il22display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il22name = ILTsMailing[indexNew].name;
                        model.il22size = ILTsMailing[indexNew].size;
                        model.il22qty = 0;
                        model.il22UOM = ILTsMailing[indexNew].UOM;
                        model.il22unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il22extCost = 0;
                        model.il22unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il22extSell = 0;
                        model.il22minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il23linetype = ILTsMailing[indexNew].lineType;
                        model.il23display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il23name = ILTsMailing[indexNew].name;
                        model.il23size = ILTsMailing[indexNew].size;
                        model.il23qty = 0;
                        model.il23UOM = ILTsMailing[indexNew].UOM;
                        model.il23unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il23extCost = 0;
                        model.il23unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il23extSell = 0;
                        model.il23minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il24linetype = ILTsMailing[indexNew].lineType;
                        model.il24display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il24name = ILTsMailing[indexNew].name;
                        model.il24size = ILTsMailing[indexNew].size;
                        model.il24qty = 0;
                        model.il24UOM = ILTsMailing[indexNew].UOM;
                        model.il24unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il24extCost = 0;
                        model.il24unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il24extSell = 0;
                        model.il24minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il25linetype = ILTsMailing[indexNew].lineType;
                        model.il25display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il25name = ILTsMailing[indexNew].name;
                        model.il25size = ILTsMailing[indexNew].size;
                        model.il25qty = 0;
                        model.il25UOM = ILTsMailing[indexNew].UOM;
                        model.il25unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il25extCost = 0;
                        model.il25unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il25extSell = 0;
                        model.il25minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il26linetype = ILTsMailing[indexNew].lineType;
                        model.il26display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il26name = ILTsMailing[indexNew].name;
                        model.il26size = ILTsMailing[indexNew].size;
                        model.il26qty = 0;
                        model.il26UOM = ILTsMailing[indexNew].UOM;
                        model.il26unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il26extCost = 0;
                        model.il26unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il26extSell = 0;
                        model.il26minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il27linetype = ILTsMailing[indexNew].lineType;
                        model.il27display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il27name = ILTsMailing[indexNew].name;
                        model.il27size = ILTsMailing[indexNew].size;
                        model.il27qty = 0;
                        model.il27UOM = ILTsMailing[indexNew].UOM;
                        model.il27unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il27extCost = 0;
                        model.il27unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il27extSell = 0;
                        model.il27minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il28linetype = ILTsMailing[indexNew].lineType;
                        model.il28display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il28name = ILTsMailing[indexNew].name;
                        model.il28size = ILTsMailing[indexNew].size;
                        model.il28qty = 0;
                        model.il28UOM = ILTsMailing[indexNew].UOM;
                        model.il28unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il28extCost = 0;
                        model.il28unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il28extSell = 0;
                        model.il28minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il29linetype = ILTsMailing[indexNew].lineType;
                        model.il29display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il29name = ILTsMailing[indexNew].name;
                        model.il29size = ILTsMailing[indexNew].size;
                        model.il29qty = 0;
                        model.il29UOM = ILTsMailing[indexNew].UOM;
                        model.il29unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il29extCost = 0;
                        model.il29unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il29extSell = 0;
                        model.il29minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il30linetype = ILTsMailing[indexNew].lineType;
                        model.il30display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il30name = ILTsMailing[indexNew].name;
                        model.il30size = ILTsMailing[indexNew].size;
                        model.il30qty = 0;
                        model.il30UOM = ILTsMailing[indexNew].UOM;
                        model.il30unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il30extCost = 0;
                        model.il30unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il30extSell = 0;
                        model.il30minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il31linetype = ILTsMailing[indexNew].lineType;
                        model.il31display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il31name = ILTsMailing[indexNew].name;
                        model.il31size = ILTsMailing[indexNew].size;
                        model.il31qty = 0;
                        model.il31UOM = ILTsMailing[indexNew].UOM;
                        model.il31unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il31extCost = 0;
                        model.il31unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il31extSell = 0;
                        model.il31minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il32linetype = ILTsMailing[indexNew].lineType;
                        model.il32display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il32name = ILTsMailing[indexNew].name;
                        model.il32size = ILTsMailing[indexNew].size;
                        model.il32qty = 0;
                        model.il32UOM = ILTsMailing[indexNew].UOM;
                        model.il32unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il32extCost = 0;
                        model.il32unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il32extSell = 0;
                        model.il32minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il33linetype = ILTsMailing[indexNew].lineType;
                        model.il33display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il33name = ILTsMailing[indexNew].name;
                        model.il33size = ILTsMailing[indexNew].size;
                        model.il33qty = 0;
                        model.il33UOM = ILTsMailing[indexNew].UOM;
                        model.il33unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il33extCost = 0;
                        model.il33unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il33extSell = 0;
                        model.il33minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il34linetype = ILTsMailing[indexNew].lineType;
                        model.il34display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il34name = ILTsMailing[indexNew].name;
                        model.il34size = ILTsMailing[indexNew].size;
                        model.il34qty = 0;
                        model.il34UOM = ILTsMailing[indexNew].UOM;
                        model.il34unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il34extCost = 0;
                        model.il34unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il34extSell = 0;
                        model.il34minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il35linetype = ILTsMailing[indexNew].lineType;
                        model.il35display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il35name = ILTsMailing[indexNew].name;
                        model.il35size = ILTsMailing[indexNew].size;
                        model.il35qty = 0;
                        model.il35UOM = ILTsMailing[indexNew].UOM;
                        model.il35unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il35extCost = 0;
                        model.il35unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il35extSell = 0;
                        model.il35minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il36linetype = ILTsMailing[indexNew].lineType;
                        model.il36display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il36name = ILTsMailing[indexNew].name;
                        model.il36size = ILTsMailing[indexNew].size;
                        model.il36qty = 0;
                        model.il36UOM = ILTsMailing[indexNew].UOM;
                        model.il36unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il36extCost = 0;
                        model.il36unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il36extSell = 0;
                        model.il36minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        #endregion

                        #region Freight

                        List<InvoiceLinesTemplate> ILTsFrieght = ILTs.Where(d => d.lineType.Contains("Freight")).OrderBy(d => d.displayOrder).ToList();
                        indexNew = 0;
                        model.il40linetype = ILTsFrieght[indexNew].lineType;
                        model.il40display = ILTsFrieght[indexNew].displayOrder.HasValue ? ILTsFrieght[indexNew].displayOrder.Value : 0;
                        model.il40name = ILTsFrieght[indexNew].name;
                        model.il40size = ILTsFrieght[indexNew].size;
                        model.il40qty = 0;
                        model.il40UOM = ILTsFrieght[indexNew].UOM;
                        model.il40unitCost = ILTsFrieght[indexNew].unitCost.HasValue ? ILTsFrieght[indexNew].unitCost.Value : 0;
                        model.il40extCost = 0;
                        model.il40unitSell = ILTsFrieght[indexNew].unitSell.HasValue ? ILTsFrieght[indexNew].unitSell.Value : 0;
                        model.il40extSell = 0;
                        model.il40minExtSell = ILTsFrieght[indexNew].minExtSell.HasValue ? ILTsFrieght[indexNew].minExtSell.Value : 0;

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ResultMessage = "Error: " + ex.Message;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateInvoice(InvoiceHandlerViewModel model)
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
                    using (var db = _DbContextFactory.CreateDbContext())
                    {
                        Invoice obj = new Invoice();

                        obj.orderID = model.orderID;
                        obj.status = "Submitted by vendor";
                        obj.vendorInvoiceNo = model.vendorInvoiceNo;
                        obj.comment = model.comment;
                        obj.datBillSubmitted = DateTime.Now;
                        obj.datBillRevised = null;
                        obj.datClientInvoiced = DateTime.Now;
                        obj.datClientInvoiceUploaded = DateTime.Now;
                        obj.datClientPayment = null;
                        obj.lastTouch = "";
                        obj.clientInvoiceFile = "";
                        obj.vendorInvoiceFile = "";
                        obj.internalInvoiceFile = "";
                        db.Invoices.Add(obj);
                        db.SaveChanges();

                        #region Printing Line Items
                        InvoiceLine IL1 = new InvoiceLine();
                        IL1.invoiceID = obj.lngID;
                        IL1.lineType = model.il1linetype;
                        IL1.displayOrder = model.il1display;
                        IL1.name = model.il1name;
                        IL1.size = model.il1size;
                        IL1.qty = model.il1qty;
                        IL1.UOM = model.il1UOM;
                        IL1.unitCost = model.il1unitCost;
                        IL1.extCost = model.il1extCost;
                        IL1.unitSell = model.il1unitSell;
                        IL1.extSell = model.il1extSell;
                        IL1.minExtSell = model.il1minExtSell;
                        IL1.minExtCost = 0;
                        db.InvoiceLines.Add(IL1);

                        InvoiceLine IL2 = new InvoiceLine();
                        IL2.invoiceID = obj.lngID;
                        IL2.lineType = model.il2linetype;
                        IL2.displayOrder = model.il2display;
                        IL2.name = model.il2name;
                        IL2.size = model.il2size;
                        IL2.qty = model.il2qty;
                        IL2.UOM = model.il2UOM;
                        IL2.unitCost = model.il2unitCost;
                        IL2.extCost = model.il2extCost;
                        IL2.unitSell = model.il2unitSell;
                        IL2.extSell = model.il2extSell;
                        IL2.minExtSell = model.il2minExtSell;
                        IL2.minExtCost = 0;
                        db.InvoiceLines.Add(IL2);

                        InvoiceLine IL3 = new InvoiceLine();
                        IL3.invoiceID = obj.lngID;
                        IL3.lineType = model.il3linetype;
                        IL3.displayOrder = model.il3display;
                        IL3.name = model.il3name;
                        IL3.size = model.il3size;
                        IL3.qty = model.il3qty;
                        IL3.UOM = model.il3UOM;
                        IL3.unitCost = model.il3unitCost;
                        IL3.extCost = model.il3extCost;
                        IL3.unitSell = model.il3unitSell;
                        IL3.extSell = model.il3extSell;
                        IL3.minExtSell = model.il3minExtSell;
                        IL3.minExtCost = 0;
                        db.InvoiceLines.Add(IL3);

                        InvoiceLine IL4 = new InvoiceLine();
                        IL4.invoiceID = obj.lngID;
                        IL4.lineType = model.il4linetype;
                        IL4.displayOrder = model.il4display;
                        IL4.name = model.il4name;
                        IL4.size = model.il4size;
                        IL4.qty = model.il4qty;
                        IL4.UOM = model.il4UOM;
                        IL4.unitCost = model.il4unitCost;
                        IL4.extCost = model.il4extCost;
                        IL4.unitSell = model.il4unitSell;
                        IL4.extSell = model.il4extSell;
                        IL4.minExtSell = model.il4minExtSell;
                        IL4.minExtCost = 0;
                        db.InvoiceLines.Add(IL4);

                        InvoiceLine IL5 = new InvoiceLine();
                        IL5.invoiceID = obj.lngID;
                        IL5.lineType = model.il5linetype;
                        IL5.displayOrder = model.il5display;
                        IL5.name = model.il5name;
                        IL5.size = model.il5size;
                        IL5.qty = model.il5qty;
                        IL5.UOM = model.il5UOM;
                        IL5.unitCost = model.il5unitCost;
                        IL5.extCost = model.il5extCost;
                        IL5.unitSell = model.il5unitSell;
                        IL5.extSell = model.il5extSell;
                        IL5.minExtSell = model.il5minExtSell;
                        IL5.minExtCost = 0;
                        db.InvoiceLines.Add(IL5);

                        InvoiceLine IL6 = new InvoiceLine();
                        IL6.invoiceID = obj.lngID;
                        IL6.lineType = model.il6linetype;
                        IL6.displayOrder = model.il6display;
                        IL6.name = model.il6name;
                        IL6.size = model.il6size;
                        IL6.qty = model.il6qty;
                        IL6.UOM = model.il6UOM;
                        IL6.unitCost = model.il6unitCost;
                        IL6.extCost = model.il6extCost;
                        IL6.unitSell = model.il6unitSell;
                        IL6.extSell = model.il6extSell;
                        IL6.minExtSell = model.il6minExtSell;
                        IL6.minExtCost = 0;
                        db.InvoiceLines.Add(IL6);

                        InvoiceLine IL7 = new InvoiceLine();
                        IL7.invoiceID = obj.lngID;
                        IL7.lineType = model.il7linetype;
                        IL7.displayOrder = model.il7display;
                        IL7.name = model.il7name;
                        IL7.size = model.il7size;
                        IL7.qty = model.il7qty;
                        IL7.UOM = model.il7UOM;
                        IL7.unitCost = model.il7unitCost;
                        IL7.extCost = model.il7extCost;
                        IL7.unitSell = model.il7unitSell;
                        IL7.extSell = model.il7extSell;
                        IL7.minExtSell = model.il7minExtSell;
                        IL7.minExtCost = 0;
                        db.InvoiceLines.Add(IL7);

                        InvoiceLine IL8 = new InvoiceLine();
                        IL8.invoiceID = obj.lngID;
                        IL8.lineType = model.il8linetype;
                        IL8.displayOrder = model.il8display;
                        IL8.name = model.il8name;
                        IL8.size = model.il8size;
                        IL8.qty = model.il8qty;
                        IL8.UOM = model.il8UOM;
                        IL8.unitCost = model.il8unitCost;
                        IL8.extCost = model.il8extCost;
                        IL8.unitSell = model.il8unitSell;
                        IL8.extSell = model.il8extSell;
                        IL8.minExtSell = model.il8minExtSell;
                        IL8.minExtCost = 0;
                        db.InvoiceLines.Add(IL8);

                        InvoiceLine IL9 = new InvoiceLine();
                        IL9.invoiceID = obj.lngID;
                        IL9.lineType = model.il9linetype;
                        IL9.displayOrder = model.il9display;
                        IL9.name = model.il9name;
                        IL9.size = model.il9size;
                        IL9.qty = model.il9qty;
                        IL9.UOM = model.il9UOM;
                        IL9.unitCost = model.il9unitCost;
                        IL9.extCost = model.il9extCost;
                        IL9.unitSell = model.il9unitSell;
                        IL9.extSell = model.il9extSell;
                        IL9.minExtSell = model.il9minExtSell;
                        IL9.minExtCost = 0;
                        db.InvoiceLines.Add(IL9);

                        InvoiceLine IL10 = new InvoiceLine();
                        IL10.invoiceID = obj.lngID;
                        IL10.lineType = model.il10linetype;
                        IL10.displayOrder = model.il10display;
                        IL10.name = model.il10name;
                        IL10.size = model.il10size;
                        IL10.qty = model.il10qty;
                        IL10.UOM = model.il10UOM;
                        IL10.unitCost = model.il10unitCost;
                        IL10.extCost = model.il10extCost;
                        IL10.unitSell = model.il10unitSell;
                        IL10.extSell = model.il10extSell;
                        IL10.minExtSell = model.il10minExtSell;
                        IL10.minExtCost = 0;
                        db.InvoiceLines.Add(IL10);

                        InvoiceLine IL11 = new InvoiceLine();
                        IL11.invoiceID = obj.lngID;
                        IL11.lineType = model.il11linetype;
                        IL11.displayOrder = model.il11display;
                        IL11.name = model.il11name;
                        IL11.size = model.il11size;
                        IL11.qty = model.il11qty;
                        IL11.UOM = model.il11UOM;
                        IL11.unitCost = model.il11unitCost;
                        IL11.extCost = model.il11extCost;
                        IL11.unitSell = model.il11unitSell;
                        IL11.extSell = model.il11extSell;
                        IL11.minExtSell = model.il11minExtSell;
                        IL11.minExtCost = 0;
                        db.InvoiceLines.Add(IL11);

                        #endregion

                        #region Mailing Line Items

                        InvoiceLine IL20 = new InvoiceLine();
                        IL20.invoiceID = obj.lngID;
                        IL20.lineType = model.il20linetype;
                        IL20.displayOrder = model.il20display;
                        IL20.name = model.il20name;
                        IL20.size = model.il20size;
                        IL20.qty = model.il20qty;
                        IL20.UOM = model.il20UOM;
                        IL20.unitCost = model.il20unitCost;
                        IL20.extCost = model.il20extCost;
                        IL20.unitSell = model.il20unitSell;
                        IL20.extSell = model.il20extSell;
                        IL20.minExtSell = model.il20minExtSell;
                        IL20.minExtCost = 0;
                        db.InvoiceLines.Add(IL20);

                        InvoiceLine IL21 = new InvoiceLine();
                        IL21.invoiceID = obj.lngID;
                        IL21.lineType = model.il21linetype;
                        IL21.displayOrder = model.il21display;
                        IL21.name = model.il21name;
                        IL21.size = model.il21size;
                        IL21.qty = model.il21qty;
                        IL21.UOM = model.il21UOM;
                        IL21.unitCost = model.il21unitCost;
                        IL21.extCost = model.il21extCost;
                        IL21.unitSell = model.il21unitSell;
                        IL21.extSell = model.il21extSell;
                        IL21.minExtSell = model.il21minExtSell;
                        IL21.minExtCost = 0;
                        db.InvoiceLines.Add(IL21);

                        InvoiceLine IL22 = new InvoiceLine();
                        IL22.invoiceID = obj.lngID;
                        IL22.lineType = model.il22linetype;
                        IL22.displayOrder = model.il22display;
                        IL22.name = model.il22name;
                        IL22.size = model.il22size;
                        IL22.qty = model.il22qty;
                        IL22.UOM = model.il22UOM;
                        IL22.unitCost = model.il22unitCost;
                        IL22.extCost = model.il22extCost;
                        IL22.unitSell = model.il22unitSell;
                        IL22.extSell = model.il22extSell;
                        IL22.minExtSell = model.il22minExtSell;
                        IL22.minExtCost = 0;
                        db.InvoiceLines.Add(IL22);

                        InvoiceLine IL23 = new InvoiceLine();
                        IL23.invoiceID = obj.lngID;
                        IL23.lineType = model.il23linetype;
                        IL23.displayOrder = model.il23display;
                        IL23.name = model.il23name;
                        IL23.size = model.il23size;
                        IL23.qty = model.il23qty;
                        IL23.UOM = model.il23UOM;
                        IL23.unitCost = model.il23unitCost;
                        IL23.extCost = model.il23extCost;
                        IL23.unitSell = model.il23unitSell;
                        IL23.extSell = model.il23extSell;
                        IL23.minExtSell = model.il23minExtSell;
                        IL23.minExtCost = 0;
                        db.InvoiceLines.Add(IL23);

                        InvoiceLine IL24 = new InvoiceLine();
                        IL24.invoiceID = obj.lngID;
                        IL24.lineType = model.il24linetype;
                        IL24.displayOrder = model.il24display;
                        IL24.name = model.il24name;
                        IL24.size = model.il24size;
                        IL24.qty = model.il24qty;
                        IL24.UOM = model.il24UOM;
                        IL24.unitCost = model.il24unitCost;
                        IL24.extCost = model.il24extCost;
                        IL24.unitSell = model.il24unitSell;
                        IL24.extSell = model.il24extSell;
                        IL24.minExtSell = model.il24minExtSell;
                        IL24.minExtCost = 0;
                        db.InvoiceLines.Add(IL24);

                        InvoiceLine IL25 = new InvoiceLine();
                        IL25.invoiceID = obj.lngID;
                        IL25.lineType = model.il25linetype;
                        IL25.displayOrder = model.il25display;
                        IL25.name = model.il25name;
                        IL25.size = model.il25size;
                        IL25.qty = model.il25qty;
                        IL25.UOM = model.il25UOM;
                        IL25.unitCost = model.il25unitCost;
                        IL25.extCost = model.il25extCost;
                        IL25.unitSell = model.il25unitSell;
                        IL25.extSell = model.il25extSell;
                        IL25.minExtSell = model.il25minExtSell;
                        IL25.minExtCost = 0;
                        db.InvoiceLines.Add(IL25);

                        InvoiceLine IL26 = new InvoiceLine();
                        IL26.invoiceID = obj.lngID;
                        IL26.lineType = model.il26linetype;
                        IL26.displayOrder = model.il26display;
                        IL26.name = model.il26name;
                        IL26.size = model.il26size;
                        IL26.qty = model.il26qty;
                        IL26.UOM = model.il26UOM;
                        IL26.unitCost = model.il26unitCost;
                        IL26.extCost = model.il26extCost;
                        IL26.unitSell = model.il26unitSell;
                        IL26.extSell = model.il26extSell;
                        IL26.minExtSell = model.il26minExtSell;
                        IL26.minExtCost = 0;
                        db.InvoiceLines.Add(IL26);

                        InvoiceLine IL27 = new InvoiceLine();
                        IL27.invoiceID = obj.lngID;
                        IL27.lineType = model.il27linetype;
                        IL27.displayOrder = model.il27display;
                        IL27.name = model.il27name;
                        IL27.size = model.il27size;
                        IL27.qty = model.il27qty;
                        IL27.UOM = model.il27UOM;
                        IL27.unitCost = model.il27unitCost;
                        IL27.extCost = model.il27extCost;
                        IL27.unitSell = model.il27unitSell;
                        IL27.extSell = model.il27extSell;
                        IL27.minExtSell = model.il27minExtSell;
                        IL27.minExtCost = 0;
                        db.InvoiceLines.Add(IL27);

                        InvoiceLine IL28 = new InvoiceLine();
                        IL28.invoiceID = obj.lngID;
                        IL28.lineType = model.il28linetype;
                        IL28.displayOrder = model.il28display;
                        IL28.name = model.il28name;
                        IL28.size = model.il28size;
                        IL28.qty = model.il28qty;
                        IL28.UOM = model.il28UOM;
                        IL28.unitCost = model.il28unitCost;
                        IL28.extCost = model.il28extCost;
                        IL28.unitSell = model.il28unitSell;
                        IL28.extSell = model.il28extSell;
                        IL28.minExtSell = model.il28minExtSell;
                        IL28.minExtCost = 0;
                        db.InvoiceLines.Add(IL28);

                        InvoiceLine IL29 = new InvoiceLine();
                        IL29.invoiceID = obj.lngID;
                        IL29.lineType = model.il29linetype;
                        IL29.displayOrder = model.il29display;
                        IL29.name = model.il29name;
                        IL29.size = model.il29size;
                        IL29.qty = model.il29qty;
                        IL29.UOM = model.il29UOM;
                        IL29.unitCost = model.il29unitCost;
                        IL29.extCost = model.il29extCost;
                        IL29.unitSell = model.il29unitSell;
                        IL29.extSell = model.il29extSell;
                        IL29.minExtSell = model.il29minExtSell;
                        IL29.minExtCost = 0;
                        db.InvoiceLines.Add(IL29);

                        InvoiceLine IL30 = new InvoiceLine();
                        IL30.invoiceID = obj.lngID;
                        IL30.lineType = model.il30linetype;
                        IL30.displayOrder = model.il30display;
                        IL30.name = model.il30name;
                        IL30.size = model.il30size;
                        IL30.qty = model.il30qty;
                        IL30.UOM = model.il30UOM;
                        IL30.unitCost = model.il30unitCost;
                        IL30.extCost = model.il30extCost;
                        IL30.unitSell = model.il30unitSell;
                        IL30.extSell = model.il30extSell;
                        IL30.minExtSell = model.il30minExtSell;
                        IL30.minExtCost = 0;
                        db.InvoiceLines.Add(IL30);

                        InvoiceLine IL31 = new InvoiceLine();
                        IL31.invoiceID = obj.lngID;
                        IL31.lineType = model.il31linetype;
                        IL31.displayOrder = model.il31display;
                        IL31.name = model.il31name;
                        IL31.size = model.il31size;
                        IL31.qty = model.il31qty;
                        IL31.UOM = model.il31UOM;
                        IL31.unitCost = model.il31unitCost;
                        IL31.extCost = model.il31extCost;
                        IL31.unitSell = model.il31unitSell;
                        IL31.extSell = model.il31extSell;
                        IL31.minExtSell = model.il31minExtSell;
                        IL31.minExtCost = 0;
                        db.InvoiceLines.Add(IL31);

                        InvoiceLine IL32 = new InvoiceLine();
                        IL32.invoiceID = obj.lngID;
                        IL32.lineType = model.il32linetype;
                        IL32.displayOrder = model.il32display;
                        IL32.name = model.il32name;
                        IL32.size = model.il32size;
                        IL32.qty = model.il32qty;
                        IL32.UOM = model.il32UOM;
                        IL32.unitCost = model.il32unitCost;
                        IL32.extCost = model.il32extCost;
                        IL32.unitSell = model.il32unitSell;
                        IL32.extSell = model.il32extSell;
                        IL32.minExtSell = model.il32minExtSell;
                        IL32.minExtCost = 0;
                        db.InvoiceLines.Add(IL32);


                        InvoiceLine IL33 = new InvoiceLine();
                        IL33.invoiceID = obj.lngID;
                        IL33.lineType = model.il33linetype;
                        IL33.displayOrder = model.il33display;
                        IL33.name = model.il33name;
                        IL33.size = model.il33size;
                        IL33.qty = model.il33qty;
                        IL33.UOM = model.il33UOM;
                        IL33.unitCost = model.il33unitCost;
                        IL33.extCost = model.il33extCost;
                        IL33.unitSell = model.il33unitSell;
                        IL33.extSell = model.il33extSell;
                        IL33.minExtSell = model.il33minExtSell;
                        IL33.minExtCost = 0;
                        db.InvoiceLines.Add(IL33);

                        InvoiceLine IL34 = new InvoiceLine();
                        IL34.invoiceID = obj.lngID;
                        IL34.lineType = model.il34linetype;
                        IL34.displayOrder = model.il34display;
                        IL34.name = model.il34name;
                        IL34.size = model.il34size;
                        IL34.qty = model.il34qty;
                        IL34.UOM = model.il34UOM;
                        IL34.unitCost = model.il34unitCost;
                        IL34.extCost = model.il34extCost;
                        IL34.unitSell = model.il34unitSell;
                        IL34.extSell = model.il34extSell;
                        IL34.minExtSell = model.il34minExtSell;
                        IL34.minExtCost = 0;
                        db.InvoiceLines.Add(IL34);

                        InvoiceLine IL35 = new InvoiceLine();
                        IL35.invoiceID = obj.lngID;
                        IL35.lineType = model.il35linetype;
                        IL35.displayOrder = model.il35display;
                        IL35.name = model.il35name;
                        IL35.size = model.il35size;
                        IL35.qty = model.il35qty;
                        IL35.UOM = model.il35UOM;
                        IL35.unitCost = model.il35unitCost;
                        IL35.extCost = model.il35extCost;
                        IL35.unitSell = model.il35unitSell;
                        IL35.extSell = model.il35extSell;
                        IL35.minExtSell = model.il35minExtSell;
                        IL35.minExtCost = 0;
                        db.InvoiceLines.Add(IL35);

                        InvoiceLine IL36 = new InvoiceLine();
                        IL36.invoiceID = obj.lngID;
                        IL36.lineType = model.il36linetype;
                        IL36.displayOrder = model.il36display;
                        IL36.name = model.il36name;
                        IL36.size = model.il36size;
                        IL36.qty = model.il36qty;
                        IL36.UOM = model.il36UOM;
                        IL36.unitCost = model.il36unitCost;
                        IL36.extCost = model.il36extCost;
                        IL36.unitSell = model.il36unitSell;
                        IL36.extSell = model.il36extSell;
                        IL36.minExtSell = model.il36minExtSell;
                        IL36.minExtCost = 0;
                        db.InvoiceLines.Add(IL36);

                        #endregion

                        #region Freight

                        InvoiceLine IL40 = new InvoiceLine();
                        IL40.invoiceID = obj.lngID;
                        IL40.lineType = model.il40linetype;
                        IL40.displayOrder = model.il40display;
                        IL40.name = model.il40name;
                        IL40.size = model.il40size;
                        IL40.qty = model.il40qty;
                        IL40.UOM = model.il40UOM;
                        IL40.unitCost = model.il40unitCost;
                        IL40.extCost = model.il40extCost;
                        IL40.unitSell = model.il40unitSell;
                        IL40.extSell = model.il40extSell;
                        IL40.minExtSell = model.il40minExtSell;
                        IL40.minExtCost = 0;
                        db.InvoiceLines.Add(IL40);

                        #endregion

                        db.SaveChanges();

                        ViewBag.ResultMessage = "Invoice created for the order # " + obj.orderID + " successfully!";
                        return RedirectToAction("InvoiceListing", new { Success = true, Details = ViewBag.ResultMessage });
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ResultMessage = "Error! " + ex.Message;
                }

            }
            return View(model);
        }

        public IActionResult EditInvoice(int OrderId)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            InvoiceHandlerViewModel model = new InvoiceHandlerViewModel();
            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {

                    if (OrderId > 0)
                    {
                        Invoice IID = db.Invoices.FirstOrDefault(d => d.orderID == OrderId);

                        Order ORD = db.Orders.FirstOrDefault(d => d.lngID == OrderId);
                        model.InvoiceOrder = ORD;
                        model.lngID = IID.lngID;
                        model.comment = IID.comment;

                        List<InvoiceLine> LIs = db.InvoiceLines.Where(d => d.invoiceID == IID.lngID).ToList();

                        List<InvoiceLine> ILTsPrinting = LIs.Where(d => d.lineType.Contains("Printing")).OrderBy(d => d.displayOrder).ToList();
                        int index = 0;

                        #region Printing Line Items

                        model.il1linetype = ILTsPrinting[index].lineType;
                        model.il1display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                        model.il1name = ILTsPrinting[index].name;
                        model.il1size = ILTsPrinting[index].size;
                        model.il1qty = ILTsPrinting[index].qty;
                        model.il1UOM = ILTsPrinting[index].UOM;
                        model.il1unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                        model.il1extCost = ILTsPrinting[index].extCost.HasValue ? ILTsPrinting[index].extCost.Value : 0;
                        model.il1unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                        model.il1extSell = ILTsPrinting[index].extSell.HasValue ? ILTsPrinting[index].extSell.Value : 0;
                        model.il1minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                        index = index + 1;
                        model.il2linetype = ILTsPrinting[index].lineType;
                        model.il2display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                        model.il2name = ILTsPrinting[index].name;
                        model.il2size = ILTsPrinting[index].size;
                        model.il2qty = ILTsPrinting[index].qty;
                        model.il2UOM = ILTsPrinting[index].UOM;
                        model.il2unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                        model.il2extCost = ILTsPrinting[index].extCost.HasValue ? ILTsPrinting[index].extCost.Value : 0; ;
                        model.il2unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                        model.il2extSell = ILTsPrinting[index].extSell.HasValue ? ILTsPrinting[index].extSell.Value : 0;
                        model.il2minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                        index = index + 1;
                        model.il3linetype = ILTsPrinting[index].lineType;
                        model.il3display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                        model.il3name = ILTsPrinting[index].name;
                        model.il3size = ILTsPrinting[index].size;
                        model.il3qty = ILTsPrinting[index].qty;
                        model.il3UOM = ILTsPrinting[index].UOM;
                        model.il3unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                        model.il3extCost = ILTsPrinting[index].extCost.HasValue ? ILTsPrinting[index].extCost.Value : 0; ;
                        model.il3unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                        model.il3extSell = ILTsPrinting[index].extSell.HasValue ? ILTsPrinting[index].extSell.Value : 0;
                        model.il3minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                        index = index + 1;
                        model.il4linetype = ILTsPrinting[index].lineType;
                        model.il4display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                        model.il4name = ILTsPrinting[index].name;
                        model.il4size = ILTsPrinting[index].size;
                        model.il4qty = ILTsPrinting[index].qty;
                        model.il4UOM = ILTsPrinting[index].UOM;
                        model.il4unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                        model.il4extCost = ILTsPrinting[index].extCost.HasValue ? ILTsPrinting[index].extCost.Value : 0; ;
                        model.il4unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                        model.il4extSell = ILTsPrinting[index].extSell.HasValue ? ILTsPrinting[index].extSell.Value : 0;
                        model.il4minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                        index = index + 1;
                        model.il5linetype = ILTsPrinting[index].lineType;
                        model.il5display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                        model.il5name = ILTsPrinting[index].name;
                        model.il5size = ILTsPrinting[index].size;
                        model.il5qty = ILTsPrinting[index].qty;
                        model.il5UOM = ILTsPrinting[index].UOM;
                        model.il5unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                        model.il5extCost = ILTsPrinting[index].extCost.HasValue ? ILTsPrinting[index].extCost.Value : 0; ;
                        model.il5unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                        model.il5extSell = ILTsPrinting[index].extSell.HasValue ? ILTsPrinting[index].extSell.Value : 0;
                        model.il5minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                        index = index + 1;
                        model.il6linetype = ILTsPrinting[index].lineType;
                        model.il6display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                        model.il6name = ILTsPrinting[index].name;
                        model.il6size = ILTsPrinting[index].size;
                        model.il6qty = ILTsPrinting[index].qty;
                        model.il6UOM = ILTsPrinting[index].UOM;
                        model.il6unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                        model.il6extCost = ILTsPrinting[index].extCost.HasValue ? ILTsPrinting[index].extCost.Value : 0; ;
                        model.il6unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                        model.il6extSell = ILTsPrinting[index].extSell.HasValue ? ILTsPrinting[index].extSell.Value : 0;
                        model.il6minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                        index = index + 1;
                        model.il7linetype = ILTsPrinting[index].lineType;
                        model.il7display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                        model.il7name = ILTsPrinting[index].name;
                        model.il7size = ILTsPrinting[index].size;
                        model.il7qty = ILTsPrinting[index].qty;
                        model.il7UOM = ILTsPrinting[index].UOM;
                        model.il7unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                        model.il7extCost = ILTsPrinting[index].extCost.HasValue ? ILTsPrinting[index].extCost.Value : 0; ;
                        model.il7unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                        model.il7extSell = ILTsPrinting[index].extSell.HasValue ? ILTsPrinting[index].extSell.Value : 0;
                        model.il7minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                        index = index + 1;
                        model.il8linetype = ILTsPrinting[index].lineType;
                        model.il8display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                        model.il8name = ILTsPrinting[index].name;
                        model.il8size = ILTsPrinting[index].size;
                        model.il8qty = ILTsPrinting[index].qty;
                        model.il8UOM = ILTsPrinting[index].UOM;
                        model.il8unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                        model.il8extCost = ILTsPrinting[index].extCost.HasValue ? ILTsPrinting[index].extCost.Value : 0; ;
                        model.il8unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                        model.il8extSell = ILTsPrinting[index].extSell.HasValue ? ILTsPrinting[index].extSell.Value : 0;
                        model.il8minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                        index = index + 1;
                        model.il9linetype = ILTsPrinting[index].lineType;
                        model.il9display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                        model.il9name = ILTsPrinting[index].name;
                        model.il9size = ILTsPrinting[index].size;
                        model.il9qty = ILTsPrinting[index].qty;
                        model.il9UOM = ILTsPrinting[index].UOM;
                        model.il9unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                        model.il9extCost = ILTsPrinting[index].extCost.HasValue ? ILTsPrinting[index].extCost.Value : 0; ;
                        model.il9unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                        model.il9extSell = ILTsPrinting[index].extSell.HasValue ? ILTsPrinting[index].extSell.Value : 0;
                        model.il9minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                        index = index + 1;
                        model.il10linetype = ILTsPrinting[index].lineType;
                        model.il10display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                        model.il10name = ILTsPrinting[index].name;
                        model.il10size = ILTsPrinting[index].size;
                        model.il10qty = ILTsPrinting[index].qty;
                        model.il10UOM = ILTsPrinting[index].UOM;
                        model.il10unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                        model.il10extCost = ILTsPrinting[index].extCost.HasValue ? ILTsPrinting[index].extCost.Value : 0; ;
                        model.il10unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                        model.il10extSell = ILTsPrinting[index].extSell.HasValue ? ILTsPrinting[index].extSell.Value : 0;
                        model.il10minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                        index = index + 1;
                        model.il11linetype = ILTsPrinting[index].lineType;
                        model.il11display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                        model.il11name = ILTsPrinting[index].name;
                        model.il11size = ILTsPrinting[index].size;
                        model.il11qty = ILTsPrinting[index].qty;
                        model.il11UOM = ILTsPrinting[index].UOM;
                        model.il11unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                        model.il11extCost = ILTsPrinting[index].extCost.HasValue ? ILTsPrinting[index].extCost.Value : 0; ;
                        model.il11unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                        model.il11extSell = ILTsPrinting[index].extSell.HasValue ? ILTsPrinting[index].extSell.Value : 0;
                        model.il11minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                        #endregion

                        List<InvoiceLine> ILTsMailing = LIs.Where(d => d.lineType.Contains("Mailing")).OrderBy(d => d.displayOrder).ToList();
                        int indexNew = 0;

                        #region Mailing Line Items

                        model.il20linetype = ILTsMailing[indexNew].lineType;
                        model.il20display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il20name = ILTsMailing[indexNew].name;
                        model.il20size = ILTsMailing[indexNew].size;
                        model.il20qty = ILTsMailing[indexNew].qty;
                        model.il20UOM = ILTsMailing[indexNew].UOM;
                        model.il20unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il20extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0; ;
                        model.il20unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il20extSell = ILTsMailing[indexNew].extSell.HasValue ? ILTsMailing[indexNew].extSell.Value : 0;
                        model.il20minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il21linetype = ILTsMailing[indexNew].lineType;
                        model.il21display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il21name = ILTsMailing[indexNew].name;
                        model.il21size = ILTsMailing[indexNew].size;
                        model.il21qty = ILTsMailing[indexNew].qty;
                        model.il21UOM = ILTsMailing[indexNew].UOM;
                        model.il21unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il21extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0; ;
                        model.il21unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il21extSell = ILTsMailing[indexNew].extSell.HasValue ? ILTsMailing[indexNew].extSell.Value : 0;
                        model.il21minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il22linetype = ILTsMailing[indexNew].lineType;
                        model.il22display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il22name = ILTsMailing[indexNew].name;
                        model.il22size = ILTsMailing[indexNew].size;
                        model.il22qty = ILTsMailing[indexNew].qty;
                        model.il22UOM = ILTsMailing[indexNew].UOM;
                        model.il22unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il22extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0; ;
                        model.il22unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il22extSell = ILTsMailing[indexNew].extSell.HasValue ? ILTsMailing[indexNew].extSell.Value : 0;
                        model.il22minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il23linetype = ILTsMailing[indexNew].lineType;
                        model.il23display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il23name = ILTsMailing[indexNew].name;
                        model.il23size = ILTsMailing[indexNew].size;
                        model.il23qty = ILTsMailing[indexNew].qty;
                        model.il23UOM = ILTsMailing[indexNew].UOM;
                        model.il23unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il23extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0; ;
                        model.il23unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il23extSell = ILTsMailing[indexNew].extSell.HasValue ? ILTsMailing[indexNew].extSell.Value : 0;
                        model.il23minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il24linetype = ILTsMailing[indexNew].lineType;
                        model.il24display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il24name = ILTsMailing[indexNew].name;
                        model.il24size = ILTsMailing[indexNew].size;
                        model.il24qty = ILTsMailing[indexNew].qty;
                        model.il24UOM = ILTsMailing[indexNew].UOM;
                        model.il24unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il24extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0; ;
                        model.il24unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il24extSell = ILTsMailing[indexNew].extSell.HasValue ? ILTsMailing[indexNew].extSell.Value : 0;
                        model.il24minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il25linetype = ILTsMailing[indexNew].lineType;
                        model.il25display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il25name = ILTsMailing[indexNew].name;
                        model.il25size = ILTsMailing[indexNew].size;
                        model.il25qty = ILTsMailing[indexNew].qty;
                        model.il25UOM = ILTsMailing[indexNew].UOM;
                        model.il25unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il25extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0; ;
                        model.il25unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il25extSell = ILTsMailing[indexNew].extSell.HasValue ? ILTsMailing[indexNew].extSell.Value : 0;
                        model.il25minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il26linetype = ILTsMailing[indexNew].lineType;
                        model.il26display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il26name = ILTsMailing[indexNew].name;
                        model.il26size = ILTsMailing[indexNew].size;
                        model.il26qty = ILTsMailing[indexNew].qty;
                        model.il26UOM = ILTsMailing[indexNew].UOM;
                        model.il26unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il26extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0; ;
                        model.il26unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il26extSell = ILTsMailing[indexNew].extSell.HasValue ? ILTsMailing[indexNew].extSell.Value : 0;
                        model.il26minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il27linetype = ILTsMailing[indexNew].lineType;
                        model.il27display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il27name = ILTsMailing[indexNew].name;
                        model.il27size = ILTsMailing[indexNew].size;
                        model.il27qty = ILTsMailing[indexNew].qty;
                        model.il27UOM = ILTsMailing[indexNew].UOM;
                        model.il27unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il27extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0; ;
                        model.il27unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il27extSell = ILTsMailing[indexNew].extSell.HasValue ? ILTsMailing[indexNew].extSell.Value : 0;
                        model.il27minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il28linetype = ILTsMailing[indexNew].lineType;
                        model.il28display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il28name = ILTsMailing[indexNew].name;
                        model.il28size = ILTsMailing[indexNew].size;
                        model.il28qty = ILTsMailing[indexNew].qty;
                        model.il28UOM = ILTsMailing[indexNew].UOM;
                        model.il28unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il28extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0; ;
                        model.il28unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il28extSell = ILTsMailing[indexNew].extSell.HasValue ? ILTsMailing[indexNew].extSell.Value : 0;
                        model.il28minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il29linetype = ILTsMailing[indexNew].lineType;
                        model.il29display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il29name = ILTsMailing[indexNew].name;
                        model.il29size = ILTsMailing[indexNew].size;
                        model.il29qty = ILTsMailing[indexNew].qty;
                        model.il29UOM = ILTsMailing[indexNew].UOM;
                        model.il29unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il29extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0; ;
                        model.il29unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il29extSell = ILTsMailing[indexNew].extSell.HasValue ? ILTsMailing[indexNew].extSell.Value : 0;
                        model.il29minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il30linetype = ILTsMailing[indexNew].lineType;
                        model.il30display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il30name = ILTsMailing[indexNew].name;
                        model.il30size = ILTsMailing[indexNew].size;
                        model.il30qty = ILTsMailing[indexNew].qty;
                        model.il30UOM = ILTsMailing[indexNew].UOM;
                        model.il30unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il30extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0; ;
                        model.il30unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il30extSell = ILTsMailing[indexNew].extSell.HasValue ? ILTsMailing[indexNew].extSell.Value : 0;
                        model.il30minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il31linetype = ILTsMailing[indexNew].lineType;
                        model.il31display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il31name = ILTsMailing[indexNew].name;
                        model.il31size = ILTsMailing[indexNew].size;
                        model.il31qty = ILTsMailing[indexNew].qty;
                        model.il31UOM = ILTsMailing[indexNew].UOM;
                        model.il31unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il31extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0; ;
                        model.il31unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il31extSell = ILTsMailing[indexNew].extSell.HasValue ? ILTsMailing[indexNew].extSell.Value : 0;
                        model.il31minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il32linetype = ILTsMailing[indexNew].lineType;
                        model.il32display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il32name = ILTsMailing[indexNew].name;
                        model.il32size = ILTsMailing[indexNew].size;
                        model.il32qty = ILTsMailing[indexNew].qty;
                        model.il32UOM = ILTsMailing[indexNew].UOM;
                        model.il32unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il32extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0; ;
                        model.il32unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il32extSell = ILTsMailing[indexNew].extSell.HasValue ? ILTsMailing[indexNew].extSell.Value : 0;
                        model.il32minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il33linetype = ILTsMailing[indexNew].lineType;
                        model.il33display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il33name = ILTsMailing[indexNew].name;
                        model.il33size = ILTsMailing[indexNew].size;
                        model.il33qty = ILTsMailing[indexNew].qty;
                        model.il33UOM = ILTsMailing[indexNew].UOM;
                        model.il33unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il33extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0; ;
                        model.il33unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il33extSell = ILTsMailing[indexNew].extSell.HasValue ? ILTsMailing[indexNew].extSell.Value : 0;
                        model.il33minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il34linetype = ILTsMailing[indexNew].lineType;
                        model.il34display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il34name = ILTsMailing[indexNew].name;
                        model.il34size = ILTsMailing[indexNew].size;
                        model.il34qty = ILTsMailing[indexNew].qty;
                        model.il34UOM = ILTsMailing[indexNew].UOM;
                        model.il34unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il34extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0; ;
                        model.il34unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il34extSell = ILTsMailing[indexNew].extSell.HasValue ? ILTsMailing[indexNew].extSell.Value : 0;
                        model.il34minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il35linetype = ILTsMailing[indexNew].lineType;
                        model.il35display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il35name = ILTsMailing[indexNew].name;
                        model.il35size = ILTsMailing[indexNew].size;
                        model.il35qty = ILTsMailing[indexNew].qty;
                        model.il35UOM = ILTsMailing[indexNew].UOM;
                        model.il35unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il35extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0; ;
                        model.il35unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il35extSell = ILTsMailing[indexNew].extSell.HasValue ? ILTsMailing[indexNew].extSell.Value : 0;
                        model.il35minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        indexNew = indexNew + 1;
                        model.il36linetype = ILTsMailing[indexNew].lineType;
                        model.il36display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                        model.il36name = ILTsMailing[indexNew].name;
                        model.il36size = ILTsMailing[indexNew].size;
                        model.il36qty = ILTsMailing[indexNew].qty;
                        model.il36UOM = ILTsMailing[indexNew].UOM;
                        model.il36unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                        model.il36extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0; ;
                        model.il36unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                        model.il36extSell = ILTsMailing[indexNew].extSell.HasValue ? ILTsMailing[indexNew].extSell.Value : 0;
                        model.il36minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                        #endregion

                        #region Freight

                        List<InvoiceLine> ILTsFrieght = LIs.Where(d => d.lineType.Contains("Freight")).OrderBy(d => d.displayOrder).ToList();
                        indexNew = 0;
                        model.il40linetype = ILTsFrieght[indexNew].lineType;
                        model.il40display = ILTsFrieght[indexNew].displayOrder.HasValue ? ILTsFrieght[indexNew].displayOrder.Value : 0;
                        model.il40name = ILTsFrieght[indexNew].name;
                        model.il40size = ILTsFrieght[indexNew].size;
                        model.il40qty = ILTsFrieght[indexNew].qty;
                        model.il40UOM = ILTsFrieght[indexNew].UOM;
                        model.il40unitCost = ILTsFrieght[indexNew].unitCost.HasValue ? ILTsFrieght[indexNew].unitCost.Value : 0;
                        model.il40extCost = ILTsFrieght[indexNew].extCost.HasValue ? ILTsFrieght[indexNew].extCost.Value : 0; ;
                        model.il40unitSell = ILTsFrieght[indexNew].unitSell.HasValue ? ILTsFrieght[indexNew].unitSell.Value : 0;
                        model.il40extSell = ILTsFrieght[indexNew].extSell.HasValue ? ILTsFrieght[indexNew].extSell.Value : 0;
                        model.il40minExtSell = ILTsFrieght[indexNew].minExtSell.HasValue ? ILTsFrieght[indexNew].minExtSell.Value : 0;

                        model.comment = IID.comment;
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ResultMessage = "Error: " + ex.Message;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult EditInvoice(InvoiceHandlerViewModel model)
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
                    using (var db = _DbContextFactory.CreateDbContext())
                    {
                        Invoice obj = db.Invoices.FirstOrDefault(d => d.lngID == model.lngID);

                        obj.orderID = model.orderID;
                        obj.status = "Submitted by vendor";
                        obj.vendorInvoiceNo = model.vendorInvoiceNo;
                        obj.comment = model.comment;
                        //obj.datBillSubmitted = DateTime.Now;
                        obj.datBillRevised = DateTime.Now;
                        obj.datClientInvoiced = DateTime.Now;
                        obj.datClientInvoiceUploaded = DateTime.Now;
                        obj.datClientPayment = null;
                        obj.lastTouch = "";
                        obj.clientInvoiceFile = "";
                        obj.vendorInvoiceFile = "";
                        obj.internalInvoiceFile = "";
                        db.SaveChanges();

                        #region Remove Old Line Items

                        List<InvoiceLine> LIsTR = db.InvoiceLines.Where(d => d.invoiceID == obj.lngID).ToList();
                        foreach (InvoiceLine IL in LIsTR)
                        {
                            db.InvoiceLines.Remove(IL);
                        }
                        db.SaveChanges();

                        #endregion

                        #region Printing Line Items
                        InvoiceLine IL1 = new InvoiceLine();
                        IL1.invoiceID = obj.lngID;
                        IL1.lineType = model.il1linetype;
                        IL1.displayOrder = model.il1display;
                        IL1.name = model.il1name;
                        IL1.size = model.il1size;
                        IL1.qty = model.il1qty;
                        IL1.UOM = model.il1UOM;
                        IL1.unitCost = model.il1unitCost;
                        IL1.extCost = model.il1extCost;
                        IL1.unitSell = model.il1unitSell;
                        IL1.extSell = model.il1extSell;
                        IL1.minExtSell = model.il1minExtSell;
                        IL1.minExtCost = 0;
                        db.InvoiceLines.Add(IL1);

                        InvoiceLine IL2 = new InvoiceLine();
                        IL2.invoiceID = obj.lngID;
                        IL2.lineType = model.il2linetype;
                        IL2.displayOrder = model.il2display;
                        IL2.name = model.il2name;
                        IL2.size = model.il2size;
                        IL2.qty = model.il2qty;
                        IL2.UOM = model.il2UOM;
                        IL2.unitCost = model.il2unitCost;
                        IL2.extCost = model.il2extCost;
                        IL2.unitSell = model.il2unitSell;
                        IL2.extSell = model.il2extSell;
                        IL2.minExtSell = model.il2minExtSell;
                        IL2.minExtCost = 0;
                        db.InvoiceLines.Add(IL2);

                        InvoiceLine IL3 = new InvoiceLine();
                        IL3.invoiceID = obj.lngID;
                        IL3.lineType = model.il3linetype;
                        IL3.displayOrder = model.il3display;
                        IL3.name = model.il3name;
                        IL3.size = model.il3size;
                        IL3.qty = model.il3qty;
                        IL3.UOM = model.il3UOM;
                        IL3.unitCost = model.il3unitCost;
                        IL3.extCost = model.il3extCost;
                        IL3.unitSell = model.il3unitSell;
                        IL3.extSell = model.il3extSell;
                        IL3.minExtSell = model.il3minExtSell;
                        IL3.minExtCost = 0;
                        db.InvoiceLines.Add(IL3);

                        InvoiceLine IL4 = new InvoiceLine();
                        IL4.invoiceID = obj.lngID;
                        IL4.lineType = model.il4linetype;
                        IL4.displayOrder = model.il4display;
                        IL4.name = model.il4name;
                        IL4.size = model.il4size;
                        IL4.qty = model.il4qty;
                        IL4.UOM = model.il4UOM;
                        IL4.unitCost = model.il4unitCost;
                        IL4.extCost = model.il4extCost;
                        IL4.unitSell = model.il4unitSell;
                        IL4.extSell = model.il4extSell;
                        IL4.minExtSell = model.il4minExtSell;
                        IL4.minExtCost = 0;
                        db.InvoiceLines.Add(IL4);

                        InvoiceLine IL5 = new InvoiceLine();
                        IL5.invoiceID = obj.lngID;
                        IL5.lineType = model.il5linetype;
                        IL5.displayOrder = model.il5display;
                        IL5.name = model.il5name;
                        IL5.size = model.il5size;
                        IL5.qty = model.il5qty;
                        IL5.UOM = model.il5UOM;
                        IL5.unitCost = model.il5unitCost;
                        IL5.extCost = model.il5extCost;
                        IL5.unitSell = model.il5unitSell;
                        IL5.extSell = model.il5extSell;
                        IL5.minExtSell = model.il5minExtSell;
                        IL5.minExtCost = 0;
                        db.InvoiceLines.Add(IL5);

                        InvoiceLine IL6 = new InvoiceLine();
                        IL6.invoiceID = obj.lngID;
                        IL6.lineType = model.il6linetype;
                        IL6.displayOrder = model.il6display;
                        IL6.name = model.il6name;
                        IL6.size = model.il6size;
                        IL6.qty = model.il6qty;
                        IL6.UOM = model.il6UOM;
                        IL6.unitCost = model.il6unitCost;
                        IL6.extCost = model.il6extCost;
                        IL6.unitSell = model.il6unitSell;
                        IL6.extSell = model.il6extSell;
                        IL6.minExtSell = model.il6minExtSell;
                        IL6.minExtCost = 0;
                        db.InvoiceLines.Add(IL6);

                        InvoiceLine IL7 = new InvoiceLine();
                        IL7.invoiceID = obj.lngID;
                        IL7.lineType = model.il7linetype;
                        IL7.displayOrder = model.il7display;
                        IL7.name = model.il7name;
                        IL7.size = model.il7size;
                        IL7.qty = model.il7qty;
                        IL7.UOM = model.il7UOM;
                        IL7.unitCost = model.il7unitCost;
                        IL7.extCost = model.il7extCost;
                        IL7.unitSell = model.il7unitSell;
                        IL7.extSell = model.il7extSell;
                        IL7.minExtSell = model.il7minExtSell;
                        IL7.minExtCost = 0;
                        db.InvoiceLines.Add(IL7);

                        InvoiceLine IL8 = new InvoiceLine();
                        IL8.invoiceID = obj.lngID;
                        IL8.lineType = model.il8linetype;
                        IL8.displayOrder = model.il8display;
                        IL8.name = model.il8name;
                        IL8.size = model.il8size;
                        IL8.qty = model.il8qty;
                        IL8.UOM = model.il8UOM;
                        IL8.unitCost = model.il8unitCost;
                        IL8.extCost = model.il8extCost;
                        IL8.unitSell = model.il8unitSell;
                        IL8.extSell = model.il8extSell;
                        IL8.minExtSell = model.il8minExtSell;
                        IL8.minExtCost = 0;
                        db.InvoiceLines.Add(IL8);

                        InvoiceLine IL9 = new InvoiceLine();
                        IL9.invoiceID = obj.lngID;
                        IL9.lineType = model.il9linetype;
                        IL9.displayOrder = model.il9display;
                        IL9.name = model.il9name;
                        IL9.size = model.il9size;
                        IL9.qty = model.il9qty;
                        IL9.UOM = model.il9UOM;
                        IL9.unitCost = model.il9unitCost;
                        IL9.extCost = model.il9extCost;
                        IL9.unitSell = model.il9unitSell;
                        IL9.extSell = model.il9extSell;
                        IL9.minExtSell = model.il9minExtSell;
                        IL9.minExtCost = 0;
                        db.InvoiceLines.Add(IL9);

                        InvoiceLine IL10 = new InvoiceLine();
                        IL10.invoiceID = obj.lngID;
                        IL10.lineType = model.il10linetype;
                        IL10.displayOrder = model.il10display;
                        IL10.name = model.il10name;
                        IL10.size = model.il10size;
                        IL10.qty = model.il10qty;
                        IL10.UOM = model.il10UOM;
                        IL10.unitCost = model.il10unitCost;
                        IL10.extCost = model.il10extCost;
                        IL10.unitSell = model.il10unitSell;
                        IL10.extSell = model.il10extSell;
                        IL10.minExtSell = model.il10minExtSell;
                        IL10.minExtCost = 0;
                        db.InvoiceLines.Add(IL10);

                        InvoiceLine IL11 = new InvoiceLine();
                        IL11.invoiceID = obj.lngID;
                        IL11.lineType = model.il11linetype;
                        IL11.displayOrder = model.il11display;
                        IL11.name = model.il11name;
                        IL11.size = model.il11size;
                        IL11.qty = model.il11qty;
                        IL11.UOM = model.il11UOM;
                        IL11.unitCost = model.il11unitCost;
                        IL11.extCost = model.il11extCost;
                        IL11.unitSell = model.il11unitSell;
                        IL11.extSell = model.il11extSell;
                        IL11.minExtSell = model.il11minExtSell;
                        IL11.minExtCost = 0;
                        db.InvoiceLines.Add(IL11);

                        #endregion

                        #region Mailing Line Items

                        InvoiceLine IL20 = new InvoiceLine();
                        IL20.invoiceID = obj.lngID;
                        IL20.lineType = model.il20linetype;
                        IL20.displayOrder = model.il20display;
                        IL20.name = model.il20name;
                        IL20.size = model.il20size;
                        IL20.qty = model.il20qty;
                        IL20.UOM = model.il20UOM;
                        IL20.unitCost = model.il20unitCost;
                        IL20.extCost = model.il20extCost;
                        IL20.unitSell = model.il20unitSell;
                        IL20.extSell = model.il20extSell;
                        IL20.minExtSell = model.il20minExtSell;
                        IL20.minExtCost = 0;
                        db.InvoiceLines.Add(IL20);

                        InvoiceLine IL21 = new InvoiceLine();
                        IL21.invoiceID = obj.lngID;
                        IL21.lineType = model.il21linetype;
                        IL21.displayOrder = model.il21display;
                        IL21.name = model.il21name;
                        IL21.size = model.il21size;
                        IL21.qty = model.il21qty;
                        IL21.UOM = model.il21UOM;
                        IL21.unitCost = model.il21unitCost;
                        IL21.extCost = model.il21extCost;
                        IL21.unitSell = model.il21unitSell;
                        IL21.extSell = model.il21extSell;
                        IL21.minExtSell = model.il21minExtSell;
                        IL21.minExtCost = 0;
                        db.InvoiceLines.Add(IL21);

                        InvoiceLine IL22 = new InvoiceLine();
                        IL22.invoiceID = obj.lngID;
                        IL22.lineType = model.il22linetype;
                        IL22.displayOrder = model.il22display;
                        IL22.name = model.il22name;
                        IL22.size = model.il22size;
                        IL22.qty = model.il22qty;
                        IL22.UOM = model.il22UOM;
                        IL22.unitCost = model.il22unitCost;
                        IL22.extCost = model.il22extCost;
                        IL22.unitSell = model.il22unitSell;
                        IL22.extSell = model.il22extSell;
                        IL22.minExtSell = model.il22minExtSell;
                        IL22.minExtCost = 0;
                        db.InvoiceLines.Add(IL22);

                        InvoiceLine IL23 = new InvoiceLine();
                        IL23.invoiceID = obj.lngID;
                        IL23.lineType = model.il23linetype;
                        IL23.displayOrder = model.il23display;
                        IL23.name = model.il23name;
                        IL23.size = model.il23size;
                        IL23.qty = model.il23qty;
                        IL23.UOM = model.il23UOM;
                        IL23.unitCost = model.il23unitCost;
                        IL23.extCost = model.il23extCost;
                        IL23.unitSell = model.il23unitSell;
                        IL23.extSell = model.il23extSell;
                        IL23.minExtSell = model.il23minExtSell;
                        IL23.minExtCost = 0;
                        db.InvoiceLines.Add(IL23);

                        InvoiceLine IL24 = new InvoiceLine();
                        IL24.invoiceID = obj.lngID;
                        IL24.lineType = model.il24linetype;
                        IL24.displayOrder = model.il24display;
                        IL24.name = model.il24name;
                        IL24.size = model.il24size;
                        IL24.qty = model.il24qty;
                        IL24.UOM = model.il24UOM;
                        IL24.unitCost = model.il24unitCost;
                        IL24.extCost = model.il24extCost;
                        IL24.unitSell = model.il24unitSell;
                        IL24.extSell = model.il24extSell;
                        IL24.minExtSell = model.il24minExtSell;
                        IL24.minExtCost = 0;
                        db.InvoiceLines.Add(IL24);

                        InvoiceLine IL25 = new InvoiceLine();
                        IL25.invoiceID = obj.lngID;
                        IL25.lineType = model.il25linetype;
                        IL25.displayOrder = model.il25display;
                        IL25.name = model.il25name;
                        IL25.size = model.il25size;
                        IL25.qty = model.il25qty;
                        IL25.UOM = model.il25UOM;
                        IL25.unitCost = model.il25unitCost;
                        IL25.extCost = model.il25extCost;
                        IL25.unitSell = model.il25unitSell;
                        IL25.extSell = model.il25extSell;
                        IL25.minExtSell = model.il25minExtSell;
                        IL25.minExtCost = 0;
                        db.InvoiceLines.Add(IL25);

                        InvoiceLine IL26 = new InvoiceLine();
                        IL26.invoiceID = obj.lngID;
                        IL26.lineType = model.il26linetype;
                        IL26.displayOrder = model.il26display;
                        IL26.name = model.il26name;
                        IL26.size = model.il26size;
                        IL26.qty = model.il26qty;
                        IL26.UOM = model.il26UOM;
                        IL26.unitCost = model.il26unitCost;
                        IL26.extCost = model.il26extCost;
                        IL26.unitSell = model.il26unitSell;
                        IL26.extSell = model.il26extSell;
                        IL26.minExtSell = model.il26minExtSell;
                        IL26.minExtCost = 0;
                        db.InvoiceLines.Add(IL26);

                        InvoiceLine IL27 = new InvoiceLine();
                        IL27.invoiceID = obj.lngID;
                        IL27.lineType = model.il27linetype;
                        IL27.displayOrder = model.il27display;
                        IL27.name = model.il27name;
                        IL27.size = model.il27size;
                        IL27.qty = model.il27qty;
                        IL27.UOM = model.il27UOM;
                        IL27.unitCost = model.il27unitCost;
                        IL27.extCost = model.il27extCost;
                        IL27.unitSell = model.il27unitSell;
                        IL27.extSell = model.il27extSell;
                        IL27.minExtSell = model.il27minExtSell;
                        IL27.minExtCost = 0;
                        db.InvoiceLines.Add(IL27);

                        InvoiceLine IL28 = new InvoiceLine();
                        IL28.invoiceID = obj.lngID;
                        IL28.lineType = model.il28linetype;
                        IL28.displayOrder = model.il28display;
                        IL28.name = model.il28name;
                        IL28.size = model.il28size;
                        IL28.qty = model.il28qty;
                        IL28.UOM = model.il28UOM;
                        IL28.unitCost = model.il28unitCost;
                        IL28.extCost = model.il28extCost;
                        IL28.unitSell = model.il28unitSell;
                        IL28.extSell = model.il28extSell;
                        IL28.minExtSell = model.il28minExtSell;
                        IL28.minExtCost = 0;
                        db.InvoiceLines.Add(IL28);

                        InvoiceLine IL29 = new InvoiceLine();
                        IL29.invoiceID = obj.lngID;
                        IL29.lineType = model.il29linetype;
                        IL29.displayOrder = model.il29display;
                        IL29.name = model.il29name;
                        IL29.size = model.il29size;
                        IL29.qty = model.il29qty;
                        IL29.UOM = model.il29UOM;
                        IL29.unitCost = model.il29unitCost;
                        IL29.extCost = model.il29extCost;
                        IL29.unitSell = model.il29unitSell;
                        IL29.extSell = model.il29extSell;
                        IL29.minExtSell = model.il29minExtSell;
                        IL29.minExtCost = 0;
                        db.InvoiceLines.Add(IL29);

                        InvoiceLine IL30 = new InvoiceLine();
                        IL30.invoiceID = obj.lngID;
                        IL30.lineType = model.il30linetype;
                        IL30.displayOrder = model.il30display;
                        IL30.name = model.il30name;
                        IL30.size = model.il30size;
                        IL30.qty = model.il30qty;
                        IL30.UOM = model.il30UOM;
                        IL30.unitCost = model.il30unitCost;
                        IL30.extCost = model.il30extCost;
                        IL30.unitSell = model.il30unitSell;
                        IL30.extSell = model.il30extSell;
                        IL30.minExtSell = model.il30minExtSell;
                        IL30.minExtCost = 0;
                        db.InvoiceLines.Add(IL30);

                        InvoiceLine IL31 = new InvoiceLine();
                        IL31.invoiceID = obj.lngID;
                        IL31.lineType = model.il31linetype;
                        IL31.displayOrder = model.il31display;
                        IL31.name = model.il31name;
                        IL31.size = model.il31size;
                        IL31.qty = model.il31qty;
                        IL31.UOM = model.il31UOM;
                        IL31.unitCost = model.il31unitCost;
                        IL31.extCost = model.il31extCost;
                        IL31.unitSell = model.il31unitSell;
                        IL31.extSell = model.il31extSell;
                        IL31.minExtSell = model.il31minExtSell;
                        IL31.minExtCost = 0;
                        db.InvoiceLines.Add(IL31);

                        InvoiceLine IL32 = new InvoiceLine();
                        IL32.invoiceID = obj.lngID;
                        IL32.lineType = model.il32linetype;
                        IL32.displayOrder = model.il32display;
                        IL32.name = model.il32name;
                        IL32.size = model.il32size;
                        IL32.qty = model.il32qty;
                        IL32.UOM = model.il32UOM;
                        IL32.unitCost = model.il32unitCost;
                        IL32.extCost = model.il32extCost;
                        IL32.unitSell = model.il32unitSell;
                        IL32.extSell = model.il32extSell;
                        IL32.minExtSell = model.il32minExtSell;
                        IL32.minExtCost = 0;
                        db.InvoiceLines.Add(IL32);


                        InvoiceLine IL33 = new InvoiceLine();
                        IL33.invoiceID = obj.lngID;
                        IL33.lineType = model.il33linetype;
                        IL33.displayOrder = model.il33display;
                        IL33.name = model.il33name;
                        IL33.size = model.il33size;
                        IL33.qty = model.il33qty;
                        IL33.UOM = model.il33UOM;
                        IL33.unitCost = model.il33unitCost;
                        IL33.extCost = model.il33extCost;
                        IL33.unitSell = model.il33unitSell;
                        IL33.extSell = model.il33extSell;
                        IL33.minExtSell = model.il33minExtSell;
                        IL33.minExtCost = 0;
                        db.InvoiceLines.Add(IL33);

                        InvoiceLine IL34 = new InvoiceLine();
                        IL34.invoiceID = obj.lngID;
                        IL34.lineType = model.il34linetype;
                        IL34.displayOrder = model.il34display;
                        IL34.name = model.il34name;
                        IL34.size = model.il34size;
                        IL34.qty = model.il34qty;
                        IL34.UOM = model.il34UOM;
                        IL34.unitCost = model.il34unitCost;
                        IL34.extCost = model.il34extCost;
                        IL34.unitSell = model.il34unitSell;
                        IL34.extSell = model.il34extSell;
                        IL34.minExtSell = model.il34minExtSell;
                        IL34.minExtCost = 0;
                        db.InvoiceLines.Add(IL34);

                        InvoiceLine IL35 = new InvoiceLine();
                        IL35.invoiceID = obj.lngID;
                        IL35.lineType = model.il35linetype;
                        IL35.displayOrder = model.il35display;
                        IL35.name = model.il35name;
                        IL35.size = model.il35size;
                        IL35.qty = model.il35qty;
                        IL35.UOM = model.il35UOM;
                        IL35.unitCost = model.il35unitCost;
                        IL35.extCost = model.il35extCost;
                        IL35.unitSell = model.il35unitSell;
                        IL35.extSell = model.il35extSell;
                        IL35.minExtSell = model.il35minExtSell;
                        IL35.minExtCost = 0;
                        db.InvoiceLines.Add(IL35);

                        InvoiceLine IL36 = new InvoiceLine();
                        IL36.invoiceID = obj.lngID;
                        IL36.lineType = model.il36linetype;
                        IL36.displayOrder = model.il36display;
                        IL36.name = model.il36name;
                        IL36.size = model.il36size;
                        IL36.qty = model.il36qty;
                        IL36.UOM = model.il36UOM;
                        IL36.unitCost = model.il36unitCost;
                        IL36.extCost = model.il36extCost;
                        IL36.unitSell = model.il36unitSell;
                        IL36.extSell = model.il36extSell;
                        IL36.minExtSell = model.il36minExtSell;
                        IL36.minExtCost = 0;
                        db.InvoiceLines.Add(IL36);

                        #endregion

                        #region Freight

                        InvoiceLine IL40 = new InvoiceLine();
                        IL40.invoiceID = obj.lngID;
                        IL40.lineType = model.il40linetype;
                        IL40.displayOrder = model.il40display;
                        IL40.name = model.il40name;
                        IL40.size = model.il40size;
                        IL40.qty = model.il40qty;
                        IL40.UOM = model.il40UOM;
                        IL40.unitCost = model.il40unitCost;
                        IL40.extCost = model.il40extCost;
                        IL40.unitSell = model.il40unitSell;
                        IL40.extSell = model.il40extSell;
                        IL40.minExtSell = model.il40minExtSell;
                        IL40.minExtCost = 0;
                        db.InvoiceLines.Add(IL40);

                        #endregion

                        db.SaveChanges();

                        ViewBag.ResultMessage = "Invoice updated for the order # " + obj.orderID + " successfully!";
                        return RedirectToAction("InvoiceListing", new { Success = true, Details = ViewBag.ResultMessage });
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ResultMessage = "Error! " + ex.Message;
                }

            }
            return View(model);
        }

        public ActionResult LoadForCreateInvoiceAPI(int OrderId, string StatusType)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            InvoiceHandlerViewModel model = new InvoiceHandlerViewModel();
            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {

                    if (OrderId > 0)
                    {
                        if (StatusType == "create")
                        {
                            Order ORD = db.Orders.FirstOrDefault(d => d.lngID == OrderId);
                            model.InvoiceOrder = ORD;
                            model.orderID = ORD.lngID;
                            model.ilOrderID = ORD.lngID;
                            model.ilStatusType = "create";
                            model.ilInvoiceID = 0;

                            model.DTClientOrderNo = ORD.lngID.ToString();
                            model.DTClientChargeTo = ORD.envelope_product_Cost_Center;
                            model.DTClientJobName = ORD.job_details_jobName;
                            model.DTVendorInvoiceNo = "";
                            model.DTVendorInvoiceDate = "";

                            List<InvoiceLinesTemplate> ILTs = db.InvoiceLinesTemplates.ToList();
                            List<InvoiceLine> LIs = new List<InvoiceLine>();

                            List<InvoiceLinesTemplate> ILTsPrinting = ILTs.Where(d => d.lineType.Contains("Printing")).OrderBy(d => d.displayOrder).ToList();
                            int index = 0;

                            #region Printing Line Items
                            model.il1linetype = ILTsPrinting[index].lineType;
                            model.il1display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                            model.il1name = ILTsPrinting[index].name;
                            model.il1size = ILTsPrinting[index].size;
                            model.il1qty = 0;
                            model.il1UOM = ILTsPrinting[index].UOM;
                            model.il1unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                            model.il1extCost = 0;
                            model.il1unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                            model.il1extSell = 0;
                            model.il1minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                            index = index + 1;
                            model.il2linetype = ILTsPrinting[index].lineType;
                            model.il2display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                            model.il2name = ILTsPrinting[index].name;
                            model.il2size = ILTsPrinting[index].size;
                            model.il2qty = 0;
                            model.il2UOM = ILTsPrinting[index].UOM;
                            model.il2unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                            model.il2extCost = 0;
                            model.il2unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                            model.il2extSell = 0;
                            model.il2minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                            index = index + 1;
                            model.il3linetype = ILTsPrinting[index].lineType;
                            model.il3display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                            model.il3name = ILTsPrinting[index].name;
                            model.il3size = ILTsPrinting[index].size;
                            model.il3qty = 0;
                            model.il3UOM = ILTsPrinting[index].UOM;
                            model.il3unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                            model.il3extCost = 0;
                            model.il3unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                            model.il3extSell = 0;
                            model.il3minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                            index = index + 1;
                            model.il4linetype = ILTsPrinting[index].lineType;
                            model.il4display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                            model.il4name = ILTsPrinting[index].name;
                            model.il4size = ILTsPrinting[index].size;
                            model.il4qty = 0;
                            model.il4UOM = ILTsPrinting[index].UOM;
                            model.il4unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                            model.il4extCost = 0;
                            model.il4unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                            model.il4extSell = 0;
                            model.il4minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                            index = index + 1;
                            model.il5linetype = ILTsPrinting[index].lineType;
                            model.il5display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                            model.il5name = ILTsPrinting[index].name;
                            model.il5size = ILTsPrinting[index].size;
                            model.il5qty = 0;
                            model.il5UOM = ILTsPrinting[index].UOM;
                            model.il5unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                            model.il5extCost = 0;
                            model.il5unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                            model.il5extSell = 0;
                            model.il5minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                            index = index + 1;
                            model.il6linetype = ILTsPrinting[index].lineType;
                            model.il6display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                            model.il6name = ILTsPrinting[index].name;
                            model.il6size = ILTsPrinting[index].size;
                            model.il6qty = 0;
                            model.il6UOM = ILTsPrinting[index].UOM;
                            model.il6unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                            model.il6extCost = 0;
                            model.il6unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                            model.il6extSell = 0;
                            model.il6minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                            index = index + 1;
                            model.il7linetype = ILTsPrinting[index].lineType;
                            model.il7display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                            model.il7name = ILTsPrinting[index].name;
                            model.il7size = ILTsPrinting[index].size;
                            model.il7qty = 0;
                            model.il7UOM = ILTsPrinting[index].UOM;
                            model.il7unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                            model.il7extCost = 0;
                            model.il7unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                            model.il7extSell = 0;
                            model.il7minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                            index = index + 1;
                            model.il8linetype = ILTsPrinting[index].lineType;
                            model.il8display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                            model.il8name = ILTsPrinting[index].name;
                            model.il8size = ILTsPrinting[index].size;
                            model.il8qty = 0;
                            model.il8UOM = ILTsPrinting[index].UOM;
                            model.il8unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                            model.il8extCost = 0;
                            model.il8unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                            model.il8extSell = 0;
                            model.il8minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                            index = index + 1;
                            model.il9linetype = ILTsPrinting[index].lineType;
                            model.il9display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                            model.il9name = ILTsPrinting[index].name;
                            model.il9size = ILTsPrinting[index].size;
                            model.il9qty = 0;
                            model.il9UOM = ILTsPrinting[index].UOM;
                            model.il9unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                            model.il9extCost = 0;
                            model.il9unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                            model.il9extSell = 0;
                            model.il9minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                            index = index + 1;
                            model.il10linetype = ILTsPrinting[index].lineType;
                            model.il10display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                            model.il10name = ILTsPrinting[index].name;
                            model.il10size = ILTsPrinting[index].size;
                            model.il10qty = 0;
                            model.il10UOM = ILTsPrinting[index].UOM;
                            model.il10unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                            model.il10extCost = 0;
                            model.il10unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                            model.il10extSell = 0;
                            model.il10minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                            index = index + 1;
                            model.il11linetype = ILTsPrinting[index].lineType;
                            model.il11display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                            model.il11name = ILTsPrinting[index].name;
                            model.il11size = ILTsPrinting[index].size;
                            model.il11qty = 0;
                            model.il11UOM = ILTsPrinting[index].UOM;
                            model.il11unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                            model.il11extCost = 0;
                            model.il11unitSell = ILTsPrinting[index].unitSell.HasValue ? ILTsPrinting[index].unitSell.Value : 0;
                            model.il11extSell = 0;
                            model.il11minExtSell = ILTsPrinting[index].minExtSell.HasValue ? ILTsPrinting[index].minExtSell.Value : 0;

                            #endregion

                            List<InvoiceLinesTemplate> ILTsMailing = ILTs.Where(d => d.lineType.Contains("Mailing")).OrderBy(d => d.displayOrder).ToList();
                            int indexNew = 0;

                            #region Mailing Line Items

                            model.il20linetype = ILTsMailing[indexNew].lineType;
                            model.il20display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il20name = ILTsMailing[indexNew].name;
                            model.il20size = ILTsMailing[indexNew].size;
                            model.il20qty = 0;
                            model.il20UOM = ILTsMailing[indexNew].UOM;
                            model.il20unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il20extCost = 0;
                            model.il20unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                            model.il20extSell = 0;
                            model.il20minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il21linetype = ILTsMailing[indexNew].lineType;
                            model.il21display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il21name = ILTsMailing[indexNew].name;
                            model.il21size = ILTsMailing[indexNew].size;
                            model.il21qty = 0;
                            model.il21UOM = ILTsMailing[indexNew].UOM;
                            model.il21unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il21extCost = 0;
                            model.il21unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                            model.il21extSell = 0;
                            model.il21minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il22linetype = ILTsMailing[indexNew].lineType;
                            model.il22display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il22name = ILTsMailing[indexNew].name;
                            model.il22size = ILTsMailing[indexNew].size;
                            model.il22qty = 0;
                            model.il22UOM = ILTsMailing[indexNew].UOM;
                            model.il22unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il22extCost = 0;
                            model.il22unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                            model.il22extSell = 0;
                            model.il22minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il23linetype = ILTsMailing[indexNew].lineType;
                            model.il23display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il23name = ILTsMailing[indexNew].name;
                            model.il23size = ILTsMailing[indexNew].size;
                            model.il23qty = 0;
                            model.il23UOM = ILTsMailing[indexNew].UOM;
                            model.il23unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il23extCost = 0;
                            model.il23unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                            model.il23extSell = 0;
                            model.il23minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il24linetype = ILTsMailing[indexNew].lineType;
                            model.il24display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il24name = ILTsMailing[indexNew].name;
                            model.il24size = ILTsMailing[indexNew].size;
                            model.il24qty = 0;
                            model.il24UOM = ILTsMailing[indexNew].UOM;
                            model.il24unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il24extCost = 0;
                            model.il24unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                            model.il24extSell = 0;
                            model.il24minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il25linetype = ILTsMailing[indexNew].lineType;
                            model.il25display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il25name = ILTsMailing[indexNew].name;
                            model.il25size = ILTsMailing[indexNew].size;
                            model.il25qty = 0;
                            model.il25UOM = ILTsMailing[indexNew].UOM;
                            model.il25unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il25extCost = 0;
                            model.il25unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                            model.il25extSell = 0;
                            model.il25minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il26linetype = ILTsMailing[indexNew].lineType;
                            model.il26display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il26name = ILTsMailing[indexNew].name;
                            model.il26size = ILTsMailing[indexNew].size;
                            model.il26qty = 0;
                            model.il26UOM = ILTsMailing[indexNew].UOM;
                            model.il26unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il26extCost = 0;
                            model.il26unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                            model.il26extSell = 0;
                            model.il26minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il27linetype = ILTsMailing[indexNew].lineType;
                            model.il27display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il27name = ILTsMailing[indexNew].name;
                            model.il27size = ILTsMailing[indexNew].size;
                            model.il27qty = 0;
                            model.il27UOM = ILTsMailing[indexNew].UOM;
                            model.il27unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il27extCost = 0;
                            model.il27unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                            model.il27extSell = 0;
                            model.il27minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il28linetype = ILTsMailing[indexNew].lineType;
                            model.il28display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il28name = ILTsMailing[indexNew].name;
                            model.il28size = ILTsMailing[indexNew].size;
                            model.il28qty = 0;
                            model.il28UOM = ILTsMailing[indexNew].UOM;
                            model.il28unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il28extCost = 0;
                            model.il28unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                            model.il28extSell = 0;
                            model.il28minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il29linetype = ILTsMailing[indexNew].lineType;
                            model.il29display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il29name = ILTsMailing[indexNew].name;
                            model.il29size = ILTsMailing[indexNew].size;
                            model.il29qty = 0;
                            model.il29UOM = ILTsMailing[indexNew].UOM;
                            model.il29unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il29extCost = 0;
                            model.il29unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                            model.il29extSell = 0;
                            model.il29minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il30linetype = ILTsMailing[indexNew].lineType;
                            model.il30display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il30name = ILTsMailing[indexNew].name;
                            model.il30size = ILTsMailing[indexNew].size;
                            model.il30qty = 0;
                            model.il30UOM = ILTsMailing[indexNew].UOM;
                            model.il30unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il30extCost = 0;
                            model.il30unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                            model.il30extSell = 0;
                            model.il30minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il31linetype = ILTsMailing[indexNew].lineType;
                            model.il31display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il31name = ILTsMailing[indexNew].name;
                            model.il31size = ILTsMailing[indexNew].size;
                            model.il31qty = 0;
                            model.il31UOM = ILTsMailing[indexNew].UOM;
                            model.il31unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il31extCost = 0;
                            model.il31unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                            model.il31extSell = 0;
                            model.il31minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il32linetype = ILTsMailing[indexNew].lineType;
                            model.il32display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il32name = ILTsMailing[indexNew].name;
                            model.il32size = ILTsMailing[indexNew].size;
                            model.il32qty = 0;
                            model.il32UOM = ILTsMailing[indexNew].UOM;
                            model.il32unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il32extCost = 0;
                            model.il32unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                            model.il32extSell = 0;
                            model.il32minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il33linetype = ILTsMailing[indexNew].lineType;
                            model.il33display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il33name = ILTsMailing[indexNew].name;
                            model.il33size = ILTsMailing[indexNew].size;
                            model.il33qty = 0;
                            model.il33UOM = ILTsMailing[indexNew].UOM;
                            model.il33unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il33extCost = 0;
                            model.il33unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                            model.il33extSell = 0;
                            model.il33minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il34linetype = ILTsMailing[indexNew].lineType;
                            model.il34display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il34name = ILTsMailing[indexNew].name;
                            model.il34size = ILTsMailing[indexNew].size;
                            model.il34qty = 0;
                            model.il34UOM = ILTsMailing[indexNew].UOM;
                            model.il34unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il34extCost = 0;
                            model.il34unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                            model.il34extSell = 0;
                            model.il34minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il35linetype = ILTsMailing[indexNew].lineType;
                            model.il35display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il35name = ILTsMailing[indexNew].name;
                            model.il35size = ILTsMailing[indexNew].size;
                            model.il35qty = 0;
                            model.il35UOM = ILTsMailing[indexNew].UOM;
                            model.il35unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il35extCost = 0;
                            model.il35unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                            model.il35extSell = 0;
                            model.il35minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il36linetype = ILTsMailing[indexNew].lineType;
                            model.il36display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il36name = ILTsMailing[indexNew].name;
                            model.il36size = ILTsMailing[indexNew].size;
                            model.il36qty = 0;
                            model.il36UOM = ILTsMailing[indexNew].UOM;
                            model.il36unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il36extCost = 0;
                            model.il36unitSell = ILTsMailing[indexNew].unitSell.HasValue ? ILTsMailing[indexNew].unitSell.Value : 0;
                            model.il36extSell = 0;
                            model.il36minExtSell = ILTsMailing[indexNew].minExtSell.HasValue ? ILTsMailing[indexNew].minExtSell.Value : 0;

                            #endregion

                            #region Freight

                            List<InvoiceLinesTemplate> ILTsFrieght = ILTs.Where(d => d.lineType.Contains("Freight")).OrderBy(d => d.displayOrder).ToList();
                            indexNew = 0;
                            model.il40linetype = ILTsFrieght[indexNew].lineType;
                            model.il40display = ILTsFrieght[indexNew].displayOrder.HasValue ? ILTsFrieght[indexNew].displayOrder.Value : 0;
                            model.il40name = ILTsFrieght[indexNew].name;
                            model.il40size = ILTsFrieght[indexNew].size;
                            model.il40qty = 0;
                            model.il40UOM = ILTsFrieght[indexNew].UOM;
                            model.il40unitCost = ILTsFrieght[indexNew].unitCost.HasValue ? ILTsFrieght[indexNew].unitCost.Value : 0;
                            model.il40extCost = 0;
                            model.il40unitSell = ILTsFrieght[indexNew].unitSell.HasValue ? ILTsFrieght[indexNew].unitSell.Value : 0;
                            model.il40extSell = 0;
                            model.il40minExtSell = ILTsFrieght[indexNew].minExtSell.HasValue ? ILTsFrieght[indexNew].minExtSell.Value : 0;

                            #endregion
                        }
                        else if (StatusType == "edit")
                        {
                            Invoice inv = db.Invoices.FirstOrDefault(d => d.orderID == OrderId);
                            Order ORD = db.Orders.FirstOrDefault(d => d.lngID == OrderId);
                            model.InvoiceOrder = ORD;
                            model.orderID = ORD.lngID;
                            model.ilOrderID = ORD.lngID;
                            model.ilStatusType = "edit";
                            model.ilInvoiceID = inv.lngID;
                            model.comment = inv.comment;

                            model.DTClientOrderNo = ORD.lngID.ToString();
                            model.DTClientChargeTo = ORD.envelope_product_Cost_Center;
                            model.DTClientJobName = ORD.job_details_jobName;
                            model.DTVendorInvoiceNo = inv.lngID.ToString();
                            model.DTVendorInvoiceDate = inv.datBillSubmitted.Value.ToString("dd/MM/yyyy");

                            List<InvoiceLine> LIs = db.InvoiceLines.Where(d => d.invoiceID == inv.lngID).ToList();
                            List<InvoiceLine> ILTsPrinting = LIs.Where(d => d.lineType.Contains("Printing")).OrderBy(d => d.displayOrder).ToList();

                            List<InvoiceLinesTemplate> ILTs = db.InvoiceLinesTemplates.ToList();
                            List<InvoiceLinesTemplate> ILTemplatePrintings = ILTs.Where(d => d.lineType.Contains("Printing")).OrderBy(d => d.displayOrder).ToList();

                            int index = 0;

                            #region Printing Line Items
                            model.il1linetype = ILTsPrinting[index].lineType;
                            model.il1display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                            model.il1name = ILTsPrinting[index].name;
                            model.il1size = ILTsPrinting[index].size;
                            model.il1qty = ILTsPrinting[index].qty;
                            model.il1UOM = ILTsPrinting[index].UOM;
                            model.il1unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                            model.il1extCost = ILTsPrinting[index].extCost.HasValue ? ILTsPrinting[index].extCost.Value : 0;
                            model.il1unitSell = ILTemplatePrintings[index].unitSell.HasValue ? ILTemplatePrintings[index].unitSell.Value : 0;
                            model.il1extSell = ILTemplatePrintings[index].unitSell.HasValue ? ILTemplatePrintings[index].unitSell.Value * ILTsPrinting[index].qty : 0;
                            model.il1minExtSell = ILTemplatePrintings[index].minExtSell.HasValue ? ILTemplatePrintings[index].minExtSell.Value : 0;

                            index = index + 1;
                            model.il2linetype = ILTsPrinting[index].lineType;
                            model.il2display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                            model.il2name = ILTsPrinting[index].name;
                            model.il2size = ILTsPrinting[index].size;
                            model.il2qty = ILTsPrinting[index].qty;
                            model.il2UOM = ILTsPrinting[index].UOM;
                            model.il2unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                            model.il2extCost = ILTsPrinting[index].extCost.HasValue ? ILTsPrinting[index].extCost.Value : 0;
                            model.il2unitSell = ILTemplatePrintings[index].unitSell.HasValue ? ILTemplatePrintings[index].unitSell.Value : 0;
                            model.il2extSell = ILTemplatePrintings[index].unitSell.HasValue ? ILTemplatePrintings[index].unitSell.Value * ILTsPrinting[index].qty : 0;
                            model.il2minExtSell = ILTemplatePrintings[index].minExtSell.HasValue ? ILTemplatePrintings[index].minExtSell.Value : 0;

                            index = index + 1;
                            model.il3linetype = ILTsPrinting[index].lineType;
                            model.il3display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                            model.il3name = ILTsPrinting[index].name;
                            model.il3size = ILTsPrinting[index].size;
                            model.il3qty = ILTsPrinting[index].qty;
                            model.il3UOM = ILTsPrinting[index].UOM;
                            model.il3unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                            model.il3extCost = ILTsPrinting[index].extCost.HasValue ? ILTsPrinting[index].extCost.Value : 0;
                            model.il3unitSell = ILTemplatePrintings[index].unitSell.HasValue ? ILTemplatePrintings[index].unitSell.Value : 0;
                            model.il3extSell = ILTemplatePrintings[index].unitSell.HasValue ? ILTemplatePrintings[index].unitSell.Value * ILTsPrinting[index].qty : 0;
                            model.il3minExtSell = ILTemplatePrintings[index].minExtSell.HasValue ? ILTemplatePrintings[index].minExtSell.Value : 0;

                            index = index + 1;
                            model.il4linetype = ILTsPrinting[index].lineType;
                            model.il4display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                            model.il4name = ILTsPrinting[index].name;
                            model.il4size = ILTsPrinting[index].size;
                            model.il4qty = ILTsPrinting[index].qty;
                            model.il4UOM = ILTsPrinting[index].UOM;
                            model.il4unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                            model.il4extCost = ILTsPrinting[index].extCost.HasValue ? ILTsPrinting[index].extCost.Value : 0;
                            model.il4unitSell = ILTemplatePrintings[index].unitSell.HasValue ? ILTemplatePrintings[index].unitSell.Value : 0;
                            model.il4extSell = ILTemplatePrintings[index].unitSell.HasValue ? ILTemplatePrintings[index].unitSell.Value * ILTsPrinting[index].qty : 0;
                            model.il4minExtSell = ILTemplatePrintings[index].minExtSell.HasValue ? ILTemplatePrintings[index].minExtSell.Value : 0;

                            index = index + 1;
                            model.il5linetype = ILTsPrinting[index].lineType;
                            model.il5display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                            model.il5name = ILTsPrinting[index].name;
                            model.il5size = ILTsPrinting[index].size;
                            model.il5qty = ILTsPrinting[index].qty;
                            model.il5UOM = ILTsPrinting[index].UOM;
                            model.il5unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                            model.il5extCost = ILTsPrinting[index].extCost.HasValue ? ILTsPrinting[index].extCost.Value : 0;
                            model.il5unitSell = ILTemplatePrintings[index].unitSell.HasValue ? ILTemplatePrintings[index].unitSell.Value : 0;
                            model.il5extSell = ILTemplatePrintings[index].unitSell.HasValue ? ILTemplatePrintings[index].unitSell.Value * ILTsPrinting[index].qty : 0;
                            model.il5minExtSell = ILTemplatePrintings[index].minExtSell.HasValue ? ILTemplatePrintings[index].minExtSell.Value : 0;

                            index = index + 1;
                            model.il6linetype = ILTsPrinting[index].lineType;
                            model.il6display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                            model.il6name = ILTsPrinting[index].name;
                            model.il6size = ILTsPrinting[index].size;
                            model.il6qty = ILTsPrinting[index].qty;
                            model.il6UOM = ILTsPrinting[index].UOM;
                            model.il6unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                            model.il6extCost = ILTsPrinting[index].extCost.HasValue ? ILTsPrinting[index].extCost.Value : 0;
                            model.il6unitSell = ILTemplatePrintings[index].unitSell.HasValue ? ILTemplatePrintings[index].unitSell.Value : 0;
                            model.il6extSell = ILTemplatePrintings[index].unitSell.HasValue ? ILTemplatePrintings[index].unitSell.Value * ILTsPrinting[index].qty : 0;
                            model.il6minExtSell = ILTemplatePrintings[index].minExtSell.HasValue ? ILTemplatePrintings[index].minExtSell.Value : 0;

                            index = index + 1;
                            model.il7linetype = ILTsPrinting[index].lineType;
                            model.il7display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                            model.il7name = ILTsPrinting[index].name;
                            model.il7size = ILTsPrinting[index].size;
                            model.il7qty = ILTsPrinting[index].qty;
                            model.il7UOM = ILTsPrinting[index].UOM;
                            model.il7unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                            model.il7extCost = ILTsPrinting[index].extCost.HasValue ? ILTsPrinting[index].extCost.Value : 0;
                            model.il7unitSell = ILTemplatePrintings[index].unitSell.HasValue ? ILTemplatePrintings[index].unitSell.Value : 0;
                            model.il7extSell = ILTemplatePrintings[index].unitSell.HasValue ? ILTemplatePrintings[index].unitSell.Value * ILTsPrinting[index].qty : 0;
                            model.il7minExtSell = ILTemplatePrintings[index].minExtSell.HasValue ? ILTemplatePrintings[index].minExtSell.Value : 0;

                            index = index + 1;
                            model.il8linetype = ILTsPrinting[index].lineType;
                            model.il8display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                            model.il8name = ILTsPrinting[index].name;
                            model.il8size = ILTsPrinting[index].size;
                            model.il8qty = ILTsPrinting[index].qty;
                            model.il8UOM = ILTsPrinting[index].UOM;
                            model.il8unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                            model.il8extCost = ILTsPrinting[index].extCost.HasValue ? ILTsPrinting[index].extCost.Value : 0;
                            model.il8unitSell = ILTemplatePrintings[index].unitSell.HasValue ? ILTemplatePrintings[index].unitSell.Value : 0;
                            model.il8extSell = ILTemplatePrintings[index].unitSell.HasValue ? ILTemplatePrintings[index].unitSell.Value * ILTsPrinting[index].qty : 0;
                            model.il8minExtSell = ILTemplatePrintings[index].minExtSell.HasValue ? ILTemplatePrintings[index].minExtSell.Value : 0;

                            index = index + 1;
                            model.il9linetype = ILTsPrinting[index].lineType;
                            model.il9display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                            model.il9name = ILTsPrinting[index].name;
                            model.il9size = ILTsPrinting[index].size;
                            model.il9qty = ILTsPrinting[index].qty;
                            model.il9UOM = ILTsPrinting[index].UOM;
                            model.il9unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                            model.il9extCost = ILTsPrinting[index].extCost.HasValue ? ILTsPrinting[index].extCost.Value : 0;
                            model.il9unitSell = ILTemplatePrintings[index].unitSell.HasValue ? ILTemplatePrintings[index].unitSell.Value : 0;
                            model.il9extSell = ILTemplatePrintings[index].unitSell.HasValue ? ILTemplatePrintings[index].unitSell.Value * ILTsPrinting[index].qty : 0;
                            model.il9minExtSell = ILTemplatePrintings[index].minExtSell.HasValue ? ILTemplatePrintings[index].minExtSell.Value : 0;

                            index = index + 1;
                            model.il10linetype = ILTsPrinting[index].lineType;
                            model.il10display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                            model.il10name = ILTsPrinting[index].name;
                            model.il10size = ILTsPrinting[index].size;
                            model.il10qty = ILTsPrinting[index].qty;
                            model.il10UOM = ILTsPrinting[index].UOM;
                            model.il10unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                            model.il10extCost = ILTsPrinting[index].extCost.HasValue ? ILTsPrinting[index].extCost.Value : 0;
                            model.il10unitSell = ILTemplatePrintings[index].unitSell.HasValue ? ILTemplatePrintings[index].unitSell.Value : 0;
                            model.il10extSell = ILTemplatePrintings[index].unitSell.HasValue ? ILTemplatePrintings[index].unitSell.Value * ILTsPrinting[index].qty : 0;
                            model.il10minExtSell = ILTemplatePrintings[index].minExtSell.HasValue ? ILTemplatePrintings[index].minExtSell.Value : 0;

                            index = index + 1;
                            model.il11linetype = ILTsPrinting[index].lineType;
                            model.il11display = ILTsPrinting[index].displayOrder.HasValue ? ILTsPrinting[index].displayOrder.Value : 0;
                            model.il11name = ILTsPrinting[index].name;
                            model.il11size = ILTsPrinting[index].size;
                            model.il11qty = ILTsPrinting[index].qty;
                            model.il11UOM = ILTsPrinting[index].UOM;
                            model.il11unitCost = ILTsPrinting[index].unitCost.HasValue ? ILTsPrinting[index].unitCost.Value : 0;
                            model.il11extCost = ILTsPrinting[index].extCost.HasValue ? ILTsPrinting[index].extCost.Value : 0;
                            model.il11unitSell = ILTemplatePrintings[index].unitSell.HasValue ? ILTemplatePrintings[index].unitSell.Value : 0;
                            model.il11extSell = ILTemplatePrintings[index].unitSell.HasValue ? ILTemplatePrintings[index].unitSell.Value * ILTsPrinting[index].qty : 0;
                            model.il11minExtSell = ILTemplatePrintings[index].minExtSell.HasValue ? ILTemplatePrintings[index].minExtSell.Value : 0;

                            #endregion
                            List<InvoiceLine> ILTsMailing = LIs.Where(d => d.lineType.Contains("Mailing")).OrderBy(d => d.displayOrder).ToList();
                            int indexNew = 0;
                            List<InvoiceLinesTemplate> ILTemplateMailing = ILTs.Where(d => d.lineType.Contains("Mailing")).OrderBy(d => d.displayOrder).ToList();


                            #region Mailing Line Items

                            model.il20linetype = ILTsMailing[indexNew].lineType;
                            model.il20display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il20name = ILTsMailing[indexNew].name;
                            model.il20size = ILTsMailing[indexNew].size;
                            model.il20qty = ILTsMailing[indexNew].qty;
                            model.il20UOM = ILTsMailing[indexNew].UOM;
                            model.il20unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il20extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0;
                            model.il20unitSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value : 0;
                            model.il20extSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value * ILTsMailing[indexNew].qty : 0;
                            model.il20minExtSell = ILTemplateMailing[indexNew].minExtSell.HasValue ? ILTemplateMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il21linetype = ILTsMailing[indexNew].lineType;
                            model.il21display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il21name = ILTsMailing[indexNew].name;
                            model.il21size = ILTsMailing[indexNew].size;
                            model.il21qty = ILTsMailing[indexNew].qty;
                            model.il21UOM = ILTsMailing[indexNew].UOM;
                            model.il21unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il21extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0;
                            model.il21unitSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value : 0;
                            model.il21extSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value * ILTsMailing[indexNew].qty : 0;
                            model.il21minExtSell = ILTemplateMailing[indexNew].minExtSell.HasValue ? ILTemplateMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il22linetype = ILTsMailing[indexNew].lineType;
                            model.il22display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il22name = ILTsMailing[indexNew].name;
                            model.il22size = ILTsMailing[indexNew].size;
                            model.il22qty = ILTsMailing[indexNew].qty;
                            model.il22UOM = ILTsMailing[indexNew].UOM;
                            model.il22unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il22extCost = 0;
                            model.il22unitSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value : 0;
                            model.il22extSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value * ILTsMailing[indexNew].qty : 0;
                            model.il22minExtSell = ILTemplateMailing[indexNew].minExtSell.HasValue ? ILTemplateMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il23linetype = ILTsMailing[indexNew].lineType;
                            model.il23display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il23name = ILTsMailing[indexNew].name;
                            model.il23size = ILTsMailing[indexNew].size;
                            model.il23qty = ILTsMailing[indexNew].qty;
                            model.il23UOM = ILTsMailing[indexNew].UOM;
                            model.il23unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il23extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0;
                            model.il23unitSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value : 0;
                            model.il23extSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value * ILTsMailing[indexNew].qty : 0;
                            model.il23minExtSell = ILTemplateMailing[indexNew].minExtSell.HasValue ? ILTemplateMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il24linetype = ILTsMailing[indexNew].lineType;
                            model.il24display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il24name = ILTsMailing[indexNew].name;
                            model.il24size = ILTsMailing[indexNew].size;
                            model.il24qty = ILTsMailing[indexNew].qty;
                            model.il24UOM = ILTsMailing[indexNew].UOM;
                            model.il24unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il24extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0;
                            model.il24unitSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value : 0;
                            model.il24extSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value * ILTsMailing[indexNew].qty : 0;
                            model.il24minExtSell = ILTemplateMailing[indexNew].minExtSell.HasValue ? ILTemplateMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il25linetype = ILTsMailing[indexNew].lineType;
                            model.il25display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il25name = ILTsMailing[indexNew].name;
                            model.il25size = ILTsMailing[indexNew].size;
                            model.il25qty = ILTsMailing[indexNew].qty;
                            model.il25UOM = ILTsMailing[indexNew].UOM;
                            model.il25unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il25extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0;
                            model.il25unitSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value : 0;
                            model.il25extSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value * ILTsMailing[indexNew].qty : 0;
                            model.il25minExtSell = ILTemplateMailing[indexNew].minExtSell.HasValue ? ILTemplateMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il26linetype = ILTsMailing[indexNew].lineType;
                            model.il26display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il26name = ILTsMailing[indexNew].name;
                            model.il26size = ILTsMailing[indexNew].size;
                            model.il26qty = ILTsMailing[indexNew].qty;
                            model.il26UOM = ILTsMailing[indexNew].UOM;
                            model.il26unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il26extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0;
                            model.il26unitSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value : 0;
                            model.il26extSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value * ILTsMailing[indexNew].qty : 0;
                            model.il26minExtSell = ILTemplateMailing[indexNew].minExtSell.HasValue ? ILTemplateMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il27linetype = ILTsMailing[indexNew].lineType;
                            model.il27display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il27name = ILTsMailing[indexNew].name;
                            model.il27size = ILTsMailing[indexNew].size;
                            model.il27qty = ILTsMailing[indexNew].qty;
                            model.il27UOM = ILTsMailing[indexNew].UOM;
                            model.il27unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il27extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0;
                            model.il27unitSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value : 0;
                            model.il27extSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value * ILTsMailing[indexNew].qty : 0;
                            model.il27minExtSell = ILTemplateMailing[indexNew].minExtSell.HasValue ? ILTemplateMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il28linetype = ILTsMailing[indexNew].lineType;
                            model.il28display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il28name = ILTsMailing[indexNew].name;
                            model.il28size = ILTsMailing[indexNew].size;
                            model.il28qty = ILTsMailing[indexNew].qty;
                            model.il28UOM = ILTsMailing[indexNew].UOM;
                            model.il28unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il28extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0;
                            model.il28unitSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value : 0;
                            model.il28extSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value * ILTsMailing[indexNew].qty : 0;
                            model.il28minExtSell = ILTemplateMailing[indexNew].minExtSell.HasValue ? ILTemplateMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il29linetype = ILTsMailing[indexNew].lineType;
                            model.il29display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il29name = ILTsMailing[indexNew].name;
                            model.il29size = ILTsMailing[indexNew].size;
                            model.il29qty = ILTsMailing[indexNew].qty;
                            model.il29UOM = ILTsMailing[indexNew].UOM;
                            model.il29unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il29extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0;
                            model.il29unitSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value : 0;
                            model.il29extSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value * ILTsMailing[indexNew].qty : 0;
                            model.il29minExtSell = ILTemplateMailing[indexNew].minExtSell.HasValue ? ILTemplateMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il30linetype = ILTsMailing[indexNew].lineType;
                            model.il30display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il30name = ILTsMailing[indexNew].name;
                            model.il30size = ILTsMailing[indexNew].size;
                            model.il30qty = ILTsMailing[indexNew].qty;
                            model.il30UOM = ILTsMailing[indexNew].UOM;
                            model.il30unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il30extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0;
                            model.il30unitSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value : 0;
                            model.il30extSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value * ILTsMailing[indexNew].qty : 0;
                            model.il30minExtSell = ILTemplateMailing[indexNew].minExtSell.HasValue ? ILTemplateMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il31linetype = ILTsMailing[indexNew].lineType;
                            model.il31display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il31name = ILTsMailing[indexNew].name;
                            model.il31size = ILTsMailing[indexNew].size;
                            model.il31qty = ILTsMailing[indexNew].qty;
                            model.il31UOM = ILTsMailing[indexNew].UOM;
                            model.il31unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il31extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0;
                            model.il31unitSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value : 0;
                            model.il31extSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value * ILTsMailing[indexNew].qty : 0;
                            model.il31minExtSell = ILTemplateMailing[indexNew].minExtSell.HasValue ? ILTemplateMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il32linetype = ILTsMailing[indexNew].lineType;
                            model.il32display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il32name = ILTsMailing[indexNew].name;
                            model.il32size = ILTsMailing[indexNew].size;
                            model.il32qty = ILTsMailing[indexNew].qty;
                            model.il32UOM = ILTsMailing[indexNew].UOM;
                            model.il32unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il32extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0;
                            model.il32unitSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value : 0;
                            model.il32extSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value * ILTsMailing[indexNew].qty : 0;
                            model.il32minExtSell = ILTemplateMailing[indexNew].minExtSell.HasValue ? ILTemplateMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il33linetype = ILTsMailing[indexNew].lineType;
                            model.il33display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il33name = ILTsMailing[indexNew].name;
                            model.il33size = ILTsMailing[indexNew].size;
                            model.il33qty = ILTsMailing[indexNew].qty;
                            model.il33UOM = ILTsMailing[indexNew].UOM;
                            model.il33unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il33extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0;
                            model.il33unitSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value : 0;
                            model.il33extSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value * ILTsMailing[indexNew].qty : 0;
                            model.il33minExtSell = ILTemplateMailing[indexNew].minExtSell.HasValue ? ILTemplateMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il34linetype = ILTsMailing[indexNew].lineType;
                            model.il34display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il34name = ILTsMailing[indexNew].name;
                            model.il34size = ILTsMailing[indexNew].size;
                            model.il34qty = ILTsMailing[indexNew].qty;
                            model.il34UOM = ILTsMailing[indexNew].UOM;
                            model.il34unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il34extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0;
                            model.il34unitSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value : 0;
                            model.il34extSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value * ILTsMailing[indexNew].qty : 0;
                            model.il34minExtSell = ILTemplateMailing[indexNew].minExtSell.HasValue ? ILTemplateMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il35linetype = ILTsMailing[indexNew].lineType;
                            model.il35display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il35name = ILTsMailing[indexNew].name;
                            model.il35size = ILTsMailing[indexNew].size;
                            model.il35qty = ILTsMailing[indexNew].qty;
                            model.il35UOM = ILTsMailing[indexNew].UOM;
                            model.il35unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il35extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0;
                            model.il35unitSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value : 0;
                            model.il35extSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value * ILTsMailing[indexNew].qty : 0;
                            model.il35minExtSell = ILTemplateMailing[indexNew].minExtSell.HasValue ? ILTemplateMailing[indexNew].minExtSell.Value : 0;

                            indexNew = indexNew + 1;
                            model.il36linetype = ILTsMailing[indexNew].lineType;
                            model.il36display = ILTsMailing[indexNew].displayOrder.HasValue ? ILTsMailing[indexNew].displayOrder.Value : 0;
                            model.il36name = ILTsMailing[indexNew].name;
                            model.il36size = ILTsMailing[indexNew].size;
                            model.il36qty = ILTsMailing[indexNew].qty;
                            model.il36UOM = ILTsMailing[indexNew].UOM;
                            model.il36unitCost = ILTsMailing[indexNew].unitCost.HasValue ? ILTsMailing[indexNew].unitCost.Value : 0;
                            model.il36extCost = ILTsMailing[indexNew].extCost.HasValue ? ILTsMailing[indexNew].extCost.Value : 0;
                            model.il36unitSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value : 0;
                            model.il36extSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value * ILTsMailing[indexNew].qty : 0;
                            model.il36minExtSell = ILTemplateMailing[indexNew].minExtSell.HasValue ? ILTemplateMailing[indexNew].minExtSell.Value : 0;

                            #endregion

                            #region Freight
                            List<InvoiceLine> ILTsFrieght = LIs.Where(d => d.lineType.Contains("Freight")).OrderBy(d => d.displayOrder).ToList();

                            indexNew = 0;
                            model.il40linetype = ILTsFrieght[indexNew].lineType;
                            model.il40display = ILTsFrieght[indexNew].displayOrder.HasValue ? ILTsFrieght[indexNew].displayOrder.Value : 0;
                            model.il40name = ILTsFrieght[indexNew].name;
                            model.il40size = ILTsFrieght[indexNew].size;
                            model.il40qty = ILTsFrieght[indexNew].qty;
                            model.il40UOM = ILTsFrieght[indexNew].UOM;
                            model.il40unitCost = ILTsFrieght[indexNew].unitCost.HasValue ? ILTsFrieght[indexNew].unitCost.Value : 0;
                            model.il40extCost = ILTsFrieght[indexNew].extCost.HasValue ? ILTsFrieght[indexNew].extCost.Value : 0;
                            model.il40unitSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value : 0;
                            model.il40extSell = ILTemplateMailing[indexNew].unitSell.HasValue ? ILTemplateMailing[indexNew].unitSell.Value * ILTsMailing[indexNew].qty : 0;
                            model.il40minExtSell = ILTemplateMailing[indexNew].minExtSell.HasValue ? ILTemplateMailing[indexNew].minExtSell.Value : 0;

                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ResultMessage = "Error: " + ex.Message;
            }
            return Json(model);
        }

        [HttpPost]
        public string SubmitForCreateInvoice(InvoiceDetails model)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return "Session expired, please login again.";
            
            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                { 
                    Invoice obj = new Invoice();

                    obj.orderID = model.ilOrderID;
                    obj.status = "Pending by vendor";
                    obj.vendorInvoiceNo = model.vendorInvoiceNo;
                    obj.comment = model.comment;
                    obj.datBillSubmitted = DateTime.Now;
                    obj.datBillRevised = null;
                    obj.datClientInvoiced = null;
                    obj.datClientInvoiceUploaded = null;
                    obj.datClientPayment = null;
                    obj.lastTouch = "";
                    obj.clientInvoiceFile = "";
                    obj.vendorInvoiceFile = "";
                    obj.internalInvoiceFile = "";
                    db.Invoices.Add(obj);
                    db.SaveChanges();

                    List<InvoiceLinesTemplate> ILTs = db.InvoiceLinesTemplates.ToList();

                    #region Printing Line Items
                    List<InvoiceLinesTemplate> ILTsPrinting = ILTs.Where(d => d.lineType.Contains("Printing")).OrderBy(d => d.displayOrder).ToList();
                    int index = 0;

                    InvoiceLine IL1 = new InvoiceLine();
                    IL1.invoiceID = obj.lngID;
                    IL1.lineType = model.il1linetype;
                    IL1.displayOrder = model.il1display;
                    IL1.name = ILTsPrinting[index].name;
                    IL1.size = ILTsPrinting[index].size;
                    IL1.qty = model.il1qty;
                    IL1.UOM = ILTsPrinting[index].UOM;
                    IL1.unitCost = model.il1unitCost;
                    IL1.extCost = model.il1extCost;
                    IL1.unitSell = model.il1unitSell;
                    IL1.extSell = model.il1extSell;
                    IL1.minExtSell = model.il1minExtSell;
                    IL1.minExtCost = 0;
                    db.InvoiceLines.Add(IL1);

                    index = index + 1;
                    InvoiceLine IL2 = new InvoiceLine();
                    IL2.invoiceID = obj.lngID;
                    IL2.lineType = model.il2linetype;
                    IL2.displayOrder = model.il2display;
                    IL2.name = ILTsPrinting[index].name;
                    IL2.size = ILTsPrinting[index].size;
                    IL2.qty = model.il2qty;
                    IL2.UOM = ILTsPrinting[index].UOM;
                    IL2.unitCost = model.il2unitCost;
                    IL2.extCost = model.il2extCost;
                    IL2.unitSell = model.il2unitSell;
                    IL2.extSell = model.il2extSell;
                    IL2.minExtSell = model.il2minExtSell;
                    IL2.minExtCost = 0;
                    db.InvoiceLines.Add(IL2);

                    index = index + 1;
                    InvoiceLine IL3 = new InvoiceLine();
                    IL3.invoiceID = obj.lngID;
                    IL3.lineType = model.il3linetype;
                    IL3.displayOrder = model.il3display;
                    IL3.name = ILTsPrinting[index].name;
                    IL3.size = ILTsPrinting[index].size;
                    IL3.qty = model.il3qty;
                    IL3.UOM = ILTsPrinting[index].UOM;
                    IL3.unitCost = model.il3unitCost;
                    IL3.extCost = model.il3extCost;
                    IL3.unitSell = model.il3unitSell;
                    IL3.extSell = model.il3extSell;
                    IL3.minExtSell = model.il3minExtSell;
                    IL3.minExtCost = 0;
                    db.InvoiceLines.Add(IL3);

                    index = index + 1;
                    InvoiceLine IL4 = new InvoiceLine();
                    IL4.invoiceID = obj.lngID;
                    IL4.lineType = model.il4linetype;
                    IL4.displayOrder = model.il4display;
                    IL4.name = ILTsPrinting[index].name;
                    IL4.size = ILTsPrinting[index].size;
                    IL4.qty = model.il4qty;
                    IL4.UOM = ILTsPrinting[index].UOM;
                    IL4.unitCost = model.il4unitCost;
                    IL4.extCost = model.il4extCost;
                    IL4.unitSell = model.il4unitSell;
                    IL4.extSell = model.il4extSell;
                    IL4.minExtSell = model.il4minExtSell;
                    IL4.minExtCost = 0;
                    db.InvoiceLines.Add(IL4);

                    index = index + 1;
                    InvoiceLine IL5 = new InvoiceLine();
                    IL5.invoiceID = obj.lngID;
                    IL5.lineType = model.il5linetype;
                    IL5.displayOrder = model.il5display;
                    IL5.name = ILTsPrinting[index].name;
                    IL5.size = ILTsPrinting[index].size;
                    IL5.qty = model.il5qty;
                    IL5.UOM = ILTsPrinting[index].UOM;
                    IL5.unitCost = model.il5unitCost;
                    IL5.extCost = model.il5extCost;
                    IL5.unitSell = model.il5unitSell;
                    IL5.extSell = model.il5extSell;
                    IL5.minExtSell = model.il5minExtSell;
                    IL5.minExtCost = 0;
                    db.InvoiceLines.Add(IL5);

                    index = index + 1;
                    InvoiceLine IL6 = new InvoiceLine();
                    IL6.invoiceID = obj.lngID;
                    IL6.lineType = model.il6linetype;
                    IL6.displayOrder = model.il6display;
                    IL6.name = ILTsPrinting[index].name;
                    IL6.size = ILTsPrinting[index].size;
                    IL6.qty = model.il6qty;
                    IL6.UOM = ILTsPrinting[index].UOM;
                    IL6.unitCost = model.il6unitCost;
                    IL6.extCost = model.il6extCost;
                    IL6.unitSell = model.il6unitSell;
                    IL6.extSell = model.il6extSell;
                    IL6.minExtSell = model.il6minExtSell;
                    IL6.minExtCost = 0;
                    db.InvoiceLines.Add(IL6);

                    index = index + 1;
                    InvoiceLine IL7 = new InvoiceLine();
                    IL7.invoiceID = obj.lngID;
                    IL7.lineType = model.il7linetype;
                    IL7.displayOrder = model.il7display;
                    IL7.name = ILTsPrinting[index].name;
                    IL7.size = ILTsPrinting[index].size;
                    IL7.qty = model.il7qty;
                    IL7.UOM = ILTsPrinting[index].UOM;
                    IL7.unitCost = model.il7unitCost;
                    IL7.extCost = model.il7extCost;
                    IL7.unitSell = model.il7unitSell;
                    IL7.extSell = model.il7extSell;
                    IL7.minExtSell = model.il7minExtSell;
                    IL7.minExtCost = 0;
                    db.InvoiceLines.Add(IL7);

                    index = index + 1;
                    InvoiceLine IL8 = new InvoiceLine();
                    IL8.invoiceID = obj.lngID;
                    IL8.lineType = model.il8linetype;
                    IL8.displayOrder = model.il8display;
                    IL8.name = ILTsPrinting[index].name;
                    IL8.size = ILTsPrinting[index].size;
                    IL8.qty = model.il8qty;
                    IL8.UOM = ILTsPrinting[index].UOM;
                    IL8.unitCost = model.il8unitCost;
                    IL8.extCost = model.il8extCost;
                    IL8.unitSell = model.il8unitSell;
                    IL8.extSell = model.il8extSell;
                    IL8.minExtSell = model.il8minExtSell;
                    IL8.minExtCost = 0;
                    db.InvoiceLines.Add(IL8);

                    index = index + 1;
                    InvoiceLine IL9 = new InvoiceLine();
                    IL9.invoiceID = obj.lngID;
                    IL9.lineType = model.il9linetype;
                    IL9.displayOrder = model.il9display;
                    IL9.name = ILTsPrinting[index].name;
                    IL9.size = ILTsPrinting[index].size;
                    IL9.qty = model.il9qty;
                    IL9.UOM = ILTsPrinting[index].UOM;
                    IL9.unitCost = model.il9unitCost;
                    IL9.extCost = model.il9extCost;
                    IL9.unitSell = model.il9unitSell;
                    IL9.extSell = model.il9extSell;
                    IL9.minExtSell = model.il9minExtSell;
                    IL9.minExtCost = 0;
                    db.InvoiceLines.Add(IL9);

                    index = index + 1;
                    InvoiceLine IL10 = new InvoiceLine();
                    IL10.invoiceID = obj.lngID;
                    IL10.lineType = model.il10linetype;
                    IL10.displayOrder = model.il10display;
                    IL10.name = ILTsPrinting[index].name;
                    IL10.size = ILTsPrinting[index].size;
                    IL10.qty = model.il10qty;
                    IL10.UOM = ILTsPrinting[index].UOM;
                    IL10.unitCost = model.il10unitCost;
                    IL10.extCost = model.il10extCost;
                    IL10.unitSell = model.il10unitSell;
                    IL10.extSell = model.il10extSell;
                    IL10.minExtSell = model.il10minExtSell;
                    IL10.minExtCost = 0;
                    db.InvoiceLines.Add(IL10);

                    index = index + 1;
                    InvoiceLine IL11 = new InvoiceLine();
                    IL11.invoiceID = obj.lngID;
                    IL11.lineType = model.il11linetype;
                    IL11.displayOrder = model.il11display;
                    IL11.name = ILTsPrinting[index].name;
                    IL11.size = ILTsPrinting[index].size;
                    IL11.qty = model.il11qty;
                    IL11.UOM = ILTsPrinting[index].UOM;
                    IL11.unitCost = model.il11unitCost;
                    IL11.extCost = model.il11extCost;
                    IL11.unitSell = model.il11unitSell;
                    IL11.extSell = model.il11extSell;
                    IL11.minExtSell = model.il11minExtSell;
                    IL11.minExtCost = 0;
                    db.InvoiceLines.Add(IL11);

                    List<InvoiceLinesTemplate> ILTsMailing = ILTs.Where(d => d.lineType.Contains("Mailing")).OrderBy(d => d.displayOrder).ToList();

                    index = 0;
                    InvoiceLine IL20 = new InvoiceLine();
                    IL20.invoiceID = obj.lngID;
                    IL20.lineType = model.il20linetype;
                    IL20.displayOrder = model.il20display;
                    IL20.name = ILTsMailing[index].name;
                    IL20.size = ILTsMailing[index].size;
                    IL20.qty = model.il20qty;
                    IL20.UOM = ILTsMailing[index].UOM;
                    IL20.unitCost = model.il20unitCost;
                    IL20.extCost = model.il20extCost;
                    IL20.unitSell = model.il20unitSell;
                    IL20.extSell = model.il20extSell;
                    IL20.minExtSell = model.il20minExtSell;
                    IL20.minExtCost = 0;
                    db.InvoiceLines.Add(IL20);

                    index = index + 1;
                    InvoiceLine IL21 = new InvoiceLine();
                    IL21.invoiceID = obj.lngID;
                    IL21.lineType = model.il21linetype;
                    IL21.displayOrder = model.il21display;
                    IL21.name = ILTsMailing[index].name;
                    IL21.size = ILTsMailing[index].size;
                    IL21.qty = model.il21qty;
                    IL21.UOM = ILTsMailing[index].UOM;
                    IL21.unitCost = model.il21unitCost;
                    IL21.extCost = model.il21extCost;
                    IL21.unitSell = model.il21unitSell;
                    IL21.extSell = model.il21extSell;
                    IL21.minExtSell = model.il21minExtSell;
                    IL21.minExtCost = 0;
                    db.InvoiceLines.Add(IL21);

                    index = index + 1;
                    InvoiceLine IL22 = new InvoiceLine();
                    IL22.invoiceID = obj.lngID;
                    IL22.lineType = model.il22linetype;
                    IL22.displayOrder = model.il22display;
                    IL22.name = ILTsMailing[index].name;
                    IL22.size = ILTsMailing[index].size;
                    IL22.qty = model.il22qty;
                    IL22.UOM = ILTsMailing[index].UOM;
                    IL22.unitCost = model.il22unitCost;
                    IL22.extCost = model.il22extCost;
                    IL22.unitSell = model.il22unitSell;
                    IL22.extSell = model.il22extSell;
                    IL22.minExtSell = model.il22minExtSell;
                    IL22.minExtCost = 0;
                    db.InvoiceLines.Add(IL22);

                    index = index + 1;
                    InvoiceLine IL23 = new InvoiceLine();
                    IL23.invoiceID = obj.lngID;
                    IL23.lineType = model.il23linetype;
                    IL23.displayOrder = model.il23display;
                    IL23.name = ILTsMailing[index].name;
                    IL23.size = ILTsMailing[index].size;
                    IL23.qty = model.il23qty;
                    IL23.UOM = ILTsMailing[index].UOM;
                    IL23.unitCost = model.il23unitCost;
                    IL23.extCost = model.il23extCost;
                    IL23.unitSell = model.il23unitSell;
                    IL23.extSell = model.il23extSell;
                    IL23.minExtSell = model.il23minExtSell;
                    IL23.minExtCost = 0;
                    db.InvoiceLines.Add(IL23);

                    index = index + 1;
                    InvoiceLine IL24 = new InvoiceLine();
                    IL24.invoiceID = obj.lngID;
                    IL24.lineType = model.il24linetype;
                    IL24.displayOrder = model.il24display;
                    IL24.name = ILTsMailing[index].name;
                    IL24.size = ILTsMailing[index].size;
                    IL24.qty = model.il24qty;
                    IL24.UOM = ILTsMailing[index].UOM;
                    IL24.unitCost = model.il24unitCost;
                    IL24.extCost = model.il24extCost;
                    IL24.unitSell = model.il24unitSell;
                    IL24.extSell = model.il24extSell;
                    IL24.minExtSell = model.il24minExtSell;
                    IL24.minExtCost = 0;
                    db.InvoiceLines.Add(IL24);

                    index = index + 1;
                    InvoiceLine IL25 = new InvoiceLine();
                    IL25.invoiceID = obj.lngID;
                    IL25.lineType = model.il25linetype;
                    IL25.displayOrder = model.il25display;
                    IL25.name = ILTsMailing[index].name;
                    IL25.size = ILTsMailing[index].size;
                    IL25.qty = model.il25qty;
                    IL25.UOM = ILTsMailing[index].UOM;
                    IL25.unitCost = model.il25unitCost;
                    IL25.extCost = model.il25extCost;
                    IL25.unitSell = model.il25unitSell;
                    IL25.extSell = model.il25extSell;
                    IL25.minExtSell = model.il25minExtSell;
                    IL25.minExtCost = 0;
                    db.InvoiceLines.Add(IL25);

                    index = index + 1;
                    InvoiceLine IL26 = new InvoiceLine();
                    IL26.invoiceID = obj.lngID;
                    IL26.lineType = model.il26linetype;
                    IL26.displayOrder = model.il26display;
                    IL26.name = ILTsMailing[index].name;
                    IL26.size = ILTsMailing[index].size;
                    IL26.qty = model.il26qty;
                    IL26.UOM = ILTsMailing[index].UOM;
                    IL26.unitCost = model.il26unitCost;
                    IL26.extCost = model.il26extCost;
                    IL26.unitSell = model.il26unitSell;
                    IL26.extSell = model.il26extSell;
                    IL26.minExtSell = model.il26minExtSell;
                    IL26.minExtCost = 0;
                    db.InvoiceLines.Add(IL26);

                    index = index + 1;
                    InvoiceLine IL27 = new InvoiceLine();
                    IL27.invoiceID = obj.lngID;
                    IL27.lineType = model.il27linetype;
                    IL27.displayOrder = model.il27display;
                    IL27.name = ILTsMailing[index].name;
                    IL27.size = ILTsMailing[index].size;
                    IL27.qty = model.il27qty;
                    IL27.UOM = ILTsMailing[index].UOM;
                    IL27.unitCost = model.il27unitCost;
                    IL27.extCost = model.il27extCost;
                    IL27.unitSell = model.il27unitSell;
                    IL27.extSell = model.il27extSell;
                    IL27.minExtSell = model.il27minExtSell;
                    IL27.minExtCost = 0;
                    db.InvoiceLines.Add(IL27);

                    index = index + 1;
                    InvoiceLine IL28 = new InvoiceLine();
                    IL28.invoiceID = obj.lngID;
                    IL28.lineType = model.il28linetype;
                    IL28.displayOrder = model.il28display;
                    IL28.name = ILTsMailing[index].name;
                    IL28.size = ILTsMailing[index].size;
                    IL28.qty = model.il28qty;
                    IL28.UOM = ILTsMailing[index].UOM;
                    IL28.unitCost = model.il28unitCost;
                    IL28.extCost = model.il28extCost;
                    IL28.unitSell = model.il28unitSell;
                    IL28.extSell = model.il28extSell;
                    IL28.minExtSell = model.il28minExtSell;
                    IL28.minExtCost = 0;
                    db.InvoiceLines.Add(IL28);

                    index = index + 1;
                    InvoiceLine IL29 = new InvoiceLine();
                    IL29.invoiceID = obj.lngID;
                    IL29.lineType = model.il29linetype;
                    IL29.displayOrder = model.il29display;
                    IL29.name = ILTsMailing[index].name;
                    IL29.size = ILTsMailing[index].size;
                    IL29.qty = model.il29qty;
                    IL29.UOM = ILTsMailing[index].UOM;
                    IL29.unitCost = model.il29unitCost;
                    IL29.extCost = model.il29extCost;
                    IL29.unitSell = model.il29unitSell;
                    IL29.extSell = model.il29extSell;
                    IL29.minExtSell = model.il29minExtSell;
                    IL29.minExtCost = 0;
                    db.InvoiceLines.Add(IL29);

                    index = index + 1;
                    InvoiceLine IL30 = new InvoiceLine();
                    IL30.invoiceID = obj.lngID;
                    IL30.lineType = model.il30linetype;
                    IL30.displayOrder = model.il30display;
                    IL30.name = ILTsMailing[index].name;
                    IL30.size = ILTsMailing[index].size;
                    IL30.qty = model.il30qty;
                    IL30.UOM = ILTsMailing[index].UOM;
                    IL30.unitCost = model.il30unitCost;
                    IL30.extCost = model.il30extCost;
                    IL30.unitSell = model.il30unitSell;
                    IL30.extSell = model.il30extSell;
                    IL30.minExtSell = model.il30minExtSell;
                    IL30.minExtCost = 0;
                    db.InvoiceLines.Add(IL30);

                    index = index + 1;
                    InvoiceLine IL31 = new InvoiceLine();
                    IL31.invoiceID = obj.lngID;
                    IL31.lineType = model.il31linetype;
                    IL31.displayOrder = model.il31display;
                    IL31.name = ILTsMailing[index].name;
                    IL31.size = ILTsMailing[index].size;
                    IL31.qty = model.il31qty;
                    IL31.UOM = ILTsMailing[index].UOM;
                    IL31.unitCost = model.il31unitCost;
                    IL31.extCost = model.il31extCost;
                    IL31.unitSell = model.il31unitSell;
                    IL31.extSell = model.il31extSell;
                    IL31.minExtSell = model.il31minExtSell;
                    IL31.minExtCost = 0;
                    db.InvoiceLines.Add(IL31);

                    index = index + 1;
                    InvoiceLine IL32 = new InvoiceLine();
                    IL32.invoiceID = obj.lngID;
                    IL32.lineType = model.il32linetype;
                    IL32.displayOrder = model.il32display;
                    IL32.name = ILTsMailing[index].name;
                    IL32.size = ILTsMailing[index].size;
                    IL32.qty = model.il32qty;
                    IL32.UOM = ILTsMailing[index].UOM;
                    IL32.unitCost = model.il32unitCost;
                    IL32.extCost = model.il32extCost;
                    IL32.unitSell = model.il32unitSell;
                    IL32.extSell = model.il32extSell;
                    IL32.minExtSell = model.il32minExtSell;
                    IL32.minExtCost = 0;
                    db.InvoiceLines.Add(IL32);

                    index = index + 1;
                    InvoiceLine IL33 = new InvoiceLine();
                    IL33.invoiceID = obj.lngID;
                    IL33.lineType = model.il33linetype;
                    IL33.displayOrder = model.il33display;
                    IL33.name = ILTsMailing[index].name;
                    IL33.size = ILTsMailing[index].size;
                    IL33.qty = model.il33qty;
                    IL33.UOM = ILTsMailing[index].UOM;
                    IL33.unitCost = model.il33unitCost;
                    IL33.extCost = model.il33extCost;
                    IL33.unitSell = model.il33unitSell;
                    IL33.extSell = model.il33extSell;
                    IL33.minExtSell = model.il33minExtSell;
                    IL33.minExtCost = 0;
                    db.InvoiceLines.Add(IL33);

                    index = index + 1;
                    InvoiceLine IL34 = new InvoiceLine();
                    IL34.invoiceID = obj.lngID;
                    IL34.lineType = model.il34linetype;
                    IL34.displayOrder = model.il34display;
                    IL34.name = ILTsMailing[index].name;
                    IL34.size = ILTsMailing[index].size;
                    IL34.qty = model.il34qty;
                    IL34.UOM = ILTsMailing[index].UOM;
                    IL34.unitCost = model.il34unitCost;
                    IL34.extCost = model.il34extCost;
                    IL34.unitSell = model.il34unitSell;
                    IL34.extSell = model.il34extSell;
                    IL34.minExtSell = model.il34minExtSell;
                    IL34.minExtCost = 0;
                    db.InvoiceLines.Add(IL34);

                    index = index + 1;
                    InvoiceLine IL35 = new InvoiceLine();
                    IL35.invoiceID = obj.lngID;
                    IL35.lineType = model.il35linetype;
                    IL35.displayOrder = model.il35display;
                    IL35.name = model.il35name;
                    IL35.size = model.il35size;
                    IL35.qty = model.il35qty;
                    IL35.UOM = ILTsMailing[index].UOM;
                    IL35.unitCost = model.il35unitCost;
                    IL35.extCost = model.il35extCost;
                    IL35.unitSell = model.il35unitSell;
                    IL35.extSell = model.il35extSell;
                    IL35.minExtSell = model.il35minExtSell;
                    IL35.minExtCost = 0;
                    db.InvoiceLines.Add(IL35);

                    index = index + 1;
                    InvoiceLine IL36 = new InvoiceLine();
                    IL36.invoiceID = obj.lngID;
                    IL36.lineType = model.il36linetype;
                    IL36.displayOrder = model.il36display;
                    IL36.name = model.il36name;
                    IL36.size = model.il36size;
                    IL36.qty = model.il36qty;
                    IL36.UOM = ILTsMailing[index].UOM;
                    IL36.unitCost = model.il36unitCost;
                    IL36.extCost = model.il36extCost;
                    IL36.unitSell = model.il36unitSell;
                    IL36.extSell = model.il36extSell;
                    IL36.minExtSell = model.il36minExtSell;
                    IL36.minExtCost = 0;
                    db.InvoiceLines.Add(IL36);



                    List<InvoiceLinesTemplate> ILTFreight = ILTs.Where(d => d.lineType.Contains("Freight")).OrderBy(d => d.displayOrder).ToList();

                    index = 0;
                    InvoiceLine IL40 = new InvoiceLine();
                    IL40.invoiceID = obj.lngID;
                    IL40.lineType = model.il40linetype;
                    IL40.displayOrder = model.il40display;
                    IL40.name = model.il40name;
                    IL40.size = model.il40size;
                    IL40.qty = model.il40qty;
                    IL40.UOM = ILTsMailing[index].UOM;
                    IL40.unitCost = model.il40unitCost;
                    IL40.extCost = model.il40extCost;
                    IL40.unitSell = model.il40unitSell;
                    IL40.extSell = model.il40extSell;
                    IL40.minExtSell = model.il40minExtSell;
                    IL40.minExtCost = 0;
                    db.InvoiceLines.Add(IL40);
                    #endregion


                    db.SaveChanges();

                    ViewBag.ResultMessage = "Invoice saved for the order # TBEN-" + obj.orderID + " successfully!";
                    return ViewBag.ResultMessage;
                }
            }
            catch (Exception ex)
            {
                ViewBag.ResultMessage = "Error! " + ex.Message;
                return ViewBag.ResultMessage;
            }
        }

        [HttpPost]
        public string SubmitForUpdateInvoice(InvoiceDetails model)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return "Session expired, please login again.";

            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                { 
                    Invoice obj = db.Invoices.FirstOrDefault(d => d.lngID == model.ilInvoiceID);
                    obj.status = "Pending by vendor";
                    obj.orderID = model.ilOrderID;
                    obj.comment = model.comment;
                    obj.datBillSubmitted = DateTime.Now;
                    obj.datClientInvoiced = null;
                    obj.datClientInvoiceUploaded = null;
                    obj.datClientPayment = null;
                    obj.lastTouch = "";
                    obj.clientInvoiceFile = "";
                    obj.vendorInvoiceFile = "";
                    obj.internalInvoiceFile = "";
                    db.SaveChanges();

                    #region Remove Old Line Items

                    List<InvoiceLine> LIsTR = db.InvoiceLines.Where(d => d.invoiceID == obj.lngID).ToList();
                    foreach (InvoiceLine IL in LIsTR)
                    {
                        db.InvoiceLines.Remove(IL);
                    }
                    db.SaveChanges();

                    #endregion


                    List<InvoiceLinesTemplate> ILTs = db.InvoiceLinesTemplates.ToList();

                    #region Printing Line Items
                    List<InvoiceLinesTemplate> ILTsPrinting = ILTs.Where(d => d.lineType.Contains("Printing")).OrderBy(d => d.displayOrder).ToList();
                    int index = 0;

                    InvoiceLine IL1 = new InvoiceLine();
                    IL1.invoiceID = obj.lngID;
                    IL1.lineType = model.il1linetype;
                    IL1.displayOrder = model.il1display;
                    IL1.name = ILTsPrinting[index].name;
                    IL1.size = ILTsPrinting[index].size;
                    IL1.qty = model.il1qty;
                    IL1.UOM = ILTsPrinting[index].UOM;
                    IL1.unitCost = model.il1unitCost;
                    IL1.extCost = model.il1extCost;
                    IL1.unitSell = model.il1unitSell;
                    IL1.extSell = model.il1extSell;
                    IL1.minExtSell = model.il1minExtSell;
                    IL1.minExtCost = 0;
                    db.InvoiceLines.Add(IL1);

                    index = index + 1;
                    InvoiceLine IL2 = new InvoiceLine();
                    IL2.invoiceID = obj.lngID;
                    IL2.lineType = model.il2linetype;
                    IL2.displayOrder = model.il2display;
                    IL2.name = ILTsPrinting[index].name;
                    IL2.size = ILTsPrinting[index].size;
                    IL2.qty = model.il2qty;
                    IL2.UOM = ILTsPrinting[index].UOM;
                    IL2.unitCost = model.il2unitCost;
                    IL2.extCost = model.il2extCost;
                    IL2.unitSell = model.il2unitSell;
                    IL2.extSell = model.il2extSell;
                    IL2.minExtSell = model.il2minExtSell;
                    IL2.minExtCost = 0;
                    db.InvoiceLines.Add(IL2);

                    index = index + 1;
                    InvoiceLine IL3 = new InvoiceLine();
                    IL3.invoiceID = obj.lngID;
                    IL3.lineType = model.il3linetype;
                    IL3.displayOrder = model.il3display;
                    IL3.name = ILTsPrinting[index].name;
                    IL3.size = ILTsPrinting[index].size;
                    IL3.qty = model.il3qty;
                    IL3.UOM = ILTsPrinting[index].UOM;
                    IL3.unitCost = model.il3unitCost;
                    IL3.extCost = model.il3extCost;
                    IL3.unitSell = model.il3unitSell;
                    IL3.extSell = model.il3extSell;
                    IL3.minExtSell = model.il3minExtSell;
                    IL3.minExtCost = 0;
                    db.InvoiceLines.Add(IL3);

                    index = index + 1;
                    InvoiceLine IL4 = new InvoiceLine();
                    IL4.invoiceID = obj.lngID;
                    IL4.lineType = model.il4linetype;
                    IL4.displayOrder = model.il4display;
                    IL4.name = ILTsPrinting[index].name;
                    IL4.size = ILTsPrinting[index].size;
                    IL4.qty = model.il4qty;
                    IL4.UOM = ILTsPrinting[index].UOM;
                    IL4.unitCost = model.il4unitCost;
                    IL4.extCost = model.il4extCost;
                    IL4.unitSell = model.il4unitSell;
                    IL4.extSell = model.il4extSell;
                    IL4.minExtSell = model.il4minExtSell;
                    IL4.minExtCost = 0;
                    db.InvoiceLines.Add(IL4);

                    index = index + 1;
                    InvoiceLine IL5 = new InvoiceLine();
                    IL5.invoiceID = obj.lngID;
                    IL5.lineType = model.il5linetype;
                    IL5.displayOrder = model.il5display;
                    IL5.name = ILTsPrinting[index].name;
                    IL5.size = ILTsPrinting[index].size;
                    IL5.qty = model.il5qty;
                    IL5.UOM = ILTsPrinting[index].UOM;
                    IL5.unitCost = model.il5unitCost;
                    IL5.extCost = model.il5extCost;
                    IL5.unitSell = model.il5unitSell;
                    IL5.extSell = model.il5extSell;
                    IL5.minExtSell = model.il5minExtSell;
                    IL5.minExtCost = 0;
                    db.InvoiceLines.Add(IL5);

                    index = index + 1;
                    InvoiceLine IL6 = new InvoiceLine();
                    IL6.invoiceID = obj.lngID;
                    IL6.lineType = model.il6linetype;
                    IL6.displayOrder = model.il6display;
                    IL6.name = ILTsPrinting[index].name;
                    IL6.size = ILTsPrinting[index].size;
                    IL6.qty = model.il6qty;
                    IL6.UOM = ILTsPrinting[index].UOM;
                    IL6.unitCost = model.il6unitCost;
                    IL6.extCost = model.il6extCost;
                    IL6.unitSell = model.il6unitSell;
                    IL6.extSell = model.il6extSell;
                    IL6.minExtSell = model.il6minExtSell;
                    IL6.minExtCost = 0;
                    db.InvoiceLines.Add(IL6);

                    index = index + 1;
                    InvoiceLine IL7 = new InvoiceLine();
                    IL7.invoiceID = obj.lngID;
                    IL7.lineType = model.il7linetype;
                    IL7.displayOrder = model.il7display;
                    IL7.name = ILTsPrinting[index].name;
                    IL7.size = ILTsPrinting[index].size;
                    IL7.qty = model.il7qty;
                    IL7.UOM = ILTsPrinting[index].UOM;
                    IL7.unitCost = model.il7unitCost;
                    IL7.extCost = model.il7extCost;
                    IL7.unitSell = model.il7unitSell;
                    IL7.extSell = model.il7extSell;
                    IL7.minExtSell = model.il7minExtSell;
                    IL7.minExtCost = 0;
                    db.InvoiceLines.Add(IL7);

                    index = index + 1;
                    InvoiceLine IL8 = new InvoiceLine();
                    IL8.invoiceID = obj.lngID;
                    IL8.lineType = model.il8linetype;
                    IL8.displayOrder = model.il8display;
                    IL8.name = ILTsPrinting[index].name;
                    IL8.size = ILTsPrinting[index].size;
                    IL8.qty = model.il8qty;
                    IL8.UOM = ILTsPrinting[index].UOM;
                    IL8.unitCost = model.il8unitCost;
                    IL8.extCost = model.il8extCost;
                    IL8.unitSell = model.il8unitSell;
                    IL8.extSell = model.il8extSell;
                    IL8.minExtSell = model.il8minExtSell;
                    IL8.minExtCost = 0;
                    db.InvoiceLines.Add(IL8);

                    index = index + 1;
                    InvoiceLine IL9 = new InvoiceLine();
                    IL9.invoiceID = obj.lngID;
                    IL9.lineType = model.il9linetype;
                    IL9.displayOrder = model.il9display;
                    IL9.name = ILTsPrinting[index].name;
                    IL9.size = ILTsPrinting[index].size;
                    IL9.qty = model.il9qty;
                    IL9.UOM = ILTsPrinting[index].UOM;
                    IL9.unitCost = model.il9unitCost;
                    IL9.extCost = model.il9extCost;
                    IL9.unitSell = model.il9unitSell;
                    IL9.extSell = model.il9extSell;
                    IL9.minExtSell = model.il9minExtSell;
                    IL9.minExtCost = 0;
                    db.InvoiceLines.Add(IL9);

                    index = index + 1;
                    InvoiceLine IL10 = new InvoiceLine();
                    IL10.invoiceID = obj.lngID;
                    IL10.lineType = model.il10linetype;
                    IL10.displayOrder = model.il10display;
                    IL10.name = ILTsPrinting[index].name;
                    IL10.size = ILTsPrinting[index].size;
                    IL10.qty = model.il10qty;
                    IL10.UOM = ILTsPrinting[index].UOM;
                    IL10.unitCost = model.il10unitCost;
                    IL10.extCost = model.il10extCost;
                    IL10.unitSell = model.il10unitSell;
                    IL10.extSell = model.il10extSell;
                    IL10.minExtSell = model.il10minExtSell;
                    IL10.minExtCost = 0;
                    db.InvoiceLines.Add(IL10);

                    index = index + 1;
                    InvoiceLine IL11 = new InvoiceLine();
                    IL11.invoiceID = obj.lngID;
                    IL11.lineType = model.il11linetype;
                    IL11.displayOrder = model.il11display;
                    IL11.name = ILTsPrinting[index].name;
                    IL11.size = ILTsPrinting[index].size;
                    IL11.qty = model.il11qty;
                    IL11.UOM = ILTsPrinting[index].UOM;
                    IL11.unitCost = model.il11unitCost;
                    IL11.extCost = model.il11extCost;
                    IL11.unitSell = model.il11unitSell;
                    IL11.extSell = model.il11extSell;
                    IL11.minExtSell = model.il11minExtSell;
                    IL11.minExtCost = 0;
                    db.InvoiceLines.Add(IL11);

                    List<InvoiceLinesTemplate> ILTsMailing = ILTs.Where(d => d.lineType.Contains("Mailing")).OrderBy(d => d.displayOrder).ToList();

                    index = 0;
                    InvoiceLine IL20 = new InvoiceLine();
                    IL20.invoiceID = obj.lngID;
                    IL20.lineType = model.il20linetype;
                    IL20.displayOrder = model.il20display;
                    IL20.name = ILTsMailing[index].name;
                    IL20.size = ILTsMailing[index].size;
                    IL20.qty = model.il20qty;
                    IL20.UOM = ILTsMailing[index].UOM;
                    IL20.unitCost = model.il20unitCost;
                    IL20.extCost = model.il20extCost;
                    IL20.unitSell = model.il20unitSell;
                    IL20.extSell = model.il20extSell;
                    IL20.minExtSell = model.il20minExtSell;
                    IL20.minExtCost = 0;
                    db.InvoiceLines.Add(IL20);

                    index = index + 1;
                    InvoiceLine IL21 = new InvoiceLine();
                    IL21.invoiceID = obj.lngID;
                    IL21.lineType = model.il21linetype;
                    IL21.displayOrder = model.il21display;
                    IL21.name = ILTsMailing[index].name;
                    IL21.size = ILTsMailing[index].size;
                    IL21.qty = model.il21qty;
                    IL21.UOM = ILTsMailing[index].UOM;
                    IL21.unitCost = model.il21unitCost;
                    IL21.extCost = model.il21extCost;
                    IL21.unitSell = model.il21unitSell;
                    IL21.extSell = model.il21extSell;
                    IL21.minExtSell = model.il21minExtSell;
                    IL21.minExtCost = 0;
                    db.InvoiceLines.Add(IL21);

                    index = index + 1;
                    InvoiceLine IL22 = new InvoiceLine();
                    IL22.invoiceID = obj.lngID;
                    IL22.lineType = model.il22linetype;
                    IL22.displayOrder = model.il22display;
                    IL22.name = ILTsMailing[index].name;
                    IL22.size = ILTsMailing[index].size;
                    IL22.qty = model.il22qty;
                    IL22.UOM = ILTsMailing[index].UOM;
                    IL22.unitCost = model.il22unitCost;
                    IL22.extCost = model.il22extCost;
                    IL22.unitSell = model.il22unitSell;
                    IL22.extSell = model.il22extSell;
                    IL22.minExtSell = model.il22minExtSell;
                    IL22.minExtCost = 0;
                    db.InvoiceLines.Add(IL22);

                    index = index + 1;
                    InvoiceLine IL23 = new InvoiceLine();
                    IL23.invoiceID = obj.lngID;
                    IL23.lineType = model.il23linetype;
                    IL23.displayOrder = model.il23display;
                    IL23.name = ILTsMailing[index].name;
                    IL23.size = ILTsMailing[index].size;
                    IL23.qty = model.il23qty;
                    IL23.UOM = ILTsMailing[index].UOM;
                    IL23.unitCost = model.il23unitCost;
                    IL23.extCost = model.il23extCost;
                    IL23.unitSell = model.il23unitSell;
                    IL23.extSell = model.il23extSell;
                    IL23.minExtSell = model.il23minExtSell;
                    IL23.minExtCost = 0;
                    db.InvoiceLines.Add(IL23);

                    index = index + 1;
                    InvoiceLine IL24 = new InvoiceLine();
                    IL24.invoiceID = obj.lngID;
                    IL24.lineType = model.il24linetype;
                    IL24.displayOrder = model.il24display;
                    IL24.name = ILTsMailing[index].name;
                    IL24.size = ILTsMailing[index].size;
                    IL24.qty = model.il24qty;
                    IL24.UOM = ILTsMailing[index].UOM;
                    IL24.unitCost = model.il24unitCost;
                    IL24.extCost = model.il24extCost;
                    IL24.unitSell = model.il24unitSell;
                    IL24.extSell = model.il24extSell;
                    IL24.minExtSell = model.il24minExtSell;
                    IL24.minExtCost = 0;
                    db.InvoiceLines.Add(IL24);

                    index = index + 1;
                    InvoiceLine IL25 = new InvoiceLine();
                    IL25.invoiceID = obj.lngID;
                    IL25.lineType = model.il25linetype;
                    IL25.displayOrder = model.il25display;
                    IL25.name = ILTsMailing[index].name;
                    IL25.size = ILTsMailing[index].size;
                    IL25.qty = model.il25qty;
                    IL25.UOM = ILTsMailing[index].UOM;
                    IL25.unitCost = model.il25unitCost;
                    IL25.extCost = model.il25extCost;
                    IL25.unitSell = model.il25unitSell;
                    IL25.extSell = model.il25extSell;
                    IL25.minExtSell = model.il25minExtSell;
                    IL25.minExtCost = 0;
                    db.InvoiceLines.Add(IL25);

                    index = index + 1;
                    InvoiceLine IL26 = new InvoiceLine();
                    IL26.invoiceID = obj.lngID;
                    IL26.lineType = model.il26linetype;
                    IL26.displayOrder = model.il26display;
                    IL26.name = ILTsMailing[index].name;
                    IL26.size = ILTsMailing[index].size;
                    IL26.qty = model.il26qty;
                    IL26.UOM = ILTsMailing[index].UOM;
                    IL26.unitCost = model.il26unitCost;
                    IL26.extCost = model.il26extCost;
                    IL26.unitSell = model.il26unitSell;
                    IL26.extSell = model.il26extSell;
                    IL26.minExtSell = model.il26minExtSell;
                    IL26.minExtCost = 0;
                    db.InvoiceLines.Add(IL26);

                    index = index + 1;
                    InvoiceLine IL27 = new InvoiceLine();
                    IL27.invoiceID = obj.lngID;
                    IL27.lineType = model.il27linetype;
                    IL27.displayOrder = model.il27display;
                    IL27.name = ILTsMailing[index].name;
                    IL27.size = ILTsMailing[index].size;
                    IL27.qty = model.il27qty;
                    IL27.UOM = ILTsMailing[index].UOM;
                    IL27.unitCost = model.il27unitCost;
                    IL27.extCost = model.il27extCost;
                    IL27.unitSell = model.il27unitSell;
                    IL27.extSell = model.il27extSell;
                    IL27.minExtSell = model.il27minExtSell;
                    IL27.minExtCost = 0;
                    db.InvoiceLines.Add(IL27);

                    index = index + 1;
                    InvoiceLine IL28 = new InvoiceLine();
                    IL28.invoiceID = obj.lngID;
                    IL28.lineType = model.il28linetype;
                    IL28.displayOrder = model.il28display;
                    IL28.name = ILTsMailing[index].name;
                    IL28.size = ILTsMailing[index].size;
                    IL28.qty = model.il28qty;
                    IL28.UOM = ILTsMailing[index].UOM;
                    IL28.unitCost = model.il28unitCost;
                    IL28.extCost = model.il28extCost;
                    IL28.unitSell = model.il28unitSell;
                    IL28.extSell = model.il28extSell;
                    IL28.minExtSell = model.il28minExtSell;
                    IL28.minExtCost = 0;
                    db.InvoiceLines.Add(IL28);

                    index = index + 1;
                    InvoiceLine IL29 = new InvoiceLine();
                    IL29.invoiceID = obj.lngID;
                    IL29.lineType = model.il29linetype;
                    IL29.displayOrder = model.il29display;
                    IL29.name = ILTsMailing[index].name;
                    IL29.size = ILTsMailing[index].size;
                    IL29.qty = model.il29qty;
                    IL29.UOM = ILTsMailing[index].UOM;
                    IL29.unitCost = model.il29unitCost;
                    IL29.extCost = model.il29extCost;
                    IL29.unitSell = model.il29unitSell;
                    IL29.extSell = model.il29extSell;
                    IL29.minExtSell = model.il29minExtSell;
                    IL29.minExtCost = 0;
                    db.InvoiceLines.Add(IL29);

                    index = index + 1;
                    InvoiceLine IL30 = new InvoiceLine();
                    IL30.invoiceID = obj.lngID;
                    IL30.lineType = model.il30linetype;
                    IL30.displayOrder = model.il30display;
                    IL30.name = ILTsMailing[index].name;
                    IL30.size = ILTsMailing[index].size;
                    IL30.qty = model.il30qty;
                    IL30.UOM = ILTsMailing[index].UOM;
                    IL30.unitCost = model.il30unitCost;
                    IL30.extCost = model.il30extCost;
                    IL30.unitSell = model.il30unitSell;
                    IL30.extSell = model.il30extSell;
                    IL30.minExtSell = model.il30minExtSell;
                    IL30.minExtCost = 0;
                    db.InvoiceLines.Add(IL30);

                    index = index + 1;
                    InvoiceLine IL31 = new InvoiceLine();
                    IL31.invoiceID = obj.lngID;
                    IL31.lineType = model.il31linetype;
                    IL31.displayOrder = model.il31display;
                    IL31.name = ILTsMailing[index].name;
                    IL31.size = ILTsMailing[index].size;
                    IL31.qty = model.il31qty;
                    IL31.UOM = ILTsMailing[index].UOM;
                    IL31.unitCost = model.il31unitCost;
                    IL31.extCost = model.il31extCost;
                    IL31.unitSell = model.il31unitSell;
                    IL31.extSell = model.il31extSell;
                    IL31.minExtSell = model.il31minExtSell;
                    IL31.minExtCost = 0;
                    db.InvoiceLines.Add(IL31);

                    index = index + 1;
                    InvoiceLine IL32 = new InvoiceLine();
                    IL32.invoiceID = obj.lngID;
                    IL32.lineType = model.il32linetype;
                    IL32.displayOrder = model.il32display;
                    IL32.name = ILTsMailing[index].name;
                    IL32.size = ILTsMailing[index].size;
                    IL32.qty = model.il32qty;
                    IL32.UOM = ILTsMailing[index].UOM;
                    IL32.unitCost = model.il32unitCost;
                    IL32.extCost = model.il32extCost;
                    IL32.unitSell = model.il32unitSell;
                    IL32.extSell = model.il32extSell;
                    IL32.minExtSell = model.il32minExtSell;
                    IL32.minExtCost = 0;
                    db.InvoiceLines.Add(IL32);

                    index = index + 1;
                    InvoiceLine IL33 = new InvoiceLine();
                    IL33.invoiceID = obj.lngID;
                    IL33.lineType = model.il33linetype;
                    IL33.displayOrder = model.il33display;
                    IL33.name = ILTsMailing[index].name;
                    IL33.size = ILTsMailing[index].size;
                    IL33.qty = model.il33qty;
                    IL33.UOM = ILTsMailing[index].UOM;
                    IL33.unitCost = model.il33unitCost;
                    IL33.extCost = model.il33extCost;
                    IL33.unitSell = model.il33unitSell;
                    IL33.extSell = model.il33extSell;
                    IL33.minExtSell = model.il33minExtSell;
                    IL33.minExtCost = 0;
                    db.InvoiceLines.Add(IL33);

                    index = index + 1;
                    InvoiceLine IL34 = new InvoiceLine();
                    IL34.invoiceID = obj.lngID;
                    IL34.lineType = model.il34linetype;
                    IL34.displayOrder = model.il34display;
                    IL34.name = ILTsMailing[index].name;
                    IL34.size = ILTsMailing[index].size;
                    IL34.qty = model.il34qty;
                    IL34.UOM = ILTsMailing[index].UOM;
                    IL34.unitCost = model.il34unitCost;
                    IL34.extCost = model.il34extCost;
                    IL34.unitSell = model.il34unitSell;
                    IL34.extSell = model.il34extSell;
                    IL34.minExtSell = model.il34minExtSell;
                    IL34.minExtCost = 0;
                    db.InvoiceLines.Add(IL34);

                    index = index + 1;
                    InvoiceLine IL35 = new InvoiceLine();
                    IL35.invoiceID = obj.lngID;
                    IL35.lineType = model.il35linetype;
                    IL35.displayOrder = model.il35display;
                    IL35.name = model.il35name;
                    IL35.size = model.il35size;
                    IL35.qty = model.il35qty;
                    IL35.UOM = ILTsMailing[index].UOM;
                    IL35.unitCost = model.il35unitCost;
                    IL35.extCost = model.il35extCost;
                    IL35.unitSell = model.il35unitSell;
                    IL35.extSell = model.il35extSell;
                    IL35.minExtSell = model.il35minExtSell;
                    IL35.minExtCost = 0;
                    db.InvoiceLines.Add(IL35);

                    index = index + 1;
                    InvoiceLine IL36 = new InvoiceLine();
                    IL36.invoiceID = obj.lngID;
                    IL36.lineType = model.il36linetype;
                    IL36.displayOrder = model.il36display;
                    IL36.name = model.il36name;
                    IL36.size = model.il36size;
                    IL36.qty = model.il36qty;
                    IL36.UOM = ILTsMailing[index].UOM;
                    IL36.unitCost = model.il36unitCost;
                    IL36.extCost = model.il36extCost;
                    IL36.unitSell = model.il36unitSell;
                    IL36.extSell = model.il36extSell;
                    IL36.minExtSell = model.il36minExtSell;
                    IL36.minExtCost = 0;
                    db.InvoiceLines.Add(IL36);



                    List<InvoiceLinesTemplate> ILTFreight = ILTs.Where(d => d.lineType.Contains("Freight")).OrderBy(d => d.displayOrder).ToList();

                    index = 0;
                    InvoiceLine IL40 = new InvoiceLine();
                    IL40.invoiceID = obj.lngID;
                    IL40.lineType = model.il40linetype;
                    IL40.displayOrder = model.il40display;
                    IL40.name = model.il40name;
                    IL40.size = model.il40size;
                    IL40.qty = model.il40qty;
                    IL40.UOM = ILTsMailing[index].UOM;
                    IL40.unitCost = model.il40unitCost;
                    IL40.extCost = model.il40extCost;
                    IL40.unitSell = model.il40unitSell;
                    IL40.extSell = model.il40extSell;
                    IL40.minExtSell = model.il40minExtSell;
                    IL40.minExtCost = 0;
                    db.InvoiceLines.Add(IL40);
                    #endregion


                    db.SaveChanges();

                    ViewBag.ResultMessage = "Invoice updated for the order # TBEN-" + obj.orderID + " successfully!";
                    return ViewBag.ResultMessage;
                }
            }
            catch (Exception ex)
            {
                ViewBag.ResultMessage = "Error! " + ex.Message;
                return ViewBag.ResultMessage;
            }
        }

        [HttpPost]
        public string FinalSubmitInvoice(InvoiceDetails model)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return "Session expired, please login again.";
            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    Order ord1 = db.Orders.FirstOrDefault(d => d.lngID == model.ilOrderID);
                    Contents content = db.Contents.FirstOrDefault(d => d.lngID == model.ilOrderID);
                    OrderCheckoutField OCFs = db.OrderCheckoutFields.Where(d => d.name == "Contact_Charge Order To" && (d.lngOrder == model.ilOrderID)).FirstOrDefault();

                    Invoice obj = new Invoice();
                    if (model.ilInvoiceID > 0)
                    {
                        obj = db.Invoices.FirstOrDefault(d => d.lngID == model.ilInvoiceID);
                        obj.status = "Submitted by vendor";
                        obj.orderID = model.ilOrderID;
                        obj.comment = model.comment;
                        obj.datBillSubmitted = DateTime.Now;
                        obj.datClientInvoiced = null;
                        obj.datClientInvoiceUploaded = null;
                        obj.datClientPayment = null;
                        obj.lastTouch = "";
                        obj.clientInvoiceFile = "";
                        obj.vendorInvoiceFile = "";
                        obj.internalInvoiceFile = "";
                        db.SaveChanges();
                        xmlGenerate(ord1, model, OCFs);

                        #region Remove Old Line Items

                        List<InvoiceLine> LIsTR = db.InvoiceLines.Where(d => d.invoiceID == obj.lngID).ToList();
                        foreach (InvoiceLine IL in LIsTR)
                        {
                            db.InvoiceLines.Remove(IL);
                        }
                        db.SaveChanges();

                        #endregion

                        List<InvoiceLinesTemplate> ILTs = db.InvoiceLinesTemplates.ToList();

                        #region Printing Line Items
                        List<InvoiceLinesTemplate> ILTsPrinting = ILTs.Where(d => d.lineType.Contains("Printing")).OrderBy(d => d.displayOrder).ToList();
                        int index = 0;

                        InvoiceLine IL1 = new InvoiceLine();
                        IL1.invoiceID = obj.lngID;
                        IL1.lineType = model.il1linetype;
                        IL1.displayOrder = model.il1display;
                        IL1.name = ILTsPrinting[index].name;
                        IL1.size = ILTsPrinting[index].size;
                        IL1.qty = model.il1qty;
                        IL1.UOM = ILTsPrinting[index].UOM;
                        IL1.unitCost = model.il1unitCost;
                        IL1.extCost = model.il1extCost;
                        IL1.unitSell = model.il1unitSell;
                        IL1.extSell = model.il1extSell;
                        IL1.minExtSell = model.il1minExtSell;
                        IL1.minExtCost = 0;
                        db.InvoiceLines.Add(IL1);

                        index = index + 1;
                        InvoiceLine IL2 = new InvoiceLine();
                        IL2.invoiceID = obj.lngID;
                        IL2.lineType = model.il2linetype;
                        IL2.displayOrder = model.il2display;
                        IL2.name = ILTsPrinting[index].name;
                        IL2.size = ILTsPrinting[index].size;
                        IL2.qty = model.il2qty;
                        IL2.UOM = ILTsPrinting[index].UOM;
                        IL2.unitCost = model.il2unitCost;
                        IL2.extCost = model.il2extCost;
                        IL2.unitSell = model.il2unitSell;
                        IL2.extSell = model.il2extSell;
                        IL2.minExtSell = model.il2minExtSell;
                        IL2.minExtCost = 0;
                        db.InvoiceLines.Add(IL2);

                        index = index + 1;
                        InvoiceLine IL3 = new InvoiceLine();
                        IL3.invoiceID = obj.lngID;
                        IL3.lineType = model.il3linetype;
                        IL3.displayOrder = model.il3display;
                        IL3.name = ILTsPrinting[index].name;
                        IL3.size = ILTsPrinting[index].size;
                        IL3.qty = model.il3qty;
                        IL3.UOM = ILTsPrinting[index].UOM;
                        IL3.unitCost = model.il3unitCost;
                        IL3.extCost = model.il3extCost;
                        IL3.unitSell = model.il3unitSell;
                        IL3.extSell = model.il3extSell;
                        IL3.minExtSell = model.il3minExtSell;
                        IL3.minExtCost = 0;
                        db.InvoiceLines.Add(IL3);

                        index = index + 1;
                        InvoiceLine IL4 = new InvoiceLine();
                        IL4.invoiceID = obj.lngID;
                        IL4.lineType = model.il4linetype;
                        IL4.displayOrder = model.il4display;
                        IL4.name = ILTsPrinting[index].name;
                        IL4.size = ILTsPrinting[index].size;
                        IL4.qty = model.il4qty;
                        IL4.UOM = ILTsPrinting[index].UOM;
                        IL4.unitCost = model.il4unitCost;
                        IL4.extCost = model.il4extCost;
                        IL4.unitSell = model.il4unitSell;
                        IL4.extSell = model.il4extSell;
                        IL4.minExtSell = model.il4minExtSell;
                        IL4.minExtCost = 0;
                        db.InvoiceLines.Add(IL4);

                        index = index + 1;
                        InvoiceLine IL5 = new InvoiceLine();
                        IL5.invoiceID = obj.lngID;
                        IL5.lineType = model.il5linetype;
                        IL5.displayOrder = model.il5display;
                        IL5.name = ILTsPrinting[index].name;
                        IL5.size = ILTsPrinting[index].size;
                        IL5.qty = model.il5qty;
                        IL5.UOM = ILTsPrinting[index].UOM;
                        IL5.unitCost = model.il5unitCost;
                        IL5.extCost = model.il5extCost;
                        IL5.unitSell = model.il5unitSell;
                        IL5.extSell = model.il5extSell;
                        IL5.minExtSell = model.il5minExtSell;
                        IL5.minExtCost = 0;
                        db.InvoiceLines.Add(IL5);

                        index = index + 1;
                        InvoiceLine IL6 = new InvoiceLine();
                        IL6.invoiceID = obj.lngID;
                        IL6.lineType = model.il6linetype;
                        IL6.displayOrder = model.il6display;
                        IL6.name = ILTsPrinting[index].name;
                        IL6.size = ILTsPrinting[index].size;
                        IL6.qty = model.il6qty;
                        IL6.UOM = ILTsPrinting[index].UOM;
                        IL6.unitCost = model.il6unitCost;
                        IL6.extCost = model.il6extCost;
                        IL6.unitSell = model.il6unitSell;
                        IL6.extSell = model.il6extSell;
                        IL6.minExtSell = model.il6minExtSell;
                        IL6.minExtCost = 0;
                        db.InvoiceLines.Add(IL6);

                        index = index + 1;
                        InvoiceLine IL7 = new InvoiceLine();
                        IL7.invoiceID = obj.lngID;
                        IL7.lineType = model.il7linetype;
                        IL7.displayOrder = model.il7display;
                        IL7.name = ILTsPrinting[index].name;
                        IL7.size = ILTsPrinting[index].size;
                        IL7.qty = model.il7qty;
                        IL7.UOM = ILTsPrinting[index].UOM;
                        IL7.unitCost = model.il7unitCost;
                        IL7.extCost = model.il7extCost;
                        IL7.unitSell = model.il7unitSell;
                        IL7.extSell = model.il7extSell;
                        IL7.minExtSell = model.il7minExtSell;
                        IL7.minExtCost = 0;
                        db.InvoiceLines.Add(IL7);

                        index = index + 1;
                        InvoiceLine IL8 = new InvoiceLine();
                        IL8.invoiceID = obj.lngID;
                        IL8.lineType = model.il8linetype;
                        IL8.displayOrder = model.il8display;
                        IL8.name = ILTsPrinting[index].name;
                        IL8.size = ILTsPrinting[index].size;
                        IL8.qty = model.il8qty;
                        IL8.UOM = ILTsPrinting[index].UOM;
                        IL8.unitCost = model.il8unitCost;
                        IL8.extCost = model.il8extCost;
                        IL8.unitSell = model.il8unitSell;
                        IL8.extSell = model.il8extSell;
                        IL8.minExtSell = model.il8minExtSell;
                        IL8.minExtCost = 0;
                        db.InvoiceLines.Add(IL8);

                        index = index + 1;
                        InvoiceLine IL9 = new InvoiceLine();
                        IL9.invoiceID = obj.lngID;
                        IL9.lineType = model.il9linetype;
                        IL9.displayOrder = model.il9display;
                        IL9.name = ILTsPrinting[index].name;
                        IL9.size = ILTsPrinting[index].size;
                        IL9.qty = model.il9qty;
                        IL9.UOM = ILTsPrinting[index].UOM;
                        IL9.unitCost = model.il9unitCost;
                        IL9.extCost = model.il9extCost;
                        IL9.unitSell = model.il9unitSell;
                        IL9.extSell = model.il9extSell;
                        IL9.minExtSell = model.il9minExtSell;
                        IL9.minExtCost = 0;
                        db.InvoiceLines.Add(IL9);

                        index = index + 1;
                        InvoiceLine IL10 = new InvoiceLine();
                        IL10.invoiceID = obj.lngID;
                        IL10.lineType = model.il10linetype;
                        IL10.displayOrder = model.il10display;
                        IL10.name = ILTsPrinting[index].name;
                        IL10.size = ILTsPrinting[index].size;
                        IL10.qty = model.il10qty;
                        IL10.UOM = ILTsPrinting[index].UOM;
                        IL10.unitCost = model.il10unitCost;
                        IL10.extCost = model.il10extCost;
                        IL10.unitSell = model.il10unitSell;
                        IL10.extSell = model.il10extSell;
                        IL10.minExtSell = model.il10minExtSell;
                        IL10.minExtCost = 0;
                        db.InvoiceLines.Add(IL10);

                        index = index + 1;
                        InvoiceLine IL11 = new InvoiceLine();
                        IL11.invoiceID = obj.lngID;
                        IL11.lineType = model.il11linetype;
                        IL11.displayOrder = model.il11display;
                        IL11.name = ILTsPrinting[index].name;
                        IL11.size = ILTsPrinting[index].size;
                        IL11.qty = model.il11qty;
                        IL11.UOM = ILTsPrinting[index].UOM;
                        IL11.unitCost = model.il11unitCost;
                        IL11.extCost = model.il11extCost;
                        IL11.unitSell = model.il11unitSell;
                        IL11.extSell = model.il11extSell;
                        IL11.minExtSell = model.il11minExtSell;
                        IL11.minExtCost = 0;
                        db.InvoiceLines.Add(IL11);

                        List<InvoiceLinesTemplate> ILTsMailing = ILTs.Where(d => d.lineType.Contains("Mailing")).OrderBy(d => d.displayOrder).ToList();

                        index = 0;
                        InvoiceLine IL20 = new InvoiceLine();
                        IL20.invoiceID = obj.lngID;
                        IL20.lineType = model.il20linetype;
                        IL20.displayOrder = model.il20display;
                        IL20.name = ILTsMailing[index].name;
                        IL20.size = ILTsMailing[index].size;
                        IL20.qty = model.il20qty;
                        IL20.UOM = ILTsMailing[index].UOM;
                        IL20.unitCost = model.il20unitCost;
                        IL20.extCost = model.il20extCost;
                        IL20.unitSell = model.il20unitSell;
                        IL20.extSell = model.il20extSell;
                        IL20.minExtSell = model.il20minExtSell;
                        IL20.minExtCost = 0;
                        db.InvoiceLines.Add(IL20);

                        index = index + 1;
                        InvoiceLine IL21 = new InvoiceLine();
                        IL21.invoiceID = obj.lngID;
                        IL21.lineType = model.il21linetype;
                        IL21.displayOrder = model.il21display;
                        IL21.name = ILTsMailing[index].name;
                        IL21.size = ILTsMailing[index].size;
                        IL21.qty = model.il21qty;
                        IL21.UOM = ILTsMailing[index].UOM;
                        IL21.unitCost = model.il21unitCost;
                        IL21.extCost = model.il21extCost;
                        IL21.unitSell = model.il21unitSell;
                        IL21.extSell = model.il21extSell;
                        IL21.minExtSell = model.il21minExtSell;
                        IL21.minExtCost = 0;
                        db.InvoiceLines.Add(IL21);

                        index = index + 1;
                        InvoiceLine IL22 = new InvoiceLine();
                        IL22.invoiceID = obj.lngID;
                        IL22.lineType = model.il22linetype;
                        IL22.displayOrder = model.il22display;
                        IL22.name = ILTsMailing[index].name;
                        IL22.size = ILTsMailing[index].size;
                        IL22.qty = model.il22qty;
                        IL22.UOM = ILTsMailing[index].UOM;
                        IL22.unitCost = model.il22unitCost;
                        IL22.extCost = model.il22extCost;
                        IL22.unitSell = model.il22unitSell;
                        IL22.extSell = model.il22extSell;
                        IL22.minExtSell = model.il22minExtSell;
                        IL22.minExtCost = 0;
                        db.InvoiceLines.Add(IL22);

                        index = index + 1;
                        InvoiceLine IL23 = new InvoiceLine();
                        IL23.invoiceID = obj.lngID;
                        IL23.lineType = model.il23linetype;
                        IL23.displayOrder = model.il23display;
                        IL23.name = ILTsMailing[index].name;
                        IL23.size = ILTsMailing[index].size;
                        IL23.qty = model.il23qty;
                        IL23.UOM = ILTsMailing[index].UOM;
                        IL23.unitCost = model.il23unitCost;
                        IL23.extCost = model.il23extCost;
                        IL23.unitSell = model.il23unitSell;
                        IL23.extSell = model.il23extSell;
                        IL23.minExtSell = model.il23minExtSell;
                        IL23.minExtCost = 0;
                        db.InvoiceLines.Add(IL23);

                        index = index + 1;
                        InvoiceLine IL24 = new InvoiceLine();
                        IL24.invoiceID = obj.lngID;
                        IL24.lineType = model.il24linetype;
                        IL24.displayOrder = model.il24display;
                        IL24.name = ILTsMailing[index].name;
                        IL24.size = ILTsMailing[index].size;
                        IL24.qty = model.il24qty;
                        IL24.UOM = ILTsMailing[index].UOM;
                        IL24.unitCost = model.il24unitCost;
                        IL24.extCost = model.il24extCost;
                        IL24.unitSell = model.il24unitSell;
                        IL24.extSell = model.il24extSell;
                        IL24.minExtSell = model.il24minExtSell;
                        IL24.minExtCost = 0;
                        db.InvoiceLines.Add(IL24);

                        index = index + 1;
                        InvoiceLine IL25 = new InvoiceLine();
                        IL25.invoiceID = obj.lngID;
                        IL25.lineType = model.il25linetype;
                        IL25.displayOrder = model.il25display;
                        IL25.name = ILTsMailing[index].name;
                        IL25.size = ILTsMailing[index].size;
                        IL25.qty = model.il25qty;
                        IL25.UOM = ILTsMailing[index].UOM;
                        IL25.unitCost = model.il25unitCost;
                        IL25.extCost = model.il25extCost;
                        IL25.unitSell = model.il25unitSell;
                        IL25.extSell = model.il25extSell;
                        IL25.minExtSell = model.il25minExtSell;
                        IL25.minExtCost = 0;
                        db.InvoiceLines.Add(IL25);

                        index = index + 1;
                        InvoiceLine IL26 = new InvoiceLine();
                        IL26.invoiceID = obj.lngID;
                        IL26.lineType = model.il26linetype;
                        IL26.displayOrder = model.il26display;
                        IL26.name = ILTsMailing[index].name;
                        IL26.size = ILTsMailing[index].size;
                        IL26.qty = model.il26qty;
                        IL26.UOM = ILTsMailing[index].UOM;
                        IL26.unitCost = model.il26unitCost;
                        IL26.extCost = model.il26extCost;
                        IL26.unitSell = model.il26unitSell;
                        IL26.extSell = model.il26extSell;
                        IL26.minExtSell = model.il26minExtSell;
                        IL26.minExtCost = 0;
                        db.InvoiceLines.Add(IL26);

                        index = index + 1;
                        InvoiceLine IL27 = new InvoiceLine();
                        IL27.invoiceID = obj.lngID;
                        IL27.lineType = model.il27linetype;
                        IL27.displayOrder = model.il27display;
                        IL27.name = ILTsMailing[index].name;
                        IL27.size = ILTsMailing[index].size;
                        IL27.qty = model.il27qty;
                        IL27.UOM = ILTsMailing[index].UOM;
                        IL27.unitCost = model.il27unitCost;
                        IL27.extCost = model.il27extCost;
                        IL27.unitSell = model.il27unitSell;
                        IL27.extSell = model.il27extSell;
                        IL27.minExtSell = model.il27minExtSell;
                        IL27.minExtCost = 0;
                        db.InvoiceLines.Add(IL27);

                        index = index + 1;
                        InvoiceLine IL28 = new InvoiceLine();
                        IL28.invoiceID = obj.lngID;
                        IL28.lineType = model.il28linetype;
                        IL28.displayOrder = model.il28display;
                        IL28.name = ILTsMailing[index].name;
                        IL28.size = ILTsMailing[index].size;
                        IL28.qty = model.il28qty;
                        IL28.UOM = ILTsMailing[index].UOM;
                        IL28.unitCost = model.il28unitCost;
                        IL28.extCost = model.il28extCost;
                        IL28.unitSell = model.il28unitSell;
                        IL28.extSell = model.il28extSell;
                        IL28.minExtSell = model.il28minExtSell;
                        IL28.minExtCost = 0;
                        db.InvoiceLines.Add(IL28);

                        index = index + 1;
                        InvoiceLine IL29 = new InvoiceLine();
                        IL29.invoiceID = obj.lngID;
                        IL29.lineType = model.il29linetype;
                        IL29.displayOrder = model.il29display;
                        IL29.name = ILTsMailing[index].name;
                        IL29.size = ILTsMailing[index].size;
                        IL29.qty = model.il29qty;
                        IL29.UOM = ILTsMailing[index].UOM;
                        IL29.unitCost = model.il29unitCost;
                        IL29.extCost = model.il29extCost;
                        IL29.unitSell = model.il29unitSell;
                        IL29.extSell = model.il29extSell;
                        IL29.minExtSell = model.il29minExtSell;
                        IL29.minExtCost = 0;
                        db.InvoiceLines.Add(IL29);

                        index = index + 1;
                        InvoiceLine IL30 = new InvoiceLine();
                        IL30.invoiceID = obj.lngID;
                        IL30.lineType = model.il30linetype;
                        IL30.displayOrder = model.il30display;
                        IL30.name = ILTsMailing[index].name;
                        IL30.size = ILTsMailing[index].size;
                        IL30.qty = model.il30qty;
                        IL30.UOM = ILTsMailing[index].UOM;
                        IL30.unitCost = model.il30unitCost;
                        IL30.extCost = model.il30extCost;
                        IL30.unitSell = model.il30unitSell;
                        IL30.extSell = model.il30extSell;
                        IL30.minExtSell = model.il30minExtSell;
                        IL30.minExtCost = 0;
                        db.InvoiceLines.Add(IL30);

                        index = index + 1;
                        InvoiceLine IL31 = new InvoiceLine();
                        IL31.invoiceID = obj.lngID;
                        IL31.lineType = model.il31linetype;
                        IL31.displayOrder = model.il31display;
                        IL31.name = ILTsMailing[index].name;
                        IL31.size = ILTsMailing[index].size;
                        IL31.qty = model.il31qty;
                        IL31.UOM = ILTsMailing[index].UOM;
                        IL31.unitCost = model.il31unitCost;
                        IL31.extCost = model.il31extCost;
                        IL31.unitSell = model.il31unitSell;
                        IL31.extSell = model.il31extSell;
                        IL31.minExtSell = model.il31minExtSell;
                        IL31.minExtCost = 0;
                        db.InvoiceLines.Add(IL31);

                        index = index + 1;
                        InvoiceLine IL32 = new InvoiceLine();
                        IL32.invoiceID = obj.lngID;
                        IL32.lineType = model.il32linetype;
                        IL32.displayOrder = model.il32display;
                        IL32.name = ILTsMailing[index].name;
                        IL32.size = ILTsMailing[index].size;
                        IL32.qty = model.il32qty;
                        IL32.UOM = ILTsMailing[index].UOM;
                        IL32.unitCost = model.il32unitCost;
                        IL32.extCost = model.il32extCost;
                        IL32.unitSell = model.il32unitSell;
                        IL32.extSell = model.il32extSell;
                        IL32.minExtSell = model.il32minExtSell;
                        IL32.minExtCost = 0;
                        db.InvoiceLines.Add(IL32);

                        index = index + 1;
                        InvoiceLine IL33 = new InvoiceLine();
                        IL33.invoiceID = obj.lngID;
                        IL33.lineType = model.il33linetype;
                        IL33.displayOrder = model.il33display;
                        IL33.name = ILTsMailing[index].name;
                        IL33.size = ILTsMailing[index].size;
                        IL33.qty = model.il33qty;
                        IL33.UOM = ILTsMailing[index].UOM;
                        IL33.unitCost = model.il33unitCost;
                        IL33.extCost = model.il33extCost;
                        IL33.unitSell = model.il33unitSell;
                        IL33.extSell = model.il33extSell;
                        IL33.minExtSell = model.il33minExtSell;
                        IL33.minExtCost = 0;
                        db.InvoiceLines.Add(IL33);

                        index = index + 1;
                        InvoiceLine IL34 = new InvoiceLine();
                        IL34.invoiceID = obj.lngID;
                        IL34.lineType = model.il34linetype;
                        IL34.displayOrder = model.il34display;
                        IL34.name = ILTsMailing[index].name;
                        IL34.size = ILTsMailing[index].size;
                        IL34.qty = model.il34qty;
                        IL34.UOM = ILTsMailing[index].UOM;
                        IL34.unitCost = model.il34unitCost;
                        IL34.extCost = model.il34extCost;
                        IL34.unitSell = model.il34unitSell;
                        IL34.extSell = model.il34extSell;
                        IL34.minExtSell = model.il34minExtSell;
                        IL34.minExtCost = 0;
                        db.InvoiceLines.Add(IL34);

                        index = index + 1;
                        InvoiceLine IL35 = new InvoiceLine();
                        IL35.invoiceID = obj.lngID;
                        IL35.lineType = model.il35linetype;
                        IL35.displayOrder = model.il35display;
                        IL35.name = model.il35name;
                        IL35.size = model.il35size;
                        IL35.qty = model.il35qty;
                        IL35.UOM = ILTsMailing[index].UOM;
                        IL35.unitCost = model.il35unitCost;
                        IL35.extCost = model.il35extCost;
                        IL35.unitSell = model.il35unitSell;
                        IL35.extSell = model.il35extSell;
                        IL35.minExtSell = model.il35minExtSell;
                        IL35.minExtCost = 0;
                        db.InvoiceLines.Add(IL35);

                        index = index + 1;
                        InvoiceLine IL36 = new InvoiceLine();
                        IL36.invoiceID = obj.lngID;
                        IL36.lineType = model.il36linetype;
                        IL36.displayOrder = model.il36display;
                        IL36.name = model.il36name;
                        IL36.size = model.il36size;
                        IL36.qty = model.il36qty;
                        IL36.UOM = ILTsMailing[index].UOM;
                        IL36.unitCost = model.il36unitCost;
                        IL36.extCost = model.il36extCost;
                        IL36.unitSell = model.il36unitSell;
                        IL36.extSell = model.il36extSell;
                        IL36.minExtSell = model.il36minExtSell;
                        IL36.minExtCost = 0;
                        db.InvoiceLines.Add(IL36);



                        List<InvoiceLinesTemplate> ILTFreight = ILTs.Where(d => d.lineType.Contains("Freight")).OrderBy(d => d.displayOrder).ToList();

                        index = 0;
                        InvoiceLine IL40 = new InvoiceLine();
                        IL40.invoiceID = obj.lngID;
                        IL40.lineType = model.il40linetype;
                        IL40.displayOrder = model.il40display;
                        IL40.name = model.il40name;
                        IL40.size = model.il40size;
                        IL40.qty = model.il40qty;
                        IL40.UOM = ILTsMailing[index].UOM;
                        IL40.unitCost = model.il40unitCost;
                        IL40.extCost = model.il40extCost;
                        IL40.unitSell = model.il40unitSell;
                        IL40.extSell = model.il40extSell;
                        IL40.minExtSell = model.il40minExtSell;
                        IL40.minExtCost = 0;
                        db.InvoiceLines.Add(IL40);
                        #endregion

                    }
                    else
                    {
                        obj = new Invoice();

                        obj.orderID = model.ilOrderID;
                        obj.status = "Submitted by vendor";
                        obj.vendorInvoiceNo = model.vendorInvoiceNo;
                        obj.comment = model.comment;
                        obj.datBillSubmitted = DateTime.Now;
                        obj.datBillRevised = null;
                        obj.datClientInvoiced = null;
                        obj.datClientInvoiceUploaded = null;
                        obj.datClientPayment = null;
                        obj.lastTouch = "";
                        obj.clientInvoiceFile = "";
                        obj.vendorInvoiceFile = "";
                        obj.internalInvoiceFile = "";
                        db.Invoices.Add(obj);
                        db.SaveChanges();


                        List<InvoiceLinesTemplate> ILTs = db.InvoiceLinesTemplates.ToList();

                        #region Printing Line Items
                        List<InvoiceLinesTemplate> ILTsPrinting = ILTs.Where(d => d.lineType.Contains("Printing")).OrderBy(d => d.displayOrder).ToList();
                        int index = 0;

                        InvoiceLine IL1 = new InvoiceLine();
                        IL1.invoiceID = obj.lngID;
                        IL1.lineType = model.il1linetype;
                        IL1.displayOrder = model.il1display;
                        IL1.name = ILTsPrinting[index].name;
                        IL1.size = ILTsPrinting[index].size;
                        IL1.qty = model.il1qty;
                        IL1.UOM = ILTsPrinting[index].UOM;
                        IL1.unitCost = model.il1unitCost;
                        IL1.extCost = model.il1extCost;
                        IL1.unitSell = model.il1unitSell;
                        IL1.extSell = model.il1extSell;
                        IL1.minExtSell = model.il1minExtSell;
                        IL1.minExtCost = 0;
                        db.InvoiceLines.Add(IL1);

                        index = index + 1;
                        InvoiceLine IL2 = new InvoiceLine();
                        IL2.invoiceID = obj.lngID;
                        IL2.lineType = model.il2linetype;
                        IL2.displayOrder = model.il2display;
                        IL2.name = ILTsPrinting[index].name;
                        IL2.size = ILTsPrinting[index].size;
                        IL2.qty = model.il2qty;
                        IL2.UOM = ILTsPrinting[index].UOM;
                        IL2.unitCost = model.il2unitCost;
                        IL2.extCost = model.il2extCost;
                        IL2.unitSell = model.il2unitSell;
                        IL2.extSell = model.il2extSell;
                        IL2.minExtSell = model.il2minExtSell;
                        IL2.minExtCost = 0;
                        db.InvoiceLines.Add(IL2);

                        index = index + 1;
                        InvoiceLine IL3 = new InvoiceLine();
                        IL3.invoiceID = obj.lngID;
                        IL3.lineType = model.il3linetype;
                        IL3.displayOrder = model.il3display;
                        IL3.name = ILTsPrinting[index].name;
                        IL3.size = ILTsPrinting[index].size;
                        IL3.qty = model.il3qty;
                        IL3.UOM = ILTsPrinting[index].UOM;
                        IL3.unitCost = model.il3unitCost;
                        IL3.extCost = model.il3extCost;
                        IL3.unitSell = model.il3unitSell;
                        IL3.extSell = model.il3extSell;
                        IL3.minExtSell = model.il3minExtSell;
                        IL3.minExtCost = 0;
                        db.InvoiceLines.Add(IL3);

                        index = index + 1;
                        InvoiceLine IL4 = new InvoiceLine();
                        IL4.invoiceID = obj.lngID;
                        IL4.lineType = model.il4linetype;
                        IL4.displayOrder = model.il4display;
                        IL4.name = ILTsPrinting[index].name;
                        IL4.size = ILTsPrinting[index].size;
                        IL4.qty = model.il4qty;
                        IL4.UOM = ILTsPrinting[index].UOM;
                        IL4.unitCost = model.il4unitCost;
                        IL4.extCost = model.il4extCost;
                        IL4.unitSell = model.il4unitSell;
                        IL4.extSell = model.il4extSell;
                        IL4.minExtSell = model.il4minExtSell;
                        IL4.minExtCost = 0;
                        db.InvoiceLines.Add(IL4);

                        index = index + 1;
                        InvoiceLine IL5 = new InvoiceLine();
                        IL5.invoiceID = obj.lngID;
                        IL5.lineType = model.il5linetype;
                        IL5.displayOrder = model.il5display;
                        IL5.name = ILTsPrinting[index].name;
                        IL5.size = ILTsPrinting[index].size;
                        IL5.qty = model.il5qty;
                        IL5.UOM = ILTsPrinting[index].UOM;
                        IL5.unitCost = model.il5unitCost;
                        IL5.extCost = model.il5extCost;
                        IL5.unitSell = model.il5unitSell;
                        IL5.extSell = model.il5extSell;
                        IL5.minExtSell = model.il5minExtSell;
                        IL5.minExtCost = 0;
                        db.InvoiceLines.Add(IL5);

                        index = index + 1;
                        InvoiceLine IL6 = new InvoiceLine();
                        IL6.invoiceID = obj.lngID;
                        IL6.lineType = model.il6linetype;
                        IL6.displayOrder = model.il6display;
                        IL6.name = ILTsPrinting[index].name;
                        IL6.size = ILTsPrinting[index].size;
                        IL6.qty = model.il6qty;
                        IL6.UOM = ILTsPrinting[index].UOM;
                        IL6.unitCost = model.il6unitCost;
                        IL6.extCost = model.il6extCost;
                        IL6.unitSell = model.il6unitSell;
                        IL6.extSell = model.il6extSell;
                        IL6.minExtSell = model.il6minExtSell;
                        IL6.minExtCost = 0;
                        db.InvoiceLines.Add(IL6);

                        index = index + 1;
                        InvoiceLine IL7 = new InvoiceLine();
                        IL7.invoiceID = obj.lngID;
                        IL7.lineType = model.il7linetype;
                        IL7.displayOrder = model.il7display;
                        IL7.name = ILTsPrinting[index].name;
                        IL7.size = ILTsPrinting[index].size;
                        IL7.qty = model.il7qty;
                        IL7.UOM = ILTsPrinting[index].UOM;
                        IL7.unitCost = model.il7unitCost;
                        IL7.extCost = model.il7extCost;
                        IL7.unitSell = model.il7unitSell;
                        IL7.extSell = model.il7extSell;
                        IL7.minExtSell = model.il7minExtSell;
                        IL7.minExtCost = 0;
                        db.InvoiceLines.Add(IL7);

                        index = index + 1;
                        InvoiceLine IL8 = new InvoiceLine();
                        IL8.invoiceID = obj.lngID;
                        IL8.lineType = model.il8linetype;
                        IL8.displayOrder = model.il8display;
                        IL8.name = ILTsPrinting[index].name;
                        IL8.size = ILTsPrinting[index].size;
                        IL8.qty = model.il8qty;
                        IL8.UOM = ILTsPrinting[index].UOM;
                        IL8.unitCost = model.il8unitCost;
                        IL8.extCost = model.il8extCost;
                        IL8.unitSell = model.il8unitSell;
                        IL8.extSell = model.il8extSell;
                        IL8.minExtSell = model.il8minExtSell;
                        IL8.minExtCost = 0;
                        db.InvoiceLines.Add(IL8);

                        index = index + 1;
                        InvoiceLine IL9 = new InvoiceLine();
                        IL9.invoiceID = obj.lngID;
                        IL9.lineType = model.il9linetype;
                        IL9.displayOrder = model.il9display;
                        IL9.name = ILTsPrinting[index].name;
                        IL9.size = ILTsPrinting[index].size;
                        IL9.qty = model.il9qty;
                        IL9.UOM = ILTsPrinting[index].UOM;
                        IL9.unitCost = model.il9unitCost;
                        IL9.extCost = model.il9extCost;
                        IL9.unitSell = model.il9unitSell;
                        IL9.extSell = model.il9extSell;
                        IL9.minExtSell = model.il9minExtSell;
                        IL9.minExtCost = 0;
                        db.InvoiceLines.Add(IL9);

                        index = index + 1;
                        InvoiceLine IL10 = new InvoiceLine();
                        IL10.invoiceID = obj.lngID;
                        IL10.lineType = model.il10linetype;
                        IL10.displayOrder = model.il10display;
                        IL10.name = ILTsPrinting[index].name;
                        IL10.size = ILTsPrinting[index].size;
                        IL10.qty = model.il10qty;
                        IL10.UOM = ILTsPrinting[index].UOM;
                        IL10.unitCost = model.il10unitCost;
                        IL10.extCost = model.il10extCost;
                        IL10.unitSell = model.il10unitSell;
                        IL10.extSell = model.il10extSell;
                        IL10.minExtSell = model.il10minExtSell;
                        IL10.minExtCost = 0;
                        db.InvoiceLines.Add(IL10);

                        index = index + 1;
                        InvoiceLine IL11 = new InvoiceLine();
                        IL11.invoiceID = obj.lngID;
                        IL11.lineType = model.il11linetype;
                        IL11.displayOrder = model.il11display;
                        IL11.name = ILTsPrinting[index].name;
                        IL11.size = ILTsPrinting[index].size;
                        IL11.qty = model.il11qty;
                        IL11.UOM = ILTsPrinting[index].UOM;
                        IL11.unitCost = model.il11unitCost;
                        IL11.extCost = model.il11extCost;
                        IL11.unitSell = model.il11unitSell;
                        IL11.extSell = model.il11extSell;
                        IL11.minExtSell = model.il11minExtSell;
                        IL11.minExtCost = 0;
                        db.InvoiceLines.Add(IL11);

                        List<InvoiceLinesTemplate> ILTsMailing = ILTs.Where(d => d.lineType.Contains("Mailing")).OrderBy(d => d.displayOrder).ToList();

                        index = 0;
                        InvoiceLine IL20 = new InvoiceLine();
                        IL20.invoiceID = obj.lngID;
                        IL20.lineType = model.il20linetype;
                        IL20.displayOrder = model.il20display;
                        IL20.name = ILTsMailing[index].name;
                        IL20.size = ILTsMailing[index].size;
                        IL20.qty = model.il20qty;
                        IL20.UOM = ILTsMailing[index].UOM;
                        IL20.unitCost = model.il20unitCost;
                        IL20.extCost = model.il20extCost;
                        IL20.unitSell = model.il20unitSell;
                        IL20.extSell = model.il20extSell;
                        IL20.minExtSell = model.il20minExtSell;
                        IL20.minExtCost = 0;
                        db.InvoiceLines.Add(IL20);

                        index = index + 1;
                        InvoiceLine IL21 = new InvoiceLine();
                        IL21.invoiceID = obj.lngID;
                        IL21.lineType = model.il21linetype;
                        IL21.displayOrder = model.il21display;
                        IL21.name = ILTsMailing[index].name;
                        IL21.size = ILTsMailing[index].size;
                        IL21.qty = model.il21qty;
                        IL21.UOM = ILTsMailing[index].UOM;
                        IL21.unitCost = model.il21unitCost;
                        IL21.extCost = model.il21extCost;
                        IL21.unitSell = model.il21unitSell;
                        IL21.extSell = model.il21extSell;
                        IL21.minExtSell = model.il21minExtSell;
                        IL21.minExtCost = 0;
                        db.InvoiceLines.Add(IL21);

                        index = index + 1;
                        InvoiceLine IL22 = new InvoiceLine();
                        IL22.invoiceID = obj.lngID;
                        IL22.lineType = model.il22linetype;
                        IL22.displayOrder = model.il22display;
                        IL22.name = ILTsMailing[index].name;
                        IL22.size = ILTsMailing[index].size;
                        IL22.qty = model.il22qty;
                        IL22.UOM = ILTsMailing[index].UOM;
                        IL22.unitCost = model.il22unitCost;
                        IL22.extCost = model.il22extCost;
                        IL22.unitSell = model.il22unitSell;
                        IL22.extSell = model.il22extSell;
                        IL22.minExtSell = model.il22minExtSell;
                        IL22.minExtCost = 0;
                        db.InvoiceLines.Add(IL22);

                        index = index + 1;
                        InvoiceLine IL23 = new InvoiceLine();
                        IL23.invoiceID = obj.lngID;
                        IL23.lineType = model.il23linetype;
                        IL23.displayOrder = model.il23display;
                        IL23.name = ILTsMailing[index].name;
                        IL23.size = ILTsMailing[index].size;
                        IL23.qty = model.il23qty;
                        IL23.UOM = ILTsMailing[index].UOM;
                        IL23.unitCost = model.il23unitCost;
                        IL23.extCost = model.il23extCost;
                        IL23.unitSell = model.il23unitSell;
                        IL23.extSell = model.il23extSell;
                        IL23.minExtSell = model.il23minExtSell;
                        IL23.minExtCost = 0;
                        db.InvoiceLines.Add(IL23);

                        index = index + 1;
                        InvoiceLine IL24 = new InvoiceLine();
                        IL24.invoiceID = obj.lngID;
                        IL24.lineType = model.il24linetype;
                        IL24.displayOrder = model.il24display;
                        IL24.name = ILTsMailing[index].name;
                        IL24.size = ILTsMailing[index].size;
                        IL24.qty = model.il24qty;
                        IL24.UOM = ILTsMailing[index].UOM;
                        IL24.unitCost = model.il24unitCost;
                        IL24.extCost = model.il24extCost;
                        IL24.unitSell = model.il24unitSell;
                        IL24.extSell = model.il24extSell;
                        IL24.minExtSell = model.il24minExtSell;
                        IL24.minExtCost = 0;
                        db.InvoiceLines.Add(IL24);

                        index = index + 1;
                        InvoiceLine IL25 = new InvoiceLine();
                        IL25.invoiceID = obj.lngID;
                        IL25.lineType = model.il25linetype;
                        IL25.displayOrder = model.il25display;
                        IL25.name = ILTsMailing[index].name;
                        IL25.size = ILTsMailing[index].size;
                        IL25.qty = model.il25qty;
                        IL25.UOM = ILTsMailing[index].UOM;
                        IL25.unitCost = model.il25unitCost;
                        IL25.extCost = model.il25extCost;
                        IL25.unitSell = model.il25unitSell;
                        IL25.extSell = model.il25extSell;
                        IL25.minExtSell = model.il25minExtSell;
                        IL25.minExtCost = 0;
                        db.InvoiceLines.Add(IL25);

                        index = index + 1;
                        InvoiceLine IL26 = new InvoiceLine();
                        IL26.invoiceID = obj.lngID;
                        IL26.lineType = model.il26linetype;
                        IL26.displayOrder = model.il26display;
                        IL26.name = ILTsMailing[index].name;
                        IL26.size = ILTsMailing[index].size;
                        IL26.qty = model.il26qty;
                        IL26.UOM = ILTsMailing[index].UOM;
                        IL26.unitCost = model.il26unitCost;
                        IL26.extCost = model.il26extCost;
                        IL26.unitSell = model.il26unitSell;
                        IL26.extSell = model.il26extSell;
                        IL26.minExtSell = model.il26minExtSell;
                        IL26.minExtCost = 0;
                        db.InvoiceLines.Add(IL26);

                        index = index + 1;
                        InvoiceLine IL27 = new InvoiceLine();
                        IL27.invoiceID = obj.lngID;
                        IL27.lineType = model.il27linetype;
                        IL27.displayOrder = model.il27display;
                        IL27.name = ILTsMailing[index].name;
                        IL27.size = ILTsMailing[index].size;
                        IL27.qty = model.il27qty;
                        IL27.UOM = ILTsMailing[index].UOM;
                        IL27.unitCost = model.il27unitCost;
                        IL27.extCost = model.il27extCost;
                        IL27.unitSell = model.il27unitSell;
                        IL27.extSell = model.il27extSell;
                        IL27.minExtSell = model.il27minExtSell;
                        IL27.minExtCost = 0;
                        db.InvoiceLines.Add(IL27);

                        index = index + 1;
                        InvoiceLine IL28 = new InvoiceLine();
                        IL28.invoiceID = obj.lngID;
                        IL28.lineType = model.il28linetype;
                        IL28.displayOrder = model.il28display;
                        IL28.name = ILTsMailing[index].name;
                        IL28.size = ILTsMailing[index].size;
                        IL28.qty = model.il28qty;
                        IL28.UOM = ILTsMailing[index].UOM;
                        IL28.unitCost = model.il28unitCost;
                        IL28.extCost = model.il28extCost;
                        IL28.unitSell = model.il28unitSell;
                        IL28.extSell = model.il28extSell;
                        IL28.minExtSell = model.il28minExtSell;
                        IL28.minExtCost = 0;
                        db.InvoiceLines.Add(IL28);

                        index = index + 1;
                        InvoiceLine IL29 = new InvoiceLine();
                        IL29.invoiceID = obj.lngID;
                        IL29.lineType = model.il29linetype;
                        IL29.displayOrder = model.il29display;
                        IL29.name = ILTsMailing[index].name;
                        IL29.size = ILTsMailing[index].size;
                        IL29.qty = model.il29qty;
                        IL29.UOM = ILTsMailing[index].UOM;
                        IL29.unitCost = model.il29unitCost;
                        IL29.extCost = model.il29extCost;
                        IL29.unitSell = model.il29unitSell;
                        IL29.extSell = model.il29extSell;
                        IL29.minExtSell = model.il29minExtSell;
                        IL29.minExtCost = 0;
                        db.InvoiceLines.Add(IL29);

                        index = index + 1;
                        InvoiceLine IL30 = new InvoiceLine();
                        IL30.invoiceID = obj.lngID;
                        IL30.lineType = model.il30linetype;
                        IL30.displayOrder = model.il30display;
                        IL30.name = ILTsMailing[index].name;
                        IL30.size = ILTsMailing[index].size;
                        IL30.qty = model.il30qty;
                        IL30.UOM = ILTsMailing[index].UOM;
                        IL30.unitCost = model.il30unitCost;
                        IL30.extCost = model.il30extCost;
                        IL30.unitSell = model.il30unitSell;
                        IL30.extSell = model.il30extSell;
                        IL30.minExtSell = model.il30minExtSell;
                        IL30.minExtCost = 0;
                        db.InvoiceLines.Add(IL30);

                        index = index + 1;
                        InvoiceLine IL31 = new InvoiceLine();
                        IL31.invoiceID = obj.lngID;
                        IL31.lineType = model.il31linetype;
                        IL31.displayOrder = model.il31display;
                        IL31.name = ILTsMailing[index].name;
                        IL31.size = ILTsMailing[index].size;
                        IL31.qty = model.il31qty;
                        IL31.UOM = ILTsMailing[index].UOM;
                        IL31.unitCost = model.il31unitCost;
                        IL31.extCost = model.il31extCost;
                        IL31.unitSell = model.il31unitSell;
                        IL31.extSell = model.il31extSell;
                        IL31.minExtSell = model.il31minExtSell;
                        IL31.minExtCost = 0;
                        db.InvoiceLines.Add(IL31);

                        index = index + 1;
                        InvoiceLine IL32 = new InvoiceLine();
                        IL32.invoiceID = obj.lngID;
                        IL32.lineType = model.il32linetype;
                        IL32.displayOrder = model.il32display;
                        IL32.name = ILTsMailing[index].name;
                        IL32.size = ILTsMailing[index].size;
                        IL32.qty = model.il32qty;
                        IL32.UOM = ILTsMailing[index].UOM;
                        IL32.unitCost = model.il32unitCost;
                        IL32.extCost = model.il32extCost;
                        IL32.unitSell = model.il32unitSell;
                        IL32.extSell = model.il32extSell;
                        IL32.minExtSell = model.il32minExtSell;
                        IL32.minExtCost = 0;
                        db.InvoiceLines.Add(IL32);

                        index = index + 1;
                        InvoiceLine IL33 = new InvoiceLine();
                        IL33.invoiceID = obj.lngID;
                        IL33.lineType = model.il33linetype;
                        IL33.displayOrder = model.il33display;
                        IL33.name = ILTsMailing[index].name;
                        IL33.size = ILTsMailing[index].size;
                        IL33.qty = model.il33qty;
                        IL33.UOM = ILTsMailing[index].UOM;
                        IL33.unitCost = model.il33unitCost;
                        IL33.extCost = model.il33extCost;
                        IL33.unitSell = model.il33unitSell;
                        IL33.extSell = model.il33extSell;
                        IL33.minExtSell = model.il33minExtSell;
                        IL33.minExtCost = 0;
                        db.InvoiceLines.Add(IL33);

                        index = index + 1;
                        InvoiceLine IL34 = new InvoiceLine();
                        IL34.invoiceID = obj.lngID;
                        IL34.lineType = model.il34linetype;
                        IL34.displayOrder = model.il34display;
                        IL34.name = ILTsMailing[index].name;
                        IL34.size = ILTsMailing[index].size;
                        IL34.qty = model.il34qty;
                        IL34.UOM = ILTsMailing[index].UOM;
                        IL34.unitCost = model.il34unitCost;
                        IL34.extCost = model.il34extCost;
                        IL34.unitSell = model.il34unitSell;
                        IL34.extSell = model.il34extSell;
                        IL34.minExtSell = model.il34minExtSell;
                        IL34.minExtCost = 0;
                        db.InvoiceLines.Add(IL34);

                        index = index + 1;
                        InvoiceLine IL35 = new InvoiceLine();
                        IL35.invoiceID = obj.lngID;
                        IL35.lineType = model.il35linetype;
                        IL35.displayOrder = model.il35display;
                        IL35.name = model.il35name;
                        IL35.size = model.il35size;
                        IL35.qty = model.il35qty;
                        IL35.UOM = ILTsMailing[index].UOM;
                        IL35.unitCost = model.il35unitCost;
                        IL35.extCost = model.il35extCost;
                        IL35.unitSell = model.il35unitSell;
                        IL35.extSell = model.il35extSell;
                        IL35.minExtSell = model.il35minExtSell;
                        IL35.minExtCost = 0;
                        db.InvoiceLines.Add(IL35);

                        index = index + 1;
                        InvoiceLine IL36 = new InvoiceLine();
                        IL36.invoiceID = obj.lngID;
                        IL36.lineType = model.il36linetype;
                        IL36.displayOrder = model.il36display;
                        IL36.name = model.il36name;
                        IL36.size = model.il36size;
                        IL36.qty = model.il36qty;
                        IL36.UOM = ILTsMailing[index].UOM;
                        IL36.unitCost = model.il36unitCost;
                        IL36.extCost = model.il36extCost;
                        IL36.unitSell = model.il36unitSell;
                        IL36.extSell = model.il36extSell;
                        IL36.minExtSell = model.il36minExtSell;
                        IL36.minExtCost = 0;
                        db.InvoiceLines.Add(IL36);



                        List<InvoiceLinesTemplate> ILTFreight = ILTs.Where(d => d.lineType.Contains("Freight")).OrderBy(d => d.displayOrder).ToList();

                        index = 0;
                        InvoiceLine IL40 = new InvoiceLine();
                        IL40.invoiceID = obj.lngID;
                        IL40.lineType = model.il40linetype;
                        IL40.displayOrder = model.il40display;
                        IL40.name = model.il40name;
                        IL40.size = model.il40size;
                        IL40.qty = model.il40qty;
                        IL40.UOM = ILTsMailing[index].UOM;
                        IL40.unitCost = model.il40unitCost;
                        IL40.extCost = model.il40extCost;
                        IL40.unitSell = model.il40unitSell;
                        IL40.extSell = model.il40extSell;
                        IL40.minExtSell = model.il40minExtSell;
                        IL40.minExtCost = 0;
                        db.InvoiceLines.Add(IL40);
                        #endregion
                    }

                    db.SaveChanges();

                    ViewBag.ResultMessage = "Invoice submitted for the order # " + model.ilOrderID + " successfully!";

                    CreateInvoicePDF(model.ilOrderID);
                    //send out email to Vendor or Accountee
                    string Username = session.getSession();
                    Account AVT = db.Account.FirstOrDefault(d => d.UserID == Username);
                    Account Accountee = db.Account.FirstOrDefault(d => d.AccountRoleID.Contains("32"));

                    // For Vendor
                    if (AVT.AccountRoleID.Contains("23"))
                    {
                        Order ORD = db.Orders.FirstOrDefault(d => d.lngID == model.ilOrderID);
                        string Subject = "Smart Source Orderflow - Order # TBEN-" + ORD.lngID + " Vendor Invoice!";
                        string Body = "Dear " + AVT.Name + ", <br/><br/> Please find attached vendor invoice for the order # TBEN-" + ORD.lngID + " submitted on " + ORD.datSubmitted.Value.ToShortDateString() + "<br/><br/>";
                        Body = Body + "<br/><br/Regards,<br/>Smart Source Orderflow Team";
                        string CompleteAttachmentPath = "";
                        var folderName = Path.Combine("Repository", "Invoices");
                        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                        CompleteAttachmentPath = Path.Combine(pathToSave, obj.lngID + "_Vendor_Invoice.pdf");
                        SendEMailWithAttachments(AVT.Email, Subject, Body, CompleteAttachmentPath);

                        // Email to Accountee
                        Subject = "Smart Source Orderflow - Order # TBEN-" + ORD.lngID + " Vendor & Client Invoice!";
                        Body = "Dear " + Accountee.Name + ", <br/><br/> Please find attached vendor & client invoice for the order # TBEN-" + ORD.lngID + " submitted on " + ORD.datSubmitted.Value.ToShortDateString() + "<br/><br/>";
                        Body = Body + "<br/><br/Regards,<br/>Smart Source Orderflow Team";

                        CompleteAttachmentPath = Path.Combine(pathToSave, obj.lngID + "_Vendor_Invoice.pdf");
                        String SecondCompleteAttachmentPath = Path.Combine(pathToSave, obj.lngID + "_Client_Invoice.pdf");
                        SendEMailWithTwoAttachments(Accountee.Email, Subject, Body, CompleteAttachmentPath, SecondCompleteAttachmentPath);
                    }

                    // For Accountee
                    if (AVT.AccountRoleID.Contains("32"))
                    {
                        Order ORD = db.Orders.FirstOrDefault(d => d.lngID == model.ilOrderID);
                        var folderName = Path.Combine("Repository", "Invoices");
                        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                        // Email to Accountee
                        string Subject = "Smart Source Orderflow - Order # TBEN-" + ORD.lngID + " Vendor & Client Invoice!";
                        string Body = "Dear " + Accountee.Name + ", <br/><br/> Please find attached vendor & client invoice for the order # TBEN-" + ORD.lngID + " submitted on " + ORD.datSubmitted.Value.ToShortDateString() + "<br/><br/>";
                        Body = Body + "<br/><br/Regards,<br/>Smart Source Orderflow Team";

                        string CompleteAttachmentPath = Path.Combine(pathToSave, obj.lngID + "_Vendor_Invoice.pdf");
                        String SecondCompleteAttachmentPath = Path.Combine(pathToSave, obj.lngID + "_Client_Invoice.pdf");
                        SendEMailWithTwoAttachments(Accountee.Email, Subject, Body, CompleteAttachmentPath, SecondCompleteAttachmentPath);
                    }


                    return ViewBag.ResultMessage;
                }
            }
            catch (Exception ex)
            {
                ViewBag.ResultMessage = "Error! " + ex.Message;
                return ViewBag.ResultMessage;
            }
        }

        public IActionResult CreateLocalPDF(string PDFTemplate)
        {
            InvoiceDisplay model = new InvoiceDisplay();
            model.pdftemplate = PDFTemplate;
            return View(model);
        }

        public IActionResult CreateInvoicePDF(int OrderId)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    string PDFTemplate = db.Configurations.First(x => x.Module == "PDFTemplate").Value;
                    string PDFTemplateVendor = db.Configurations.First(x => x.Module == "PDFTemplateVendor").Value;

                    if (OrderId > 0)
                    {
                        Invoice VendorInvoice = db.Invoices.FirstOrDefault(d => d.orderID == OrderId);
                        Order ClientOrder = db.Orders.FirstOrDefault(d => d.lngID == OrderId);

                        List<InvoiceLine> ILs = db.InvoiceLines.Where(d => d.invoiceID == VendorInvoice.lngID).ToList();


                        if (ClientOrder != null & VendorInvoice != null)
                        {
                            PDFTemplate = PDFTemplate.Replace("@ClientOrderNo", "TBEN-" + ClientOrder.lngID.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@ClientOrderNo", "TBEN-" + ClientOrder.lngID.ToString());

                            PDFTemplate = PDFTemplate.Replace("@ClientChargeTo", ClientOrder.envelope_product_Cost_Center);
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@ClientChargeTo", ClientOrder.envelope_product_Cost_Center);

                            PDFTemplate = PDFTemplate.Replace("@ClientJobName", ClientOrder.job_details_jobName);
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@ClientJobName", ClientOrder.job_details_jobName + "-" + ClientOrder.envelope_product_Cost_Center.Substring(0,4));

                            PDFTemplate = PDFTemplate.Replace("@VendorInvoiceNo", VendorInvoice.lngID.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@VendorInvoiceNo", VendorInvoice.lngID.ToString());

                            PDFTemplate = PDFTemplate.Replace("@InvoiceDate", VendorInvoice.datBillSubmitted.Value.ToString("dd/MM/yyyy"));
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@InvoiceDate", VendorInvoice.datBillSubmitted.Value.ToString("dd/MM/yyyy"));

                            PDFTemplate = PDFTemplate.Replace("@Q1", ILs.First(X => X.displayOrder == 10).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@Q1", ILs.First(X => X.displayOrder == 10).qty.ToString());

                            PDFTemplate = PDFTemplate.Replace("@PEC1", ILs.First(X => X.displayOrder == 10).extCost.HasValue ? ILs.First(X => X.displayOrder == 10).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PEC1", ILs.First(X => X.displayOrder == 10).extCost.HasValue ? ILs.First(X => X.displayOrder == 10).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PUS1", ILs.First(X => X.displayOrder == 10).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PES1", ILs.First(X => X.displayOrder == 10).extSell.HasValue ? ILs.First(X => X.displayOrder == 10).extSell.Value.ToString("0.###") : "");

                            PDFTemplate = PDFTemplate.Replace("@Q2", ILs.First(X => X.displayOrder == 20).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@Q2", ILs.First(X => X.displayOrder == 20).qty.ToString());

                            PDFTemplate = PDFTemplate.Replace("@PEC2", ILs.First(X => X.displayOrder == 20).extCost.HasValue ? ILs.First(X => X.displayOrder == 20).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PEC2", ILs.First(X => X.displayOrder == 20).extCost.HasValue ? ILs.First(X => X.displayOrder == 20).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PUS2", ILs.First(X => X.displayOrder == 20).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PES2", ILs.First(X => X.displayOrder == 20).extSell.HasValue ? ILs.First(X => X.displayOrder == 20).extSell.Value.ToString("0.###") : "");

                            PDFTemplate = PDFTemplate.Replace("@Q3", ILs.First(X => X.displayOrder == 30).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@Q3", ILs.First(X => X.displayOrder == 30).qty.ToString());

                            PDFTemplate = PDFTemplate.Replace("@PEC3", ILs.First(X => X.displayOrder == 30).extCost.HasValue ? ILs.First(X => X.displayOrder == 30).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PEC3", ILs.First(X => X.displayOrder == 30).extCost.HasValue ? ILs.First(X => X.displayOrder == 30).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PUS3", ILs.First(X => X.displayOrder == 30).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PES3", ILs.First(X => X.displayOrder == 30).extSell.HasValue ? ILs.First(X => X.displayOrder == 30).extSell.Value.ToString("0.###") : "");

                            PDFTemplate = PDFTemplate.Replace("@Q4", ILs.First(X => X.displayOrder == 40).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@Q4", ILs.First(X => X.displayOrder == 40).qty.ToString());

                            PDFTemplate = PDFTemplate.Replace("@PEC4", ILs.First(X => X.displayOrder == 40).extCost.HasValue ? ILs.First(X => X.displayOrder == 40).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PEC4", ILs.First(X => X.displayOrder == 40).extCost.HasValue ? ILs.First(X => X.displayOrder == 40).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PUS4", ILs.First(X => X.displayOrder == 40).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PES4", ILs.First(X => X.displayOrder == 40).extSell.HasValue ? ILs.First(X => X.displayOrder == 40).extSell.Value.ToString("0.###") : "");

                            PDFTemplate = PDFTemplate.Replace("@Q5", ILs.First(X => X.displayOrder == 50).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@Q5", ILs.First(X => X.displayOrder == 50).qty.ToString());

                            PDFTemplate = PDFTemplate.Replace("@PEC5", ILs.First(X => X.displayOrder == 50).extCost.HasValue ? ILs.First(X => X.displayOrder == 50).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PEC5", ILs.First(X => X.displayOrder == 50).extCost.HasValue ? ILs.First(X => X.displayOrder == 50).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PUS5", ILs.First(X => X.displayOrder == 50).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PES5", ILs.First(X => X.displayOrder == 50).extSell.HasValue ? ILs.First(X => X.displayOrder == 50).extSell.Value.ToString("0.###") : "");

                            PDFTemplate = PDFTemplate.Replace("@Q6", ILs.First(X => X.displayOrder == 60).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@Q6", ILs.First(X => X.displayOrder == 60).qty.ToString());

                            PDFTemplate = PDFTemplate.Replace("@PEC6", ILs.First(X => X.displayOrder == 60).extCost.HasValue ? ILs.First(X => X.displayOrder == 60).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PEC6", ILs.First(X => X.displayOrder == 60).extCost.HasValue ? ILs.First(X => X.displayOrder == 60).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PUS6", ILs.First(X => X.displayOrder == 60).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PES6", ILs.First(X => X.displayOrder == 60).extSell.HasValue ? ILs.First(X => X.displayOrder == 60).extCost.Value.ToString("0.###") : "");

                            PDFTemplate = PDFTemplate.Replace("@Q7", ILs.First(X => X.displayOrder == 70).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@Q7", ILs.First(X => X.displayOrder == 70).qty.ToString());

                            PDFTemplate = PDFTemplate.Replace("@PEC7", ILs.First(X => X.displayOrder == 70).extCost.HasValue ? ILs.First(X => X.displayOrder == 70).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PEC7", ILs.First(X => X.displayOrder == 70).extCost.HasValue ? ILs.First(X => X.displayOrder == 70).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PUS7", ILs.First(X => X.displayOrder == 70).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PES7", ILs.First(X => X.displayOrder == 70).extSell.HasValue ? ILs.First(X => X.displayOrder == 70).extSell.Value.ToString("0.###") : "");

                            PDFTemplate = PDFTemplate.Replace("@Q8", ILs.First(X => X.displayOrder == 75).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@Q8", ILs.First(X => X.displayOrder == 75).qty.ToString());

                            PDFTemplate = PDFTemplate.Replace("@PEC8", ILs.First(X => X.displayOrder == 75).extCost.HasValue ? ILs.First(X => X.displayOrder == 75).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PEC8", ILs.First(X => X.displayOrder == 75).extCost.HasValue ? ILs.First(X => X.displayOrder == 75).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PUS8", ILs.First(X => X.displayOrder == 75).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PES8", ILs.First(X => X.displayOrder == 75).extSell.HasValue ? ILs.First(X => X.displayOrder == 75).extSell.Value.ToString("0.###") : "");

                            PDFTemplate = PDFTemplate.Replace("@Q9", ILs.First(X => X.displayOrder == 80).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@Q9", ILs.First(X => X.displayOrder == 80).qty.ToString());


                            PDFTemplate = PDFTemplate.Replace("@PEC9", ILs.First(X => X.displayOrder == 80).extCost.HasValue ? ILs.First(X => X.displayOrder == 80).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PEC9", ILs.First(X => X.displayOrder == 80).extCost.HasValue ? ILs.First(X => X.displayOrder == 80).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PUS9", ILs.First(X => X.displayOrder == 80).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PES9", ILs.First(X => X.displayOrder == 80).extSell.HasValue ? ILs.First(X => X.displayOrder == 80).extSell.Value.ToString("0.###") : "");


                            if (ILs.First(X => X.displayOrder == 950).name.ToString() == "Additional Charge")
                            {
                                PDFTemplate = PDFTemplate.Replace("@PACName", "");
                                PDFTemplateVendor = PDFTemplateVendor.Replace("@PACName", "");

                            }
                            else
                            {
                                PDFTemplate = PDFTemplate.Replace("@PACName", ILs.First(X => X.displayOrder == 950).name.ToString());
                                PDFTemplateVendor = PDFTemplateVendor.Replace("@PACName", ILs.First(X => X.displayOrder == 950).name.ToString());
                            }

                            if (ILs.First(X => X.displayOrder == 950).size.ToString() == "Additional ChargeSize")
                            {
                                PDFTemplate = PDFTemplate.Replace("@PACNameSize", "");
                                PDFTemplateVendor = PDFTemplateVendor.Replace("@PACNameSize", "");
                            }
                            else
                            {
                                PDFTemplate = PDFTemplate.Replace("@PACNameSize", ILs.First(X => X.displayOrder == 950).size.ToString());
                                PDFTemplateVendor = PDFTemplateVendor.Replace("@PACNameSize", ILs.First(X => X.displayOrder == 950).size.ToString());
                            }

                            PDFTemplate = PDFTemplate.Replace("@W1", ILs.First(X => X.displayOrder == 950).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@W1", ILs.First(X => X.displayOrder == 950).qty.ToString());
                            
                            /*PTotalQty*/
                            PDFTemplate = PDFTemplate.Replace("@PTotalQty", ILs.First(X => X.displayOrder == 990).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PTotalQty", ILs.First(X => X.displayOrder == 990).qty.ToString());
                            
                            /*PTotalUnitCost*/
                            PDFTemplate = PDFTemplate.Replace("@PTotalUnitCost", ILs.First(X => X.displayOrder == 990).unitCost.HasValue ? ILs.First(X => X.displayOrder == 990).unitCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PTotalUnitCost", ILs.First(X => X.displayOrder == 990).unitCost.HasValue ? ILs.First(X => X.displayOrder == 990).unitCost.Value.ToString("0.###") : "");

                            /*@PTotalUnitSell*/
                            PDFTemplate = PDFTemplate.Replace("@PTotalUnitSell", ILs.First(X => X.displayOrder == 990).unitSell.HasValue ? ILs.First(X => X.displayOrder == 990).unitSell.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PTotalUnitSell", ILs.First(X => X.displayOrder == 990).unitSell.HasValue ? ILs.First(X => X.displayOrder == 990).unitSell.Value.ToString("0.###") : "");


                            PDFTemplate = PDFTemplate.Replace("@PACUOM", ILs.First(X => X.displayOrder == 950).UOM.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PACUOM", ILs.First(X => X.displayOrder == 950).UOM.ToString());


                            PDFTemplate = PDFTemplate.Replace("@PACUC", ILs.First(X => X.displayOrder == 950).unitCost.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PACUC", ILs.First(X => X.displayOrder == 950).unitCost.ToString());

                            PDFTemplate = PDFTemplate.Replace("@PUSAEC", ILs.First(X => X.displayOrder == 950).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PUSAEC", ILs.First(X => X.displayOrder == 950).unitSell.ToString());


                            PDFTemplate = PDFTemplate.Replace("@PACExtCost", ILs.First(X => X.displayOrder == 950).extCost.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PACExtCost", ILs.First(X => X.displayOrder == 950).extCost.ToString());

                            PDFTemplate = PDFTemplate.Replace("@PESAEC", ILs.First(X => X.displayOrder == 950).extSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PESAEC", ILs.First(X => X.displayOrder == 950).extSell.ToString());

                            PDFTemplate = PDFTemplate.Replace("@PExtSell", ILs.First(X => X.displayOrder == 990).extSell.HasValue ? ILs.First(X => X.displayOrder == 990).extSell.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PExtSell", ILs.First(X => X.displayOrder == 990).extSell.HasValue ? ILs.First(X => X.displayOrder == 990).extSell.Value.ToString("0.###") : "");


                            PDFTemplate = PDFTemplate.Replace("@PTotalExtCost", ILs.First(X => X.displayOrder == 990).extCost.HasValue ? ILs.First(X => X.displayOrder == 990).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PTotalExtCost", ILs.First(X => X.displayOrder == 990).extCost.HasValue ? ILs.First(X => X.displayOrder == 990).extCost.Value.ToString("0.###") : "");

                            /*PDFTemplate = PDFTemplate.Replace("@PExtSell", ILs.First(X => X.displayOrder == 990).extSell.HasValue ? ILs.First(X => X.displayOrder == 990).extSell.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PExtSell", ILs.First(X => X.displayOrder == 990).extSell.HasValue ? ILs.First(X => X.displayOrder == 990).extSell.Value.ToString("0.###") : "");
                            */

                            /*PDFTemplate = PDFTemplate.Replace("@PUnitSell", ILs.First(X => X.displayOrder == 990).unitSell.HasValue ? ILs.First(X => X.displayOrder == 990).extSell.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@PUnitSell", ILs.First(X => X.displayOrder == 990).unitSell.HasValue ? ILs.First(X => X.displayOrder == 990).extSell.Value.ToString("0.###") : "");
                            */

                            PDFTemplate = PDFTemplate.Replace("@W2", ILs.First(X => X.displayOrder == 1010).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@W2", ILs.First(X => X.displayOrder == 1010).qty.ToString());

                            PDFTemplate = PDFTemplate.Replace("@MEC1", ILs.First(X => X.displayOrder == 1010).extCost.HasValue ? ILs.First(X => X.displayOrder == 1010).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MEC1", ILs.First(X => X.displayOrder == 1010).extCost.HasValue ? ILs.First(X => X.displayOrder == 1010).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MUS1", ILs.First(X => X.displayOrder == 1010).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MES1", ILs.First(X => X.displayOrder == 1010).extSell.HasValue ? ILs.First(X => X.displayOrder == 1010).extSell.Value.ToString("0.###") : "");

                            PDFTemplate = PDFTemplate.Replace("@W3", ILs.First(X => X.displayOrder == 1020).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@W3", ILs.First(X => X.displayOrder == 1020).qty.ToString());

                            PDFTemplate = PDFTemplate.Replace("@MEC2", ILs.First(X => X.displayOrder == 1020).extCost.HasValue ? ILs.First(X => X.displayOrder == 1020).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MEC2", ILs.First(X => X.displayOrder == 1020).extCost.HasValue ? ILs.First(X => X.displayOrder == 1020).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MUS2", ILs.First(X => X.displayOrder == 1020).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MES2", ILs.First(X => X.displayOrder == 1020).extSell.HasValue ? ILs.First(X => X.displayOrder == 1020).extSell.Value.ToString("0.###") : "");

                            PDFTemplate = PDFTemplate.Replace("@W4", ILs.First(X => X.displayOrder == 1030).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@W4", ILs.First(X => X.displayOrder == 1030).qty.ToString());

                            PDFTemplate = PDFTemplate.Replace("@MEC3", ILs.First(X => X.displayOrder == 1030).extCost.HasValue ? ILs.First(X => X.displayOrder == 1030).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MEC3", ILs.First(X => X.displayOrder == 1030).extCost.HasValue ? ILs.First(X => X.displayOrder == 1030).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MUS3", ILs.First(X => X.displayOrder == 1030).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MES3", ILs.First(X => X.displayOrder == 1030).extSell.HasValue ? ILs.First(X => X.displayOrder == 1030).extSell.Value.ToString("0.###") : "");

                            PDFTemplate = PDFTemplate.Replace("@W5", ILs.First(X => X.displayOrder == 1040).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@W5", ILs.First(X => X.displayOrder == 1040).qty.ToString());

                            PDFTemplate = PDFTemplate.Replace("@MEC4", ILs.First(X => X.displayOrder == 1040).extCost.HasValue ? ILs.First(X => X.displayOrder == 1040).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MEC4", ILs.First(X => X.displayOrder == 1040).extCost.HasValue ? ILs.First(X => X.displayOrder == 1040).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MUS4", ILs.First(X => X.displayOrder == 1040).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MES4", ILs.First(X => X.displayOrder == 1040).extSell.HasValue ? ILs.First(X => X.displayOrder == 1040).extSell.Value.ToString("0.###") : "");


                            PDFTemplate = PDFTemplate.Replace("@W6", ILs.First(X => X.displayOrder == 1050).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@W6", ILs.First(X => X.displayOrder == 1050).qty.ToString());

                            PDFTemplate = PDFTemplate.Replace("@MEC5", ILs.First(X => X.displayOrder == 1050).extCost.HasValue ? ILs.First(X => X.displayOrder == 1050).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MEC5", ILs.First(X => X.displayOrder == 1050).extCost.HasValue ? ILs.First(X => X.displayOrder == 1050).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MUS5", ILs.First(X => X.displayOrder == 1050).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MES5", ILs.First(X => X.displayOrder == 1050).extSell.HasValue ? ILs.First(X => X.displayOrder == 1050).extSell.Value.ToString("0.###") : "");

                            PDFTemplate = PDFTemplate.Replace("@W7", ILs.First(X => X.displayOrder == 1060).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@W7", ILs.First(X => X.displayOrder == 1060).qty.ToString());

                            PDFTemplate = PDFTemplate.Replace("@MEC6", ILs.First(X => X.displayOrder == 1060).extCost.HasValue ? ILs.First(X => X.displayOrder == 1060).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MEC6", ILs.First(X => X.displayOrder == 1060).extCost.HasValue ? ILs.First(X => X.displayOrder == 1060).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MUS6", ILs.First(X => X.displayOrder == 1060).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MES6", ILs.First(X => X.displayOrder == 1060).extSell.HasValue ? ILs.First(X => X.displayOrder == 1060).extSell.Value.ToString("0.###") : "");

                            PDFTemplate = PDFTemplate.Replace("@W8", ILs.First(X => X.displayOrder == 1070).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@W8", ILs.First(X => X.displayOrder == 1070).qty.ToString());

                            PDFTemplate = PDFTemplate.Replace("@MEC7", ILs.First(X => X.displayOrder == 1070).extCost.HasValue ? ILs.First(X => X.displayOrder == 1070).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MEC7", ILs.First(X => X.displayOrder == 1070).extCost.HasValue ? ILs.First(X => X.displayOrder == 1070).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MUS7", ILs.First(X => X.displayOrder == 1070).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MES7", ILs.First(X => X.displayOrder == 1070).extSell.HasValue ? ILs.First(X => X.displayOrder == 1070).extSell.Value.ToString("0.###") : "");

                            PDFTemplate = PDFTemplate.Replace("@W9", ILs.First(X => X.displayOrder == 1080).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@W9", ILs.First(X => X.displayOrder == 1080).qty.ToString());

                            PDFTemplate = PDFTemplate.Replace("@MEC8", ILs.First(X => X.displayOrder == 1080).extCost.HasValue ? ILs.First(X => X.displayOrder == 1080).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MEC8", ILs.First(X => X.displayOrder == 1080).extCost.HasValue ? ILs.First(X => X.displayOrder == 1080).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MUS8", ILs.First(X => X.displayOrder == 1080).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MES8", ILs.First(X => X.displayOrder == 1080).extSell.HasValue ? ILs.First(X => X.displayOrder == 1080).extSell.Value.ToString("0.###") : "");

                            PDFTemplate = PDFTemplate.Replace("@E1", ILs.First(X => X.displayOrder == 1085).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@E1", ILs.First(X => X.displayOrder == 1085).qty.ToString());

                            PDFTemplate = PDFTemplate.Replace("@MEC9", ILs.First(X => X.displayOrder == 1085).extCost.HasValue ? ILs.First(X => X.displayOrder == 1085).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MEC9", ILs.First(X => X.displayOrder == 1085).extCost.HasValue ? ILs.First(X => X.displayOrder == 1085).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MUS9", ILs.First(X => X.displayOrder == 1085).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MES9", ILs.First(X => X.displayOrder == 1085).extSell.HasValue ? ILs.First(X => X.displayOrder == 1085).extSell.Value.ToString("0.###") : "");


                            PDFTemplate = PDFTemplate.Replace("@E2", ILs.First(X => X.displayOrder == 1090).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@E2", ILs.First(X => X.displayOrder == 1090).qty.ToString());

                            PDFTemplate = PDFTemplate.Replace("@MECM1", ILs.First(X => X.displayOrder == 1090).extCost.HasValue ? ILs.First(X => X.displayOrder == 1090).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MECM1", ILs.First(X => X.displayOrder == 1090).extCost.HasValue ? ILs.First(X => X.displayOrder == 1090).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MNUS1", ILs.First(X => X.displayOrder == 1090).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MNES1", ILs.First(X => X.displayOrder == 1090).extSell.HasValue ? ILs.First(X => X.displayOrder == 1090).extSell.Value.ToString("0.###") : "");

                            PDFTemplate = PDFTemplate.Replace("@E3", ILs.First(X => X.displayOrder == 1100).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@E3", ILs.First(X => X.displayOrder == 1100).qty.ToString());

                            PDFTemplate = PDFTemplate.Replace("@MECM2", ILs.First(X => X.displayOrder == 1100).extCost.HasValue ? ILs.First(X => X.displayOrder == 1100).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MECM2", ILs.First(X => X.displayOrder == 1100).extCost.HasValue ? ILs.First(X => X.displayOrder == 1100).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MNUS2", ILs.First(X => X.displayOrder == 1100).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MNES2", ILs.First(X => X.displayOrder == 1100).extSell.HasValue ? ILs.First(X => X.displayOrder == 1100).extSell.Value.ToString("0.###") : "");

                            PDFTemplate = PDFTemplate.Replace("@E4", ILs.First(X => X.displayOrder == 1110).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@E4", ILs.First(X => X.displayOrder == 1110).qty.ToString());

                            PDFTemplate = PDFTemplate.Replace("@MECM3", ILs.First(X => X.displayOrder == 1110).extCost.HasValue ? ILs.First(X => X.displayOrder == 1110).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MECM3", ILs.First(X => X.displayOrder == 1110).extCost.HasValue ? ILs.First(X => X.displayOrder == 1110).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MNUS3", ILs.First(X => X.displayOrder == 1110).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MNES3", ILs.First(X => X.displayOrder == 1110).extSell.HasValue ? ILs.First(X => X.displayOrder == 1110).extSell.Value.ToString("0.###") : "");

                            PDFTemplate = PDFTemplate.Replace("@E5", ILs.First(X => X.displayOrder == 1115).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@E5", ILs.First(X => X.displayOrder == 1115).qty.ToString());

                            PDFTemplate = PDFTemplate.Replace("@MECM4", ILs.First(X => X.displayOrder == 1115).extCost.HasValue ? ILs.First(X => X.displayOrder == 1115).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MECM4", ILs.First(X => X.displayOrder == 1115).extCost.HasValue ? ILs.First(X => X.displayOrder == 1115).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MNUS4", ILs.First(X => X.displayOrder == 1115).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MNES4", ILs.First(X => X.displayOrder == 1115).extSell.HasValue ? ILs.First(X => X.displayOrder == 1115).extSell.Value.ToString("0.###") : "");

                            PDFTemplate = PDFTemplate.Replace("@E6", ILs.First(X => X.displayOrder == 1120).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@E6", ILs.First(X => X.displayOrder == 1120).qty.ToString());

                            PDFTemplate = PDFTemplate.Replace("@MECM5", ILs.First(X => X.displayOrder == 1120).extCost.HasValue ? ILs.First(X => X.displayOrder == 1120).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MECM5", ILs.First(X => X.displayOrder == 1120).extCost.HasValue ? ILs.First(X => X.displayOrder == 1120).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MNUS5", ILs.First(X => X.displayOrder == 1120).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MNES5", ILs.First(X => X.displayOrder == 1120).extSell.HasValue ? ILs.First(X => X.displayOrder == 1120).extSell.Value.ToString("0.###") : "");

                            PDFTemplate = PDFTemplate.Replace("@E7", ILs.First(X => X.displayOrder == 1900).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@E7", ILs.First(X => X.displayOrder == 1900).qty.ToString());

                            PDFTemplate = PDFTemplate.Replace("@MECM6", ILs.First(X => X.displayOrder == 1900).extCost.HasValue ? ILs.First(X => X.displayOrder == 1900).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MECM6", ILs.First(X => X.displayOrder == 1900).extCost.HasValue ? ILs.First(X => X.displayOrder == 1900).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MNUS6", ILs.First(X => X.displayOrder == 1900).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MNES6", ILs.First(X => X.displayOrder == 1900).extSell.HasValue ? ILs.First(X => X.displayOrder == 1900).extSell.Value.ToString("0.###") : "");

                            PDFTemplate = PDFTemplate.Replace("@E8", ILs.First(X => X.displayOrder == 1950).qty.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@E8", ILs.First(X => X.displayOrder == 1950).qty.ToString());

                            PDFTemplate = PDFTemplate.Replace("@MECM7", ILs.First(X => X.displayOrder == 1950).extCost.ToString());//HasValue? ILs.First(X => X.displayOrder == 1950).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MECM7", ILs.First(X => X.displayOrder == 1950).extCost.HasValue ? ILs.First(X => X.displayOrder == 1950).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MNUS7", ILs.First(X => X.displayOrder == 1950).unitSell.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MNES7", ILs.First(X => X.displayOrder == 1950).extSell.HasValue ? ILs.First(X => X.displayOrder == 1950).extSell.Value.ToString("0.###") : "");

                            PDFTemplate = PDFTemplate.Replace("@MTotalExtCost", ILs.First(X => X.displayOrder == 1990).extCost.HasValue ? ILs.First(X => X.displayOrder == 1990).extCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MTotalExtCost", ILs.First(X => X.displayOrder == 1990).extCost.HasValue ? ILs.First(X => X.displayOrder == 1990).extCost.Value.ToString("0.###") : "");

                            PDFTemplate = PDFTemplate.Replace("@MUnitCost", ILs.First(X => X.displayOrder == 1990).unitCost.HasValue ? ILs.First(X => X.displayOrder == 1990).unitCost.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MUnitCost", ILs.First(X => X.displayOrder == 1990).unitCost.HasValue ? ILs.First(X => X.displayOrder == 1990).unitCost.Value.ToString("0.###") : "");


                            PDFTemplate = PDFTemplate.Replace("@MTotalUnitSell", ILs.First(X => X.displayOrder == 1990).unitSell.HasValue ? ILs.First(X => X.displayOrder == 1990).unitSell.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MTotalUnitSell", ILs.First(X => X.displayOrder == 1990).unitSell.HasValue ? ILs.First(X => X.displayOrder == 1990).unitSell.Value.ToString("0.###") : "");

                            PDFTemplate = PDFTemplate.Replace("@MTotalExtSelll", ILs.First(X => X.displayOrder == 1990).extSell.HasValue ? ILs.First(X => X.displayOrder == 1990).extSell.Value.ToString("0.###") : "");
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@MTotalExtSell", ILs.First(X => X.displayOrder == 1990).extSell.HasValue ? ILs.First(X => X.displayOrder == 1990).extSell.Value.ToString("0.###") : "");

                            PDFTemplate = PDFTemplate.Replace("@Freight", ILs.First(X => X.displayOrder == 2010).extCost.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@Freight", ILs.First(X => X.displayOrder == 2010).extCost.ToString());

                            PDFTemplate = PDFTemplate.Replace("@FCost", ILs.First(X => X.displayOrder == 2010).unitCost.ToString());
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@FCost", ILs.First(X => X.displayOrder == 2010).unitCost.ToString());

                            PDFTemplate = PDFTemplate.Replace("@Comments", VendorInvoice.comment);
                            PDFTemplateVendor = PDFTemplateVendor.Replace("@Comments", VendorInvoice.comment);

                        }

                        InvoiceDisplay ID = new InvoiceDisplay();
                        ID.pdftemplate = PDFTemplate;
                        ViewAsPdf pdf = new ViewAsPdf("CreateLocalPDF", ID)
                        {
                            FileName = VendorInvoice.lngID + "_Client_Invoice.pdf",
                            CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                        };

                        Task<byte[]> pdfData = pdf.BuildFile(ControllerContext);
                        var folderName = Path.Combine("Repository", "Invoices");
                        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                        string fullPath = pathToSave + "\\" + pdf.FileName;
                        using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                        {
                            fileStream.Write(pdfData.Result, 0, pdfData.Result.Length);
                        }

                        InvoiceDisplay VendorID = new InvoiceDisplay();
                        VendorID.pdftemplate = PDFTemplateVendor;
                        ViewAsPdf pdfVendor = new ViewAsPdf("CreateLocalPDF", VendorID)
                        {
                            FileName = VendorInvoice.lngID + "_Vendor_Invoice.pdf",
                            CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                        };

                        Task<byte[]> pdfDataVendor = pdfVendor.BuildFile(ControllerContext);
                        folderName = Path.Combine("Repository", "Invoices");
                        pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                        fullPath = pathToSave + "\\" + pdfVendor.FileName;
                        using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                        {
                            fileStream.Write(pdfDataVendor.Result, 0, pdfDataVendor.Result.Length);
                        }

                        return new ViewAsPdf("CreateLocalPDF", VendorID);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ResultMessage = "Error: " + ex.Message;
            }

            return View();
        }

        public IActionResult UploadClientInvoice(int OrderId)
        {
            UploadClientInvoiceViewModel model = new UploadClientInvoiceViewModel();
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");
            model.PageTitle = "Upload Client Invoice";
            model.OrderID = OrderId;

            return View(model);
        }

        [HttpPost, DisableRequestSizeLimit]
        public IActionResult UploadClientInvoice(UploadClientInvoiceViewModel model)
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
                    using (var db = _DbContextFactory.CreateDbContext())
                    {
                        string SessionKey = "UOD" + UserNameFromSession;

                        string FileNameforDB = "";
                        if (Request.Form.Files.Count > 0)
                        {
                            var file = Request.Form.Files[0];
                            var folderName = Path.Combine("Repository", "Invoices", "ClientInvoices");
                            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                            bool exists = System.IO.Directory.Exists(pathToSave);
                            if (!exists)
                                System.IO.Directory.CreateDirectory(pathToSave);

                            if (file.Length > 0)
                            {
                                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                                FileNameforDB = fileName;
                                string fileType = fileName.Split('.')[1];
                                string newFileName = model.OrderID.ToString() + "." + fileType;

                                if (fileType == "pdf")
                                {
                                    var fullPath = Path.Combine(pathToSave, newFileName);
                                    var dbPath = Path.Combine(folderName, newFileName);
                                    using (var stream = new FileStream(fullPath, FileMode.Create))
                                    {
                                        file.CopyTo(stream);
                                    }

                                    Invoice INV = db.Invoices.FirstOrDefault(d => d.orderID == model.OrderID);

                                    if (INV != null)
                                    {
                                        INV.datClientInvoiceUploaded = DateTime.Now;
                                        INV.clientInvoiceFile = newFileName;
                                        INV.status = "Client invoice uploaded by Grandflow";
                                        db.SaveChanges();
                                    }

                                    ViewBag.ResultMessage = "Invoice uploaded successfully!";
                                    return RedirectToAction("InvoiceListing", new { Success = true, Details = ViewBag.ResultMessage });
                                }
                                else
                                {
                                    ViewBag.ResultMessage = "Only .pdf extentions allowed for upload!";
                                    return RedirectToAction("UploadClientInvoice", new { Success = false, Details = ViewBag.ResultMessage });
                                }
                            }
                            else
                            {
                                ViewBag.ResultMessage = "Please select a file for upload!";
                                return RedirectToAction("UploadClientInvoice", new { Success = false, Details = ViewBag.ResultMessage });
                            }
                        }
                        else
                        {
                            ViewBag.ResultMessage = "Please select a file for upload!";
                            return RedirectToAction("UploadClientInvoice", new { Success = false, Details = ViewBag.ResultMessage });
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

        public IActionResult SendClientInvoice(int OrderId)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    Invoice INV = db.Invoices.FirstOrDefault(d => d.orderID == OrderId);

                    if (INV != null)
                    {
                        INV.datClientInvoiced = DateTime.Now;
                        INV.status = "Client invoice sent by Grandflow";
                        db.SaveChanges();

                        //send out email to Client
                        Order ORD = db.Orders.FirstOrDefault(d => d.lngID == INV.orderID);
                        Account AVT = db.Account.FirstOrDefault(d => d.lngID == ORD.lngAccount);

                        string Subject = "Smart Source Orderflow - Order # TBEN-" + ORD.lngID + " Invoice!";
                        string Body = "Dear " + AVT.Name + ", <br/><br/> Please find attached invoice for your order # TBEN-" + ORD.lngID + " submitted on " + ORD.datSubmitted.Value.ToShortDateString() + "<br/><br/>";
                        List<Configuration> Emailconfs = db.Configurations.Where(x => x.Module == "Email Server Configuration").ToList<Configuration>();
                        Body = Body + "<br/><br/Regards,<br/>Smart Source Orderflow Team";

                        string CompleteAttachmentPath = "";

                        var folderName = Path.Combine("Repository", "Invoices", "ClientInvoices");
                        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                        CompleteAttachmentPath = Path.Combine(pathToSave, INV.clientInvoiceFile);

                        SendEMailWithAttachments(AVT.Email, Subject, Body, CompleteAttachmentPath);
                    }

                    ViewBag.ResultMessage = "Invoice sent to client successfully!";
                    return RedirectToAction("InvoiceListing", new { Success = true, Details = ViewBag.ResultMessage });

                }
            }
            catch (Exception ex)
            {
                ViewBag.ResultMessage = "Error! " + ex.Message;
                return RedirectToAction("InvoiceListing", new { Success = false, Details = ViewBag.ResultMessage });
            }
        }

        public IActionResult ReceiveClientPayment(int OrderId)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    Invoice INV = db.Invoices.FirstOrDefault(d => d.orderID == OrderId);

                    if (INV != null)
                    {
                        INV.datClientPayment = DateTime.Now;
                        INV.status = "Client payment received by Grandflow";
                        db.SaveChanges();
                    }

                    ViewBag.ResultMessage = "Client payment status updated successfully!";
                    return RedirectToAction("InvoiceListing", new { Success = true, Details = ViewBag.ResultMessage });

                }
            }
            catch (Exception ex)
            {
                ViewBag.ResultMessage = "Error! " + ex.Message;
                return RedirectToAction("InvoiceListing", new { Success = false, Details = ViewBag.ResultMessage });
            }
        }

        public string SubmitTermsAccepted()
        {
            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {
                    SessionTest session = new SessionTest(_httpContextAccessor);
                    session.setSessionTerms("Accepted");
                    return "Success";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ResultMessage = "Error! " + ex.Message;
                return "Error";
            }
        }

        public IActionResult xmlGenerate(Order ord1, InvoiceDetails model, OrderCheckoutField OCFs)
        {
            SessionTest session = new SessionTest(_httpContextAccessor);
            string UserNameFromSession = session.getSession();
            if (String.IsNullOrEmpty(UserNameFromSession))
                return RedirectToAction("Login", "Login");

            try
            {
                using (var db = _DbContextFactory.CreateDbContext())
                {

                    XmlDocument doc = new XmlDocument();

                    XmlElement orderDataNode = doc.CreateElement("cXML");
                    (orderDataNode).SetAttribute("timestamp", DateTime.Now.ToString("yyyy-M-d-HH:m:s"));
                    string newGuid = System.Guid.NewGuid().ToString();
                    (orderDataNode).SetAttribute("payloadId", newGuid);
                    doc.AppendChild(orderDataNode);

                    XmlElement credentialsRecordNode = doc.CreateElement("Credentials");
                    (credentialsRecordNode).SetAttribute("domain", "DUNS");

                    XmlElement credentialsRecordNode4 = doc.CreateElement("Credentials");
                    (credentialsRecordNode4).SetAttribute("domain", "Ad-hoc");

                    XmlElement credentialsRecordNode5 = doc.CreateElement("Credentials");
                    (credentialsRecordNode5).SetAttribute("domain", "Ad-hoc");

                    XmlElement requestNode1 = doc.CreateElement("Request");
                    (requestNode1).SetAttribute("deploymentMode", "production");

                    XmlElement orderRequestNodeHeader1 = doc.CreateElement("OrderRequestHeader");
                    (orderRequestNodeHeader1).SetAttribute("orderDate", DateTime.Now.ToString("yyyy-M-d-HH:m:s"));
                    (orderRequestNodeHeader1).SetAttribute("orderID", "TBEN-" + ord1.lngID.ToString());

                    XmlElement postalAddressHeader1 = doc.CreateElement("PostalAddress");
                    (postalAddressHeader1).SetAttribute("name", "default");

                    XmlElement PrintpostalAddressHeader = doc.CreateElement("PostalAddress");
                    (PrintpostalAddressHeader).SetAttribute("name", "default");

                    XmlElement MailpostalAddressHeader = doc.CreateElement("PostalAddress");
                    (MailpostalAddressHeader).SetAttribute("name", "default");

                    XmlElement PostagepostalAddressHeader = doc.CreateElement("PostalAddress");
                    (PostagepostalAddressHeader).SetAttribute("name", "default");

                    /*PRINT*/

                    XmlElement Extrinsic1 = doc.CreateElement("Extrinsic");
                    (Extrinsic1).SetAttribute("name", "costCenter");

                    XmlElement Extrinsic4 = doc.CreateElement("Extrinsic");
                    (Extrinsic4).SetAttribute("name", "BuyerID");

                    XmlElement ExtTBENSYSUnitCost = doc.CreateElement("Extrinsic");
                    (ExtTBENSYSUnitCost).SetAttribute("name", "TBENSYSUnitCost");

                    XmlElement ExtTBENSYSExtCost = doc.CreateElement("Extrinsic");
                    (ExtTBENSYSExtCost).SetAttribute("name", "costCenter");

                    XmlElement ExtTBENSYSUnitSell = doc.CreateElement("Extrinsic");
                    (ExtTBENSYSUnitSell).SetAttribute("name", "TBENSYSUnitSell");

                    XmlElement ExtTBENSYSExtSell = doc.CreateElement("Extrinsic");
                    (ExtTBENSYSExtSell).SetAttribute("name", "TBENSYSPO_Code");

                    XmlElement ItemOutNode1 = doc.CreateElement("ItemOut");
                    (ItemOutNode1).SetAttribute("quantity", "1");
                    (ItemOutNode1).SetAttribute("lineNumber", "1");


                    /*Mail*/

                    XmlElement ExtcostCenterMail = doc.CreateElement("Extrinsic");
                    (ExtcostCenterMail).SetAttribute("name", "costCenter");

                    XmlElement ExtTBENSYSUnitCostMail = doc.CreateElement("Extrinsic");
                    (ExtTBENSYSUnitCostMail).SetAttribute("name", "TBENSYSUnitCost");

                    XmlElement ExtTBENSYSUnitSellMail = doc.CreateElement("Extrinsic");
                    (ExtTBENSYSUnitSellMail).SetAttribute("name", "TBENSYSUnitSell");

                    XmlElement ExtTBENSYSExtCostMail = doc.CreateElement("Extrinsic");
                    (ExtTBENSYSExtCostMail).SetAttribute("name", "TBENSYSPO_Code");

                    //XmlElement ExtTBENSYSExtSellMail = doc.CreateElement("Extrinsic");
                    //(ExtTBENSYSExtSellMail).SetAttribute("name", "TBENSYSExtSell");

                    XmlElement ItemOutNode2 = doc.CreateElement("ItemOut");
                    (ItemOutNode2).SetAttribute("quantity", "1");
                    (ItemOutNode2).SetAttribute("lineNumber", "2");


                    /*Postage*/

                    XmlElement ExtcostCenterPostage = doc.CreateElement("Extrinsic");
                    (ExtcostCenterPostage).SetAttribute("name", "costCenter");

                    XmlElement ExtTBENSYSUnitCostPostage = doc.CreateElement("Extrinsic");
                    (ExtTBENSYSUnitCostPostage).SetAttribute("name", "TBENSYSUnitCost");

                    //XmlElement ExtTBENSYSExtCostPostage = doc.CreateElement("Extrinsic");
                    //(ExtTBENSYSExtCostPostage).SetAttribute("name", "ExtTBENSYSExtCost");

                    XmlElement ExtTBENSYSUnitSellPostage = doc.CreateElement("Extrinsic");
                    (ExtTBENSYSUnitSellPostage).SetAttribute("name", "TBENSYSUnitSell");

                    XmlElement ExtTBENSYSExtSellPostage = doc.CreateElement("Extrinsic");
                    (ExtTBENSYSExtSellPostage).SetAttribute("name", "TBENSYSPO_Code");

                    XmlElement ItemOutNode3 = doc.CreateElement("ItemOut");
                    (ItemOutNode3).SetAttribute("quantity", model.il34qty.ToString());
                    (ItemOutNode3).SetAttribute("lineNumber", "3");


                    XmlElement billTocountryNode = doc.CreateElement("Country");
                    (billTocountryNode).SetAttribute("isoCountryCode", "US");

                    XmlElement Extrinsic5 = doc.CreateElement("Extrinsic");
                    (Extrinsic5).SetAttribute("name", "costCenter");

                    XmlElement printToExtrinsicNode = doc.CreateElement("Extrinsic");
                    (printToExtrinsicNode).SetAttribute("name", "costCenter");

                    XmlElement mailToExtrinsicNode = doc.CreateElement("Extrinsic");
                    (mailToExtrinsicNode).SetAttribute("name", "costCenter");

                    XmlElement pastageExtrinsicNode = doc.CreateElement("Extrinsic");
                    (pastageExtrinsicNode).SetAttribute("name", "costCenter");

                    XmlElement printTocountryNode = doc.CreateElement("Country");
                    (printTocountryNode).SetAttribute("isoCountryCode", "US");

                    XmlElement mailTocountryNode = doc.CreateElement("Country");
                    (mailTocountryNode).SetAttribute("isoCountryCode", "US");

                    XmlElement postageTocountryNode = doc.CreateElement("Country");
                    (postageTocountryNode).SetAttribute("isoCountryCode", "US");

                    XmlElement billToPostalAddress = doc.CreateElement("PostalAddress");
                    (billToPostalAddress).SetAttribute("name", "default");

                    XmlElement shipToPostalAddressPrint = doc.CreateElement("PostalAddress");
                    (shipToPostalAddressPrint).SetAttribute("name", "default");

                    XmlElement ShipToPostalAddressMail = doc.CreateElement("PostalAddress");
                    (ShipToPostalAddressMail).SetAttribute("name", "default");

                    XmlElement ShiToPostalAddressPostage = doc.CreateElement("PostalAddress");
                    (ShiToPostalAddressPostage).SetAttribute("name", "default");

                    //Header Start
                    XmlNode headertNode = doc.CreateElement("Header");
                    orderDataNode.AppendChild(headertNode);

                    /*From*/
                    XmlNode fromRecordNode = doc.CreateElement("From");
                    headertNode.AppendChild(fromRecordNode);

                    XmlNode credentialsRecordNode2 = doc.CreateElement("Credentials");
                    fromRecordNode.AppendChild(credentialsRecordNode);

                    XmlNode contentDateNode = doc.CreateElement("Identity");
                    contentDateNode.AppendChild(doc.CreateTextNode("0300006415"));
                    credentialsRecordNode.AppendChild(contentDateNode);

                    /*To*/
                    XmlNode toNode = doc.CreateElement("To");
                    headertNode.AppendChild(toNode);

                    XmlNode credentialsRecordNode1 = doc.CreateElement("Credentials");
                    toNode.AppendChild(credentialsRecordNode4);

                    XmlNode identityDateNode = doc.CreateElement("Identity");
                    identityDateNode.AppendChild(doc.CreateTextNode("Smart Source of GA"));
                    credentialsRecordNode4.AppendChild(identityDateNode);

                    /* Sender */
                    XmlNode senderNode = doc.CreateElement("Sender");
                    headertNode.AppendChild(senderNode);

                    XmlNode credentialsRecordNode3 = doc.CreateElement("Credentials");
                    senderNode.AppendChild(credentialsRecordNode5);

                    XmlNode identityDateNode1 = doc.CreateElement("Identity");
                    identityDateNode1.AppendChild(doc.CreateTextNode("TBEN-" + ord1.lngID.ToString() + "-00139012"));
                    credentialsRecordNode5.AppendChild(identityDateNode1);

                    /* Request */
                    XmlNode requestNode = doc.CreateElement("Request");
                    doc.DocumentElement.AppendChild(requestNode1);

                    XmlNode orderRequestNode = doc.CreateElement("OrderRequest");
                    requestNode1.AppendChild(orderRequestNode);

                    XmlNode orderRequestNodeHeader = doc.CreateElement("OrderRequestHeader");
                    requestNode1.AppendChild(orderRequestNodeHeader1);
                    orderRequestNode.AppendChild(orderRequestNodeHeader1);

                    XmlNode BillToNode = doc.CreateElement("BillTo");
                    requestNode1.AppendChild(BillToNode);
                    orderRequestNode.AppendChild(BillToNode);
                    orderRequestNodeHeader1.AppendChild(BillToNode);

                    XmlNode addressNode = doc.CreateElement("Address");
                    BillToNode.AppendChild(addressNode);

                    XmlNode postalAddressHeader = doc.CreateElement("PostalAddress");
                    addressNode.AppendChild(postalAddressHeader1);
                    billToPostalAddress.AppendChild(postalAddressHeader);

                    XmlNode deliverToNode = doc.CreateElement("DeliverTo");
                    deliverToNode.AppendChild(doc.CreateTextNode(OCFs.value));
                    addressNode.AppendChild(deliverToNode);
                    postalAddressHeader1.AppendChild(deliverToNode);

                    XmlNode streetNode = doc.CreateElement("Street");
                    streetNode.AppendChild(doc.CreateTextNode(ord1.envelope_product_Address_Line_4));
                    addressNode.AppendChild(streetNode);
                    postalAddressHeader1.AppendChild(streetNode);

                    XmlNode streetNode1 = doc.CreateElement("Street");
                    streetNode.AppendChild(doc.CreateTextNode(""));
                    addressNode.AppendChild(streetNode1);
                    postalAddressHeader1.AppendChild(streetNode1);

                    string cityValuePrint = "";

                    if (!String.IsNullOrEmpty(ord1.envelope_product_City_State_Zip_Line_5))
                    {
                        if (ord1.envelope_product_City_State_Zip_Line_5.Split(',').Length > 0)
                            cityValuePrint = ord1.envelope_product_City_State_Zip_Line_5.Split(',')[0];
                    }

                    XmlNode Citynode = doc.CreateElement("City");
                    Citynode.AppendChild(doc.CreateTextNode(cityValuePrint));
                    addressNode.AppendChild(Citynode);
                    postalAddressHeader1.AppendChild(Citynode);

                    string stateValuePrint = "";

                    if (!String.IsNullOrEmpty(ord1.envelope_product_City_State_Zip_Line_5))
                    {
                        if (ord1.envelope_product_City_State_Zip_Line_5.Split(',').Length > 0)
                            if (ord1.envelope_product_City_State_Zip_Line_5.Split(',')[1].Trim().Split(' ').Length > 0)
                                stateValuePrint = ord1.envelope_product_City_State_Zip_Line_5.Split(',')[1].Trim().Split(' ')[0];
                    }

                    XmlNode StateNode = doc.CreateElement("State");
                    StateNode.AppendChild(doc.CreateTextNode(stateValuePrint));
                    addressNode.AppendChild(StateNode);
                    postalAddressHeader1.AppendChild(StateNode);

                    string postalValuePrint = "";

                    if (!String.IsNullOrEmpty(ord1.envelope_product_City_State_Zip_Line_5))
                    {
                        if (ord1.envelope_product_City_State_Zip_Line_5.Split(',').Length > 0)
                            if (ord1.envelope_product_City_State_Zip_Line_5.Split(',')[1].Trim().Split(' ').Length > 0)
                                postalValuePrint = ord1.envelope_product_City_State_Zip_Line_5.Split(',')[1].Trim().Split(' ')[1];
                    }


                    XmlNode PostalCode = doc.CreateElement("PostalCode");
                    PostalCode.AppendChild(doc.CreateTextNode(postalValuePrint));
                    addressNode.AppendChild(PostalCode);
                    postalAddressHeader1.AppendChild(PostalCode);

                    XmlNode Country = doc.CreateElement("Country");
                    billTocountryNode.AppendChild(doc.CreateTextNode("United States"));
                    addressNode.AppendChild(billTocountryNode);
                    postalAddressHeader1.AppendChild(billTocountryNode);

                    XmlNode Extrinsic = doc.CreateElement("Extrinsic");
                    Extrinsic5.AppendChild(doc.CreateTextNode(""));
                    addressNode.AppendChild(Extrinsic5);
                    postalAddressHeader1.AppendChild(Extrinsic5);

                    XmlNode Name = doc.CreateElement("Name");
                    Name.AppendChild(doc.CreateTextNode(""));
                    addressNode.AppendChild(Name);

                    XmlNode Email = doc.CreateElement("Email");
                    Email.AppendChild(doc.CreateTextNode(""));
                    addressNode.AppendChild(Email);


                    XmlNode Phone = doc.CreateElement("Phone");
                    Phone.AppendChild(doc.CreateTextNode(""));
                    addressNode.AppendChild(Phone);

                    XmlNode shippingNode = doc.CreateElement("Shipping");
                    requestNode1.AppendChild(shippingNode);
                    orderRequestNode.AppendChild(shippingNode);
                    orderRequestNodeHeader1.AppendChild(shippingNode);

                    var postageQty = model.il11qty.ToString();
                    //if (VendorInvoice != null && ClientOrder != null)
                    //{
                    if (model.il34qty > 0)
                    {
                        XmlNode descriptionLineNode1 = doc.CreateElement("Description");
                        descriptionLineNode1.AppendChild(doc.CreateTextNode("UPS"));
                        shippingNode.AppendChild(descriptionLineNode1);
                    }
                    else
                    {
                        XmlNode descriptionLineNode = doc.CreateElement("Description");
                        descriptionLineNode.AppendChild(doc.CreateTextNode("Ground"));
                        shippingNode.AppendChild(descriptionLineNode);
                    }
                    // }
                    /*XmlNode buyerIdNode = doc.CreateElement("BuyerId");
                    buyerIdNode.AppendChild(doc.CreateTextNode("11001390120001"));
                    shippingNode.AppendChild(buyerIdNode);*/

                    XmlNode Comments = doc.CreateElement("Comments");
                    requestNode1.AppendChild(Comments);
                    orderRequestNode.AppendChild(Comments);
                    orderRequestNodeHeader1.AppendChild(Comments);

                    XmlNode Extrinsic3 = doc.CreateElement("Extrinsic");
                    Extrinsic4.AppendChild(doc.CreateTextNode("0300006415"));
                    requestNode1.AppendChild(Extrinsic4);
                    orderRequestNode.AppendChild(Extrinsic4);
                    orderRequestNodeHeader1.AppendChild(Extrinsic4);
                    //orderRequestNodeHeader.AppendChild(Extrinsic4);

                    XmlNode itemIoutNode = doc.CreateElement("ItemOut");
                    requestNode1.AppendChild(ItemOutNode1);
                    orderRequestNode.AppendChild(ItemOutNode1);
                    //orderRequestNode.AppendChild(itemIoutNode);

                    XmlNode itemIDNode = doc.CreateElement("ItemID");
                    itemIoutNode.AppendChild(itemIDNode);
                    ItemOutNode1.AppendChild(itemIDNode);

                    XmlNode supplierPartLineNode = doc.CreateElement("SupplierPartID");
                    supplierPartLineNode.AppendChild(doc.CreateTextNode("PRINT"));
                    itemIDNode.AppendChild(supplierPartLineNode);

                    XmlNode itemDetailNode = doc.CreateElement("ItemDetail");
                    itemIoutNode.AppendChild(itemDetailNode);
                    ItemOutNode1.AppendChild(itemDetailNode);

                    XmlNode Description = doc.CreateElement("Description");
                    itemDetailNode.AppendChild(Description);
                    /*PRINT*/
                    /*XmlNode extrinsicCostCenter = doc.CreateElement("Extrinsic");
                    Extrinsic1.AppendChild(doc.CreateTextNode("123456"));
                    itemDetailNode.AppendChild(Extrinsic1);*/

                    XmlNode extrinsicTBENSYSExtCost = doc.CreateElement("Extrinsic");
                    ExtTBENSYSExtCost.AppendChild(doc.CreateTextNode(ord1.envelope_product_Cost_Center.Substring(0, 4)));
                    itemDetailNode.AppendChild(ExtTBENSYSExtCost);

  
                    XmlNode extrinsicTBENSYSUnitCost = doc.CreateElement("Extrinsic");
                    ExtTBENSYSUnitCost.AppendChild(doc.CreateTextNode(model.il11unitCost.ToString()));
                    itemDetailNode.AppendChild(ExtTBENSYSUnitCost);

                    XmlNode extrinsicTBENSYSUnitSell = doc.CreateElement("Extrinsic");
                    ExtTBENSYSUnitSell.AppendChild(doc.CreateTextNode(""));
                    itemDetailNode.AppendChild(ExtTBENSYSUnitSell);

                    XmlNode extrinsicTBENSYSExtSell = doc.CreateElement("Extrinsic");
                    ExtTBENSYSExtSell.AppendChild(doc.CreateTextNode("1"));
                    itemDetailNode.AppendChild(ExtTBENSYSExtSell);

                    #region
                    /* [Start] We added shipTo in print tag*/
                    XmlNode ShipToNode = doc.CreateElement("ShipTo");
                    requestNode1.AppendChild(ShipToNode);
                    orderRequestNode.AppendChild(ShipToNode);
                    ItemOutNode1.AppendChild(ShipToNode);

                    XmlNode addressNodePrint = doc.CreateElement("Address");
                    ShipToNode.AppendChild(addressNodePrint);

                    XmlNode postalAddressHeaderPrint = doc.CreateElement("PostalAddress");
                    addressNodePrint.AppendChild(PrintpostalAddressHeader);
                    //postalAddressHeaderPrint.AppendChild(doc.CreateTextNode(""));
                    shipToPostalAddressPrint.AppendChild(postalAddressHeaderPrint);

                    XmlNode deliverToNodePrint = doc.CreateElement("DeliverTo");
                    deliverToNodePrint.AppendChild(doc.CreateTextNode(OCFs.value));
                    addressNode.AppendChild(deliverToNodePrint);
                    PrintpostalAddressHeader.AppendChild(deliverToNodePrint);

                    XmlNode streetNodePrint = doc.CreateElement("Street");
                    streetNodePrint.AppendChild(doc.CreateTextNode(ord1.envelope_product_Address_Line_4));
                    addressNode.AppendChild(streetNodePrint);
                    PrintpostalAddressHeader.AppendChild(streetNodePrint);

                    XmlNode streetNodePrint1 = doc.CreateElement("Street");
                    streetNodePrint.AppendChild(doc.CreateTextNode(""));
                    addressNodePrint.AppendChild(streetNodePrint);
                    PrintpostalAddressHeader.AppendChild(streetNodePrint); 

                    string cityValue = "";

                    if (!String.IsNullOrEmpty(ord1.envelope_product_City_State_Zip_Line_5))
                    {
                        if (ord1.envelope_product_City_State_Zip_Line_5.Split(',').Length > 0)
                            cityValue = ord1.envelope_product_City_State_Zip_Line_5.Split(',')[0];
                    }

                    XmlNode CitynodePrint = doc.CreateElement("City");
                    CitynodePrint.AppendChild(doc.CreateTextNode(cityValue));
                    addressNodePrint.AppendChild(CitynodePrint);
                    PrintpostalAddressHeader.AppendChild(CitynodePrint);

                    string stateValue = "";

                    if (!String.IsNullOrEmpty(ord1.envelope_product_City_State_Zip_Line_5))
                    {
                        if (ord1.envelope_product_City_State_Zip_Line_5.Split(',').Length > 0)
                            if (ord1.envelope_product_City_State_Zip_Line_5.Split(',')[1].Trim().Split(' ').Length > 0)
                                stateValue = ord1.envelope_product_City_State_Zip_Line_5.Split(',')[1].Trim().Split(' ')[0];
                    }

                    XmlNode StateNodePrint = doc.CreateElement("State");
                    StateNodePrint.AppendChild(doc.CreateTextNode(stateValue));
                    addressNodePrint.AppendChild(StateNodePrint);
                    PrintpostalAddressHeader.AppendChild(StateNodePrint);

                    string postalValue = "";

                    if (!String.IsNullOrEmpty(ord1.envelope_product_City_State_Zip_Line_5))
                    {
                        if (ord1.envelope_product_City_State_Zip_Line_5.Split(',').Length > 0)
                            if (ord1.envelope_product_City_State_Zip_Line_5.Split(',')[1].Trim().Split(' ').Length > 0)
                                postalValue = ord1.envelope_product_City_State_Zip_Line_5.Split(',')[1].Trim().Split(' ')[1];
                    }

                    XmlNode PostalCodePrint = doc.CreateElement("PostalCode");
                    PostalCodePrint.AppendChild(doc.CreateTextNode(postalValue));
                    addressNodePrint.AppendChild(PostalCodePrint);
                    PrintpostalAddressHeader.AppendChild(PostalCodePrint);

                    XmlNode CountryPrint = doc.CreateElement("Country");
                    printTocountryNode.AppendChild(doc.CreateTextNode("United States"));
                    addressNodePrint.AppendChild(printTocountryNode);
                    PrintpostalAddressHeader.AppendChild(printTocountryNode);

                    XmlNode ExtrinsicPrint = doc.CreateElement("Extrinsic");
                    addressNodePrint.AppendChild(printToExtrinsicNode);
                    PrintpostalAddressHeader.AppendChild(printToExtrinsicNode);

                    XmlNode NamePrint = doc.CreateElement("Name");
                    NamePrint.AppendChild(doc.CreateTextNode("Steve Boyett"));
                    addressNodePrint.AppendChild(NamePrint);

                    XmlNode EmailPrint = doc.CreateElement("Email");
                    EmailPrint.AppendChild(doc.CreateTextNode("sboyett@smartsourcellc.com"));
                    addressNodePrint.AppendChild(EmailPrint);

                    XmlNode PhonePrint = doc.CreateElement("Phone");
                    PhonePrint.AppendChild(doc.CreateTextNode("7704496300"));
                    addressNodePrint.AppendChild(PhonePrint);

                    XmlNode Extrinsic2Print = doc.CreateElement("Extrinsic");
                    Extrinsic1.AppendChild(doc.CreateTextNode(ord1.envelope_product_Cost_Center.Substring(0, 4)));
                    addressNode.AppendChild(Extrinsic1);
                    /*end*/
                    #endregion

                    XmlNode itemIoutNode1 = doc.CreateElement("ItemOut");
                    requestNode1.AppendChild(ItemOutNode2);
                    orderRequestNode.AppendChild(ItemOutNode2);

                    XmlNode itemIDNode1 = doc.CreateElement("ItemID");
                    itemIoutNode1.AppendChild(itemIDNode1);
                    ItemOutNode2.AppendChild(itemIDNode1);

                    XmlNode supplierPartMailNode = doc.CreateElement("SupplierPartID");
                    supplierPartMailNode.AppendChild(doc.CreateTextNode("MAILING"));
                    itemIDNode1.AppendChild(supplierPartMailNode);

                    XmlNode itemDetailNode1 = doc.CreateElement("ItemDetail");
                    itemIoutNode1.AppendChild(itemDetailNode1);
                    ItemOutNode2.AppendChild(itemDetailNode1);

                    XmlNode Description1 = doc.CreateElement("Description");
                    itemDetailNode1.AppendChild(Description1);

                    /*Mail*/

                    XmlNode extrinsicCostCenter1 = doc.CreateElement("Extrinsic");
                    ExtcostCenterMail.AppendChild(doc.CreateTextNode(ord1.envelope_product_Cost_Center.Substring(0, 4)));
                    itemDetailNode1.AppendChild(ExtcostCenterMail);

                    XmlNode extrinsicTBENSYSUnitCost1 = doc.CreateElement("Extrinsic");
                    ExtTBENSYSUnitCostMail.AppendChild(doc.CreateTextNode(model.il36unitCost.ToString()));
                    itemDetailNode1.AppendChild(ExtTBENSYSUnitCostMail);

                    XmlNode extrinsicTBENSYSUnitSell1 = doc.CreateElement("Extrinsic");
                    ExtTBENSYSUnitSellMail.AppendChild(doc.CreateTextNode(""));
                    itemDetailNode1.AppendChild(ExtTBENSYSUnitSellMail);

                    XmlNode extrinsicTBENSYSExtCost1 = doc.CreateElement("Extrinsic");
                    ExtTBENSYSExtCostMail.AppendChild(doc.CreateTextNode("1"));
                    itemDetailNode1.AppendChild(ExtTBENSYSExtCostMail);



                    //XmlNode extrinsicTBENSYSExtSell1 = doc.CreateElement("Extrinsic");
                    //ExtTBENSYSExtSellMail.AppendChild(doc.CreateTextNode("296.97"));
                    //itemDetailNode1.AppendChild(ExtTBENSYSExtSellMail);

                    #region
                    /* [Start] We added shipTo in Mail tag*/
                    XmlNode ShipToNode1 = doc.CreateElement("ShipTo");
                    requestNode1.AppendChild(ShipToNode1);
                    orderRequestNode.AppendChild(ShipToNode1);
                    ItemOutNode2.AppendChild(ShipToNode1);

                    XmlNode addressNodeMail = doc.CreateElement("Address");
                    ShipToNode1.AppendChild(addressNodeMail);

                    XmlNode postalAddressHeaderMail = doc.CreateElement("PostalAddress");
                    addressNodeMail.AppendChild(MailpostalAddressHeader);
                    ShipToPostalAddressMail.AppendChild(postalAddressHeaderMail);

                    XmlNode deliverToNodeMail = doc.CreateElement("DeliverTo");
                    deliverToNodeMail.AppendChild(doc.CreateTextNode(OCFs.value));
                    addressNode.AppendChild(deliverToNodeMail);
                    MailpostalAddressHeader.AppendChild(deliverToNodeMail);

                    XmlNode streetNodeMail = doc.CreateElement("Street");
                    streetNodeMail.AppendChild(doc.CreateTextNode(ord1.envelope_product_Address_Line_4));
                    addressNode.AppendChild(streetNodeMail);
                    MailpostalAddressHeader.AppendChild(streetNodeMail);

                    XmlNode streetNodeMail1 = doc.CreateElement("Street");
                    streetNodeMail.AppendChild(doc.CreateTextNode(""));
                    addressNodeMail.AppendChild(streetNodeMail);
                    MailpostalAddressHeader.AppendChild(streetNodeMail);

                    string cityValueMail = "";

                    if (!String.IsNullOrEmpty(ord1.envelope_product_City_State_Zip_Line_5))
                    {
                        if (ord1.envelope_product_City_State_Zip_Line_5.Split(',').Length > 0)
                            cityValueMail = ord1.envelope_product_City_State_Zip_Line_5.Split(',')[0];
                    }
                    XmlNode CitynodeMail = doc.CreateElement("City");
                    CitynodeMail.AppendChild(doc.CreateTextNode(cityValueMail));
                    addressNodeMail.AppendChild(CitynodeMail);
                    MailpostalAddressHeader.AppendChild(CitynodeMail);


                    string stateValueMail = "";

                    if (!String.IsNullOrEmpty(ord1.envelope_product_City_State_Zip_Line_5))
                    {
                            if(ord1.envelope_product_City_State_Zip_Line_5.Split(',').Length > 0)
                            if(ord1.envelope_product_City_State_Zip_Line_5.Split(',')[1].Trim().Split(' ').Length > 0)
                            stateValueMail = ord1.envelope_product_City_State_Zip_Line_5.Split(',')[1].Trim().Split(' ')[0];
                    }
                    XmlNode StateNodeMail = doc.CreateElement("State");
                    StateNodeMail.AppendChild(doc.CreateTextNode(stateValueMail));
                    addressNodeMail.AppendChild(StateNodeMail);
                    MailpostalAddressHeader.AppendChild(StateNodeMail);

                    string postalValueMail = "";

                    if (!String.IsNullOrEmpty(ord1.envelope_product_City_State_Zip_Line_5))
                    {
                        if (ord1.envelope_product_City_State_Zip_Line_5.Split(',').Length > 0)
                           if (ord1.envelope_product_City_State_Zip_Line_5.Split(',')[1].Trim().Split(' ').Length > 0)
                                postalValueMail = ord1.envelope_product_City_State_Zip_Line_5.Split(',')[1].Trim().Split(' ')[1];
                    }

                    XmlNode PostalCodeMail = doc.CreateElement("PostalCode");
                    PostalCodeMail.AppendChild(doc.CreateTextNode(postalValueMail));
                    addressNodeMail.AppendChild(PostalCodeMail);
                    MailpostalAddressHeader.AppendChild(PostalCodeMail);

                    XmlNode CountryMail = doc.CreateElement("Country");
                    mailTocountryNode.AppendChild(doc.CreateTextNode("United States"));
                    addressNodeMail.AppendChild(mailTocountryNode);
                    MailpostalAddressHeader.AppendChild(mailTocountryNode);

                    XmlNode ExtrinsicMail = doc.CreateElement("Extrinsic");
                    addressNodeMail.AppendChild(mailToExtrinsicNode);
                    MailpostalAddressHeader.AppendChild(mailToExtrinsicNode);

                    XmlNode NameMail = doc.CreateElement("Name");
                    NameMail.AppendChild(doc.CreateTextNode("Steve Boyett"));
                    addressNodeMail.AppendChild(NameMail);

                    XmlNode EmailMail = doc.CreateElement("Email");
                    EmailMail.AppendChild(doc.CreateTextNode("sboyett@smartsourcellc.com"));
                    addressNodeMail.AppendChild(EmailMail);

                    XmlNode PhoneMail = doc.CreateElement("Phone");
                    PhoneMail.AppendChild(doc.CreateTextNode("7704496300"));
                    addressNodeMail.AppendChild(PhoneMail);

                    XmlNode Extrinsic2Mail = doc.CreateElement("Extrinsic");
                    //Extrinsic1.AppendChild(doc.CreateTextNode("123456"));
                    addressNode.AppendChild(Extrinsic2Mail);
                    /*end*/
                    #endregion

                    XmlNode itemIoutNode2 = doc.CreateElement("ItemOut");
                    requestNode1.AppendChild(ItemOutNode3);
                    orderRequestNode.AppendChild(ItemOutNode3);


                    XmlNode itemIDNode2 = doc.CreateElement("ItemID");
                    itemIoutNode2.AppendChild(itemIDNode2);
                    ItemOutNode3.AppendChild(itemIDNode2);

                    XmlNode supplierPartPOSTAGENode = doc.CreateElement("SupplierPartID");
                    supplierPartPOSTAGENode.AppendChild(doc.CreateTextNode("POSTAGE"));
                    itemIDNode2.AppendChild(supplierPartPOSTAGENode);

                    XmlNode itemDetailNode2 = doc.CreateElement("ItemDetail");
                    itemIoutNode2.AppendChild(itemDetailNode2);
                    ItemOutNode3.AppendChild(itemDetailNode2);

                    XmlNode Description2 = doc.CreateElement("Description");
                    itemDetailNode2.AppendChild(Description2);

                    /*POSTAGE*/

                    XmlNode extrinsicCostCenter2 = doc.CreateElement("Extrinsic");
                    ExtcostCenterPostage.AppendChild(doc.CreateTextNode(ord1.envelope_product_Cost_Center.Substring(0, 4)));
                    itemDetailNode2.AppendChild(ExtcostCenterPostage);

                    XmlNode extrinsicTBENSYSUnitCost2 = doc.CreateElement("Extrinsic");
                    ExtTBENSYSUnitCostPostage.AppendChild(doc.CreateTextNode(model.il34unitCost.ToString()));
                    itemDetailNode2.AppendChild(ExtTBENSYSUnitCostPostage);

                    //XmlNode extrinsicTBENSYSExtCost2 = doc.CreateElement("Extrinsic");
                    //ExtTBENSYSExtCostPostage.AppendChild(doc.CreateTextNode("245.39"));
                    //itemDetailNode2.AppendChild(ExtTBENSYSExtCostPostage);

                    XmlNode extrinsicTBENSYSUnitSell2 = doc.CreateElement("Extrinsic");
                    ExtTBENSYSUnitSellPostage.AppendChild(doc.CreateTextNode(model.il34unitSell.ToString()));
                    itemDetailNode2.AppendChild(ExtTBENSYSUnitSellPostage);

                    XmlNode extrinsicTBENSYSExtSell2 = doc.CreateElement("Extrinsic");
                    ExtTBENSYSExtSellPostage.AppendChild(doc.CreateTextNode("1"));
                    itemDetailNode2.AppendChild(ExtTBENSYSExtSellPostage);

                    #region
                    /* [Start] We added shipTo in Postage tag*/
                    XmlNode ShipToNode2 = doc.CreateElement("ShipTo");
                    requestNode1.AppendChild(ShipToNode2);
                    orderRequestNode.AppendChild(ShipToNode2);
                    ItemOutNode3.AppendChild(ShipToNode2);

                    XmlNode addressNodePostage = doc.CreateElement("Address");
                    ShipToNode2.AppendChild(addressNodePostage);

                    XmlNode postalAddressHeaderPostage = doc.CreateElement("PostalAddress");
                    addressNodePostage.AppendChild(PostagepostalAddressHeader);
                    ShiToPostalAddressPostage.AppendChild(postalAddressHeaderPrint);

                    XmlNode deliverToNodePostage = doc.CreateElement("DeliverTo");
                    deliverToNodePostage.AppendChild(doc.CreateTextNode(OCFs.value));
                    addressNode.AppendChild(deliverToNodePostage);
                    PostagepostalAddressHeader.AppendChild(deliverToNodePostage);

                    XmlNode streetNodePostage = doc.CreateElement("Street");
                    streetNodePostage.AppendChild(doc.CreateTextNode(ord1.envelope_product_Address_Line_4));
                    addressNode.AppendChild(streetNodePostage);
                    PostagepostalAddressHeader.AppendChild(streetNodePostage);

                    XmlNode streetNodePostage1 = doc.CreateElement("Street");
                    streetNodePostage.AppendChild(doc.CreateTextNode(""));
                    addressNodePostage.AppendChild(streetNodePostage);
                    PostagepostalAddressHeader.AppendChild(streetNodePostage);

                    string cityValuePostage = "";

                    if (!String.IsNullOrEmpty(ord1.envelope_product_City_State_Zip_Line_5))
                    {
                        if (ord1.envelope_product_City_State_Zip_Line_5.Split(',').Length > 0)
                            cityValuePostage = ord1.envelope_product_City_State_Zip_Line_5.Split(',')[0];
                    }

                    XmlNode CitynodePostage = doc.CreateElement("City");
                    CitynodePostage.AppendChild(doc.CreateTextNode(cityValuePostage));
                    addressNodePostage.AppendChild(CitynodePostage);
                    PostagepostalAddressHeader.AppendChild(CitynodePostage);

                    string stateValuePostage = "";

                    if (!String.IsNullOrEmpty(ord1.envelope_product_City_State_Zip_Line_5))
                    {
                        if (ord1.envelope_product_City_State_Zip_Line_5.Split(',').Length > 0)
                            if (ord1.envelope_product_City_State_Zip_Line_5.Split(',')[1].Trim().Split(' ').Length > 0)
                                stateValuePostage = ord1.envelope_product_City_State_Zip_Line_5.Split(',')[1].Trim().Split(' ')[0];
                    }

                    XmlNode StateNodePostage = doc.CreateElement("State");
                    StateNodePostage.AppendChild(doc.CreateTextNode(stateValuePostage));
                    addressNodePostage.AppendChild(StateNodePostage);
                    PostagepostalAddressHeader.AppendChild(StateNodePostage);

                    string postalValuePostage = "";

                    if (!String.IsNullOrEmpty(ord1.envelope_product_City_State_Zip_Line_5))
                    {
                        if (ord1.envelope_product_City_State_Zip_Line_5.Split(',').Length > 0)
                            if (ord1.envelope_product_City_State_Zip_Line_5.Split(',')[1].Trim().Split(' ').Length > 0)
                                postalValuePostage = ord1.envelope_product_City_State_Zip_Line_5.Split(',')[1].Trim().Split(' ')[1];
                    }

                    XmlNode PostalCodePostage = doc.CreateElement("PostalCode");
                    PostalCodePostage.AppendChild(doc.CreateTextNode(postalValuePostage));
                    addressNodePostage.AppendChild(PostalCodePostage);
                    PostagepostalAddressHeader.AppendChild(PostalCodePostage);

                    XmlNode CountryPostage = doc.CreateElement("Country");
                    postageTocountryNode.AppendChild(doc.CreateTextNode("United States"));
                    addressNodePostage.AppendChild(postageTocountryNode);
                    PostagepostalAddressHeader.AppendChild(postageTocountryNode);

                    XmlNode ExtrinsicPostage = doc.CreateElement("Extrinsic");
                    addressNodePostage.AppendChild(pastageExtrinsicNode);
                    PostagepostalAddressHeader.AppendChild(pastageExtrinsicNode);

                    XmlNode NamePostage = doc.CreateElement("Name");
                    NamePostage.AppendChild(doc.CreateTextNode("Steve Boyett"));
                    addressNodePostage.AppendChild(NamePostage);

                    XmlNode EmailPostage = doc.CreateElement("Email");
                    EmailPostage.AppendChild(doc.CreateTextNode("sboyett@smartsourcellc.com"));
                    addressNodePostage.AppendChild(EmailPostage);

                    XmlNode PhonePostage = doc.CreateElement("Phone");
                    PhonePostage.AppendChild(doc.CreateTextNode("7704496300"));
                    addressNodePostage.AppendChild(PhonePostage);

                    XmlNode Extrinsic2Postage = doc.CreateElement("Extrinsic");
                    //Extrinsic1.AppendChild(doc.CreateTextNode("123456"));
                    addressNode.AppendChild(Extrinsic2Postage);
                    /*end*/
                    #endregion

                    var basePath = Path.Combine(Environment.CurrentDirectory, @"XMLFiles\");
                    if (!Directory.Exists(basePath))
                    {
                        Directory.CreateDirectory(basePath);
                    }
                    var newFileName = string.Format("{0}{1}", Guid.NewGuid().ToString("N"), ".xml");
                    doc.Save(basePath + newFileName);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ResultMessage = "Error! " + ex.Message;
                return RedirectToAction("OrderPreview", new { Success = false, Details = ViewBag.ResultMessage });
            }
            return RedirectToAction("Index");
        }
    }
}
