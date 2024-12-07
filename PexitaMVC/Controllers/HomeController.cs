using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        [Authorize]
        public IActionResult CreateBill()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserId = userId;

            return View();
        }

        [Authorize]
        public async Task<IActionResult> BillTable(bool getUnpaidBills = false)
        {
            try
            {
                string UserID = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new ArgumentException($"User Not Logged In");
                List<BillDTO> result;

                if (getUnpaidBills)
                    result = (await _userService.GetUnpaidBillsForUserAsync(UserID)).ToList();
                else
                    result = (await _userService.GetAllBillsForUserAsync(UserID)).ToList();

                ViewBag.UserId = UserID;
                return PartialView("BillTable", result);
            }
            catch (ArgumentException e)
            {
                return Unauthorized(e.Message);
            }
            catch (Exception e)
            {
                DebugError error = new() { Message = e.Message, StackTrace = e.StackTrace ?? "", InnerException = e.InnerException?.ToString() ?? "" };
                return BadRequest(error);
            }
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateBillAction([FromBody] BillCreateDTO createDTO)
        {
            try
            {
                var newBill = await _billService.AddBillAsync(createDTO);
                return Ok(newBill);
            }

            catch (Exception e)
            {
                DebugError error = new() { Message = e.Message, StackTrace = e.StackTrace ?? "", InnerException = e.InnerException?.ToString() ?? "" };
                return StatusCode(500, error);
            }
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteBillAction([FromBody] int BillID)
        {
            try
            {
                await _billService.DeleteBillAsync(BillID);

                return Ok("The Bill Was Deleted Successfully.");
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                DebugError error = new() { Message = e.Message, StackTrace = e.StackTrace ?? "", InnerException = e.InnerException?.ToString() ?? "" };
                return StatusCode(500, error);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PayBillAction([FromBody] int paymentID)
        {
            try
            {
                var result = await _paymentService.PayAsync(paymentID);
                return Ok(result);
            }

            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                DebugError error = new() { Message = e.Message, StackTrace = e.StackTrace ?? "", InnerException = e.InnerException?.ToString() ?? "" };
                return StatusCode(500, error);
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
