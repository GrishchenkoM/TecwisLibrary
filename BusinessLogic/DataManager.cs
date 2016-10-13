using BusinessLogic.Repositories;

namespace BusinessLogic
{
    public interface IDataManager
    {
        AuthorRepository Authors { get; }
        BookRepository Books { get; }
    }
    
    public class DataManager : IDataManager
    {
        public DataManager(
            AuthorRepository authorRepository,
            BookRepository bookRepository, 
            DbDataContext context)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _context = context;
        }

        public AuthorRepository Authors => _authorRepository ?? new Authors(_context);
        public BookRepository Books => _bookRepository ?? new Books(_context);

        private readonly AuthorRepository _authorRepository;
        private readonly BookRepository _bookRepository;
        private readonly DbDataContext _context;
    }
}