using System;
using System.Web.Http;
using BusinessLogic;
using Web.Models.EntityModels;

namespace Web.Controllers
{
    public class BookController : BaseApiController<BookModelFactory>
    {
        public BookController(IDataManager dataManager) : base(dataManager)
        { }

        public IHttpActionResult Get()
        {
            try
            {
                var model = DataManager.Books.Get();
                if (model != null && model.Count > 0)
                    return Ok(model);

                return InternalServerError(new Exception("Database is empty."));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult Post([FromBody] BookModel model)
        {
            try
            {
                var entity = ModelFactory.Create(model);
                var createdEntity = DataManager.Books.Create(entity);

                if (createdEntity != null)
                {
                    ModelFactory.CurrentId = createdEntity.Id;
                    ModelFactory.RequestMessage = Request;

                    var entityModel = ModelFactory.Create(createdEntity);

                    return Created(entityModel.Url, entityModel);
                }

                return InternalServerError(new Exception("Recording has not created"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult Put([FromBody] BookModel model)
        {
            try
            {
                var entity = ModelFactory.Create(model);
                var updatedEntity = DataManager.Books.Update(entity);

                if (updatedEntity != null)
                {
                    var entityModel = ModelFactory.Create(updatedEntity);
                    return Ok(entityModel);
                }

                return InternalServerError(new Exception("Recording has not created"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult Delete(int id)
        {
            try
            {
                var entity = DataManager.Books.Get(id);
                if (entity != null)
                    DataManager.Books.Delete(entity);
                else
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
