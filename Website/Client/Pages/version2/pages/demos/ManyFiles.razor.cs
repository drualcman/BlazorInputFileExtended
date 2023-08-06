using System.Collections.Generic;

namespace Website.Client.Pages.version2.pages.demos
{
    public partial class ManyFiles
    {
        List<BlazorInputFileExtended.FileUploadContent> Files = new List<BlazorInputFileExtended.FileUploadContent>();
        string Messages;

        void GetFile(BlazorInputFileExtended.FileUploadEventArgs e)
        {
            Files.Add(e.File);
            if(Files.Count > 6) Files.RemoveAt(0);
            Messages = $"Files selected: {Files.Count}";
        }

        async Task Upload()
        {
            int c = Files.Count;
            if(c > 0)
            {
                for(int i = 0; i < c; i++)
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
