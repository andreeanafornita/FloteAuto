using FloteAuto.Models;
using FloteAuto.Services;

namespace FloteAuto.Views;

public partial class LogTripPage : ContentPage
{
    TripService service = new();
    public LogTripPage()
	{
		InitializeComponent();
	}
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            var trip = new Trip
            {
                VehicleName = vehicleEntry.Text,
                Date = datePicker.Date,
                KmStart = double.Parse(kmStartEntry.Text),
                KmEnd = double.Parse(kmEndEntry.Text),
                Purpose = purposeEditor.Text
            };

            await service.AddTrip(trip);
            await DisplayAlert("Succes", "Cursă salvată!", "OK");

            // reset
            vehicleEntry.Text = "";
            kmStartEntry.Text = "";
            kmEndEntry.Text = "";
            purposeEditor.Text = "";
        }
        catch
        {
            await DisplayAlert("Eroare", "Verifică datele introduse!", "OK");
        }
    }
}