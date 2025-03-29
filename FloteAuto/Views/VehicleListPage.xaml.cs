using FloteAuto.Models;
using FloteAuto.Services;
using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace FloteAuto.Views
{
    public partial class VehicleListPage : ContentPage
    {
        private VehicleService _vehicleService; // Serviciul de vehicule
        private ObservableCollection<Vehicle> _vehicles;

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
            // Reîncărcăm lista atunci când pagina devine activă din nou (după ce se adaugă/modifică un vehicul)
            await LoadVehicles();
        }


        // Selecția unui vehicul din listă
        async void OnVehicleSelected(object sender, SelectionChangedEventArgs e)
        {
            var selectedVehicle = e.CurrentSelection.FirstOrDefault() as Vehicle;
            if (selectedVehicle != null)
            {
                // Poți adăuga logica de navigare către pagina de detalii dacă dorești, dar nu este necesar pentru ștergere
                // De exemplu: await Navigation.PushAsync(new VehicleDetailsPage(selectedVehicle));
                ((CollectionView)sender).SelectedItem = null; // Deselecționează vehiculul
            }
        }

        // Handler pentru butonul de ștergere
        async void OnDeleteClicked(object sender, EventArgs e)
        {
            // Obținem vehiculul selectat din butonul care a fost apăsat
            var button = sender as Button;
            var vehicle = button?.BindingContext as Vehicle;

            if (vehicle != null)
            {
                // Confirmăm ștergerea vehiculului
                var confirm = await DisplayAlert("Ștergere Vehicul", "Sigur vrei să ștergi acest vehicul?", "Da", "Nu");
                if (confirm)
                {
                    // Ștergem vehiculul din baza de date
                    await _vehicleService.DeleteVehicle(vehicle);

                    // Înlăturăm vehiculul din lista locală
                    _vehicles.Remove(vehicle);

                    // Opțional, poți adăuga un mesaj de succes
                    await DisplayAlert("Succes", "Vehiculul a fost șters!", "OK");
                }
            }

        }

    }
}
