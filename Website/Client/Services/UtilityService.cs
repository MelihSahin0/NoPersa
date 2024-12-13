using Microsoft.JSInterop;

namespace Website.Client.Services
{
    public class UtilityService
    {
        private readonly IJSRuntime jSRuntime;

        public UtilityService(IJSRuntime jSRuntime)
        {
            this.jSRuntime = jSRuntime;
        }

        public async Task<bool> IsOverflowActive(string element)
        {
            return await jSRuntime.InvokeAsync<bool>("isOverflowActive", element);
        }
    }
}
