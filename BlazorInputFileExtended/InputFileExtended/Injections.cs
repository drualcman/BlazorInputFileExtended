using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http;

namespace BlazorInputFileExtended
{
    public partial class InputFileExtended
    {
        /// <summary>
        /// If want to use the upload must be have HttpClient.
        /// That's why is automatic injected
        /// </summary>
        [Inject] public HttpClient Client { get; set; }
        /// <summary>
        /// Inject JavaScript interoperability
        /// </summary>
        [Inject] public IJSRuntime JavaScript { get; set; }
    }
}
