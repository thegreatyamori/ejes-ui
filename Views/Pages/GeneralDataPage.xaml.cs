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
        public ViewModels.GeneralPageViewModel ViewModel
        {
            get;
        }
        public GeneralDataPage(ViewModels.GeneralPageViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

        private void ToggleButton_Checked(object sender, EventArgs e)
        {
            ToggleButton someButton = (ToggleButton)sender;
#pragma warning disable CS8629 // Nullable value type may be null.
            if ((bool)someButton.IsChecked)
            {
                ViewModel.OnChangeToggleButton("SI", (bool)someButton.IsChecked);
            }
            else
            {
                ViewModel.OnChangeToggleButton("FPS", (bool)someButton.IsChecked);
            }
#pragma warning restore CS8629 // Nullable value type may be null.
        }
    }
}
