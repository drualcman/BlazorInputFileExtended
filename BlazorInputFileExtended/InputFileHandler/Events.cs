using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorInputFileExtended
{
    public partial class InputFileHandler
    {
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

    }
}
