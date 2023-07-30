using BlazorInputFileExtended.Exceptions;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorInputFileExtended
{
    public partial class InputFileExtended
    {
        #region public
        /// <summary>
        /// Clean all the files
        /// </summary>
        public void Clean()
        {
            Files.Clean();
            FileBytes = null;
            SelectionInfo = string.Empty;
        }

        /// <summary>
        /// Expose method for execute Save when is inside of the form need to be validated.
        /// </summary>
        public async Task FormSave()
        {
            if(Context is null)
            {
                await SendFile();
            }
            else if(Context.Validate())
            {
                await SendFile();
            }
            else
            {
                StringBuilder errors = new StringBuilder();
                foreach(string err in Context.GetValidationMessages())
                {
                    errors.Append(err);
                    errors.Append(", ");
                }
                errors.Remove(errors.Length - 2, 2);
                await OnError.InvokeAsync(new InputFileException(errors.ToString(), "Save"));
            }
        }
        #endregion

        #region private
        void Change(InputFileChangeEventArgs e)
        {
            Files.UploadFile(e);
            OnChange.InvokeAsync(e);
        }

        async Task SendFile()
        {
            if(string.IsNullOrEmpty(TargetToPostFile))
            {
                await OnError.InvokeAsync(new InputFileException("Don't have endpoint to call."));
            }
            else if(Files.Count < 1)
            {
                await OnError.InvokeAsync(new InputFileException("No files chosen"));
            }
            else
            {
                HttpResponseMessage response;
                if(TargetFormDataContent is not null) response = await Files.UploadAsync(TargetToPostFile, TargetFormDataContent, !MultiFile);
                else if(TargetDataObject is not null) response = await Files.UploadAsync(TargetToPostFile, TargetDataObject, !MultiFile);
                else if(Context is not null) response = await Files.UploadAsync(TargetToPostFile, Context, !MultiFile);
                else response = await Files.UploadAsync(TargetToPostFile, new MultipartFormDataContent(), !MultiFile);
                await OnSave.InvokeAsync(response);
                if(CleanOnSuccessUpload) Clean();
            }
            await InvokeAsync(StateHasChanged);
        }
        #endregion

    }
}
