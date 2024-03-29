[![Nuget](https://img.shields.io/nuget/v/BlazorInputFileExtended?style=for-the-badge)](https://www.nuget.org/packages/BlazorInputFileExtended)
[![Nuget](https://img.shields.io/nuget/dt/BlazorInputFileExtended?style=for-the-badge)](https://www.nuget.org/packages/BlazorInputFileExtended)

# Description
Extend the traditional component InputFile with more options like drag and drop, copay and paste (if drag and drop is enabled) and upload methods directly. Less codding for all. Oficial [web](https://blazorinputfileextended.community-mall.com/) documentation and examples.
# How to use
Import the name space adding to _Imports.razor this line:
```
@using BlazorInputFileExtended
```
Add into your component:
```
<InputFileExtended TargetToPostFile="[full url endpoint where to post the file/files]" ButtonShow="true" CleanOnSuccessUpload="true" />
```
* Where TargetToPostFile is the endpoint where will upload the file.
* ButtonShow display the upload button
* CleanOnSuccessUpload remove the file form the view after upload the file.
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
## Methods
* FormSave: Required component reference, and can use with EditForm event OnValidSubmit. This action send the form. 
* * If TargetDataObject is set, this have preference when send the form.
* * If TargetDataObject is not set, then send the Context with the form data.
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
* AutoUpload: Indicate to upload the file when is selected. Required TargetToPostFile with the url to upload the file.
# Events
* OnUploadedFile: When each file is uploaded. Returns FileUploadEventArgs.
* OnUploadComleted: When all files is uploaded. Returns FilesUploadEventArgs.
* OnError: When some exception. Returns InputFileException.
* OnSave: When click on SAVE button. Returns HttpMessageResponse.
* OnChange: Normal InputFile OnChange. Returns InputFileChangeEventArgs.
# Exception
* InputFileException: encapsulate the exceptions when upload file.
# Handler
You can use the class BlazorInputFileExtenden.InputFileHandler to implement your own logic or the logic for the Authorization send files.
