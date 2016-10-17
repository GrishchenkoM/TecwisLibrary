using System;
using System.Collections;
using System.Collections.Generic;
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
            try{
                var books = DataManager.Books.Get();
                var authors = DataManager.Authors.Get();

                
                var withBooks = from b in books
                    join a in authors on b.AuthorId equals a.Id
                    group a by a.Name
                    into newGroup
                    orderby newGroup.Key  
                    select newGroup;

                var model = new ViewModel();
                foreach (var group in withBooks)
                {
                    var queue = new Dictionary<Author, int>();
                    foreach (var a in group)
                    {
                        if (queue.ContainsKey(a))
                            queue[a] = ++queue[a];
                        else
                            queue.Add(a, 1);
                    }
                    foreach (var q in queue)
                    {
                        model.ContentModels.Add(new ContentModel()
                        {
                            AuthorId = q.Key.Id,
                            AuthorName = q.Key.Name,
                            Count = q.Value
                        });
                    }
                    
                }

                var withoutBooks = from a in authors
                                   join b in books on a.Id equals b.AuthorId  into ps
                                   from p in ps.DefaultIfEmpty() 
                                   select new 
                                   {
                                       AuthorId = a.Id,
                                       AuthorName = a.Name,
                                       BookId = p == null ? -1 : p.Id
                                   };
                
                var wb = withoutBooks.Where(x => x.BookId == -1);
                foreach (var a in wb)
                {
                    model.ContentModels.Add(new ContentModel()
                    {
                        AuthorId = a.AuthorId,
                        AuthorName = a.AuthorName,
                        Count = 0
                    });
                }


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
        public IHttpActionResult GetAuthors(int? index)
        {
            try
            {
                if (index != null)
                {
                    var books = DataManager.Books.Get();
                    var authors = DataManager.Authors.Get();

                    var withBooks = from b in books
                                    join a in authors on b.AuthorId equals a.Id
                                    group a by a.Name
                    into newGroup
                                    orderby newGroup.Key
                                    select newGroup;

                    var model = new ViewModel();
                    foreach (var group in withBooks)
                    {
                        var queue = new Dictionary<Author, int>();
                        foreach (var a in group)
                        {
                            if (queue.ContainsKey(a))
                                queue[a] = ++queue[a];
                            else
                                queue.Add(a, 1);
                        }
                        foreach (var q in queue)
                        {
                            model.ContentModels.Add(new ContentModel()
                            {
                                AuthorId = q.Key.Id,
                                AuthorName = q.Key.Name,
                                Count = q.Value
                            });
                        }

                    }

                    var withoutBooks = from a in authors
                                       join b in books on a.Id equals b.AuthorId into ps
                                       from p in ps.DefaultIfEmpty()
                                       select new
                                       {
                                           AuthorId = a.Id,
                                           AuthorName = a.Name,
                                           BookId = p == null ? -1 : p.Id
                                       };

                    var wb = withoutBooks.Where(x => x.BookId == -1);
                    foreach (var a in wb)
                    {
                        model.ContentModels.Add(new ContentModel()
                        {
                            AuthorId = a.AuthorId,
                            AuthorName = a.AuthorName,
                            Count = 0
                        });
                    }

                    var newModel = new ViewModel();
                    newModel.ContentModels.Add(model.ContentModels.FirstOrDefault(a => a.AuthorId == index));


                    if (newModel.ContentModels != null && newModel.ContentModels.Count > 0)
                        return Ok(newModel);
                }

                return InternalServerError(new Exception("Database is empty."));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/content/PostBook")]
        public IHttpActionResult PostBook([FromBody] ContentModel model)
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

        [Route("api/content/PostAuthor")]
        public IHttpActionResult PostAuthor([FromBody] ContentModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Author author = null;

            try
            {
                var list = ModelFactory.Create(new ViewModel() { ContentModel = model });

                foreach (var unit in list)
                {
                    if (unit is Author)
                        author = unit as Author;
                }
                if (author != null)
                {
                    if (author.Id == -1)
                    {
                        author = DataManager.Authors.Create(author);
                        var entityModel = ModelFactory.Create(new ArrayList() { author });
                        return Created($"/api/author/{author.Id}", entityModel);
                    }
                    else
                    {
                        author = DataManager.Authors.Get().Where(x => x.Name.Equals(author.Name)).ToList()[0];
                        var entityModel = ModelFactory.Create(new ArrayList() { author });
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

        [Route("api/content/PutBook")]
        public IHttpActionResult PutBook([FromBody] ContentModel model)
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
                    var author = DataManager.Authors.Get().Where(x => x.Name.Equals(authorModel.Name)).ToList();
                    if (!author.Any())
                    {
                        authorModel = DataManager.Authors.Create(authorModel);
                    }

                    if (bookModel != null)
                    {
                        bookModel.AuthorId = authorModel.Id;
                        var book = DataManager.Books.Update(bookModel);

                        var entityModel = ModelFactory.Create(new ArrayList() { book, authorModel });
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

        [Route("api/content/PutAuthor")]
        public IHttpActionResult PutAuthor([FromBody] ContentModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Author author = null;

            try
            {
                var list = ModelFactory.Create(new ViewModel() { ContentModel = model });

                foreach (var unit in list)
                {
                    if (unit is Author)
                        author = unit as Author;
                }

                if (author != null)
                {
                    author = DataManager.Authors.Update(author);
                    
                    var entityModel = ModelFactory.Create(new ArrayList() {author});
                    return Ok(entityModel);
                }

                return InternalServerError(new Exception("Recording has not created"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/content/DeleteBook")]
        public IHttpActionResult DeleteBook(int id)
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

        [Route("api/content/DeleteAuthor")]
        public IHttpActionResult DeleteAuthor(int id)
        {
            try
            {
                var entity = DataManager.Authors.Get(id);
                if (entity != null)
                    DataManager.Authors.Delete(entity);
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
