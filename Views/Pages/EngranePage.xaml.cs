using System;
using System.Windows;
using System.Windows.Controls;
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

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            foreach (ComboBoxItem item in e.AddedItems)
            {
                if ((string)item.Content == "Recto")
                {
                    ViewModel.DisplayHelice = Visibility.Hidden;
                } else
                {
                    ViewModel.DisplayHelice = Visibility.Visible;
                }
            }
        }
    }
}
