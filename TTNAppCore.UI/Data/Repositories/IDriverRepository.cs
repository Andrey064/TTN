using TTNAppCore.Model;

namespace TTNAppCore.UI.Data.Repositories
{
    public interface IDriverRepository : IGenericRepository<Driver>
    {
        Task<bool> HasTtnAsync(int ttnId);
    }
}