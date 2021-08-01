using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorInputFileExtended;

namespace Website.Client.Pages.demos
{
    public partial class ManyFiles
    {
        List<FileUploadContent> Files = new List<FileUploadContent>();
        string Messages;

        void GetFile(FileUploadEventArgs e)
        {
            Files.Add(e.File);
            if (Files.Count > 6) Files.RemoveAt(0);
            Messages = $"Files selected: {Files.Count}";
        }

        async Task Upload()
        {
            int c = Files.Count;
            if (c> 0)
            {
                for (int i = 0; i < c; i++)
                {
                    Messages = $"Simulate Uploading file {Files[i].Name}";
                    StateHasChanged();
                    await Task.Delay(750);
                }
                Messages = $"Files uploaded {c}";                
                StateHasChanged();
            }
            else
            {
                Messages = "No files to upload";
            }
        }
    }
}
