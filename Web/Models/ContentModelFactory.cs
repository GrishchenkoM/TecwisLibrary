using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;
using Core.Entities;
using Web.Models.EntityModels.Interfaces;

namespace Web.Models
{
    public class ContentModelFactory
        : IModelFactory<ArrayList, ViewModel>
    {
        public ViewModel Create(ArrayList unit)
        {
            List<ContentModel> list = null;
            List<Author> authors = null;
            List<Book> books = null;
            Author author = null;
            Book book = null;
            var page = (int)State.Empty;
            var pageSize = (int)State.Empty;

            foreach (dynamic u in unit)
            {
                if (u is List<Author>)
                    authors = u as List<Author>;
                else if (u is List<Book>)
                    books = u as List<Book>;
                else if (u is Author)
                    author = u as Author;
                else if (u is Book)
                    book = u as Book;
                else if (u is int)
                {
                    if (pageSize == (int)State.Empty)
                        pageSize = (int)u;
                    else if (page == (int)State.Empty)
                        page = (int) u;
                }
            }

            if (authors != null)
            {
                if (books != null)
                {
                    list = (from b in books
                            join a in authors on b.AuthorId equals a.Id
                            select new ContentModel
                            {
                                BookId = b.Id,
                                BookName = b.Name,
                                Year = b.Year,
                                AuthorId = a.Id,
                                AuthorName = a.Name
                            }).ToList();
                }
                if (books == null)
                {
                    list = authors.Select(a => new ContentModel()
                    {
                        AuthorId = a.Id,
                        AuthorName = a.Name
                    }).ToList();
                }
            }

            if (author != null && book != null)
            {
                list = new List<ContentModel>();
                var model = new ContentModel()
                {
                    AuthorId = author.Id,
                    AuthorName = author.Name,
                    BookId = book.Id,
                    BookName = book.Name,
                    Year = book.Year
                };

                list.Add(model);
            }
            
            ViewModel viewModel = null;
            if (list != null)
            {
                var pageInfo = new PageInfo
                {
                    PageNumber = 1,
                    PageSize = pageSize,
                    TotalItems = list.Count
                };

                if (page != (int)State.Empty)
                    viewModel = new ViewModel(list.Skip((page - 1)*pageSize).Take(pageSize).ToList());
                else if (pageSize != (int)State.Empty)
                    viewModel = new ViewModel(list.Take(pageSize).ToList());
                else
                    viewModel = new ViewModel(list);

                viewModel.PageInfo = pageInfo;

                if(CurrentId != (int)State.Empty && RequestMessage != null)
                    viewModel.Url = new UrlHelper(RequestMessage).Link("Default", new {id = CurrentId });
            }
            
            return viewModel;
        }

        public ArrayList Create(ViewModel model)
        {
            var list = new ArrayList();
            var bookModel = new Book()
            {
                Id = model.ContentModel.BookId,
                Name = model.ContentModel.BookName,
                Year = model.ContentModel.Year,
                AuthorId = model.ContentModel.AuthorId
            };
            var authorModel = new Author()
            {
                Id = model.ContentModel.AuthorId,
                Name = model.ContentModel.AuthorName
            };
            list.Add(bookModel);
            list.Add(authorModel);

            return list;
        }
        
        public int CurrentId { get; set; } = (int)State.Empty;
        public HttpRequestMessage RequestMessage { get; set; }
    }

    public class ViewModel : IModel
    {
        public ViewModel()
        {
            ContentModels = new List<ContentModel>();
        }
        public ViewModel(List<ContentModel> list)
        {
            ContentModels = list;
        }
        public List<ContentModel> ContentModels { get; set; }
        public ContentModel ContentModel { get; set; }
        public PageInfo PageInfo { get; set; }
        public string Url { get; set; }
    }

    public class ContentModel : IModel
    {
        public int BookId { get; set; }

        [Display(Name = "Name")]
        public string BookName { get; set; }

        public int AuthorId { get; set; }

        [Display(Name = "Author")]
        public string AuthorName { get; set; }

        [Display(Name = "Year")]
        public int Year { get; set; }

        [Display(Name = "Books count")]
        public int Count { get; set; }

        public string Url { get; set; }
    }

    public class PageInfo
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
    }
}