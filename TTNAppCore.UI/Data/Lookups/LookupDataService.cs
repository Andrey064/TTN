using Microsoft.EntityFrameworkCore;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTNAppCore.DataAccess;
using TTNAppCore.Model;

namespace TTNAppCore.UI.Data.Lookups
{
    public class LookupDataService : ITTNLookupDataService, IDriverLookupDataService
    {
        private Func<TTNDbContext> _contextCreator;

        public LookupDataService(Func<TTNDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<IEnumerable<LookupItem>> GetTTNLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Ttns.AsNoTracking()
                    .Select(f =>
                    new LookupItem
                    {
                        Id = f.Id,
                        DisplayMember = "№ " + f.Num + " от " + f.Date.ToShortDateString(),
                    })
                    .ToListAsync();
            }

        }

        public async Task<List<LookupItem>> GetDriverLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                var items = await ctx.Drivers.AsNoTracking()
                    .Select(f =>
                    new LookupItem
                    {
                        Id = f.Id,
                        DisplayMember = f.Name,
                    })
                    .ToListAsync();

                return items;
            }

        }
    }
}
