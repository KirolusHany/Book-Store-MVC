using E_Commerce.DataAccess.Repository.IRepository;
using E_Commerce_app.DataAccess.Data;
using E_Commerce_app.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_app.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository Category { get;private set; }
        public IProductRepository Product { get;private set; }
        public ICompanyRepository Company { get;private set; }
        public IAppUserRepository AppUser { get;private set; }
        public IShoppingCartRepository ShoppingCart { get;private set; }
        public IOrderHeaderRepository OrderHeader { get;private set; }
        public IOrderDetialRepository OrderDetial { get;private set; }

        public UnitOfWork(ApplicationDbContext db, IProductRepository product, ICompanyRepository company, IShoppingCartRepository shoppingCart, IAppUserRepository appUser, IOrderDetialRepository orderDetial, IOrderHeaderRepository orderHeader)
        {
            _db = db;
            Category = new CategoryRepository(db);
            Product = new ProductRepository(db);
            Company = company;
            AppUser = appUser;
            ShoppingCart = shoppingCart;
            OrderDetial = orderDetial;
            OrderHeader = orderHeader;
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
