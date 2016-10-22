using System.Collections.Generic;
using Core.Entities;

namespace BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BusinessLogic.DbDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "BusinessLogic.DbDataContext";
        }

        protected override void Seed(DbDataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            AuthorsInfo();
            CreateAuthors(context);

            BooksInfo();
            CreateBooks(context);
        }

        private void CreateAuthors(DbDataContext context)
        {
            foreach (var author in _authors)
            {
                try
                {
                    context.Authors.Add(author);
                }
                catch { /*ignore*/ }
            }
        }
        private void CreateBooks(DbDataContext context)
        {
            try
            {
                foreach (var book in _books)
                {
                    context.Books.Add(book);
                }
            }
            catch { /*ignore*/ }
        }

        private void AuthorsInfo()
        {
            _authors = new List<Author>
            {
                new Author() { Name = "������ ��������"     },
                new Author() { Name = "����� ����� �����"   },
                new Author() { Name = "������ ���������"    },
                new Author() { Name = "��� ��������"        },
                new Author() { Name = "��� �������"         },
                new Author() { Name = "������ ������"       },
                new Author() { Name = "���� �����"          },
                new Author() { Name = "������ ��"           }
            };
        }
        private void BooksInfo()
        {
            _books = new List<Book>
            {
                new Book() {Author = _authors[0], Name = "������� ������", Year = 1925},
                new Book() {Author = _authors[0], Name = "����� �������", Year = 1924},
                new Book() {Author = _authors[1], Name = "����������� ������� ������", Year = 1927},
                new Book() {Author = _authors[2], Name = "�� ��� ������ �������", Year = 1940},
                new Book() {Author = _authors[2], Name = "������, ������!", Year = 1929},
                new Book() {Author = _authors[2], Name = "������ � ����", Year = 1952},
                new Book() {Author = _authors[3], Name = "451 ������ �� ����������", Year = 1953},
                new Book() {Author = _authors[3], Name = "���� �� �����������", Year = 1957},
                new Book() {Author = _authors[4], Name = "���� ������", Year = 1911},
                new Book() {Author = _authors[5], Name = "���� �������", Year = 1925},
                new Book() {Author = _authors[5], Name = "׸���� �������", Year = 1925},
                new Book() {Author = _authors[6], Name = "Ҹ���� �����", Year = 1938},
                new Book() {Author = _authors[6], Name = "˸���� �������", Year = 1916},
                new Book() {Author = _authors[7], Name = "����� ������������", Year = 1960},
            };
        }

        private static List<Author> _authors;
        private static List<Book> _books;
    }
}
