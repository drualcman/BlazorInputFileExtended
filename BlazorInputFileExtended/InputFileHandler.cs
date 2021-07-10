using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorInputFileExtended.Helpers;

namespace BlazorInputFileExtended
{
    /// <summary>
    /// Manage upload files
    /// </summary>
    public class InputFileHandler : IDisposable
    {
        #region variables
        /// <summary>
        /// Need to use the post actions
        /// </summary>
        protected HttpClient HttpClient;

        int MaxAllowedFiles;
        long MaxAllowedSize;
        string FormField;

        /// <summary>
        /// All files uploaded
        /// </summary>
        protected List<FileUploadContent> UploadedFiles = new List<FileUploadContent>();
        #endregion

        #region constructor
        /// <summary>
        /// Can upload files
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="maxFiles">Maximum files allowed to upload</param>
        /// <param name="maxSize">Maximum file size to upload</param>
        /// <param name="formField">Form content name to upload the file</param>
        public InputFileHandler(HttpClient httpClient = null, int maxFiles = 5, long maxSize = 512000, string formField = "files")
        {
            if (httpClient is not null) HttpClient = httpClient;

            MaxAllowedFiles = maxFiles;
            MaxAllowedSize = maxSize;
            FormField = formField;
        }
        #endregion

        #region properties
        /// <summary>
        /// Define the indexer to allow client code to use [] notation to access directly to the file. 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public FileUploadContent this[int index] => UploadedFiles[index];

        /// <summary>
        /// Define the indexer to allow client code to use [] notation to access directly to the file by file name. 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public FileUploadContent this[string fileName] => UploadedFiles.First(n => n.Name == fileName);

        /// <summary>
        /// Return first image from the dictionary
        /// </summary>
        public FileUploadContent First
        {
            get
            {
                int c = UploadedFiles.Count;
                if (c > 0)
                {
                    return UploadedFiles[0];
                }
                else
                {
                    if (OnUploadError is not null)
                    {
                        OnUploadError(this, new ArgumentException("No images found", "First"));
                    }
                    return null;
                }
            }
        }

        /// <summary>
        /// Return last image from the dictionary
        /// </summary>
        public FileUploadContent Last
        {
            get
            {
                int c = UploadedFiles.Count;
                if (c > 0)
                {
                    return UploadedFiles[c - 1];
                }
                else
                {
                    if (OnUploadError is not null)
                    {
                        OnUploadError(this, new ArgumentException("No images found", "Last"));
                    }
                    return null;
                }
            }
        }

        /// <summary>
        /// Return how many images have stored
        /// </summary>
        public int Count => UploadedFiles.Count;

        /// <summary>
        /// Return total file size uploaded
        /// </summary>
        public long Size => UploadedFiles.Sum(s => s.Size);
        #endregion

        #region fields
        /// <summary>
        /// Last File stream uploaded
        /// </summary>
        public StreamContent UploadedImage;
        /// <summary>
        /// Last File name uploaded
        /// </summary>
        public string FileName;
        #endregion

        #region events
        /// <summary>
        /// Delegate to manage OnUploadFile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public delegate void UploadEventHandler(object sender, FileUploadEventArgs e);
        /// <summary>
        /// Delegate to manage OnUploaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public delegate void UploadsEventHandler(object sender, FilesUploadEventArgs e);
        /// <summary>
        /// Delegate to manage OnUploadError
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public delegate void UploadErrorEventHandler(object sender, ArgumentException e);
        /// <summary>
        /// Delegate to manage OnAPIError
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void APIErrorEventHandler(object sender, ArgumentException e);

        /// <summary>
        /// Event to notify each file uploaded
        /// </summary>
        public event UploadEventHandler OnUploadFile;

        /// <summary>
        /// Event to notify all files are uploaded
        /// </summary>
        public event UploadsEventHandler OnUploaded;

        /// <summary>
        /// Event to notify errors occurs
        /// </summary>
        public event UploadErrorEventHandler OnUploadError;

        /// <summary>
        /// Event to notify errors occurs uploading to the api
        /// </summary>
        public event APIErrorEventHandler OnAPIError;

        #region expose events to derivate class
        /// <summary>
        /// Trigger the OnUploadFile from a derivated class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnUploadFileEvent(object sender, FileUploadEventArgs e)
        {
            if (OnUploadFile is not null)
            {
                OnUploadFile(sender, e);
            }
        }

