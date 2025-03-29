using FloteAuto.Models;
using FloteAuto.Services;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Formats.Tar;

namespace FloteAuto.Views
{
    public partial class VehicleDetailsPage : ContentPage
    {
        private VehicleService _vehicleService = new();
        private Vehicle _vehicle;
        private bool _isNew;
        public event EventHandler VehicleAdded;

        public VehicleDetailsPage(Vehicle? vehicle = null)
        {
            InitializeComponent();
            _isNew = vehicle == null;
            _vehicle = vehicle ?? new Vehicle();
            BindingContext = _vehicle;
            _vehicleService = new VehicleService();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                _vehicle.NumarInmatriculare = numarEntry.Text;
                _vehicle.Marca = marcaEntry.Text;
                _vehicle.Model = modelEntry.Text;
                _vehicle.DataExpirareITP = itpDatePicker.Date;
                _vehicle.DataExpirareActe = acteDatePicker.Date;

                if (_isNew)
                {
                    await _vehicleService.AddVehicle(_vehicle);
                    await DisplayAlert("Succes", "Vehicul adăugat cu succes!", "OK");
                }
                else
                {
                    await _vehicleService.UpdateVehicle(_vehicle);
                    await DisplayAlert("Succes", "Vehicul modificat cu succes!", "OK");
                }

                await Navigation.PopAsync();  // Înapoi la lista de vehicule
            }
            catch
            {
                await DisplayAlert("Eroare", "Verifică datele introduse!", "OK");
            }
        }
    }
}
