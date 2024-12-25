using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class SuccessResult : Result
    {
        public SuccessResult(string message) : base(true, message) // Base ile de parent class'a gider
        {

        }
        public SuccessResult() : base(true) //mesaj yazılmazsa direkt tek parametre true döner
        {

        }

    }
}
