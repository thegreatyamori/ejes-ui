﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using EjesUI.Models;
using System.Collections.Generic;
using System;
using EjesUI.Helpers;
using System.Windows.Media.Imaging;
using System.IO;
using EjesUI.Services;
using Wpf.Ui.Mvvm.Contracts;

namespace EjesUI.ViewModels
{
    public partial class DashboardViewModel : ObservableObject, INavigationAware
    {
        private ApiService api;
        private AppConfig appConfig;
        private PdfService pdf;
        private SnackBarService snackbar;

        [ObservableProperty]
        private string _exercise = string.Empty;
        [ObservableProperty]
        private IEnumerable<ComponentButton> _buttons;

        public DashboardViewModel(ISnackbarService snackbarService)
        {
            this.api = new ApiService();
            this.appConfig = new AppConfig();
            this.pdf = new PdfService();
            this.snackbar = new SnackBarService(snackbarService);

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
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
           dynamic rawPdf = this.api.Get("/join-pdf", ("uuid", ExerciseModel.Uuid));
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
           string downloadURL = $"{this.appConfig.DefaultDownloadPath}result_{ExerciseModel.Uuid}.pdf";

           File.WriteAllBytes(downloadURL, rawPdf);

            snackbar.Show("PDF", "PDF descargado !");
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
