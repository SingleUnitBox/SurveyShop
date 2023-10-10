using SurveyShop.DataAccess.Repository.IRepository;
using SurveyShop.Models;
using SurveyShopWeb.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyShop.DataAccess.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public OrderDetailRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void Update(OrderDetail orderDetail)
        {
            _applicationDbContext.OrderDetails.Add(orderDetail);
        }

    }
}
