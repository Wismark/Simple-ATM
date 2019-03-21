using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CM.Services;

namespace Cash_Machine.Controllers
{
    public class AtmController : Controller
    {
        private readonly CardHandler _cardService;
        public AtmController(CardHandler cardH)
        {
            _cardService = cardH;
        }

        public AtmController( )
        { }

        [HttpGet]
        public ActionResult EnterAtm()
        {
            return View();
        }

        [HttpPost]
        public JsonResult EnterAtm(string cardNumber)
        {
            //if (_cardService.CheckCard(cardNumber)) 
            //{
            //   return Json(new { url = "/Atm/PinCodeCheck?cardNumber="+cardNumber});
            //}
            return Json(new { url = "/Atm/CardError/" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PinCodeCheck(string cardNumber)
        {
            if (cardNumber != null)
            {
                ViewBag.card = cardNumber;
                return View();
            }

            return RedirectToAction("EnterAtm");
        }

        [HttpPost]
        public JsonResult PinCodeCheck(string pinCode, string cardNumber)
        {
            if (_cardService.CheckPinCode(pinCode, cardNumber))
            {
                return Json(new { url = "/Atm/GetOperations/" });
            } 
            return Json(new { url = "/Atm/PinCodeCheck/" });
        }

        [HttpGet]
        public ViewResult CardError()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetOperations()
        {
            return View();
        }
    }
}