using EjesUI.ViewModels;
using System.Windows;
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
    }
}
