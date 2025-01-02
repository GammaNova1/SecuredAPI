using EntityLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOS
{
    public class UserDetailDto :IDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
