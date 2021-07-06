using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Client.Pages.howto
{
    public partial class Basic
    {
        string Messages;

        async Task Save()
        {
            Messages = "Uploading image ... (Simulate 3 seconds)";
            await Task.Delay(3000);
            Messages = "Image Uploaded!";
        }
    }
}
