using Core.Entities;
using Web.Models.EntityModels.Interfaces;

namespace Web.Models.EntityModels
{
    public class AuthorModelFactory 
        : IModelFactory<Author, AuthorModel>
    {
        public AuthorModel Create(Author unit)
        {
            return new AuthorModel()
            {
                Id = unit.Id,
                Name = unit.Name
            };
        }

        public Author Create(AuthorModel model)
        {
            return new Author()
            {
                Name = model.Name
            };
        }
    }

    public class AuthorModel : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}