using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorInputFileExtended
{
    public partial class InputFileHandler
    {

        /// <summary>
        /// Enumerator to get all the files
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
                    if (this.MaxAllowedFiles == 1)          //if only allowed 1 file always reset the dictionary
                        UploadedFiles.Clear();

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
            long t = this.Size;
            UploadedFiles.Clear();
            UploadedImage = null;
            FileName = string.Empty;
            if (OnUploaded is not null)
            {
                OnUploaded(this, new FilesUploadEventArgs { Files = null, Count = c, Size = t, Action = "Clean" });
            }
        }
        

    }
}
