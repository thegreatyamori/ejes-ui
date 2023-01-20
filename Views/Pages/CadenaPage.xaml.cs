
using Wpf.Ui.Common.Interfaces;

namespace EjesUI.Views.Pages
{
    /// <summary>
    /// Interaction logic for CadenaPage.xaml
    /// </summary>
    public partial class CadenaPage : INavigableView<ViewModels.CadenaViewModel>
    {
        public ViewModels.CadenaViewModel ViewModel { get; }

        public CadenaPage(ViewModels.CadenaViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
