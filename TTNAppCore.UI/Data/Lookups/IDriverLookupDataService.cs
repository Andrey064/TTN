using TTNAppCore.Model;

namespace TTNAppCore.UI.Data.Lookups
{
    public interface IDriverLookupDataService
    {
        Task<List<LookupItem>> GetDriverLookupAsync();
    }
}