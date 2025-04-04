using Microsoft.Maui.ApplicationModel;

namespace FloteAuto.Views;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
	}

    private async void CreateCarClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new VehicleDetailsPage());  // Navigate to Create Car Page
    }

    private async void CreateTripClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LogTripPage());  // Navigate to Create Ride Page
    }

    private async void TripHistoryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TripJournalPage());  // Navigate to Ride History Page
    }

    private async void MapClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MapPage());  // Navigate to Map Page
    }

    private async void CarListClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new VehicleListPage());  // Navigate to Car List Page
    }

    private async void SettingsClicked(object sender, EventArgs e)
    {
        // Toggle Dark/Light Mode
        var action = await DisplayActionSheet("Settings", "Cancel", null, "Switch Mode");

        if (App.Current.RequestedTheme == AppTheme.Dark)
        {
            App.Current.UserAppTheme = AppTheme.Light;
        }
        else
        {
            App.Current.UserAppTheme = AppTheme.Dark;
        }
    }
}