using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Moq;
using SurveyShop.DataAccess.Repository.IRepository;
using SurveyShop.Models;
using SurveyShopWeb.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SurveyShopWeb
{
    [TestFixture]
    public class CategoryControllerTests
    {
        private CategoryController _categoryController;
        private Mock<IUnitOfWork> _unitOfWork;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _categoryController = new CategoryController(_unitOfWork.Object);
        }

        [Test]
        public void IndexPage_GetRequest_VerifyGetAllInvoked()
        {
            //arrange
            _unitOfWork.Setup(x => x.Category.GetAll(x => x.Id == 10, null))
                .Returns(new List<Category>());
            //act
            _categoryController.Index();
            //assert
            _unitOfWork.Verify(x => x.Category.GetAll(null, null), Times.Exactly(1));
        }
        [Test]
        public void CreateCategory_InvalidModel_ReturnsView()
        {
            _categoryController.ModelState.AddModelError("testError", "testErrorMessage");

            var result = _categoryController.Create(new Category());

            ViewResult viewResult = result as ViewResult;
            _unitOfWork.Verify(x => x.Category.Add(new Category()), Times.Never);
        }
        [Test]
        public void EditCategory_InvalidInput_ReturnsNotFoundResult()
        {
            var invalidInput = 0;
            var result = _categoryController.Edit(invalidInput);

            Assert.AreEqual(typeof(NotFoundResult), result.GetType());
        }
    }
}
