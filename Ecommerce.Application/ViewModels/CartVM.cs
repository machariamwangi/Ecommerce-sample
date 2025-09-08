using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.ViewModels
{
    public class CartVM
    { 
        public OrderProduct OrderProduct { get; set; }
        public IEnumerable<Cart> ListCart { get; set; }
    }
}
