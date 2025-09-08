using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
    public class OrderProductRepository : Repository<OrderProduct>, IOrderProductRepository
    {
        private ApplicationDbContext _db;
        public OrderProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void UodateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
        }
    }
}
