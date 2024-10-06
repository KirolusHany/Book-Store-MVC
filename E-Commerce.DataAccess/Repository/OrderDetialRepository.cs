using E_Commerce_app.DataAccess.Data;
using E_Commerce_app.DataAccess.Repository.IRepository;
using E_Commerce_app.DataAccess.Repository;
using E_Commerce_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.DataAccess.Repository.IRepository;

namespace E_Commerce.DataAccess.Repository
{
    public class OrderDetialRepository : Repository<OrderDetial>, IOrderDetialRepository
    {
        private ApplicationDbContext _db;

        public OrderDetialRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderDetial orderDetial)
        {
            _db.orderDetials.Update(orderDetial);
        }
    }
}
