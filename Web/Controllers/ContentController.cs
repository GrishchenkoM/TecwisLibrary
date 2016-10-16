using System;
using System.Collections;
using System.Linq;
using System.Web.Http;
using BusinessLogic;
using Core.Entities;
using Web.Models;

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
                ArrayList list;

                var books = DataManager.Books.Get();
                var authors = DataManager.Authors.Get();
                list = new ArrayList() {books, authors};
                
                var model = ModelFactory.Create(list);
                if (model.ContentModels != null && model.ContentModels.Count > 0)
                    return Ok(model);

                return InternalServerError(new Exception("Database is empty."));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [Route("api/content/GetBooks")]
        public IHttpActionResult GetBooks(int? index)
        {
            try
            {
                ArrayList list = null;
                if (index != null)
                {
                    var book = DataManager.Books.Get((int)index);
                    var author = DataManager.Authors.Get(book.AuthorId);
                    list = new ArrayList() { book, author };
                }

                var model = ModelFactory.Create(list);

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

        public IHttpActionResult Put([FromBody] ContentModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Author authorModel = null;
            Book bookModel = null;

            try
            {
                var list = ModelFactory.Create(new ViewModel() { ContentModel = model });

                foreach (var unit in list)
                {
                    if (unit is Book)
                        bookModel = unit as Book;
                    if (unit is Author)
                        authorModel = unit as Author;
                }

                if (authorModel != null)
                {
                    authorModel = authorModel.Id == -1
                        ? DataManager.Authors.Create(authorModel)
                        : DataManager.Authors.Get().Where(x => x.Name.Equals(authorModel.Name)).ToList()[0];

                    if (bookModel != null)
                    {
                        bookModel.AuthorId = authorModel.Id;
                        bookModel = DataManager.Books.Update(bookModel);

                        var entityModel = ModelFactory.Create(new ArrayList() { bookModel, authorModel });
                        return Ok(entityModel);
                    }
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
