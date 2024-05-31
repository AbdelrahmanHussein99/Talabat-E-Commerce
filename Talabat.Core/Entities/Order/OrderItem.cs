﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order
{
    public class OrderItem :BaseEntity
    {
        public OrderItem()
        {
            
        }
        public OrderItem(ProductItemOredered product, decimal price, int quantity)
        {
            Product = product;
            Price = price;
            Quantity = quantity;
        }

        public ProductItemOredered Product { get; set; }
        public decimal Price { get; set; }

        public int Quantity { get; set; }

    }
}
