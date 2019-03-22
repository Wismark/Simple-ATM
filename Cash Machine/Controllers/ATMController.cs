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
        public ActionResult EnterAtm(string cardNumber)
        {
            if (_cardService.CheckCard(cardNumber))
            {
                Session["CardNumber"] = cardNumber;
                return RedirectToAction("PinCodeCheck");
            }

            return View("CardError");
        }

        [HttpGet]
        public ViewResult PinCodeCheck()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PinCodeCheck(string pinCode)
        {
            string cardNumber = Session["CardNumber"] as string;

            if (_cardService.CheckPinCode(pinCode, cardNumber)) //_cardService.CheckPinCode(pinCode, cardNumber)
            {
               return RedirectToAction("GetOperations", "Atm");
            }

            if (_cardService.CheckOnAttempts(cardNumber) == 0)
                return RedirectToAction("CardIsBlockedError", "Atm");

            return RedirectToAction("PinCodeCheck", "Atm", new { cardNumber = cardNumber });
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