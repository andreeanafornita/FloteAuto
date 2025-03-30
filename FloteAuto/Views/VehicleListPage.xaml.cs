using FloteAuto.Models;
using FloteAuto.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FloteAuto.Views
{
    public partial class VehicleListPage : ContentPage
    {
        private VehicleService _vehicleService; // Serviciul de vehicule
        private ObservableCollection<Vehicle> _vehicles;
        private Vehicle _selectedVehicle; // Vehiculul selectat pentru actualizare imagine

        public VehicleListPage()
        {
            InitializeComponent();
            _vehicleService = new VehicleService();  // Inițializarea serviciului de vehicule
            LoadVehicles();  // Încarcă vehiculele
        }

        // Încarcă vehiculele din baza de date
        public async Task LoadVehicles()
        {
            var vehicles = await _vehicleService.GetVehicles();  // Obține vehiculele din baza de date
            _vehicles = new ObservableCollection<Vehicle>(vehicles);  // Actualizează lista de vehicule
            VehicleListView.ItemsSource = _vehicles;  // Leagă lista de vehicule la CollectionView
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadVehicles(); // Reîncarcă lista când pagina devine activă
        }

        // Selecția unui vehicul din listă
        private void OnVehicleSelected(object sender, SelectionChangedEventArgs e)
        {
            _selectedVehicle = e.CurrentSelection.FirstOrDefault() as Vehicle;
        }

        // Handler pentru butonul de ștergere
        async void OnDeleteClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var vehicle = button?.BindingContext as Vehicle;

            if (vehicle != null)
            {
                bool confirm = await DisplayAlert("Ștergere Vehicul", "Sigur vrei să ștergi acest vehicul?", "Da", "Nu");
                if (confirm)
                {
                    await _vehicleService.DeleteVehicle(vehicle);
                    _vehicles.Remove(vehicle);
                    await DisplayAlert("Succes", "Vehiculul a fost șters!", "OK");
                }
            }
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = e.NewTextValue?.ToLower() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                VehicleListView.ItemsSource = _vehicles;
            }
            else
            {
                var filteredVehicles = _vehicles.Where(v =>
                    v.NumarInmatriculare.ToLower().Contains(searchText) ||
                    v.Marca.ToLower().Contains(searchText) ||
                    v.Model.ToLower().Contains(searchText))
                    .ToList();

                VehicleListView.ItemsSource = new ObservableCollection<Vehicle>(filteredVehicles);
            }
        }

        // Selectează și salvează o imagine pentru vehiculul selectat
        private async void OnSelectImageClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var vehicle = button?.BindingContext as Vehicle;

            if (vehicle == null) return;

            try
            {
                // Deschide selectorul de fișiere
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Selectează o imagine"
                });

                if (result != null)
                {
                    // Creează o cale unde să salvezi imaginea
                    string targetPath = Path.Combine(FileSystem.AppDataDirectory, result.FileName);

                    using (var stream = await result.OpenReadAsync())
                    using (var newStream = File.Create(targetPath))
                    {
                        await stream.CopyToAsync(newStream);
                    }

                    // Setează calea imaginii în obiectul Vehicle
                    vehicle.ImagePath = targetPath;

                    // Actualizează vizual lista
                    VehicleListView.ItemsSource = null;
                    VehicleListView.ItemsSource = _vehicles;

                    // Salvează în baza de date
                    await _vehicleService.UpdateVehicle(vehicle);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Eroare", $"A apărut o problemă: {ex.Message}", "OK");
            }
        }
    }
}