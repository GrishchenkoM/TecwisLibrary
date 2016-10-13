using System.Web.Http;
using BusinessLogic;
using Web.Models.EntityModels.Interfaces;

namespace Web.Controllers
{
    public class BaseApiController<TModelFactory> 
        : ApiController where TModelFactory : IModelFactory, new()
    {
        public BaseApiController(IDataManager dataManager)
        {
            DataManager = dataManager;
            ModelFactory = new TModelFactory();
        }

        public IDataManager DataManager { get; }
        public TModelFactory ModelFactory { get; }
    }
}
