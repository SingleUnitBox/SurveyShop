using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SurveyShop.DataAccess.Repository.IRepository;
using SurveyShop.Models;
using SurveyShop.Models.ViewModels;
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
    public class ProductControllerTests
    {
        private ProductController _productController;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IWebHostEnvironment> _webHostEnvironment;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _webHostEnvironment = new Mock<IWebHostEnvironment>();
            _productController = new ProductController(_unitOfWorkMock.Object,
                _webHostEnvironment.Object);

            _unitOfWorkMock.Reset();

            // mock CategoryList
            _unitOfWorkMock.Setup(x => x.Category.GetAll(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<string>()))
                .Returns(new List<Category>());

            _webHostEnvironment.Setup(x => x.WebRootPath).Returns("\\wwwroot");

        }
        [Test]
        public void IndexPage_GetRequest_ReturnsProduct()
        {
            _unitOfWorkMock.Setup(x => x.Product.GetAll(It.IsAny<Expression<Func<Product, bool>>>(),
                It.IsAny<string>())).Returns(new List<Product>());

            var result = _productController.Index();
            ViewResult viewResult = result as ViewResult;

            Assert.AreEqual(typeof(ViewResult), result.GetType());
            Assert.AreEqual(typeof(List<Product>), viewResult.Model.GetType());

        }
        [Test]
        public void Upsert_GetRequestWithoutId_ReturnsViewWithNewProduct()
        {
            var result = _productController.Upsert(null);
            ViewResult viewResult = result as ViewResult;
            var productViewModel = viewResult.Model as ProductViewModel;

            Assert.IsNotNull(viewResult);
            Assert.AreEqual(typeof(ProductViewModel), viewResult.ViewData.Model.GetType());
            Assert.AreEqual(0, productViewModel.Product.Id);
        }
        [Test]
        public void Upsert_GetRequestWithValidId_ReturnsViewWithValidProduct()
        {
            Product validProduct = new Product()
            {
                Id = 1,
                Name = "product1",
                BarCode = "123456",
                Price = 100
            };

            _unitOfWorkMock.Setup(x => x.Product.Get(It.IsAny<Expression<Func<Product, bool>>>(),
                It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(validProduct);

            var result = _productController.Upsert(1);
            ViewResult viewResult = result as ViewResult;
            var productViewModel = viewResult.Model as ProductViewModel;

            Assert.AreEqual(validProduct.Id, productViewModel.Product.Id);
            Assert.AreEqual(validProduct, productViewModel.Product);
        }
        [Test]
        public void Upsert_PostRequestWithInvalidModel_ReturnsViewWithModelError()
        {
            _productController.ModelState.AddModelError("testError", "testErrorMessage");

            var result = _productController.Upsert(null);
            ViewResult viewResult = result as ViewResult;

            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
        }
        [Test]
        public void Upsert_HandleFile_ReturnsCorrectImageUrl()
        {
            //create a mock for file
            var file = new Mock<IFormFile>();
            file.Setup(x => x.FileName).Returns("test_image.jpg");

            var productViewModel = new ProductViewModel
            {
                Product = new Product { Id = 1, Name = "Product1" }
            };

            var productControllerContextMock = new Mock<ControllerContext>();
            productControllerContextMock.SetupGet(x => x.HttpContext.Request.Form.Files)
                .Returns(new FormFileCollection { file.Object });
            _productController.ControllerContext = productControllerContextMock.Object;

            var result = _productController.Upsert(productViewModel, file.Object);
            ViewResult viewResult = result as ViewResult;
            var productViewModelResult = viewResult.Model as ProductViewModel;

        }
    }
}
