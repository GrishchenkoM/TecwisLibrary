using System.Collections.Generic;
using BusinessLogic;
using Core.Entities;

namespace Web
{
    public class SourceData
    {
        private SourceData(DataManager dataManager)
        {
            _dataManager = dataManager;

            List<Author> authors = null;
            List<Book> books = null;

            try
            {
                books = _dataManager.Books.Get();
            }
            catch
            {
                // ignored
            }
            try
            {
                authors = _dataManager.Authors.Get();
            }
            catch
            {
                // ignored
            }

            if ((authors == null && books == null) ||
                (authors.Count == 0 && books.Count == 0))
            {
                AuthorsInfo();
                CreateAuthors();

                BooksInfo();
                CreateBooks();
            }
        }

        private void AuthorsInfo()
        {
            _authors = new List<Author>
            {
                new Author() { Name = "Михаил Булгаков"     },
                new Author() { Name = "Артур Конан Дойль"   },
                new Author() { Name = "Эрнест Хемингуэй"    },
                new Author() { Name = "Рэй Брэдбери"        },
                new Author() { Name = "Лев Толстой"         },
                new Author() { Name = "Сергей Есенин"       },
                new Author() { Name = "Иван Бунин"          },
                new Author() { Name = "Харпер Ли"           }
            };
        }
        private void BooksInfo()
        {
            _books = new List<Book>
            {
                new Book() {AuthorId = _authors[0].Id,Name = "Собачье сердце", Year = 1925},
                new Book() {AuthorId = _authors[0].Id,Name = "Белая гвардия", Year = 1924},
                new Book() {AuthorId = _authors[1].Id,Name = "Приключения Шерлока Холмса", Year = 1927},
                new Book() {AuthorId = _authors[2].Id,Name = "По ком звонит колокол", Year = 1940},
                new Book() {AuthorId = _authors[2].Id,Name = "Прощай, оружие!", Year = 1929},
                new Book() {AuthorId = _authors[2].Id,Name = "Старик и море", Year = 1952},
                new Book() {AuthorId = _authors[3].Id,Name = "451 градус по Фаренгейту", Year = 1953},
                new Book() {AuthorId = _authors[3].Id,Name = "Вино из одуванчиков", Year = 1957},
                new Book() {AuthorId = _authors[4].Id,Name = "Отец Сергий", Year = 1911},
                new Book() {AuthorId = _authors[5].Id,Name = "Анна Снегина", Year = 1925},
                new Book() {AuthorId = _authors[5].Id,Name = "Чёрный Человек", Year = 1925},
                new Book() {AuthorId = _authors[6].Id,Name = "Тёмные аллеи", Year = 1938},
                new Book() {AuthorId = _authors[6].Id,Name = "Лёгкое дыхание", Year = 1916},
                new Book() {AuthorId = _authors[7].Id,Name = "Убить пересмешника", Year = 1960},
            };
        }

        private void CreateAuthors()
        {
            foreach (var author in _authors)
            {
                author.Id = _dataManager.Authors.Create(author).Id;
            }
        }
        private void CreateBooks()
        {
            foreach (var book in _books)
            {
                _dataManager.Books.Create(book);
            }
        }

        public static SourceData GetInstance(DataManager dataManager)
        {
            if(_instance == null)
                lock (Obj)
                    if(_instance == null)
                        _instance = new SourceData(dataManager);

            return _instance;
        }

        private static SourceData _instance;
        private static readonly object Obj = new object();
        private static DataManager _dataManager;

        private static List<Author> _authors;
        private static List<Book> _books;
    }
}