using System;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EjesUI.Helpers;
using EjesUI.Models;
using EjesUI.Services;
using System.Collections.Generic;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace EjesUI.ViewModels
{
    public partial class CadenaViewModel : ObservableObject, INavigationAware
    {
        private readonly ApiService api;
        private readonly AppConfig appConfig;
        private readonly PdfService pdf;
        private readonly SnackBarNotifierService snackbar;

        [ObservableProperty]
        private string _filenamePath = string.Empty;
        [ObservableProperty]
        private BitmapImage _cadenaImg = new();
        [ObservableProperty]
        private bool _downloadPDFCadenaButton = false;
        [ObservableProperty]
        private bool _addCadenaButton = true;
        [ObservableProperty]
        private CadenaFormDataModel _formDataModel = new();
        [ObservableProperty]
        private bool _testCadenaToggle = false;
        [ObservableProperty]
        private string _pesoPH = string.Empty;
        [ObservableProperty]
        private string _diametroPH = string.Empty;
        [ObservableProperty]
        private string _potenciaPH = string.Empty;
        [ObservableProperty]
        private string _inclinacionPH = string.Empty;

        public CadenaViewModel(ISnackbarService snackbarService)
        {
            this.api = new ApiService();
            this.appConfig = new AppConfig();
            this.pdf = new PdfService();
            this.snackbar = new SnackBarNotifierService(snackbarService);
        }

        public void OnNavigatedTo()
        {
            DownloadPDFCadenaButton = false;
            AddCadenaButton = true;
            CadenaImg = ImageProcessor.SetDefaultImage();

            bool isButtonChecked = ExerciseModel.GeneralData.unidades;
            PesoPH = isButtonChecked ? "N" : "Lbf";
            DiametroPH = isButtonChecked ? "mm" : "in";
            PotenciaPH = isButtonChecked ? "kW" : "Hp";
            InclinacionPH = isButtonChecked ? "°" : "°";
         }

        public void OnNavigatedFrom()
        {
            ResetForm();
        }

        [RelayCommand]
        public void ClearForm()
        {
            ResetForm();
            AddCadenaButton = true;
        }

        [RelayCommand]
        public void OnClickPDFButton()
        {
            pdf.Download(FilenamePath);
            Process.Start("explorer.exe", this.appConfig.DefaultDownloadPath);
            snackbar.Show("Ejes", "Componente Añadido!", 3);
        }

        [RelayCommand]
        public void OnClickSaveButton()
        {
            GeneralDataModel generalData = ExerciseModel.GeneralData;
            PopulateFormData();

            CadenaCalculateModel calculateData = CalculateComponent();

            Pdf payload = BuildData(calculateData);

            BitmapImage img = ImageProcessor.ProcessImage(payload.images?.frontal.Value);

            string filename = pdf.Generate(payload);

            ExerciseModel.Components.Add(new ComponentModel
            {
                FormData = FormDataModel,
                Calculate = calculateData
            });

            FilenamePath = filename;
            CadenaImg = img;
            DownloadPDFCadenaButton = true;
            AddCadenaButton = false;

            snackbar.Show("Ejes", "Componente Añadido!", 3);
        }

        private void PopulateFormData()
        {
            string title = "Cadena " + ExerciseModel.GetNextComponentLetter();
            Opts opts = new()
            {
                type = "cadena",
                subtype = null
            };

            FormDataModel.title = title;
            FormDataModel.opts = opts;

            if (TestCadenaToggle)
            {
                // Ejemplo 1
                //FormDataModel.potencia = 11;
                //FormDataModel.diametro = 10;
                //FormDataModel.inclinacion = 15;
                //FormDataModel.energia = "Recibe";
                //FormDataModel.peso = 18;
                //FormDataModel.ubicacion = 6;

                // Ejemplo 2
                FormDataModel.peso = 18;
                FormDataModel.diametro = 10;
                FormDataModel.potencia = 5;
                FormDataModel.inclinacion = 30;
                FormDataModel.ubicacion = 6;
                FormDataModel.energia = "Consume";
                return;
            }

            FormDataModel.energia = FormDataModel.energia.Split(":")[1].Trim();
        }

        private Pdf BuildData(CadenaCalculateModel data)
        {
            GeneralDataModel generalData = ExerciseModel.GeneralData;
            FormDataModel.opts.system = generalData.unidades ? appConfig.SI : appConfig.FPS;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            dynamic image = api.Get(
                "/cadena",
                ("system", FormDataModel.opts.system),
                ("diameter", FormDataModel.diametro.ToString()),
                ("inclination_degree", FormDataModel.inclinacion.ToString()),
                ("orientation", FormDataModel.energia),
                ("direction", generalData.sentidoGiro),
                ("name", ExerciseModel.GetNextComponentLetter())
            );
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            dynamic[] torqueValues = { FormDataModel.potencia, generalData.numeroVuelta, data.torque };
            dynamic?[] generalForces = { data.torque, data.fuerzaTangencial, null, data.radio, null, null };
            dynamic[] tangentialDescomposition = { data.inclinacion, data.fuerzaTangencial, data.fuerzaTangencialZ, data.fuerzaTangencialY };

            Pdf pdf = new();
            pdf.uuid = ExerciseModel.Uuid;
            pdf.title = FormDataModel.title;
            pdf.images = image;
            pdf.opts = FormDataModel.opts;
            pdf.torque_values = new List<dynamic>(torqueValues);
            pdf.general_forces = new List<dynamic?>(generalForces);
            pdf.tangential_descomposition = new List<dynamic>(tangentialDescomposition);

            return pdf;
        }

        private CadenaCalculateModel CalculateComponent()
        {
            GeneralDataModel generalData = ExerciseModel.GeneralData;

            double peso = FormDataModel.peso;
            int constante = generalData.unidades ? this.appConfig.CONSTANTE_TORQUE_SI : appConfig.CONSTANTE_TORQUE_FPS;
            double torque = constante * (FormDataModel.potencia / generalData.numeroVuelta);
            double radio = FormDataModel.diametro / 2;
            double fuerzaTangencial = torque / radio;
            double inclinacion = FormDataModel.inclinacion;

            if (
                (generalData.sentidoGiro == "Horario" && FormDataModel.energia == "Recibe") ||
                (generalData.sentidoGiro == "Antihorario" && FormDataModel.energia == "Consume"))
            {
                inclinacion = (-(inclinacion + 90) * Math.PI) / 180;
            }

            if (
                (generalData.sentidoGiro == "Antihorario" && FormDataModel.energia == "Recibe") ||
                (generalData.sentidoGiro == "Horario" && FormDataModel.energia == "Consume"))
            {
                inclinacion = ((inclinacion + 90) * Math.PI) / 180;
            }

            double fuerzaTangencialZ = Math.Round(fuerzaTangencial * Math.Cos(inclinacion), 3);
            double fuerzaTangencialY = Math.Round(fuerzaTangencial * Math.Sin(inclinacion), 3);
            // TODO: mover esta logica al cs de la page
            // mainHandler.datosCadena.fuerzaFinalZ.Add(fuerzaTangencialZ);
            // mainHandler.datosCadena.fuerzaFinalY.Add(fuerzaTangencialY - FormDataModel.peso);

            return new CadenaCalculateModel
            {
                radio = radio,
                torque = torque,
                inclinacion = inclinacion,
                fuerzaTangencial = fuerzaTangencial,
                fuerzaTangencialZ = fuerzaTangencialZ,
                fuerzaTangencialY = fuerzaTangencialY
            };
        }

        private void ResetForm()
        {
            FormDataModel = new CadenaFormDataModel();
        }
    }
}
