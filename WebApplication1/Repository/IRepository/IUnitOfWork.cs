namespace WebApplication1.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public IBookRepository Book { get; }
        void Save();

    }
}
