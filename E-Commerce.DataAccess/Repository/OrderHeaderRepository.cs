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
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader orderHeader)
        {
            _db.orderHeaders.Update(orderHeader);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var order = _db.orderHeaders.FirstOrDefault(u => u.Id == id); 
            if (order != null) {
                order.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    order.PaymentStatus = paymentStatus;
                }
                    
            }
        }       
        public void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId)
        {
            var order = _db.orderHeaders.FirstOrDefault(u => u.Id == id);
            if(!string.IsNullOrEmpty(sessionId)) {
                order.SessionId= sessionId;
            }
            if(!string.IsNullOrEmpty(paymentIntentId))
            {
                order.PaymentIntentId = paymentIntentId;
                order.PaymentDate = DateTime.Now;
            }
        }
    }
}
