using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

            return new ViewModel(list);
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
        public List<ContentModel> ContentModels;
        public ContentModel ContentModel;
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
    }
}