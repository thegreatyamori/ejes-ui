
using Wpf.Ui.Common.Interfaces;

namespace EjesUI.Views.Pages
{
    /// <summary>
    /// Interaction logic for RodamientoPage.xaml
    /// </summary>
    public partial class RodamientoPage : INavigableView<ViewModels.RodamientoViewModel>
    {
        public ViewModels.RodamientoViewModel ViewModel { get; }

        public RodamientoPage(ViewModels.RodamientoViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
