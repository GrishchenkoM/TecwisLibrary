using System.Net.Http;
using System.Web.Http.Routing;
using Core.Entities;
using Web.Models.EntityModels.Interfaces;

namespace Web.Models.EntityModels
{
    public class BookModelFactory
        : IModelFactory<Book, BookModel>
    {
        public BookModel Create(Book unit)
        {
            var model = new BookModel()
            {
                Id = unit.Id,
                Name = unit.Name,
                Year = unit.Year,
                AuthorId = unit.AuthorId ?? (int)State.Empty
            };

            if (CurrentId != (int)State.Empty && RequestMessage != null)
                model.Url = new UrlHelper(RequestMessage).Link("Default", new { id = CurrentId });

            return model;
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

        public int CurrentId { get; set; } = (int)State.Empty;
        public HttpRequestMessage RequestMessage { get; set; }
    }

    public class BookModel : IModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public int Year { get; set; }
    }
}