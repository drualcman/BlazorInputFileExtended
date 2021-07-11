using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Website.Client.Pages.howto
{
    public partial class ChangeButtons
    {
        string Messages;
        string SelectText = "Choose File";
        string UploadText = "Upload";
        string ChosenText = "chosen";

        async Task Save(HttpResponseMessage respose)
        {
            Messages = "Uploading image ... (Simulate 3 seconds)";
            await Task.Delay(3000);
            if (respose.IsSuccessStatusCode) Messages = $"Image Upload with result {await respose.Content.ReadFromJsonAsync<bool>()}";
            else Messages = $"Can't upload images.";
        }
    }
}
