using System;
using System.Web.Mvc;
using CM.Entites.Entities;
using CM.Services.interfaces.Abstract;

namespace Cash_Machine.Controllers
{
    public class AtmController : Controller
    {
        private readonly ICardHandler _cardService;
        public AtmController(ICardHandler handler)
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
                Session["CardNumber"] = cardNumber;
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpGet]
        public ActionResult PinCodeCheck()
        {
            if (Session["CardNumber"] is string )
            {
                Session["AttemptsNumber"] = 4;
                return View();
            }
            return new HttpNotFoundResult("404");
        }

        [HttpPost]
        public JsonResult PinCodeCheck(string pinCode)
        {
            if((Session["AttemptsNumber"] is null)) return Json(new { error = true });

            string cardNumber = Session["CardNumber"] as string;
            
            int attemptsNum = (int)Session["AttemptsNumber"];

            if(cardNumber==null) return Json(new { error = true });

            if (_cardService.CheckPinCode(pinCode, cardNumber)) 
            {
                Session["PinCodeCorrect"] = true;
                return Json(new { success = true });
            }
               
            attemptsNum--;
            Session["AttemptsNumber"] = attemptsNum;
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
                var cardNumber = Session["CardNumber"] as string;
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
            if (Session["CardNumber"] is string)
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
                var cardNumber = Session["CardNumber"] as string;
                ViewBag.CardNumber =string.Format($"{cardNumber.Substring(0, 4)}-{cardNumber.Substring(4, 4)}-{cardNumber.Substring(8, 4)}-{cardNumber.Substring(12, 4)}");
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
                var cardNumber = Session["CardNumber"] as string;
                if (sum > 0)
                {
                    if (_cardService.Withdraw(cardNumber, sum))
                    return Json(new {success = true, sum = sum  });
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
                var cardNumber = Session["CardNumber"] as string;
                ViewBag.CardNumber = string.Format($"{cardNumber.Substring(0, 4)}-{cardNumber.Substring(4, 4)}-{cardNumber.Substring(8, 4)}-{cardNumber.Substring(12, 4)}");
                ViewBag.Sum = sum.ToString("C");
                ViewBag.Date = DateTime.Now.Date.ToString("d");
                ViewBag.MoneyLeft = (_cardService.GetBalance(cardNumber).ToString("C"));
                return View();
            }
            return new HttpNotFoundResult("404");
        }

    }
}