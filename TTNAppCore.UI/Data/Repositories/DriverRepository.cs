using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTNAppCore.DataAccess;
using TTNAppCore.Model;

namespace TTNAppCore.UI.Data.Repositories
{
    public class DriverRepository : GenericRepository<Driver, TTNDbContext>, IDriverRepository
    {
        public DriverRepository(TTNDbContext context) : base(context)
        {
        }

        public async override Task<Driver> GetByIdAsync(int id)
        {
            return await Context.Drivers
                .Include(d => d.Ttn)
                .SingleAsync(d => d.Id == id);
        }

        public async Task<bool> HasTtnAsync(int driverId)
        {
            return await Context.Ttns.AsNoTracking()
                .Include(d => d.CurrentDriver)
                .AnyAsync(d => d.CurrentDriver.Id == driverId);
        }
    }
}
