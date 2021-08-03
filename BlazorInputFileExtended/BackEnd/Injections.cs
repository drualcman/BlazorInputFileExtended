using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// To setup correctly the urls to get the javascripts
        /// </summary>
        [Inject] public NavigationManager Navigation { get; set; }
    }
}
