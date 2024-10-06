using E_Commerce.DataAccess.Repository.IRepository;
using E_Commerce_app.DataAccess.Data;
using E_Commerce_app.DataAccess.Repository;
using E_Commerce_app.Models;



namespace E_Commerce.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private ApplicationDbContext _db;
        public CompanyRepository(ApplicationDbContext db):base(db) 
        {
            _db = db;
        }

        public void Update(Company company)
        {
            _db.Update(company);
        }
    }
}
