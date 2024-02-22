using WebApplication1.Data;
using WebApplication1.Repository.IRepository;

namespace WebApplication1.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Book = new BookRepository(context);
        }
        public IBookRepository Book { private set; get; }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
