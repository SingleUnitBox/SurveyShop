using Microsoft.EntityFrameworkCore;
using SurveyShop.DataAccess.Repository;
using SurveyShop.DataAccess.Repository.IRepository;
using SurveyShop.Models;
using SurveyShopWeb.DataAccess.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyShop.DataAccess
{
    [TestFixture]
    public class CategoryRepositoryTests
    {
        private Category _categoryOne;
        private Category _categoryTwo;
        private DbContextOptions<ApplicationDbContext> _options;
        public CategoryRepositoryTests()
        {
            _categoryOne = new Category { Id = 1, Name = "cat1", DisplayOrder = 1 };
            _categoryTwo = new Category { Id = 2, Name = "cat2", DisplayOrder = 2 };
        }
        [SetUp]
        public void SetUp()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "temp_SurveyShop").Options;
        }
        [Test]
        [Order(1)]
        public void AddCategory_CategoryOne_CheckTheValuesFromDatabase()
        {
            using (var dbContext = new ApplicationDbContext(_options))
            {
                var unitOfWork = new UnitOfWork(dbContext);
                //var repository = new CategoryRepository(dbContext);
                //repository.Add(_categoryOne);
                unitOfWork.Category.Add(_categoryOne);
                unitOfWork.Save();
            }

            using (var dbContext = new ApplicationDbContext(_options))
            {
                var categoryFromDb = dbContext.Categories.FirstOrDefault();
                Assert.AreEqual(_categoryOne.Id, categoryFromDb.Id);
                Assert.AreEqual(_categoryOne.Name, categoryFromDb.Name);
                Assert.AreEqual(_categoryOne.DisplayOrder, categoryFromDb.DisplayOrder);
            }
        }
        [Test]
        [Order(3)]
        public void RemoveCategory_CategoryOne_CheckIfRemovedFromDatabase()
        {
            using (var dbContext = new ApplicationDbContext(_options))
            {
                var unitOfWork = new UnitOfWork(dbContext);

                unitOfWork.Category.Remove(_categoryOne);
                unitOfWork.Save();

                var categoryFromDb = dbContext.Categories.ToList();

                Assert.IsEmpty(categoryFromDb);
            }
        }
        [Test]
        [Order(2)]
        public void GetCategory_CategoryOne_CheckTheValuesFromDatabase()
        {
            using (var dbContext = new ApplicationDbContext(_options))
            {
                var unitOfWork = new UnitOfWork(dbContext);
                //var repository = new CategoryRepository(dbContext);
                //repository.Add(_categoryOne);
                unitOfWork.Category.Get(x => x.Id == _categoryOne.Id);
            }
            using (var dbContext = new ApplicationDbContext(_options))
            {
                var categoryFromDb = dbContext.Categories.FirstOrDefault();
                Assert.AreEqual(_categoryOne.Id, categoryFromDb.Id);
                Assert.AreEqual(_categoryOne.Name, categoryFromDb.Name);
                Assert.AreEqual(_categoryOne.DisplayOrder, categoryFromDb.DisplayOrder);
            }
        }
        [Test]
        public void GetAllCategories_CategoryOneAndTwo_CheckBothCategoriesFromDatabase()
        {
            var expectedResult = new List<Category>()
            {
                _categoryOne,
                _categoryTwo
            };

            using (var dbContext = new ApplicationDbContext(_options))
            {
                dbContext.Database.EnsureDeleted();
                var unitOfWork = new UnitOfWork(dbContext);

                unitOfWork.Category.Add(_categoryOne);
                unitOfWork.Category.Add(_categoryTwo);
                unitOfWork.Save();
            }

            List<Category> actualList;
            using (var dbContext = new ApplicationDbContext(_options))
            {
                var repository = new CategoryRepository(dbContext);
                actualList = repository.GetAll().ToList();
            }

            CollectionAssert.AreEqual(expectedResult, actualList, new CategoryCompare());
        }

        private class CategoryCompare : IComparer
        {
            public int Compare (object x, object y)
            {
                var category1 = (Category)x;
                var category2 = (Category)y;
                if (category1.Id != category2.Id)
                {
                    return 1;
                }
                else
                    return 0;
            }
        }
    }
}
