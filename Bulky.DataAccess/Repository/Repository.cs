using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Repository
{
    public class Repository<A> : IRepository<A> where A : class     //Genaric Repository  (Add(),Get(), IEnumerable<A> GetAll(),Remove(), RemoveRange())
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<A> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<A>();
            //_db.Categories == dbSet;
        }

        public void Add(A entity)
        {
            dbSet.Add(entity);
        }

        public A Get(Expression<Func<A, bool>> filter)
        {
            IQueryable<A> q = dbSet;
            q= q.Where(filter);

            return q.FirstOrDefault();
        }

        public IEnumerable<A> GetAll()
        {
            IQueryable<A> q = dbSet;
            return q.ToList();
        }

        public void Remove(A entity)
        {
           dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<A> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
