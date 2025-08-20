using CoreLayer.Models;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.Branch.Controllers
{

    [Area("Branch")]
    [Authorize]
    public class BranchController : Controller
    {

        private readonly IUnitOfWork _UnitOfWork;


        public BranchController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var branches = _UnitOfWork.Branches.GetAsync(include: [x => x.BranchManager]);

            return View(branches);
        }

        public IActionResult Save(int? Id)
        {

            var branch = _UnitOfWork.Branches.GetAsync(x => x.Id == Id);

            if (branch is not null) { 
            
            
                return View(branch);
            }

            return View();
        }

        //public IActionResult Save(int? Id)
        //{

        //    var branch = _UnitOfWork.Branches.GetAsync(x => x.Id == Id);

        //    if (branch is not null)
        //    {


        //        return View(branch);
        //    }

        //    return View();
        //}

        public IActionResult Delete()
        {
            return View();
        }
    }
}
