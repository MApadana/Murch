using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Exon.Recab.Payment.Controllers
{
    public class CtlrController : Controller
    {
        private readonly Exon.Recab.Service.Implement.Payment.MellatPaymentService _mellatPayment;
        public CtlrController()
        {
            _mellatPayment = new Service.Implement.Payment.MellatPaymentService("", "", "", "", "");
        }

        public ActionResult Payment(string id)
        {

            string data=  Encoding.ASCII.GetString(Convert.FromBase64String(id));

            


            return View();
        }


        public ActionResult Error()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
