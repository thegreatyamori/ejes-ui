using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using EjesUI.Models;
using EjesUI.Services;

namespace EjesUI.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private bool _isInitialized = false;
        private SnackBarNotifierService snackbar;

        [ObservableProperty]
        private string _applicationTitle = String.Empty;

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationItems = new();

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationFooter = new();

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new();

        //[ObservableProperty]
        //private bool _isNewExerciseEnabled = false;

        public MainWindowViewModel(INavigationService navigationService, ISnackbarService snackbarService)
        {
            this.snackbar = new SnackBarNotifierService(snackbarService);
            if (!_isInitialized)
                InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            ApplicationTitle = "EjesUI";

            NavigationItems = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Content = "Home",
                    PageTag = "home",
                    Icon = SymbolRegular.Cube24,
                    PageType = typeof(Views.Pages.DashboardPage)
                },
                new NavigationItem()
                {
                    Content = "General",
                    PageTag = "general",
                    Icon = SymbolRegular.ArrowRotateClockwise24,
                    PageType = typeof(Views.Pages.GeneralDataPage)
                },
                new NavigationItem()
                {
                    Content = "Engranes",
                    PageTag = "engrane",
                    Icon = SymbolRegular.Settings24,
                    PageType = typeof(Views.Pages.EngranePage)
                },
                new NavigationItem()
                {
                    Content = "Cadenas",
                    PageTag = "cadena",
                    Icon = SymbolRegular.LinkSquare24,
                    PageType = typeof(Views.Pages.CadenaPage)
                },
                new NavigationItem()
                {
                    Content = "Poleas",
                    PageTag = "polea",
                    Icon = SymbolRegular.FastAcceleration24,
                    PageType = typeof(Views.Pages.PoleaPage)
                },
                new NavigationItem()
                {
                    Content = "Rodamientos",
                    PageTag = "rodamiento",
                    Icon = SymbolRegular.ArrowCircleDownSplit24,
                    PageType = typeof(Views.Pages.RodamientoPage)
                }
            };

            NavigationFooter = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Content = "Settings",
                    PageTag = "settings",
                    Icon = SymbolRegular.Settings24,
                    PageType = typeof(Views.Pages.SettingsPage)
                }
            };

            TrayMenuItems = new ObservableCollection<MenuItem>
            {
                new MenuItem
                {
                    Header = "Home",
                    Tag = "tray_home"
                }
            };

            _isInitialized = true;
        }

        [RelayCommand]
        private void OnClickNewExercise()
        {
            ExerciseModel.Name = "Ejercicio Ejes #1";
            ExerciseModel.Uuid = Guid.NewGuid().ToString();
            ExerciseModel.IsActive = true;
            snackbar.Show("Nuevo Ejercicio", "En progreso!", 2);
        }
    }
}
