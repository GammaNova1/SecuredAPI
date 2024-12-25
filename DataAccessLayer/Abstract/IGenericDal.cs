
using EntityLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IGenericDal<T> where T : class, IEntity, new()
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null); //LINQ çalışması için
        T Get(Expression<Func<T, bool>> filter); // filter >> filter null olamaz
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
