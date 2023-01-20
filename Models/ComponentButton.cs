using CommunityToolkit.Mvvm.Input;
using System.Windows.Media.Imaging;

namespace EjesUI.Models
{
    public struct ComponentButton
    {
        public string Title { get; set; }
        public BitmapSource Image { get; set; }
        public RelayCommand OnClick { get; set; }
    }
}
