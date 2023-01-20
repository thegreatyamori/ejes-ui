using Wpf.Ui.Common.Interfaces;

namespace EjesUI.Views.Pages
{
    /// <summary>
    /// Interaction logic for EngranePage.xaml
    /// </summary>
    public partial class EngranePage : INavigableView<ViewModels.EngraneViewModel>
    {
        public ViewModels.EngraneViewModel ViewModel
        {
            get;
        }
        public EngranePage(ViewModels.EngraneViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
