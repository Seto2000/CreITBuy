using CreITBuy.Infrastructures.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreITBuy.Infrastructure.Data.Common
{
    public class Repo : Repository, IRepo
    {
        public Repo(ApplicationDbContext context)
        {
            this.Context = context;
        }
    }
}
