namespace Website.Client.Pages.version2.pages.howto
{
    public partial class ChangeButtons
    {
        string Messages;
        string SelectText = "Choose File";
        string UploadText = "Upload";
        string ChosenText = "chosen";

        async Task Save(HttpResponseMessage respose)
        {
            Messages = "Uploading image ... (Simulate 3 seconds)";
            await Task.Delay(3000);
            if(respose.IsSuccessStatusCode) Messages = $"Image Upload with result {await respose.Content.ReadFromJsonAsync<bool>()}";
            else Messages = $"Can't upload images.";
        }
    }
}
