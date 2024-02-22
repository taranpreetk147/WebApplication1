using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Repository.IRepository;
using System.Linq.Dynamic.Core;
using static System.Reflection.Metadata.BlobBuilder;

namespace WebApplication1.Controllers
{
    public class BookController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        public BookController(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }
        public IActionResult Index()
        {
            //var data = _unitOfWork.Book.GetAll();
            return View();
        }
        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    var bookList = _unitOfWork.Book.GetAll();
        //    return Json(new { data = bookList });
        //}
        #region APIs
        [HttpPost]
        public IActionResult GetAll()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var bookData = (from Books in _context.Books select Books);
                if (!(string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection)))
                {
                    bookData = bookData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    bookData = bookData.Where(m => m.Name.Contains(searchValue)
                                                || m.ISBN.Contains(searchValue)
                                                || m.Author.Contains(searchValue)
                                                || m.Description.Contains(searchValue)
                                                || m.Price.ToString().Contains(searchValue)
                                                );
                }
                recordsTotal = bookData.Count();
                var data = bookData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception )
            {
                throw;
            }
        }
        #endregion

    }
}
