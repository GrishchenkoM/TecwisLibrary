using BusinessLogic.Repositories;
using Core.Entities;

namespace BusinessLogic
{
    public interface IDataManager
    {
        Repository<Author> AuthorRepository { get; }
        Repository<Book> BookRepository { get; }
    }
    
    public class DataManager : IDataManager
    {
        public DataManager(
            Repository<Author> authorRepository, 
            Repository<Book> bookRepository, 
            DbDataContext context)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _context = context;
        }

        public Repository<Author> AuthorRepository => _authorRepository ?? new AuthorRepository(_context);
        public Repository<Book> BookRepository => _bookRepository ?? new BookRepository(_context);

        private readonly Repository<Author> _authorRepository;
        private readonly Repository<Book> _bookRepository;
        private readonly DbDataContext _context;
    }
}