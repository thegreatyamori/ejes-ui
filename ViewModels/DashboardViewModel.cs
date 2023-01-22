using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using EjesUI.Models;
using System.Collections.Generic;
using System;
using EjesUI.Helpers;
using System.Windows.Media.Imaging;
using System.IO;

namespace EjesUI.ViewModels
{
    public partial class DashboardViewModel : ObservableObject, INavigationAware
    {
        [ObservableProperty]
        private string _exercise = string.Empty;
        [ObservableProperty]
        private IEnumerable<ComponentButton> _buttons;

        public DashboardViewModel()
        {
            Exercise = ExerciseModel.Name;
        }

        public void OnNavigatedTo()
        {
            Exercise = ExerciseModel.Name;
            InitializeViewModel();
        }

        public void OnNavigatedFrom()
        {
        }

        [RelayCommand]
        private void OnClickSavePDF()
        {
            /*dynamic rawPdf = this.api.Get(
                "/join-pdf",
                ("uuid", this.uuid)
            );
            string downloadURL = $"{this.defaultDownloadRoute}result_{this.uuid}.pdf";
            
            File.WriteAllBytes(downloadURL, rawPdf);
            */
            //MessageBox.Show("PDF descargado !", "PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        [RelayCommand]
        private void OnClickSaveWord()
        {
            /*
            string pdfFileName = "";
            string downloadDocxURL = $"{this.defaultDownloadRoute}{pdfFileName}.docx";
            string pdfPathFile = $"{this.defaultDownloadRoute}{pdfFileName}.pdf";

            SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();
            f.OpenPdf(pdfPathFile);

            if (f.PageCount > 0)
            {
                int result = f.ToWord(downloadDocxURL);

                // Open Word document
                if (result == 0)
                {
                    //MessageBox.Show("PDF convertido correctamente !", "Word", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            */
        }

        private void InitializeViewModel()
        {
            var buttonCollection = new List<ComponentButton>();
            var components = ExerciseModel.Components;

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

                buttonCollection.Add(new ComponentButton
                {
                    Title= components[i].FormData.title,
                    OnClick= new RelayCommand(NavigateTo(components[i].FormData.title)),
                    Image= image
                });
            };

            Buttons = buttonCollection;

            //_isInitialized = true;
        }

        private Action NavigateTo(string message)
        {
            return () => { System.Windows.MessageBox.Show(message); };
        }
    }
}
