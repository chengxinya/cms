using System.Threading.Tasks;
using static SSCMS.Cli.Services.ApiService;

namespace SSCMS.Cli.Abstractions
{
  public partial interface IApiService
    {
        Task<(bool success, PluginAndUser result, string failureMessage)> PluginShowAsync(string pluginId);
    }
}
