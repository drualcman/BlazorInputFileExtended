using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Client.Pages.howto
{
    public partial class ChangeButtons
    {
        string Messages;
        string SelectText = "Choose File";
        string UploadText = "Upload";
        string ChosenText = "chosen";

        async Task Save()
        {
            Messages = "Uploading image ... (Simulate 3 seconds)";
            await Task.Delay(3000);
            Messages = "Image Uploaded!";
        }
    }
}
