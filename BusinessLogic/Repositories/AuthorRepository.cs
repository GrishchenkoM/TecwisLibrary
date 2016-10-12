using System.Linq;
using Core.Entities;

namespace BusinessLogic.Repositories
{
    public class AuthorRepository : Repository<Author>
    {
        public AuthorRepository(DbDataContext context) : base(context)
        {
            _context = context;
        }

        public Author Get(int id)
        {
            var query = from a in _context.Authors
                        where a.Id == id
                        orderby a.Name
                        select a;

            return query.ToList()[0];
        }

        private readonly DbDataContext _context;
    }
}
