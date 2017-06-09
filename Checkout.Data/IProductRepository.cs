﻿namespace Checkout.Data
{
    using System.Collections.Generic;

    public interface IProductRepository
    {
        IEnumerable<Product> SelectAll();

        Product SelectById(int id);

        void Insert(Product product);

        void Update(Product product);

        void Delete(int id);

        void Save();

        int GetProductUnitPrice(string skuCode);
    }
}