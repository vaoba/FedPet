namespace FedPet;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        GetPermissions();
        InitializeComponent();
    }

    private async void GetPermissions()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();

        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.PostNotifications>();
            
            if (status == PermissionStatus.Granted) Console.WriteLine("Permission granted.");
            else
            {
                await DisplayAlert("Notifications permission denied.", "Notifications are disabled. To make sure the application works as intended, please enable them in application settings.", "OK");
                // System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
        else Console.WriteLine("Permission granted.");
    }
}