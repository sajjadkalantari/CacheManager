using CacheManager.Repositories;
using CacheManager.Services;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CacheManager.Tests
{
    public class UnitTests
    {

        [Fact]
        public async Task CommentService_ReturnsACommentFromCache()
        {
            // Arrange
            var mockRepo = new Mock<IRepository<int, Comment>>();
            mockRepo.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(new Comment());

            var mockCacheManager = new Mock<ICacheManager>();
            mockCacheManager.Setup(repo => repo.GetAsync<int, Comment>(It.IsAny<int>(), It.IsAny<Func<int, Task<Comment>>>()))
                .ReturnsAsync(GetTestComment()[0]);

            var commentService = new CommentService(mockCacheManager.Object, mockRepo.Object);

            // Act
            var result = await commentService.GetComment(1000);

            // Assert
            Assert.Equal(GetTestComment()[0].BookName, result.BookName);
        }


        [Fact]
        public async Task CommentRepository_ReturnsABook()
        {
            // Arrange

            var commentRepository = new CommentRepository(new HttpClient(), new ApiEndPointsSetting
            {
                BookEndPoint = "http://get.taaghche.ir/v2/book/{id}",
                CommentEndPoint = "https://get.taaghche.ir/v2/comment/{id}/replies"
            });

            // Act
            var result = await commentRepository.GetAsync(1000);

            // Assert
            Assert.NotNull(result);
        }





        [Fact]
        public async Task BookService_ReturnsABookFromCache()
        {
            // Arrange
            var mockRepo = new Mock<IRepository<int, Book>>();
            mockRepo.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(new Book());

            var mockCacheManager = new Mock<ICacheManager>();
            mockCacheManager.Setup(repo => repo.GetAsync<int, Book>(It.IsAny<int>(), It.IsAny<Func<int, Task<Book>>>()))
                .ReturnsAsync(GetTestBook()[0]);

            var bookService = new BookService(mockCacheManager.Object, mockRepo.Object);

            // Act
            var result = await bookService.GetBook(1000);

            // Assert
            Assert.Equal("Test Title", result.Title);
        }

        [Fact]
        public async Task BookRepository_ReturnsABook()
        {
            // Arrange

            var bookService = new BookRepository(new HttpClient(), new ApiEndPointsSetting
            {
                BookEndPoint = "http://get.taaghche.ir/v2/book/{id}",
                CommentEndPoint = "https://get.taaghche.ir/v2/comment/{id}/replies"
            });

            // Act
            var result = await bookService.GetAsync(10000);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CacheManager_ReturnsABookFromFirstLayerCache()
        {
            // Arrange
            var cacheHieracky = ArrangeCacheWrapper();

            var cacheManager = new SampleCacheManager(cacheHieracky);

            // Act
            var result = await cacheManager.GetAsync<int, Book>(1000, async (bookId) => await Task.FromResult(new Book() { Title = "test 3" }));

            // Assert
            Assert.Equal(GetTestBook()[0].Title, result.Title);
        }

        [Fact]
        public async Task CacheManager_ReturnsABookFromSecondLayerCache()
        {
            // Arrange
            var cacheHieracky = ArrangeCacheWrapperSecond();

            var cacheManager = new SampleCacheManager(cacheHieracky);

            // Act
            var result = await cacheManager.GetAsync<int, Book>(1000, async (bookId) => await Task.FromResult(new Book() { Title = "test 3" }));

            // Assert
            Assert.Equal(GetTestBook()[1].Title, result.Title);
        }


        [Fact]
        public async Task CacheManager_ReturnsABookFromFactory()
        {
            // Arrange
            var cacheHieracky = ArrangeCacheWrapperThird();

            var cacheManager = new SampleCacheManager(cacheHieracky);

            // Act
            var book = new Book() { Title = "test 3" };
            var result = await cacheManager.GetAsync<int, Book>(1000, async (bookId) => await Task.FromResult(book));

            // Assert
            Assert.Equal(book.Title, result.Title);
        }

        private LinkedListNode<CacheWrapper> ArrangeCacheWrapperThird()
        {

            var mockMemoryCache = new Mock<IDistributedCache>();
            mockMemoryCache.Setup(repo => repo.GetAsync(It.IsAny<string>(), default(CancellationToken))).ReturnsAsync((byte[])null);

            var mockRedisCache = new Mock<IDistributedCache>();
            mockRedisCache.Setup(repo => repo.GetAsync(It.IsAny<string>(), default(CancellationToken))).ReturnsAsync((byte[])null);

            var caches = new List<IDistributedCache>()
            {
                mockMemoryCache.Object, mockRedisCache.Object
            };

            var settings = new List<ICacheSetting>()
            {
                new CacheSetting{Level = 0, AbsoluteExpirationDay = 1, AbsoluteExpirationRelativeToNowInMinutes = 15},
                new CacheSetting{Level = 1, AbsoluteExpirationDay = 7, AbsoluteExpirationRelativeToNowInMinutes = 60}
            };

            return CacheManagerFactory.CreateCacheHierachy(caches, settings);

        }



        private LinkedListNode<CacheWrapper> ArrangeCacheWrapperSecond()
        {

            var mockMemoryCache = new Mock<IDistributedCache>();
            mockMemoryCache.Setup(repo => repo.GetAsync(It.IsAny<string>(), default(CancellationToken))).ReturnsAsync((byte[])null);

            var mockRedisCache = new Mock<IDistributedCache>();
            mockRedisCache.Setup(repo => repo.GetAsync(It.IsAny<string>(), default(CancellationToken))).ReturnsAsync((GetTestBook()[1]).SerializeObj());

            var caches = new List<IDistributedCache>()
            {
                mockMemoryCache.Object, mockRedisCache.Object
            };

            var settings = new List<ICacheSetting>()
            {
                new CacheSetting{Level = 0, AbsoluteExpirationDay = 1, AbsoluteExpirationRelativeToNowInMinutes = 15},
                new CacheSetting{Level = 1, AbsoluteExpirationDay = 7, AbsoluteExpirationRelativeToNowInMinutes = 60}
            };

            return CacheManagerFactory.CreateCacheHierachy(caches, settings);

        }

        private LinkedListNode<CacheWrapper> ArrangeCacheWrapper()
        {

            var mockMemoryCache = new Mock<IDistributedCache>();
            mockMemoryCache.Setup(repo => repo.GetAsync(It.IsAny<string>(), default(CancellationToken))).ReturnsAsync((GetTestBook()[0]).SerializeObj());

            var mockRedisCache = new Mock<IDistributedCache>();
            mockRedisCache.Setup(repo => repo.GetAsync(It.IsAny<string>(), default(CancellationToken))).ReturnsAsync((GetTestBook()[1]).SerializeObj());

            var caches = new List<IDistributedCache>()
            {
                mockMemoryCache.Object, mockRedisCache.Object
            };

            var settings = new List<ICacheSetting>()
            {
                new CacheSetting{Level = 0, AbsoluteExpirationDay = 1, AbsoluteExpirationRelativeToNowInMinutes = 15},
                new CacheSetting{Level = 1, AbsoluteExpirationDay = 7, AbsoluteExpirationRelativeToNowInMinutes = 60}
            };

            return CacheManagerFactory.CreateCacheHierachy(caches, settings);

        }
        private List<Book> GetTestBook()
        {

            var books = new List<Book>
            {
                new Book
            {
                Id = 1000,
                Description = "some description",
                Title = "Test Title"
            },
                new Book
            {
                Id = 1001,
                Description = "some description 2",
                Title = "Test Title 2"
            }
            };
            return books;
        }



        private List<Comment> GetTestComment()
        {

            var comments = new List<Comment>
            {
                new Comment
            {
                Id = 1000,
                BookName = "some Book 1",
                Nickname = "Test Title"
            },
                new Comment
            {
                Id = 1001,
                BookName = "some Book 2",
                Nickname = "Test Title"
            }
            };
            return comments;
        }
    }
}
