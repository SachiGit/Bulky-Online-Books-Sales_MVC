using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IRepository <A> where A : class    //Genaric Repository Interface  (Add(),Get(), IEnumerable<A> GetAll(),Remove(), RemoveRange())
    {
        //A - Category
        IEnumerable<A> GetAll();
        A Get(Expression<Func<A, bool>> filter);
        void Add(A entity);
        void Remove(A entity);
        void RemoveRange(IEnumerable<A> entity);
        //void DeleteAll(A entity);
         
    }
}
