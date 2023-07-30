using BlazorInputFileExtended.Exceptions;
using BlazorInputFileExtended.Helpers;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorInputFileExtended
{
    public partial class InputFileHandler
    {
        #region HttpResponseMessage
        /// <summary>
        /// Upload image using the object with the data for the form content
        /// </summary>
        /// <typeparam name="TData">Model of the data to send with the form and the file</typeparam>
        /// <param name="TargetToPostFile"></param>
        /// <param name="data">Object with the data to send with the file</param>
        /// <param name="ignoreFiles">Indicate if need to ignore the dictionary files or not. False upload the last image selected.</param>
        /// <returns></returns>
        public virtual async Task<HttpResponseMessage> UploadAsync<TData>(string TargetToPostFile, TData data, bool ignoreFiles = true) =>
            await UploadAsync(TargetToPostFile, FormData.SetMultipartFormDataContent(data), ignoreFiles);


        /// <summary>
        /// Upload a image using the endpoint send
        /// </summary>
        /// <param name="TargetToPostFile"></param>
        /// <returns></returns>
        public virtual async Task<HttpResponseMessage> UploadAsync(string TargetToPostFile) =>
            await UploadAsync(TargetToPostFile, new MultipartFormDataContent(), true);

        /// <summary>
        /// Upload a image using the endpoint send
        /// </summary>
        /// <param name="TargetToPostFile"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public virtual async Task<HttpResponseMessage> UploadAsync(string TargetToPostFile, InputFileChangeEventArgs files) =>
            await UploadAsync(TargetToPostFile, new MultipartFormDataContent(), files);

        /// <summary>
        /// Upload a image using the endpoint send
        /// </summary>
        /// <param name="TargetToPostFile"></param>
        /// <param name="content">form content to send to the url end point</param>
        /// <returns></returns>
        public virtual async Task<HttpResponseMessage> UploadAsync(string TargetToPostFile, MultipartFormDataContent content) =>
            await UploadAsync(TargetToPostFile, content, true);

        /// <summary>
        /// Upload a image using the endpoint send
        /// </summary>
        /// <param name="TargetToPostFile"></param>
        /// <param name="content">form content to send to the url end point</param>
        /// <param name="ignoreFiles">Indicate if need to ignore the dictionary files or not. False upload the last image selected.</param>
        /// <returns></returns>
        public virtual async Task<HttpResponseMessage> UploadAsync(string TargetToPostFile, MultipartFormDataContent content, bool ignoreFiles)
        {
            if(ignoreFiles)
            {
                if(UploadedImage is not null)
                {
                    content.Add(
                        content: UploadedImage,
                        name: FormField,
                        fileName: FileName
                    );
                }
            }
            return await UploadFilesAsync(TargetToPostFile, content, ignoreFiles);
        }

        /// <summary>
        /// Upload a image using the endpoint send
        /// </summary>
        /// <param name="TargetToPostFile"></param>
        /// <param name="content">form content to send to the url end point</param>
        /// <param name="files"></param>
        /// <returns></returns>
        public virtual async Task<HttpResponseMessage> UploadAsync(string TargetToPostFile, MultipartFormDataContent content, InputFileChangeEventArgs files)
        {
            UploadFile(files);
            return await UploadFilesAsync(TargetToPostFile, content, false);
        }

        /// <summary>
        /// Upload a image using the endpoint send
        /// </summary>
        /// <param name="TargetToPostFile"></param>
        /// <param name="content">form content to send to the url end point</param>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual async Task<HttpResponseMessage> UploadAsync(string TargetToPostFile, MultipartFormDataContent content, StreamContent file, string fileName = "")
        {
            if(file is not null)
            {
                content.Add(
                    content: file,
                    name: FormField,
                    fileName: string.IsNullOrEmpty(fileName) ? FileName : fileName
                );
            }
            else
            {
                if(OnUploadError is not null)
                {
                    OnUploadError(this, new InputFileException($"No files to upload", "UploadAsync"));
                }
            }
            return await UploadFilesAsync(TargetToPostFile, content, true);
        }

        /// <summary>
        /// Upload all files uploaded
        /// </summary>
        /// <param name="TargetToPostFile"></param>
        /// <param name="content"></param>
        /// <param name="ignoreFiles">Indicate if need to ignore the dictionary files or not. False upload the last image selected.</param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> UploadFilesAsync(string TargetToPostFile, MultipartFormDataContent content,
            bool ignoreFiles)
        {
            if(HttpClient is null) throw new InputFileException("At least HttpClient Must be provided.", "UploadFilesAsync");
            if(!ignoreFiles)
            {
                long size = 0;
                int c = 0;
                foreach(FileUploadContent item in this)
                {
                    content.Add(
                        content: item.FileStreamContent,
                        name: FormField,
                        fileName: item.Name
                    );
                    size += item.Size;
                    c++;
                }
                if(OnUploaded is not null)
                {
                    OnUploaded(this, new FilesUploadEventArgs { Count = c, Files = UploadedFiles, Size = size, Action = "Upload" });
                }
            }

            HttpResponseMessage response;
            try
            {
                if(this.Count < 1)
                {
                    if(OnUploadError is not null)
                    {
                        OnUploadError(this, new InputFileException($"No files to upload", "UploadFilesAsync"));
                    }
                }
                response = await HttpClient.PostAsync(TargetToPostFile, content);
            }
            catch(Exception ex)
            {
                if(OnAPIError is not null)
                {
                    OnAPIError(this, new InputFileException($"{TargetToPostFile}: Exception: {ex.Message}", "UploadFilesAsync", ex));
                }
                response = null;
            }
            return response;
        }

        #endregion

        #region TModel
        /// <summary>
        /// Upload image using the object with the data for the form content
        /// </summary>
        /// <typeparam name="TModel">Model to use on the response from the Target to post file</typeparam>
        /// <typeparam name="TData">Model of the data to send with the form and the file</typeparam>
        /// <param name="TargetToPostFile"></param>
        /// <param name="data">Object with the data to send with the file</param>
        /// <param name="ignoreFiles">Indicate if need to ignore the dictionary files or not. False upload the last image selected.</param>
        /// <returns></returns>
        public virtual async Task<TModel> UploadAsync<TModel, TData>(string TargetToPostFile, TData data, bool ignoreFiles = true) =>
            await UploadAsync<TModel>(TargetToPostFile, FormData.SetMultipartFormDataContent(data), ignoreFiles);


        /// <summary>
        /// Upload a image using the endpoint send
        /// </summary>
        /// <typeparam name="TModel">Model to use on the response from the Target to post file</typeparam>
        /// <param name="TargetToPostFile"></param>
        /// <returns></returns>
        public virtual async Task<TModel> UploadAsync<TModel>(string TargetToPostFile) =>
            await UploadAsync<TModel>(TargetToPostFile, new MultipartFormDataContent(), true);

        /// <summary>
        /// Upload a image using the endpoint send
        /// </summary>
        /// <typeparam name="TModel">Model to use on the response from the Target to post file</typeparam>
        /// <param name="TargetToPostFile"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public virtual async Task<TModel> UploadAsync<TModel>(string TargetToPostFile, InputFileChangeEventArgs files) =>
            await UploadAsync<TModel>(TargetToPostFile, new MultipartFormDataContent(), files);

        /// <summary>
        /// Upload a image using the endpoint send
        /// </summary>
        /// <typeparam name="TModel">Model to use on the response from the Target to post file</typeparam>
        /// <param name="TargetToPostFile"></param>
        /// <param name="content">form content to send to the url end point</param>
        /// <returns></returns>
        public virtual async Task<TModel> UploadAsync<TModel>(string TargetToPostFile, MultipartFormDataContent content) =>
            await UploadAsync<TModel>(TargetToPostFile, content, true);

        /// <summary>
        /// Upload a image using the endpoint send
        /// </summary>
        /// <typeparam name="TModel">Model to use on the response from the Target to post file</typeparam>
        /// <param name="TargetToPostFile"></param>
        /// <param name="content">form content to send to the url end point</param>
        /// <param name="ignoreFiles">Indicate if need to ignore the dictionary files or not. False upload the last image selected.</param>
        /// <returns></returns>
        public virtual async Task<TModel> UploadAsync<TModel>(string TargetToPostFile, MultipartFormDataContent content, bool ignoreFiles)
        {
            if(ignoreFiles)
            {
                if(UploadedImage is not null)
                {
                    content.Add(
                        content: UploadedImage,
                        name: FormField,
                        fileName: FileName
                    );
                }
            }
            return await UploadFilesAsync<TModel>(TargetToPostFile, content, ignoreFiles);
        }

        /// <summary>
        /// Upload a image using the endpoint send
        /// </summary>
        /// <typeparam name="TModel">Model to use on the response from the Target to post file</typeparam>
        /// <param name="TargetToPostFile"></param>
        /// <param name="content">form content to send to the url end point</param>
        /// <param name="files"></param>
        /// <returns></returns>
        public virtual async Task<TModel> UploadAsync<TModel>(string TargetToPostFile, MultipartFormDataContent content, InputFileChangeEventArgs files)
        {
            UploadFile(files);
            return await UploadFilesAsync<TModel>(TargetToPostFile, content, false);
        }

        /// <summary>
        /// Upload a image using the endpoint send
        /// </summary>
        /// <typeparam name="TModel">Model to use on the response from the Target to post file</typeparam>
        /// <param name="TargetToPostFile"></param>
        /// <param name="content">form content to send to the url end point</param>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual async Task<TModel> UploadAsync<TModel>(string TargetToPostFile, MultipartFormDataContent content, StreamContent file, string fileName = "")
        {
            if(file is not null)
            {
                content.Add(
                    content: file,
                    name: FormField,
                    fileName: string.IsNullOrEmpty(fileName) ? FileName : fileName
                );
            }
            else
            {
                if(OnUploadError is not null)
                {
                    OnUploadError(this, new InputFileException($"No files to upload", "UploadAsync"));
                }
            }
            return await UploadFilesAsync<TModel>(TargetToPostFile, content, true);
        }

        /// <summary>
        /// Upload all files uploaded
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="TargetToPostFile"></param>
        /// <param name="content"></param>
        /// <param name="ignoreFiles">Indicate if need to ignore the dictionary files or not. False upload the last image selected.</param>
        /// <returns></returns>
        private async Task<TModel> UploadFilesAsync<TModel>(string TargetToPostFile, MultipartFormDataContent content, bool ignoreFiles)
        {
            TModel response;
            try
            {
                if(this.Count < 1)
                {
                    if(OnUploadError is not null)
                    {
                        OnUploadError(this, new InputFileException($"No files to upload", "UploadFilesAsync"));
                    }
                }
                using HttpResponseMessage result = await UploadFilesAsync(TargetToPostFile, content, ignoreFiles);
                if(result.IsSuccessStatusCode) response = await result.Content.ReadFromJsonAsync<TModel>();
                else
                {
                    if(OnAPIError is not null)
                    {
                        //decode the error from the call of the end point                        
                        string jsonElement = await result.Content.ReadAsStringAsync();
                        OnAPIError(this, new InputFileException($"{TargetToPostFile}: {result.ReasonPhrase} [{(int)result.StatusCode} {result.StatusCode}]: {jsonElement}", "UploadFilesAsync"));
                    }
                    response = default(TModel);
                }
            }
            catch(Exception ex)
            {
                if(OnAPIError is not null)
                {
                    OnAPIError(this, new InputFileException($"{TargetToPostFile}: Exception: {ex.Message}", "UploadFilesAsync", ex));
                }
                response = default(TModel);
            }
            return response;
        }

        /// <summary>
        /// Delete the image
        /// </summary>
        /// <param name="TargetToPostFile">Must be return boolean the endpoint</param>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(string TargetToPostFile, int index) =>
            await DeleteAsync(TargetToPostFile, this[index].Name);

        /// <summary>
        /// Delete the image from the filename
        /// </summary>
        /// <param name="TargetToPostFile">Must be return boolean the endpoint</param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(string TargetToPostFile, string filename)
        {
            if(HttpClient is null) throw new InputFileException("At least HttpClient Must be provided. Use HttpClient or IDefaultServices.");
            if(string.IsNullOrEmpty(filename)) return false;

            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new StringContent(filename));

            using HttpResponseMessage response = await HttpClient.PostAsync(TargetToPostFile, content);
            return await response.Content.ReadFromJsonAsync<bool>();
        }
        #endregion

    }
}
