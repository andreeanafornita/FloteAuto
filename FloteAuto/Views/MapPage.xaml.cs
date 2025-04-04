using FloteAuto.Services;
using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Reflection;
namespace FloteAuto.Views;

public partial class MapPage : ContentPage
{
    private VehicleService _vehicleService;

    public MapPage()
	{
		InitializeComponent();
        _vehicleService = new VehicleService();

        var htmlSource = new HtmlWebViewSource
        {
            Html = LoadHtmlFromResources("LeafletMap.html")
        };

        MapWebView.Source = htmlSource;
    }

    private string LoadHtmlFromResources(string fileName)
    {
        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        var resourceName = $"FloteAuto.Resources.Raw.{fileName}";
        using Stream stream = assembly.GetManifestResourceStream(resourceName);
        using StreamReader reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private async void LocateButtonClicked(object sender, EventArgs e)
    {
        string nrInmatriculare = LocateEntry.Text.Trim().ToUpper();

        // Get vehicle from SQLite
        var vehicles = await _vehicleService.GetVehicles();
        var vehicle = vehicles.FirstOrDefault(v => v.NumarInmatriculare.ToUpper() == nrInmatriculare);

        if (vehicle != null)
        {
            if (vehicle.latitudine != 0 && vehicle.longitudine != 0)
            {
                string js = $"map.setView([{vehicle.latitudine}, {vehicle.longitudine}], 15); " +
                            $"L.marker([{vehicle.latitudine}, {vehicle.longitudine}])" +
                            $".addTo(map).bindPopup('{vehicle.NumarInmatriculare}').openPopup();";

                await MapWebView.EvaluateJavaScriptAsync(js);
            }
            else
            {
                await DisplayAlert("Fara locatie", "Acest vehicul nu are coordonate setate.", "OK");
            }
        }
        else
        {
            await DisplayAlert("Lipsa masina", "Numarul nu a fost gasit in baza de date!", "OK");
        }
    }
}
