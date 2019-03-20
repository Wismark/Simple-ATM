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
        private CardHandler _cardService;
        public AtmController(CardHandler cardH)
        {
            _cardService = cardH;
        }

        [HttpGet]
        public ActionResult EnterAtm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EnterAtm(string cardNumber)
        {        
            if (_cardService.CheckCard(cardNumber))
            {
                return RedirectToAction("");
            }
            else
            {
                return View("Error");
            }
        }



    }
}