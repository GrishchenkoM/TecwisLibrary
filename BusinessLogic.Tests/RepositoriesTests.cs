using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BusinessLogic.Repositories;
using Core.Entities;
using Moq;
using NUnit.Framework;

namespace BusinessLogic.Tests
{
    [TestFixture]
    public class RepositoriesTests
    {
        [SetUp]
        public void SetUp()
        {
            _bookList = new List<Book>()
            {
                new Book() {Id = 0, Name = "Book1"}
            }.AsQueryable();

            _authorList = new List<Author>()
            {
                new Author() {Id = 0, Name = "Author1"}
            }.AsQueryable();

            // arrange

            var mockSetBook = new Mock<DbSet<Book>>();
            mockSetBook.As<IQueryable<Book>>().Setup(x => x.Provider).Returns(_bookList.Provider);
            mockSetBook.As<IQueryable<Book>>().Setup(x => x.Expression).Returns(_bookList.Expression);
            mockSetBook.As<IQueryable<Book>>().Setup(x => x.ElementType).Returns(_bookList.ElementType);
            mockSetBook.As<IQueryable<Book>>().Setup(x => x.GetEnumerator()).Returns(_bookList.GetEnumerator);

            var mockSetAuthor = new Mock<DbSet<Author>>();
            mockSetAuthor.As<IQueryable<Author>>().Setup(x => x.Provider).Returns(_authorList.Provider);
            mockSetAuthor.As<IQueryable<Author>>().Setup(x => x.Expression).Returns(_authorList.Expression);
            mockSetAuthor.As<IQueryable<Author>>().Setup(x => x.ElementType).Returns(_authorList.ElementType);
            mockSetAuthor.As<IQueryable<Author>>().Setup(x => x.GetEnumerator()).Returns(_authorList.GetEnumerator);

            _mockContext = new Mock<DbDataContext>();
            _mockContext.Setup(x => x.Books).Returns(mockSetBook.Object);
            _mockContext.Setup(x => x.Authors).Returns(mockSetAuthor.Object);
        }

        /// <summary>
        /// Testing query scenario
        /// </summary>
        [Test]
        public void GetBookTest()
        {
            // act
            var repo = new Books(_mockContext.Object);
            var obj = repo.Get(0);

            // assert
            Assert.AreEqual(_bookList.ToList()[0].Name, obj.Name);
        }

        /// <summary>
        /// Testing query scenario
        /// </summary>
        [Test]
        public void GetAuthorTest()
        {
            // act
            var repo = new Authors(_mockContext.Object);
            var obj = repo.Get(0);

            // assert
            Assert.AreEqual(_authorList.ToList()[0].Name, obj.Name);
        }

        private Mock<DbDataContext> _mockContext;
        private IQueryable<Book> _bookList;
        private IQueryable<Author> _authorList;
    }
}
