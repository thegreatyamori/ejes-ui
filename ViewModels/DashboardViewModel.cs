using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using System.Collections.Generic;
using System;
using EjesUI.Models;
using EjesUI.Helpers;
using EjesUI.Services;
using System.Windows.Media.Imaging;
using System.IO;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Appearance;
using System.Windows;
using System.Diagnostics;
using System.Windows.Media;
using Newtonsoft.Json;

namespace EjesUI.ViewModels
{
    public partial class DashboardViewModel : ObservableObject, INavigationAware
    {
        private readonly ApiService api;
        private readonly PdfService pdf;
        private readonly AppConfig appConfig;
        private readonly GraphicService graphics;
        private readonly SnackBarNotifierService snackbar;
        private Wpf.Ui.Appearance.ThemeType CurrentTheme = Wpf.Ui.Appearance.ThemeType.Unknown;

        [ObservableProperty]
        private string _exercise = string.Empty;
        [ObservableProperty]
        private Visibility _labelVisibility = Visibility.Visible;
        [ObservableProperty]
        private Visibility _buttonsVisibility = Visibility.Hidden;
        [ObservableProperty]
        private bool _pdfButtonEnabled = false;
        [ObservableProperty]
        private bool _wordButtonEnabled = false;
        [ObservableProperty]
        private IEnumerable<ComponentButton> _buttons;
        [ObservableProperty]
        private SegundaIteracionModel _formDataModel = new();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DashboardViewModel(ISnackbarService snackbarService)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            this.api = new ApiService();
            this.appConfig = new AppConfig();
            this.pdf = new PdfService();
            this.snackbar = new SnackBarNotifierService(snackbarService);
            this.graphics = new GraphicService();

            Exercise = ExerciseModel.Name;
        }

        public void OnNavigatedTo()
        {
            var rodamientoItems = ExerciseModel.rodamientoCount == 2;

            Exercise = ExerciseModel.Name;
            LabelVisibility = ExerciseModel.IsActive ? Visibility.Hidden : Visibility.Visible;
            ButtonsVisibility = ExerciseModel.IsActive ? Visibility.Visible : Visibility.Hidden;
            PdfButtonEnabled = rodamientoItems;

            InitializeViewModel();
        }

        public void OnNavigatedFrom()
        {
        }

        [RelayCommand]
        private void OnClickSavePDF()
        {

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            dynamic rawPdf = api.Get("/join-pdf", ("uuid", ExerciseModel.Uuid));
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            string downloadURL = $"{this.appConfig.DefaultDownloadPath}result_{ExerciseModel.Uuid}.pdf";

            File.WriteAllBytes(downloadURL, rawPdf);
            Process.Start("explorer.exe", this.appConfig.DefaultDownloadPath);

            WordButtonEnabled = PdfButtonEnabled;

            snackbar.Show("PDF", "PDF descargado !", 2);
        }

        [RelayCommand]
        private void OnClickGenerateGraphics()
        {
            graphics.Generate(FormDataModel);

            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {"generalData", ExerciseModel.GeneralData},
                {"elements", ExerciseModel.Components}
            };
            string payload = JsonConvert.SerializeObject(data);
            api.Post("/generate-data", payload);
            snackbar.Show("Gráficos", "Generados !");
        }

        [RelayCommand]
        private void OnClickSaveWord()
        {
            string downloadDocxURL = $"{this.appConfig.DefaultDownloadPath}result_{ExerciseModel.Uuid}.docx";
            string pdfPathFile = $"{this.appConfig.DefaultDownloadPath}result_{ExerciseModel.Uuid}.pdf";

            SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();
            f.OpenPdf(pdfPathFile);

            if (f.PageCount > 0)
            {
                int result = f.ToWord(downloadDocxURL);

                if (result == 0)
                {
                    snackbar.Show("Word", "PDF convertido correctamente!", 2);
                }
            }
        }

        private void InitializeViewModel()
        {
            var buttonCollection = new List<ComponentButton>();
            var components = ExerciseModel.Components;
            CurrentTheme = Theme.GetAppTheme();
            var titleColor = new SolidColorBrush(CurrentTheme == ThemeType.Light ? Color.FromRgb(0, 0, 0) : Color.FromRgb(255, 255, 255));

            for (int i = 0; i < components.Count; i++)
            {
                var title = components[i].FormData.title;
                var image = new BitmapImage();

                if (title == "Engrane")
                {
                    image = ImageProcessor.SetDefaultImage("/Assets/engrane.jpg");
                }

                if (title == "Cadena")
                {
                    image = ImageProcessor.SetDefaultImage("/Assets/cadena.jpg");
                }

                if (title == "Polea")
                {
                    image = ImageProcessor.SetDefaultImage("/Assets/polea.png");
                }

                if (title == "Rodamiento")
                {
                    image = ImageProcessor.SetDefaultImage("/Assets/polea.png");
                }

                buttonCollection.Add(new ComponentButton
                {
                    Title = title,
                    OnClick = new RelayCommand(NavigateTo(title)),
                    Image = image,
                    Color = titleColor
                });
            };

            Buttons = buttonCollection;

            //_isInitialized = true;
        }

        private Action NavigateTo(string message)
        {
            return () => { snackbar.Show("Componente", message, 2); };
        }
    }
}
