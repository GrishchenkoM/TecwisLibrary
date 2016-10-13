using Core.Entities;

namespace Web.Models.EntityModels.Interfaces
{
    public interface IModelFactory<TEntity, TModel> : IModelFactory
        where TEntity : EntityBase
        where TModel : IModel
    {
        TModel Create(TEntity unit);
        TEntity Create(TModel model);
    }

    public interface IModelFactory { }

    public interface IModel { }
}