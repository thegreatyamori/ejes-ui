using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EjesUI.Models;
using EjesUI.Services;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace EjesUI.ViewModels
{
    public partial class RodamientoViewModel : ObservableObject, INavigationAware
    {
        private ApiService api;
        private PdfService pdf;
        private AppConfig appConfig;
        private SnackBarNotifierService snackbar;

        [ObservableProperty]
        private string _filenamePath = string.Empty;
        [ObservableProperty]
        private RodamientoFormDataModel _formDataModel = new();

        public RodamientoViewModel(ISnackbarService snackbarService)
        {
            this.api = new ApiService();
            this.appConfig = new AppConfig();
            this.pdf = new PdfService();
            this.snackbar = new SnackBarNotifierService(snackbarService);
        }

        public void OnNavigatedTo()
        {
        }

        public void OnNavigatedFrom()
        {
            ResetForm();
        }

        [RelayCommand]
        public void OnClickSaveButton()
        {
            PopulateFormData();

            if (ExerciseModel.rodamientoCount > 2)
            {
                snackbar.Show("Ejes", "Dos rodamientos permitidos!", 5);
                return;
            }

            ExerciseModel.Components.Add(new ComponentModel
            {
                FormData = FormDataModel,
                Calculate = null
            });
            ExerciseModel.rodamientoCount++;


            snackbar.Show("Ejes", "Componente Añadido!", 3);
        }

        private void PopulateFormData()
        {
            string title = "Rodamiento " + ExerciseModel.GetNextComponentLetter();
            Opts opts = new Opts
            {
                type = "rodamiento",
                subtype = null
            };

            FormDataModel.title = title;
            FormDataModel.opts = opts;
            FormDataModel.tipo = FormDataModel.tipo.Split(":")[1].Trim();
        }

        private void ResetForm()
        {
            FormDataModel = new RodamientoFormDataModel();
        }
    }
}
