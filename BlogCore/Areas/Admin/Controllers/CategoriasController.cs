using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriasController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _dbContext;


        public CategoriasController(IUnitOfWork unitOfWork, ApplicationDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork._categoriaRepository.Add(categoria);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Categoria categoria = new Categoria();
            categoria = _unitOfWork._categoriaRepository.Get(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork._categoriaRepository.Update(categoria);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        #region
        [HttpGet]
        public IActionResult GetAll()
        {

            return Json(new { Data = _unitOfWork._categoriaRepository.GetAll() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDeb = _unitOfWork._categoriaRepository.Get(id);
            if (objFromDeb == null)
            {
                return Json(new { success = false, message = "Error borrando categoria" });
            }
            _unitOfWork._categoriaRepository.Remove(objFromDeb);
            return Json(new { success = true, message = "Categoria borrada" });
        }

        #endregion

    }
}
