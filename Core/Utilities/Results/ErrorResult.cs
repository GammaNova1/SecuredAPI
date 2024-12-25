using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class ErrorResult : Result
    {
        public ErrorResult(string message) : base(false, message) // Base ile de parent class'a gider
        {

        }
        public ErrorResult() : base(false) //mesaj yazılmazsa direkt tek parametre false döner
        {

        }
    }
}
