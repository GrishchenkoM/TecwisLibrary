using System.Net.Http;
using System.Web.Http.Routing;
using Core.Entities;
using Web.Models.EntityModels.Interfaces;

namespace Web.Models.EntityModels
{
    public class AuthorModelFactory 
        : IModelFactory<Author, AuthorModel>
    {
        public AuthorModel Create(Author unit)
        {
            var model = new AuthorModel()
            {
                Id = unit.Id,
                Name = unit.Name
            };

            if (CurrentId != -1 && RequestMessage != null)
                model.Url = new UrlHelper(RequestMessage).Link("Default", new {id = CurrentId});

            return model;
        }

        public Author Create(AuthorModel model)
        {
            return new Author()
            {
                Name = model.Name
            };
        }

        public int CurrentId { get; set; } = -1;

        public HttpRequestMessage RequestMessage { get; set; }
    }

    public class AuthorModel : IModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
    }
}