using Core.Entities;

namespace BusinessLogic.Repositories
{
    public class AuthorRepository : Repository<Author>
    {
        public AuthorRepository(DbDataContext context) : base(context)
        { }
    }
}
