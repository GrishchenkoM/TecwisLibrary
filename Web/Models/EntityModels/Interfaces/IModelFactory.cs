using System.Net.Http;

namespace Web.Models.EntityModels.Interfaces
{
    public interface IModelFactory<TEntity, TModel> : IModelFactory where TModel : IModel
    {
        TModel Create(TEntity unit);
        TEntity Create(TModel model);
    }

    public interface IModelFactory
    {
        int CurrentId { get; set; }

        HttpRequestMessage RequestMessage { get; set; }
    }

    public interface IModel
    {
        string Url { get; set; }
    }
}