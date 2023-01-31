using EjesUI.Helpers;
using EjesUI.Models;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using Wpf.Ui.Common.Interfaces;

namespace EjesUI.Views.Pages
{
    /// <summary>
    /// Interaction logic for GeneralDataPage.xaml
    /// </summary>
    public partial class GeneralDataPage : INavigableView<ViewModels.GeneralPageViewModel>
    {
        private readonly AppConfig appConfig;
        public ViewModels.GeneralPageViewModel ViewModel
        {
            get;
        }
        public GeneralDataPage(ViewModels.GeneralPageViewModel viewModel)
        {
            ViewModel = viewModel;
            this.appConfig = new AppConfig();

            InitializeComponent();
        }

        private void ToggleButton_Checked(object sender, EventArgs e)
        {
            ToggleButton someButton = (ToggleButton)sender;
            bool isButtonChecked = (bool)someButton.IsChecked;
            string system = isButtonChecked ? appConfig.SI : this.appConfig.FPS;

            ViewModel.OnChangeToggleButton(system, isButtonChecked);
            ViewModel.NumeroVueltaPH = isButtonChecked ? "Rpm" : "Rpm";
            ViewModel.LimitePH = isButtonChecked ? "MPa" : "Ksi";
        }
    }
}
