using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public interface IDataResult<T> : IResult //succes var mesaj var parent'da , tekrar yazmaya gerek yok
    {
        T Data { get; }



    }
}
