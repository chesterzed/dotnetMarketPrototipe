using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFirstProject.Domain;

namespace MyFirstProject.Controllers.Admin
{
    [Authorize(Roles ="admin")]
    public partial class AdminController : Controller
    {
        private readonly DataManager _dataManager;

        public AdminController(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
