using Wpf.Ui.Common.Interfaces;

namespace EjesUI.Views.Pages
{
    /// <summary>
    /// Interaction logic for PoleaPage.xaml
    /// </summary>
    public partial class PoleaPage : INavigableView<ViewModels.PoleaViewModel>
    {
        public ViewModels.PoleaViewModel ViewModel
        {
            get;
        }
        public PoleaPage(ViewModels.PoleaViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
