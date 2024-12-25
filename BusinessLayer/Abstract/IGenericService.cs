using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IGenericService<T>
    {
        Task<IResult> TInsert(T t);
        Task<IResult> TDelete(T t);
        Task<IResult> TUpdate(T t);
        IDataResult<List<T>> TGetList(Expression<Func<T, bool>> filter = null);
        IDataResult<T> TGet(Expression<Func<T, bool>> filter);

    }
}
