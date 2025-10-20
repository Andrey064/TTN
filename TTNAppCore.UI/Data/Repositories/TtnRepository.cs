using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTNAppCore.DataAccess;
using TTNAppCore.Model;
using TTNAppCore.UI.Data.Repositories;

namespace TTNAppCore.UI.Data.Repositories
{

    public class TtnRepository : GenericRepository<Ttn, TTNDbContext>, ITtnRepository
    {
        public TtnRepository(TTNDbContext context) : base(context)
        {

        }

        public override async Task<Ttn> GetByIdAsync(int ttnId)
        {
            return await Context.Ttns.SingleAsync(f => f.Id == ttnId);

        }
    }
}
