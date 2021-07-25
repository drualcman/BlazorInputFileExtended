[![Nuget](https://img.shields.io/nuget/v/BlazorInputFileExtended?style=for-the-badge)](https://www.nuget.org/packages/BlazorInputFileExtended)
[![Nuget](https://img.shields.io/nuget/dt/BlazorInputFileExtended?style=for-the-badge)](https://www.nuget.org/packages/BlazorInputFileExtended)

# Description
Extend the traditional component InputFile with more options like drag and drop and upload methods directly. Less codding for all.
# Properties
## Upload management
* MultiFile: Indicates can accept multiple files on the selection box.
* MaxUploatedFiles: Indicates how many files can be selected. Default 5.
* MaxFileSize: Indicates maximum file size per each file selected. Default 512000 bytes.
* CleanOnSuccessUpload: Indicates clean all loaded files after upload to the server. Default false.
* SelectionText: Set the text when files are chosen. Default chosen.
* SelectionCss: Set the CSS to format the text when files are chosen. Default info.
## Input formating
* InputContent: HTML for the choose file input.
* InputCss: Set the CSS to format the input file tag.
* InputTitle: Set the title when the user hover the input file tag.
* InputFileTypes: Indicates what kind of files can be selected. Ex: images/*.
## Button formating
* ButtonContent: HTML inside the button for upload.
* ButtonShow: Button to upload can be hide if you want to manage externally. Default true.
* ButtonCss: Set the CSS to format the button.
* ButtonTitle: Set the title when the user hover the button.
## Preview setup only for images
* IsImage: Indicate the files to select is images. Default true.
* ShowPreview: Indicates if need to show a preview for the file selected. Default true.
* PreviewWrapperCss: Set the CSS to format the wrapper for the figure tag content the image. Default image
* FileCss: Set the CSS to format the image tag.
* FileBytes: Store the byte[] about the last image selected.
## Drag and Drop
* CanDropFiles: Enable drag and drop.
* DropZoneCss: CSS to use for format the drop zone.
* DroppingCss: CSS to use when user are dropping the file.
### Methods Drag and Drop
* LoadDropScriptsAsync: If the component load with CanDropfiles = false, with this method can change and enable dropping files.
* UnLoadDropScriptsAsync: If the component is already setup for dropping files with this method can disabled.
## Post actions
If you want to upload files with some other data, send the model data.
* TargetFormDataContent: MultipartFormDataContent with the form data to send with the files.
* TargetDataObject: Object with the data model to send with the files. This will encapsulated into a MultipartFormDataContent before send to server.
* TargetFormFieldName: Indicates the field form name to send the files. Default files
* TargetToPostFile: Indicate the URL to use for the post action. If it's not setup return a event error if try to upload images.
# Events
* OnUploadedFile: When each file is uploaded. Returns FileUploadEventArgs.
* OnUploadComleted: When all files is uploaded. Returns FilesUploadEventArgs.
* OnError: When some exception. Returns ArgumentException.
* OnSave: When click on SAVE button. Returns HttpMessageResponse.
* OnChange: Notmal InputFile OnChange. Returns InputFileChangeEventArgs.
# Handler
You can use the class BlazorInputFileExtenden.InputFileHandler to implement your own logic or the logic for the Authorization send files.
