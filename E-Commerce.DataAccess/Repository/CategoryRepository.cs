
using E_Commerce_app.DataAccess.Data;
using E_Commerce_app.DataAccess.Repository.IRepository;
using E_Commerce_app.Models;

namespace E_Commerce_app.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category> , ICategoryRepository    
    {
        private ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Category obj)
        {
            _db.Categories.Update(obj);
        }
    }
}