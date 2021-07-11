using BlazorInputFileExtended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Website.Client.Pages.howto
{
    public partial class Basic
    {
        string Messages;
        string Basic1 = "active";
        string Basic2 = string.Empty;
        bool ShowUpload = false;

        InputFileExtended Files;

        void ChangeTab(int tab)
        {
            Files.Clean();
            if (tab == 0)
            {
                ShowUpload = false;
                Basic1 = "active";
                Basic2 = string.Empty;
            }
            else
            {
                ShowUpload = true;
                Basic2 = "active";
                Basic1 = string.Empty;
            }
        }

        async Task Save(HttpResponseMessage respose)
        {
            Messages = "Uploading image ... (Simulate 3 seconds)";
            await Task.Delay(3000);
            if (respose.IsSuccessStatusCode) Messages = $"Image Upload with result {await respose.Content.ReadFromJsonAsync<bool>()}";
            else Messages = $"Can't upload images.";
        }
    }
}
