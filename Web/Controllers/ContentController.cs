using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessLogic;
using Core.Entities;
using Web.Models;
using Web.Models.EntityModels;

namespace Web.Controllers
{
    public class ContentController : BaseApiController<ContentModelFactory>
    {
        public ContentController(IDataManager dataManager) : base(dataManager)
        { }

        [Route("api/content/GetBooks")]
        public IHttpActionResult GetBooks()
        {
            try
            {
                var books = DataManager.Books.Get();
                var authors = DataManager.Authors.Get();
                
                var model = ModelFactory.Create(new ArrayList() { books, authors });
                
                if (model.ContentModels != null && model.ContentModels.Count > 0)
                    return Ok(model);

                return InternalServerError(new Exception("Database is empty."));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/content/GetAuthors")]
        public IHttpActionResult GetAuthors()
        {
            try
            {
                var authors = DataManager.Authors.Get();

                var model = ModelFactory.Create(new ArrayList() { authors });

                if (model.ContentModels != null && model.ContentModels.Count > 0)
                    return Ok(model);

                return InternalServerError(new Exception("Database is empty."));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult Post([FromBody] ContentModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Author author = null;
            Book book = null;

            try
            {
                var list = ModelFactory.Create(new ViewModel() {ContentModel = model});

                foreach (var unit in list)
                {
                    if (unit is Book)
                        book = unit as Book;
                    if (unit is Author)
                        author = unit as Author;
                }
                if (author != null)
                {
                    author = author.Id == -1 
                        ? DataManager.Authors.Create(author) 
                        : DataManager.Authors.Get().Where(x => x.Name.Equals(author.Name)).ToList()[0];

                    if (book != null)
                    {
                        book.AuthorId = author.Id;
                        book = DataManager.Books.Create(book);
                    
                        var entityModel = ModelFactory.Create(new ArrayList() {book, author});
                        return Created($"/api/book/{book.Id}", entityModel);
                    }
                }

                return InternalServerError(new Exception("Recording has not created"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //public IHttpActionResult Put([FromBody] AuthorModel model)
        //{
        //    try
        //    {
        //        var entity = ModelFactory.Create(model);
        //        var updatedEntity = DataManager.Authors.Update(entity);

        //        if (updatedEntity != null)
        //        {
        //            var entityModel = ModelFactory.Create(updatedEntity);
        //            return Ok(entityModel);
        //        }

        //        return InternalServerError(new Exception("Recording has not created"));
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}

        //public IHttpActionResult Delete(int id)
        //{
        //    try
        //    {
        //        var entity = DataManager.Authors.Get(id);
        //        if (entity != null)
        //            DataManager.Authors.Delete(entity);
        //        else
        //            return NotFound();

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}
    }
}
