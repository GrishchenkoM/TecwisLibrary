using Core.Entities;
using Web.Models.EntityModels.Interfaces;

namespace Web.Models.EntityModels
{
    public class BookModelFactory
        : IModelFactory<Book, BookModel>
    {
        public BookModel Create(Book unit)
        {
            return new BookModel()
            {
                Id = unit.Id,
                Name = unit.Name,
                Year = unit.Year,
                AuthorId = unit.AuthorId
            };
        }

        public Book Create(BookModel model)
        {
            return new Book()
            {
               Name = model.Name,
               Year = model.Year,
               AuthorId = model.AuthorId
            };
        }
    }

    public class BookModel : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public int Year { get; set; }
    }
}