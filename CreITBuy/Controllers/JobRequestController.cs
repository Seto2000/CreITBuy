using CreITBuy.Core.Contracts;
using CreITBuy.Core.ViewModels.JobRequest;
using CreITBuy.Infrastructure.Data.Common;
using CreITBuy.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CreITBuy.Controllers
{
    public class JobRequestController : Controller
    {
        UserManager<User> userManager;
        IJobRequestService jobRequestService;
        private readonly IUserService userService;
        
        public JobRequestController(UserManager<User> _userManager,
            IJobRequestService _jobRequestService,
            IUserService _userService)

        {
            userService = _userService;
            userManager = _userManager;
            jobRequestService = _jobRequestService;
        }
        public async Task<IActionResult> AddJobRequest(string toUserId)
        {
            ViewData["IsAuthenticated"] = HttpContext.User.Identity.IsAuthenticated;
            ViewData["viewName"] = "Add";
            ViewData["controlerName"] = "Job Request";
            ViewData["toUserId"] = toUserId;
            User user = userService.FindUserByName(User.Identity.Name);
            ViewData["User"] = user;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddJobRequest(string toUserId, JobRequestViewModel model)
        {
            (bool isAdded, string errrors) = jobRequestService
                .AddRequest(await userManager.FindByNameAsync(User.Identity.Name),
                await userManager.FindByIdAsync(toUserId),
                model);

            if (isAdded)
            {
                return Redirect("/");
            }
            else
            {
                ViewData["Errors"] = errrors.Split("; ",StringSplitOptions.RemoveEmptyEntries).ToList();
                return View("Error");
            }
        }
        public IActionResult Remove(string requestId)
        {
            (bool isRemoved, string errors) = jobRequestService.Remove(requestId);
            if (isRemoved)
            {
                return Redirect("/");
            }
            ViewData["Errors"] = new List<string>(errors.Split("; ").ToList());
            return View("Error");
        }
        public IActionResult Details(string requestId)
        {
            ViewData["IsAuthenticated"] = HttpContext.User.Identity.IsAuthenticated;
            ViewData["viewName"] = "Details";
            ViewData["controlerName"] = "Job Request";
            User user = userService.FindUserByName(User.Identity.Name);
            ViewData["User"] = user;
            ViewData["Item"] = user.JobRequests.SingleOrDefault(x=>x.Id == requestId);
            return View();
        }

    }
}
