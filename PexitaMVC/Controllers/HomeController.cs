using Microsoft.AspNetCore.Mvc;
using PexitaMVC.Application.DTOs;
using PexitaMVC.Application.Exceptions;
using PexitaMVC.Application.Interfaces;
using PexitaMVC.Core.Entites;
using System.Diagnostics;
using System.Security.Claims;
namespace PexitaMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBillService _billService;
        private readonly IUserService _userService;
        private readonly IPaymentService _paymentService;

        public HomeController(IBillService billService, IUserService userService, IPaymentService paymentService)
        {
            _billService = billService;
            _userService = userService;
            _paymentService = paymentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> BillList(string UserID)
        {
            List<BillDTO> result = (await _userService.GetAllBillsForUserAsync(UserID)).ToList();

            return View(result);
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult CreateBill()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserId = userId;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBillAction(BillCreateDTO createDTO)
        {
            if (ModelState.IsValid)
            {
                var newBill = await _billService.AddBillAsync(createDTO);
                return Ok(newBill);
            }
            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBillAction(int BillID)
        {
            try
            {
                await _billService.DeleteBillAsync(BillID);

                return Ok();
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PayBillAction(int paymentID)
        {
            try
            {
                var result = await _paymentService.Pay(paymentID);
                return Ok(result);
            }

            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
