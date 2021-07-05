# BlazorInputFileExtended
Component Blazor extend the normal InputFile with all the necessary to upload files
# Properties
## Upload management
* MultiFile: Indicates can accept multiple files on the selection box.
* MaxUploatedFiles: Indicates how many files can be selected. Default 5.
* MaxFileSize: Indicates maximum file size per each file selected. Default 512000 bytes.
## Input formating
* InputCss: Set the CSS to format the input file tag.
* InputTitle: Set the title when the user hover the input file tag.
* InputFileTypes: Indicates what kind of files can be selected. Ex: images/*.
## Button formating
* ButtonShow: Button to upload can be hide if you want to manage externally. Default true.
* ButtonCss: Set the CSS to format the button.
* ButtonText: Set the text on the button.
* ButtonTitle: Set the title when the user hover the button.
## Preview setup only for images
* IsImage: Indicate the files to select is images. Default false.
* ShowPreview: Indicates if need to show a preview for the file selected. Default false
* PreviewWrapperCss: Set the CSS to format the wrapper for the figure tag content the image. Default image
* FileCss: Set the CSS to format the image tag.
* FileBytes: Store the byte[] about the last image selected.
## Post actions
If you want to upload files with some other data, send the model data.
* FormData: Object MultipartFormDataContent with the form data to send with the files.
* FormField: Indicates the field form name to send the files. Default files
* EndPoint: Indicate the URL to use for the post action. If it's not setup return a event error if try to upload images.
# Events
* OnUploadedFile: When each file is uploaded. Returns FileUploadEventArgs.
* OnUploadComleted: When all files is uploaded. Returns FilesUploadEventArgs.
* OnError: When some exception. Returns ArgumentException.
* OnSave: When click on SAVE button. Returns TResponse with the model from the response.
