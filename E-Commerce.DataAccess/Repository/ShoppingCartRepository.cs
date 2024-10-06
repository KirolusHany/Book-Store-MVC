﻿using E_Commerce.DataAccess.Repository.IRepository;
using E_Commerce_app.DataAccess.Data;
using E_Commerce_app.DataAccess.Repository;
using E_Commerce_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public void Update(ShoppingCart cart)
        {
            _db.ShoppingCarts.Update(cart);
        }
    }
}