using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticulosController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _dbContext;

        public ArticulosController(IUnitOfWork unitOfWork, ApplicationDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }


        public IActionResult Index()
        {
            return View();
        }

        #region
        [HttpGet]
        public IActionResult GetAll()
        {

            return Json(new { Data = _unitOfWork._articuloRepository.GetAll() });
        }


        #endregion
    }
}
