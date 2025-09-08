using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IApplicationUserRepository AppUser { get;  }
        ICartRepository Cart { get;  }
        ICategoryRepository Category { get;  }
        IOrderDetailsRepository OrderDetails { get; }
        IOrderProductRepository OrderProduct { get; }

        IProductRepository Product { get; }

        void Save();
    }
}