        /// <summary>
        /// Trigger the OnUploaded from a derivated class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnUploadedEvent(object sender, FilesUploadEventArgs e)
        {
            if (OnUploaded is not null)
            {
                OnUploaded(sender, e);
            }
        }

        /// <summary>
        /// Trigger the OnUploadError from a derivated class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnUploadErrorEvent(object sender, ArgumentException e)
        {
            if (OnUploadError is not null)
            {
                OnUploadError(sender, e);
            }
        }

        /// <summary>
        /// Trigger the OnAPIError from a derivated class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnAPIErrorEvent(object sender, ArgumentException e)
        {
            if (OnAPIError is not null)
            {
                OnAPIError(sender, e);
            }
        }
        #endregion

        #endregion

        #region methods to manage files
        /// <summary>
        /// Foreach enumerator to get all the files
        /// </summary>
        /// <returns></returns>
        public IEnumerator<FileUploadContent> GetEnumerator()
        {
            foreach (FileUploadContent item in UploadedFiles)
                yield return item;
        }

        /// <summary>
        /// Use with InputFile OnChange
        /// </summary>
        /// <param name="e">InputFileChangeEventArgs</param>
        public void UploadFile(InputFileChangeEventArgs e)
        {
            try
            {
                if (e.FileCount == 0)
                {
                    UploadedImage = null;
                    FileName = null;
                    if (OnUploadError is not null)
                    {
                        OnUploadError(this, new ArgumentException("No images found", "UploadFile"));
                    }
                }
                else
                {
                    if (e.FileCount > this.MaxAllowedFiles)
                    {
                        if (OnUploadError is not null)
                        {
                            OnUploadError(this, new ArgumentException($"Max files can be selected is {this.MaxAllowedFiles}", "UploadFile"));
                        }
                    }
                    else if (this.Count >= this.MaxAllowedFiles)
                    {
                        if (OnUploadError is not null)
                        {
                            OnUploadError(this, new ArgumentException($"Max files [{this.MaxAllowedFiles}] already selected. For upload more files please remove some.", "UploadFile"));
                        }
                    }
                    else
                    {
                        if (this.MaxAllowedFiles == 1)          //if only allowed 1 file always reset the dictionary
                            UploadedFiles = new List<FileUploadContent>();

                        int files = 0;
                        long size = 0;
                        foreach (IBrowserFile file in e.GetMultipleFiles(maximumFileCount: MaxAllowedFiles))
                        {
                            size += file.Size;
                            Add(new FileUploadContent
                            {
                                Name = file.Name,
                                LastModified = file.LastModified,
                                Size = file.Size,
                                ContentType = file.ContentType,
                                FileStreamContent = new StreamContent(file.OpenReadStream(maxAllowedSize: MaxAllowedSize))
                            });
                            files++;
                        }

                        if (OnUploaded is not null)
                        {
                            OnUploaded(this, new FilesUploadEventArgs { Files = UploadedFiles, Count = files, Size = size, Action = "Added" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (OnUploadError is not null)
                {
                    OnUploadError(this, new ArgumentException($"Exception: {ex.Message}", "UploadFile", ex));
                }
            }
        }

        #region object CRUD
        /// <summary>
        /// Add a image
        /// </summary>
        /// <param name="image"></param>
        public void Add(FileUploadContent image)
        {
            try
            {
                if (image.Size < this.MaxAllowedSize)
                {
                    if (this.MaxAllowedFiles == 1)          //if only allowed 1 file always reset the dictionary
                        UploadedFiles = new List<FileUploadContent>();

                    int count = UploadedFiles.Count;

                    if (count < this.MaxAllowedFiles)
                    {
                        //last image added is the default image to send
                        UploadedImage = image.FileStreamContent;
                        FileName = image.Name;
                        UploadedFiles.Add(image);
                        if (OnUploadFile is not null)
                        {
                            OnUploadFile(this, new FileUploadEventArgs { File = image, FileId = image.FileId, Action = "Added" });
                        }
                    }
                    else
                    {
                        if (OnUploadError is not null)
                        {
                            OnUploadError(this, new ArgumentException($"Max files is {this.MaxAllowedFiles}", "Add"));
                        }
                    }
                }
                else
                {
                    if (OnUploadError is not null)
                    {
                        OnUploadError(this, new ArgumentException($"File {image.Name} overload {this.MaxAllowedSize}", "Add"));
                    }
                }
            }
            catch (Exception ex)
            {
                if (OnUploadError is not null)
                {
                    OnUploadError(this, new ArgumentException($"Exception: {ex.Message}", "Add", ex));
                }
            }

        }

        /// <summary>
        /// Update image by index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="image"></param>
        public bool Update(int index, FileUploadContent image)
        {
            bool result;
            try
            {
                UploadedFiles[index] = image;
                result = true;
                if (OnUploadFile is not null)
                {
                    OnUploadFile(this, new FileUploadEventArgs { File = image, FileId = image.FileId, Action = "Updated" });
                }
            }
            catch (IndexOutOfRangeException ix)
            {
                result = false;
                if (OnUploadError is not null)
                {
                    OnUploadError(this, new ArgumentException($"File index {index} not found", "Update", ix));
                }
            }
            catch (Exception ex)
            {
                result = false;
                if (OnUploadError is not null)
                {
                    OnUploadError(this, new ArgumentException($"Exception: {ex.Message}", "Remove", ex));
                }
            }
            return result;
        }

        /// <summary>
        /// Update image by file name
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="image"></param>
        public bool Update(string fileName, FileUploadContent image)
        {
            bool result;
            try
            {
                FileUploadContent file = UploadedFiles.First(i => i.Name == fileName);
                if (file is null)
                {
                    result = false;
                    if (OnUploadError is not null)
                    {
                        OnUploadError(this, new ArgumentException($"File {fileName} not found", "Update"));
                    }
                }
                else
                {                    
                    result = Update(UploadedFiles.IndexOf(file), image);
                }
            }
            catch (Exception ex)
            {
                result = false;
                if (OnUploadError is not null)
                {
                    OnUploadError(this, new ArgumentException($"Exception: {ex.Message}", "Update", ex));
                }
            }
            return result;
        }

        /// <summary>
        /// Update image by file FileId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="image"></param>
        public bool Update(Guid id, FileUploadContent image)
        {
            bool result;
            try
            {
                FileUploadContent file = UploadedFiles.First(i => i.FileId == id);
                if (file is null)
                {
                    result = false;
                    if (OnUploadError is not null)
                    {
                        OnUploadError(this, new ArgumentException($"File {id} not found", "Update"));
                    }
                }
                else
                {
                    result = Update(UploadedFiles.IndexOf(file), image);
                }
            }
            catch (Exception ex)
            {
                result = false;
                if (OnUploadError is not null)
                {
                    OnUploadError(this, new ArgumentException($"Exception: {ex.Message}", "Update", ex));
                }
            }
            return result;
        }

        /// <summary>
        /// Remove image from index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool Remove(int index)
        {
            bool result;
            try
            {
                result = Remove(UploadedFiles[index]);
            }
            catch (IndexOutOfRangeException ix)
            {
                result = false;
                if (OnUploadError is not null)
                {
                    OnUploadError(this, new ArgumentException($"File index {index} not found", "Remove", ix));
                }
            }
            catch (Exception ex)
            {
                result = false;
                if (OnUploadError is not null)
                {
                    OnUploadError(this, new ArgumentException($"Exception: {ex.Message}", "Remove", ex));
                }
            }            
            return result;
        }

        /// <summary>
        /// Remove image
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool Remove(FileUploadContent file)
        {
            bool result;
            try
            {
                result = UploadedFiles.Remove(file);
                if (result)
                {
                    if (OnUploadFile is not null)
                    {
                        OnUploadFile(this, new FileUploadEventArgs { File = file, FileId = file.FileId, Action = "Removed" });
                    }
                }
                else
                {
                    if (OnUploadFile is not null)
                    {
                        OnUploadFile(this, new FileUploadEventArgs { File = file, FileId = file.FileId, Action = "Remove failed" });
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                if (OnUploadError is not null)
                {
                    OnUploadError(this, new ArgumentException($"Exception: {ex.Message}", "Remove", ex));
                }
            }

            return result;
        }

        /// <summary>
        /// Remove image from file name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Remove(Guid id)
        {
            bool result;
            try
            {
                FileUploadContent file = UploadedFiles.First(i => i.FileId == id);
                if (file is null)
                {
                    result = false;
                    if (OnUploadError is not null)
                    {
                        OnUploadError(this, new ArgumentException($"File {id} not found", "Remove"));
                    }
                }
                else
                {
                    result = Remove(file);
                }
            }
            catch (Exception ex)
            {
                result = false;
                if (OnUploadError is not null)
                {
                    OnUploadError(this, new ArgumentException($"Exception: {ex.Message}", "Remove", ex));
                }
            }
            return result;

        }

        /// <summary>
        /// Remove image from file name
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool Remove(string fileName)
        {
            bool result;
            try
            {
                FileUploadContent file = UploadedFiles.First(i => i.Name == fileName);
                if (file is null)
                {
                    result = false;
                    if (OnUploadError is not null)
                    {
                        OnUploadError(this, new ArgumentException($"File {fileName} not found", "Remove"));
                    }
                }
                else
                {
                    result = Remove(file);
                }
            }
            catch (Exception ex)
            {
                result = false;
                if (OnUploadError is not null)
                {
                    OnUploadError(this, new ArgumentException($"Exception: {ex.Message}", "Remove", ex));
                }
            }
            return result;

        }
        #endregion

        /// <summary>
        /// Remove all the files into the object
        /// </summary>
        public void Clean()
        {
            int c = this.Count;
            for (int i = 0; i < c; i++)
            {
                Remove(i);
            }
            UploadedImage = null;
            FileName = string.Empty;
            if (OnUploaded is not null)
            {
                OnUploaded(this, new FilesUploadEventArgs { Files = null, Count = 0, Size = 0, Action = "Clean" });
            }
        }
        #endregion

        #region methods mange object
        /// <summary>
        /// Set HttpClient if is not from the constructor
        /// </summary>
        /// <param name="httpClient"></param>
        public void SetHttpClient(HttpClient httpClient) => HttpClient = httpClient;
        /// <summary>
        /// Set the max allowed files
        /// </summary>
        /// <param name="maxfile"></param>
        public void SetMaxFiles(int maxfile) => MaxAllowedFiles = maxfile;
        /// <summary>
        /// Set the max file size allowed
        /// </summary>
        /// <param name="maxSize"></param>
        public void SetMaxFileSize(long maxSize) => MaxAllowedSize = maxSize;
        /// <summary>
        /// Set the field name for the form when upload files
        /// </summary>
        /// <param name="field"></param>
        public void SetFormField(string field) => FormField = field;
        #endregion

        #region HttpClient calls
        /// <summary>
        /// Upload image using the object with the data for the form content
        /// </summary>
        /// <typeparam name="TModel">Model to use on the response from the Target to post file</typeparam>
        /// <typeparam name="TData">Model of the data to send with the form and the file</typeparam>
        /// <param name="TargetToPostFile"></param>
        /// <param name="data">Object with the data to send with the file</param>
        /// <param name="ignoreFiles">Indicate if need to ignore the dictionary files or not. False upload the last image selected.</param>
        /// <returns></returns>
        public async Task<TModel> UploadAsync<TModel, TData>(string TargetToPostFile, TData data, bool ignoreFiles = true) =>
            await UploadAsync<TModel>(TargetToPostFile, FormData.SetMultipartFormDataContent(data), ignoreFiles);


        /// <summary>
        /// Upload a image using the endpoint send
        /// </summary>
        /// <typeparam name="TModel">Model to use on the response from the Target to post file</typeparam>
        /// <param name="TargetToPostFile"></param>
        /// <returns></returns>
        public async Task<TModel> UploadAsync<TModel>(string TargetToPostFile) =>
            await UploadAsync<TModel>(TargetToPostFile, new MultipartFormDataContent(), true);

        /// <summary>
        /// Upload a image using the endpoint send
        /// </summary>
        /// <typeparam name="TModel">Model to use on the response from the Target to post file</typeparam>
        /// <param name="TargetToPostFile"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public async Task<TModel> UploadAsync<TModel>(string TargetToPostFile, InputFileChangeEventArgs files) =>
            await UploadAsync<TModel>(TargetToPostFile, new MultipartFormDataContent(), files);

        /// <summary>
        /// Upload a image using the endpoint send
        /// </summary>
        /// <typeparam name="TModel">Model to use on the response from the Target to post file</typeparam>
        /// <param name="TargetToPostFile"></param>
        /// <param name="content">form content to send to the url end point</param>
        /// <returns></returns>
        public async Task<TModel> UploadAsync<TModel>(string TargetToPostFile, MultipartFormDataContent content) =>
            await UploadAsync<TModel>(TargetToPostFile, content, true);

        /// <summary>
        /// Upload a image using the endpoint send
        /// </summary>
        /// <typeparam name="TModel">Model to use on the response from the Target to post file</typeparam>
        /// <param name="TargetToPostFile"></param>
        /// <param name="content">form content to send to the url end point</param>
        /// <param name="ignoreFiles">Indicate if need to ignore the dictionary files or not. False upload the last image selected.</param>
        /// <returns></returns>
        public async Task<TModel> UploadAsync<TModel>(string TargetToPostFile, MultipartFormDataContent content, bool ignoreFiles)
        {
            if (ignoreFiles)
            {
                if (UploadedImage is not null)
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
        public async Task<TModel> UploadAsync<TModel>(string TargetToPostFile, MultipartFormDataContent content, InputFileChangeEventArgs files)
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
        public async Task<TModel> UploadAsync<TModel>(string TargetToPostFile, MultipartFormDataContent content, StreamContent file, string fileName = "")
        {
            if (file is not null)
            {
                content.Add(
                    content: file,
                    name: FormField,
                    fileName: string.IsNullOrEmpty(fileName) ? FileName : fileName
                );
            }
            else
            {
                if (OnUploadError is not null)
                {
                     OnUploadError(this, new ArgumentException($"No files to upload", "UploadAsync"));
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
        private async Task<TModel> UploadFilesAsync<TModel>(string TargetToPostFile, MultipartFormDataContent content,
            bool ignoreFiles)
        {
            if (HttpClient is null) throw new ArgumentException("At least HttpClient Must be provided.", "UploadFilesAsync");
            if (!ignoreFiles)
            {                
                long size = 0;
                int c = 0;
                foreach (FileUploadContent item in this)
                {
                    content.Add(
                        content: item.FileStreamContent,
                        name: FormField,
                        fileName: item.Name
                    );
                    size += item.Size;
                    c++;
                }               
                if (OnUploaded is not null)
                {
                    OnUploaded(this, new FilesUploadEventArgs { Count = c, Files = UploadedFiles, Size = size , Action = "Upload"});
                }
            }
            try
            {
                if (this.Size > 0)
                {
                    using HttpResponseMessage result = await HttpClient.PostAsync(TargetToPostFile, content);
                    if (result.IsSuccessStatusCode)
                    {
                        TModel response = await result.Content.ReadFromJsonAsync<TModel>();
                        return response;
                    }
                    else
                    {
                        if (OnAPIError is not null)
                        {
                            //decode the error from the call of the end point                        
                            string jsonElement = await result.Content.ReadAsStringAsync();
                            OnAPIError(this, new ArgumentException($"{TargetToPostFile}: {result.ReasonPhrase} [{(int)result.StatusCode} {result.StatusCode}]: {jsonElement}", "UploadFilesAsync"));
                        }
                        return default(TModel);
                    }
                }
                else
                {
                    if (OnUploadError is not null)
                    {
                        OnUploadError(this, new ArgumentException($"No files to upload", "UploadFilesAsync"));
                    }
                    return default(TModel);
                }
            }
            catch (Exception ex)
            {
                if (OnAPIError is not null)
                {
                    OnAPIError(this, new ArgumentException($"{TargetToPostFile}: Exception: {ex.Message}", "UploadFilesAsync", ex));
                }
                return default(TModel);
            }
        }

        /// <summary>
        /// Delete the image
        /// </summary>
        /// <param name="TargetToPostFile">Must be return boolean the endpoint</param>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string TargetToPostFile, int index) =>
            await DeleteAsync(TargetToPostFile, this[index].Name);

        /// <summary>
        /// Delete the image from the filename
        /// </summary>
        /// <param name="TargetToPostFile">Must be return boolean the endpoint</param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string TargetToPostFile, string filename)
        {
            if (HttpClient is null) throw new ArgumentException("At least HttpClient Must be provided. Use HttpClient or IDefaultServices.");
            if (string.IsNullOrEmpty(filename)) return false;

            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new StringContent(filename));

            using HttpResponseMessage response = await HttpClient.PostAsync(TargetToPostFile, content);
            return await response.Content.ReadFromJsonAsync<bool>();
        }
        #endregion

        #region dispose
        private bool disposedValue;
        /// <summary>
        /// Overwrite the dispose to clean the object
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    UploadedImage?.Dispose();
                    int c = UploadedFiles.Count;
                    for (int i = 0; i < c; i++)
                    {
                        UploadedFiles[i]?.FileStreamContent?.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Dispose action
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
