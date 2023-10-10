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
	public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
	{
		private readonly ApplicationDbContext _applicationDbContext;

		public OrderHeaderRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
		{
			_applicationDbContext = applicationDbContext;
		}

		public void Update(OrderHeader orderHeader)
		{
			_applicationDbContext.OrderHeaders.Update(orderHeader);
		}
	}
}
