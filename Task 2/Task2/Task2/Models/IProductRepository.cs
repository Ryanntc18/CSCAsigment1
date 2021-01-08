﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task2.Models
{
    interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product Get(int id);
        Product Add(Product item);
        void Remove(int id);
        bool Update(Product item);

    }
}