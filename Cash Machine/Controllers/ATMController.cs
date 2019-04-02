using System;
using System.Web.Mvc;
using CM.Entites;
using CM.Services.interfaces;

namespace Cash_Machine.Controllers
{
    public class AtmController : Controller
    {
        private readonly ICard _cardService;
        private static string _cardNumber;

        public AtmController(ICard handler)
        {
            _cardService = handler;
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
                _cardNumber = cardNumber;
                Session["CardChecksOut"] = true;
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpGet]
        public ActionResult PinCodeCheck()
        {
            if (Session["CardChecksOut"] is bool )
            {
                Session["PinCodeCheck"] = true;
                return View();
            }
            return new HttpNotFoundResult("404");
        }

        [HttpPost]
        public JsonResult PinCodeCheck(string pinCode)
        {
            if((Session["PinCodeCheck"] is null)) return Json(new { error = true });

            string cardNumber = _cardNumber; 

            int attemptsNum = _cardService.GetAttemptsNumber(cardNumber);

            if(cardNumber==null) return Json(new { error = true });

            if (_cardService.CheckPinCode(pinCode, cardNumber)) 
            {
                _cardService.UpdateAttemptsNumber(cardNumber, 4);
                Session["PinCodeCorrect"] = true;
                return Json(new { success = true });
            }
               
            attemptsNum--;
            _cardService.UpdateAttemptsNumber(cardNumber, attemptsNum);
            if (attemptsNum == 0)
            {
                return Json(_cardService.BlockCard(cardNumber) ? new { blocked = true } : new { blocked = false });
            }

            return Json(new { success = false , attempts = attemptsNum });
        }

        [HttpGet]
        public ActionResult CardError()
        {
           return View();
        }

        [HttpGet]
        public ActionResult GetOperations()
        {
            if (Session["PinCodeCorrect"] is bool)
            {
                return View();
            }
            return new HttpNotFoundResult("404");
        }

        [HttpPost]
        public JsonResult GetOperations(OperationType operation)
        {
            if (Session["PinCodeCorrect"] is bool)
            {
                var cardNumber = _cardNumber; 
                if (operation == OperationType.Balance)
                {
                    _cardService.RegisterOperation(cardNumber, operation);
                    return Json(new { });
                }
            }
            return Json(new { });
        }

        [HttpGet]
        public ActionResult CardIsBlockedError()
        {
            if (Session["CardChecksOut"] is bool)
            {
                Session.Clear();
                return View();
            }
            return new HttpNotFoundResult("404");
        }

        [HttpGet]
        public ActionResult Balance()
        {
            if (Session["PinCodeCorrect"] is bool)
            {
                var cardNumber = _cardNumber;
                ViewBag.CardNumber =string.Format($"{cardNumber?.Substring(0, 4)}-{cardNumber?.Substring(4, 4)}-{cardNumber?.Substring(8, 4)}-{cardNumber?.Substring(12, 4)}");
                ViewBag.Date = DateTime.Now.Date.ToString("d");
                ViewBag.Balance = (_cardService.GetBalance(cardNumber).ToString("C"));
                return View();
            }
            return new HttpNotFoundResult("404");
        }

        [HttpGet]
        public ActionResult Withdraw()
        {
            if (Session["PinCodeCorrect"] is bool)
            {
                return View();
            }
            return new HttpNotFoundResult("404");
        }

        [HttpPost]
        public JsonResult Withdraw(int sum)
        {
            if (Session["PinCodeCorrect"] is bool)
            {
                var cardNumber = _cardNumber; 
                if (sum > 0)
                {
                    if (_cardService.Withdraw(cardNumber, sum))
                    return Json(new {success = true, sum });
                }

                return Json(new {success = false});
            }
            return Json(new { });
        }

        [HttpGet]
        public ActionResult WithdrawError()
        {
            if (Session["PinCodeCorrect"] is bool)
            {
                return View();
            }
            return new HttpNotFoundResult("404");
        }

        public ActionResult WithdrawReport(int sum)
        {
            if (Session["PinCodeCorrect"] is bool)
            {
                var cardNumber = _cardNumber; 
                ViewBag.CardNumber = string.Format($"{cardNumber?.Substring(0, 4)}-{cardNumber?.Substring(4, 4)}-{cardNumber?.Substring(8, 4)}-{cardNumber?.Substring(12, 4)}");
                ViewBag.Sum = sum.ToString("C");
                ViewBag.Date = DateTime.Now.Date.ToString("d");
                ViewBag.MoneyLeft = (_cardService.GetBalance(cardNumber).ToString("C"));
                return View();
            }
            return new HttpNotFoundResult("404");
        }

    }
}