using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class _3BookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly string _connectionString = "constr";

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks(string searchKeyword = null, string sortColumn = "Id", string sortOrder = "ASC", int pageSize = 10, int pageNumber = 1)
        {
            var books = new List<Book>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "GetBooks";
                    command.Parameters.Add(new SqlParameter("@SearchKeyword", searchKeyword ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@SortColumn", sortColumn));
                    command.Parameters.Add(new SqlParameter("@SortOrder", sortOrder));
                    command.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                    command.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var book = new Book
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                ISBN = reader.GetString(2),
                                Author = reader.GetString(3),
                                Description = reader.GetString(4),
                                Price = reader.GetInt32(5)
                            };
                            books.Add(book);
                        }
                    }
                }
            }

            return books;
        }

    }
}