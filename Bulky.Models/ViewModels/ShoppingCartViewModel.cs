using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }        //For Shopping Cart Items List

        public OrderHeader OrderHeader { get; set; }  //'OrderTotal' is already in OrderHeader
        //public double OrderTotal { get; set;}       //'OrderTotal' is already in OrderHeader
    }
}
