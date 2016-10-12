using System.Linq;
using Core.Entities;

namespace BusinessLogic.Repositories
{
    public class BookRepository : Repository<Book>
    {
        public BookRepository(DbDataContext context) : base(context)
        {
            _context = context;
        }

        public Book Get(int id)
        {
            var query = from a in _context.Books
                        where a.Id == id
                        orderby a.Name
                        select a;

            return query.ToList()[0];
        }

        private readonly DbDataContext _context;
    }
}
