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
            _db.Products.Include(u => u.Category).Include(u => u.CategoryId);
        }

        public void Add(A entity)
        {
            dbSet.Add(entity);
        }

        public A Get(Expression<Func<A, bool>> filter, string? includeProperties = null, bool tracked = false)    //GET
        {
            IQueryable<A> q;
            if (tracked != false)    //tracked is TRUE
            {
                q = dbSet;
            }
            else                   //tracked is FALSE
            {
                q = dbSet.AsNoTracking();
            }

            q = q.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    q = q.Include(includeProp);
                }
            }

            return q.FirstOrDefault();
        }

        //Any Category, CoverType
        public IEnumerable<A> GetAll(Expression<Func<A, bool>>? filter, string? includeProperties = null)    //GET ALL
        {
            IQueryable<A> q = dbSet;

            if (filter != null)
            {
                q = q.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    q = q.Include(includeProp);
                }
            }

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