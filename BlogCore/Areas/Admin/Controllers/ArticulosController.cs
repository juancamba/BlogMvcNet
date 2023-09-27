using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticulosController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ArticulosController(IUnitOfWork unitOfWork, ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public IActionResult Create()
        {
            ArticuloVM articuloVM = new ArticuloVM()
            {
                Articulo = new Articulo(),
                ListaCategorias = _unitOfWork._categoriaRepository.GetListaCategorias()
            };

            return View(articuloVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ArticuloVM articuloVm)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _webHostEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                if (articuloVm.Articulo.Id == 0)
                {
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                    var extension = Path.GetExtension(archivos[0].FileName);

                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    articuloVm.Articulo.UrlImagen = @"\imagenes\articulos\" + nombreArchivo + extension;
                    articuloVm.Articulo.FechaCreacion = DateTime.Now;
                    _unitOfWork._articuloRepository.Add(articuloVm.Articulo);
                    _unitOfWork.Save();

                    return RedirectToAction(nameof(Index));
                }

            }
            // si no pasa el modelo, se le pasa la lista de categorias
            articuloVm.ListaCategorias = _unitOfWork._categoriaRepository.GetListaCategorias();
            return View(articuloVm);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ArticuloVM articuloVM = new ArticuloVM()
            {
                Articulo = new Articulo(),
                ListaCategorias = _unitOfWork._categoriaRepository.GetListaCategorias()
            };
            if (id != null)
            {
                articuloVM.Articulo = _unitOfWork._articuloRepository.Get(id.GetValueOrDefault());
            }
            return View(articuloVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ArticuloVM articuloVm)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _webHostEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                var articuloDesdeDb = _unitOfWork._articuloRepository.Get(articuloVm.Articulo.Id);

                if (archivos.Count() > 0)
                {
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    var nuevaExtension = Path.GetExtension(archivos[0].FileName);
                    var rutaImagen = Path.Combine(rutaPrincipal, articuloDesdeDb.UrlImagen.TrimStart('\\'));
                    // si existe la imagen, la borra
                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }
                    //vuelve a subir la imagen
                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    articuloVm.Articulo.UrlImagen = @"imagenes\articulos\" + nombreArchivo + extension;
                    articuloVm.Articulo.FechaCreacion = DateTime.Now;
                    _unitOfWork._articuloRepository.Update(articuloVm.Articulo);
                    _unitOfWork.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    articuloVm.Articulo.UrlImagen = articuloDesdeDb.UrlImagen;
                }
                _unitOfWork._articuloRepository.Update(articuloVm.Articulo);
                _unitOfWork.Save();
            }
            // si no pasa el modelo, se le pasa la lista de categorias
            articuloVm.ListaCategorias = _unitOfWork._categoriaRepository.GetListaCategorias();
            return View(articuloVm);
        }


        #region
        [HttpGet]
        public IActionResult GetAll()
        {

            return Json(new { Data = _unitOfWork._articuloRepository.GetAll(includeProperties: "Categoria") });
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {

            var articuloFromDb = _unitOfWork._articuloRepository.Get(id);
            string rutaDirectorioPrincipal = _webHostEnvironment.WebRootPath;
            var rutaImagen = Path.Combine(rutaDirectorioPrincipal, articuloFromDb.UrlImagen.TrimStart('\\'));

            if (System.IO.File.Exists(rutaImagen))
            {
                System.IO.File.Delete(rutaImagen);
            }
            if (articuloFromDb == null)
            {

                return Json(new { success = false, message = "Error borrando articulo" });
            }


            _unitOfWork._articuloRepository.Remove(articuloFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Articulo borrado" });
        }


        #endregion
    }
}
