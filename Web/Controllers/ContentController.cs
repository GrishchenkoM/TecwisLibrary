using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BusinessLogic;
using Core.Entities;
using Web.Filters;
using Web.Models;

namespace Web.Controllers
{
    public class ContentController : BaseApiController<ContentModelFactory>
    {
        public ContentController(IDataManager dataManager) : base(dataManager)
        {
            _pageSize = 5;
        }

        [Route("api/content/GetBooks")]
        public IHttpActionResult GetBooks()
        {
            try
            {
                var books = DataManager.Books.Get();
                var authors = DataManager.Authors.Get();

                var model = ModelFactory.Create(new ArrayList { books, authors, _pageSize });
                
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
        public IHttpActionResult GetBooks(int index)
        {
            try
            {
                var book = DataManager.Books.Get(index);
                var author = DataManager.Authors.Get(book.AuthorId ?? (int)State.Empty);
                var list = new ArrayList {book, author};
                
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
        [Route("api/content/GetBooksByPage")]
        public IHttpActionResult GetBooksByPage(int? page)
        {
            try
            {
                var books = DataManager.Books.Get();
                var authors = DataManager.Authors.Get();

                var list = new ArrayList {books, authors, _pageSize};
                if (page != null)
                    list.Add(page);
                
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
                var model = GetAuthorsViewModel(_pageSize);

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
                    var model = GetAuthorsViewModel();

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
        public IHttpActionResult GetAuthorsByPage(int? page)
        {
            try
            {
                var model = GetAuthorsViewModel(_pageSize, page);

                if (model.ContentModels != null && model.ContentModels.Count > 0)
                    return Ok(model);

                return InternalServerError(new Exception("Database is empty."));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/content/PostBook")]
        [ModelValidator]
        public IHttpActionResult PostBook([FromBody] ContentModel model)
        {
            Author author = null;
            Book book = null;

            try
            {
                GetEntities(model, ref author, ref book);

                if (author != null)
                {
                    author = author.Id == (int)State.Empty 
                        ? DataManager.Authors.Create(author) 
                        : DataManager.Authors.Get().Where(x => x.Name.Equals(author.Name)).ToList()[0];

                    if (book != null)
                    {
                        book.AuthorId = author.Id;
                        book = DataManager.Books.Create(book);

                        ModelFactory.CurrentId = book.Id;
                        ModelFactory.RequestMessage = Request;

                        var entityModel = ModelFactory.Create(new ArrayList {book, author});

                        return Created(entityModel.Url, entityModel);
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
        [ModelValidator]
        public IHttpActionResult PostAuthor([FromBody] ContentModel model)
        {
            Author author = null;

            try
            {
                GetEntities(model, ref author);

                if (author != null)
                {
                    if (author.Id == (int)State.Empty)
                    {
                        author = DataManager.Authors.Create(author);

                        ModelFactory.CurrentId = author.Id;
                        ModelFactory.RequestMessage = Request;

                        var entityModel = ModelFactory.Create(new ArrayList { author });

                        return Created(entityModel.Url, entityModel);
                    }
                    else
                    {
                        author = DataManager.Authors.Get().Where(x => x.Name.Equals(author.Name)).ToList()[0];
                        var entityModel = ModelFactory.Create(new ArrayList { author });
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
        [ModelValidator]
        public IHttpActionResult PutBook([FromBody] ContentModel model)
        {
            Author authorModel = null;
            Book bookModel = null;

            try
            {
                GetEntities(model, ref authorModel, ref bookModel);

                if (authorModel != null)
                {
                    var author = DataManager.Authors.Get().FirstOrDefault(x => x.Name.Equals(authorModel.Name));
                    if (author == null)
                    {
                        author = DataManager.Authors.Create(authorModel);
                    }

                    if (bookModel != null)
                    {
                        bookModel.AuthorId = author.Id;
                        var book = DataManager.Books.Update(bookModel);

                        var entityModel = ModelFactory.Create(new ArrayList { book, author });
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
        [ModelValidator]
        public IHttpActionResult PutAuthor([FromBody] ContentModel model)
        {
            Author author = null;

            try
            {
                GetEntities(model, ref author);

                if (author != null)
                {
                    author = DataManager.Authors.Update(author);
                    
                    var entityModel = ModelFactory.Create(new ArrayList {author});
                    return Ok(entityModel);
                }

                return InternalServerError(new Exception("Recording has not created"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        
        [Route("api/content/QuickSearch")]
        [HttpGet]
        public IHttpActionResult QuickSearch(string term)
        {
            var books = DataManager.Books.Get();
            var authors = DataManager.Authors.Get();

            var items = (from b in books
                join a in authors on b.AuthorId equals a.Id
                where a.Name.ToLower().Contains(term.ToLower())
                      || b.Name.ToLower().Contains(term.ToLower())
                select new ContentModel
                {
                    BookId = b.Id,
                    BookName = b.Name,
                    Year = b.Year,
                    AuthorId = a.Id,
                    AuthorName = a.Name
                }).ToList();

            var model = new ViewModel(items);
            
            return Ok(model);
        }

        private ViewModel GetAuthorsViewModel()
        {
            var model = new ViewModel();

            var books = DataManager.Books.Get();
            var authors = DataManager.Authors.Get();

            AuthorsWithBooks(model, books, authors);

            AuthorsWithoutBooks(model, authors, books);

            return model;
        }
        private ViewModel GetAuthorsViewModel(int pageSize, int? page = null)
        {
            var model = GetAuthorsViewModel();

            model.ContentModels.Sort((x,y) => y.Count - x.Count);

            var pageInfo = new PageInfo
            {
                PageNumber = 1,
                PageSize = pageSize,
                TotalItems = model.ContentModels.Count
            };

            model.ContentModels = page != null 
                ? model.ContentModels.Skip(((int)page - 1) * pageSize).Take(pageSize).ToList() 
                : model.ContentModels.Take(pageSize).ToList();

            model.PageInfo = pageInfo;

            return model;
        }
        private static void AuthorsWithoutBooks(ViewModel model, List<Author> authors, List<Book> books)
        {
            var withoutBooks = from a in authors
                join b in books on a.Id equals b.AuthorId into ps
                from p in ps.DefaultIfEmpty()
                select new
                {
                    AuthorId = a.Id,
                    AuthorName = a.Name,
                    BookId = p == null ? (int)State.Empty : p.Id
                };

            var wb = withoutBooks.Where(x => x.BookId == (int)State.Empty);
            foreach (var a in wb)
            {
                model.ContentModels.Add(new ContentModel
                {
                    AuthorId = a.AuthorId,
                    AuthorName = a.AuthorName,
                    Count = 0
                });
            }
        }
        private static void AuthorsWithBooks(ViewModel model, List<Book> books, List<Author> authors)
        {
            var withBooks = from b in books
                join a in authors on b.AuthorId equals a.Id
                group a by a.Name
                into newGroup
                orderby newGroup.Key
                select newGroup;


            foreach (var group in withBooks)
            {
                var queue = new Dictionary<Author, int>();
                foreach (var a in @group)
                {
                    if (queue.ContainsKey(a))
                        queue[a] = ++queue[a];
                    else
                        queue.Add(a, 1);
                }
                foreach (var q in queue)
                {
                    model.ContentModels.Add(new ContentModel
                    {
                        AuthorId = q.Key.Id,
                        AuthorName = q.Key.Name,
                        Count = q.Value
                    });
                }
            }
        }

        private void GetEntities(ContentModel model, ref Author author, ref Book book)
        {
            var list = ModelFactory.Create(new ViewModel { ContentModel = model });

            foreach (var unit in list)
            {
                if (unit is Book)
                    book = unit as Book;
                if (unit is Author)
                    author = unit as Author;
            }
        }
        private void GetEntities(ContentModel model, ref Author author)
        {
            var list = ModelFactory.Create(new ViewModel { ContentModel = model });

            foreach (var unit in list)
            {
                if (unit is Author)
                    author = unit as Author;
            }
        }

        private readonly int _pageSize;
    }
}
