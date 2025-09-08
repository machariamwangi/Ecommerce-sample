using Ecommerce.Domain.Interfaces;
using Ecommerce.Infrastructure.Data;

namespace Ecommerce.Infrastructure.Repositories
{
    public  class UnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
        }
        public IApplicationUserRepository AppUser =>   new ApplicationUserRepository(_db);
        public ICartRepository Cart  => new CartRepository(_db);
        public ICategoryRepository Category => new CategoryRepository(_db);
        public IOrderDetailsRepository OrderDetails => new OrderDetailsRepository(_db);
        public IOrderProductRepository OrderProduct => new OrderProductRepository(_db);
        public IProductRepository Product => new ProductRepository(_db);
  
        public void Save()
        {
            _db.SaveChanges();
        }
        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
 