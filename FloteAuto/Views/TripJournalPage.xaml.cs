using FloteAuto.Models;
using FloteAuto.Services;
using Microsoft.Maui.Storage;

namespace FloteAuto.Views;

public partial class TripJournalPage : ContentPage
{
    TripService service = new();

    public TripJournalPage()
    {
        InitializeComponent();
        LoadTrips();
    }

    private async void LoadTrips()
    {
        var trips = await service.GetTrips();
        tripCollection.ItemsSource = trips;
    }

    private async void OnFilterClicked(object sender, EventArgs e)
    {
        var filter = filterEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(filter))
        {
            LoadTrips(); // Dacă nu scrii nimic, arată tot
        }
        else
        {
            var trips = await service.GetTripsByVehicle(filter);
            tripCollection.ItemsSource = trips;
        }
    }
    private async void OnExportClicked(object sender, EventArgs e)
    {
        try
        {
            // Calea către fișier
            string fileName = $"trips_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            // Exportă folosind serviciul
            await service.ExportToJson(filePath);

            await DisplayAlert("Export realizat", $"Fișier salvat în:\n{filePath}", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare la export", ex.Message, "OK");
        }
    }
    private async void OnExportXmlClicked(object sender, EventArgs e)
    {
        try
        {
            string fileName = $"trips_{DateTime.Now:yyyyMMdd_HHmmss}.xml";
            string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            await service.ExportToXml(filePath);

            await DisplayAlert("Export realizat", $"Fișier XML salvat în:\n{filePath}", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare la export XML", ex.Message, "OK");
        }
    }
    private async void OnShareJsonClicked(object sender, EventArgs e)
    {
        try
        {
            // Salvează temporar JSON
            string fileName = $"trips_export.json";
            string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
            await service.ExportToJson(filePath);

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Trimite fișier JSON",
                File = new ShareFile(filePath)
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare partajare", ex.Message, "OK");
        }
    }


}
