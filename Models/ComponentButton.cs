using CommunityToolkit.Mvvm.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace EjesUI.Models
{
    public struct ComponentButton
    {
        public string Title { get; set; }
        public BitmapSource Image { get; set; }
        public Brush Color { get; set; }
        public RelayCommand OnClick { get; set; }
    }
}
