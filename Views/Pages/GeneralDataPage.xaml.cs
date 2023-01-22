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

        private void ToggleButton_Checked(ToggleButton sender, RoutedEventArgs e)
        {
#pragma warning disable CS8629 // Nullable value type may be null.
            if ((bool)sender.IsChecked)
            {
                ViewModel.OnChangeToggleButton("SI", (bool)sender.IsChecked);
            }
            else
            {
                ViewModel.OnChangeToggleButton("FPS", (bool)sender.IsChecked);
            }
#pragma warning restore CS8629 // Nullable value type may be null.
        }
    }
}
