using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using CM.Services;
using CM.Services.interfaces.Abstract;

namespace Cash_Machine.Controllers
{
    public class AtmController : Controller
    {
        private readonly ICardHandler _cardService;
        public AtmController() //ICardHandler handler
        {
          //  DiResolver.GetContainer().Resolve<ICardHandler>();
            _cardService = DiResolver.GetContainer().Resolve<ICardHandler>();
        }

        [HttpGet]
        public ViewResult EnterAtm()
        {
            return View();
        }

        [HttpPost]
        public JsonResult EnterAtm(string cardNumber)
        {
            if (_cardService.CheckCard(cardNumber))
            {
                Session["CardNumber"] = cardNumber;
                // return RedirectToAction("PinCodeCheck");
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpGet]
        public ActionResult PinCodeCheck()
        {
            var cardNumber = Session["CardNumber"] as string;
            if (cardNumber != null)
            {
                Session["AttemptsNumber"] = 4;
                return View();
            }
            return new HttpNotFoundResult("404");
        }

        [HttpPost]
        public JsonResult PinCodeCheck(string pinCode)
        {
            string cardNumber = Session["CardNumber"] as string;
            int? attemptsNum = (int)Session["AttemptsNumber"];

            if(cardNumber==null) return Json(new { error = true });

            if (_cardService.CheckPinCode(pinCode, cardNumber)) //_cardService.CheckPinCode(pinCode, cardNumber)
            {
               //return RedirectToAction("GetOperations", "Atm");
               return Json(new { success = true });
            }
               
            attemptsNum--;
            Session["AttemptsNumber"] = attemptsNum;
            if (attemptsNum == 0)
            {
                // return RedirectToAction("CardIsBlockedError", "Atm");
                Session.Clear();
                _cardService.BlockCard(cardNumber);
                return Json(new { blocked = true });
            }

            return Json(new { success = false , attempts = attemptsNum });
            //return RedirectToAction("PinCodeCheck", "Atm", new { cardNumber = cardNumber });
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

        [HttpGet]
        public ViewResult CardIsBlockedError()
        {
            return View();
        }
    }
}