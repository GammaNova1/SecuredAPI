
using EntityLayer.Abstract;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class User : IdentityUser<int>,IEntity
    {
            public string Name { get; set; }
            public string Surname { get; set; }

            public bool Status { get; set; }
        }
    }

