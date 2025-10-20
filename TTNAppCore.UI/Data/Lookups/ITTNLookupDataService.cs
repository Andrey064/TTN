using TTNAppCore.Model;

namespace TTNAppCore.UI.Data.Lookups
{
    public interface ITTNLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetTTNLookupAsync();
    }
}