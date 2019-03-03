using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Entities
{
    public interface IOrderProcessor
    {
    void ProcessOrder(Cart cart, ShoppingDetails shoppingDetails);    
    }
}
