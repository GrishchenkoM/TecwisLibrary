using System.Collections.Generic;

namespace Core.Entities
{
    public class Author : EntityBase
    {
        public Author()
        {
            Books = new List<Book>();
        }
        public ICollection<Book> Books { get; set; }
    }
}
