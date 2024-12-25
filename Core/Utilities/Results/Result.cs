using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class Result : IResult
    {
        public Result(bool isSuccess, string message) : this(isSuccess)  //içinde bulunulan claass'ın yani Result'ın içerisindeki diğer tek parametreli constructur'una yolla
        { 
         Message = message;
        
        
        }


        public Result(bool isSuccess) //üstteki constructor çalışırsa otomatik bu constructor da çalışır
        {
           
            IsSuccess = isSuccess;

        }
        public bool IsSuccess { get; }

        public string Message { get; }
    }
}
