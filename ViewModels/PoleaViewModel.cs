using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EjesUI.Helpers;
using EjesUI.Models;
using EjesUI.Services;
using System;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace EjesUI.ViewModels
{
    public partial class PoleaViewModel : ObservableObject, INavigationAware
    {
        private ApiService api;
        private AppConfig appConfig;
        private PdfService pdf;
        private SnackBarService snackbar;

        [ObservableProperty]
        private string _filenamePath = string.Empty;
        [ObservableProperty]
        private BitmapImage _poleaImg = new();
        [ObservableProperty]
        private bool _downloadPDFPoleaButton = false;
        [ObservableProperty]
        private bool _addPoleaButton = true;
        [ObservableProperty]
        private PoleaFormDataModel _formDataModel = new();
        [ObservableProperty]
        private bool _testPoleaToggle = false;

        public PoleaViewModel(ISnackbarService snackbarService)
        {
            this.api = new ApiService();
            this.appConfig = new AppConfig();
            this.pdf = new PdfService();
            this.snackbar = new SnackBarService(snackbarService);
        }

        public void OnNavigatedTo()
        {
            DownloadPDFPoleaButton = false;
            AddPoleaButton = true;
            PoleaImg = ImageProcessor.SetDefaultImage();
        }

        public void OnNavigatedFrom()
        {
            // TODO: Limpiar formulario
        }

        [RelayCommand]
        public void OnClickPDFButton()
        {
            pdf.Download(FilenamePath);
            Process.Start("explorer.exe", this.appConfig.DefaultDownloadPath);
            snackbar.Show("PDF", "PDF Generado!", 2);
        }

        [RelayCommand]
        public void OnClickSaveButton()
        {
            PopulateFormData();

            PoleaCalculateModel calculateData = CalculateComponent();

            Pdf payload = BuildData(calculateData);

            BitmapImage img = ImageProcessor.ProcessImage(payload.images?.frontal.Value);

            string filename = pdf.Generate(payload);

            ExerciseModel.Components.Add(new ComponentModel {
                FormData = FormDataModel,
                Calculate = calculateData
            });

            FilenamePath = filename;
            PoleaImg = img;
            DownloadPDFPoleaButton = true;
            AddPoleaButton = false;

            snackbar.Show("Ejes", "Componente Añadido!", 3);
        }

        private void PopulateFormData()
        {
            string title = "Polea " + ExerciseModel.GetNextComponentLetter();
            Opts opts = new()
            {
                type = "polea",
                subtype = null
            };

            FormDataModel.title = title;
            FormDataModel.opts = opts;

            if (TestPoleaToggle)
            {
                // Ejemplo 1 - Polea 1
                // FormDataModel.peso = 13;
                // FormDataModel.diametro = 4;
                // FormDataModel.potencia = 3;
                // FormDataModel.inclinacion = 90;
                // FormDataModel.relacionTension = 3;
                // FormDataModel.energia = "Consume";
                // FormDataModel.ubicacion = 6;
                // Ejemplo 1 - Polea 2
                //FormDataModel.peso = 13;
                //FormDataModel.diametro = 4;
                //FormDataModel.potencia = 3;
                //FormDataModel.inclinacion = 30;
                //FormDataModel.relacionTension = 3;
                //FormDataModel.energia = "Consume";
                //FormDataModel.ubicacion = 4;

                // Ejemplo 2 - Polea 1
                //FormDataModel.peso = 0;
                //FormDataModel.diametro = 220;
                //FormDataModel.potencia = 8.9484;
                //FormDataModel.inclinacion = 10;
                //FormDataModel.relacionTension = 2.75;
                //FormDataModel.energia = "Recibe";
                //FormDataModel.ubicacion = 150;

                // Ejemplo 3 - Polea 1
                FormDataModel.peso = 0;
                FormDataModel.diametro = 5;
                FormDataModel.potencia = 12;
                FormDataModel.inclinacion = 270;
                FormDataModel.relacionTension = 3;
                FormDataModel.energia = "Recibe";
                FormDataModel.ubicacion = 4;
                return;
            }
        }

        private Pdf BuildData(PoleaCalculateModel data)
        {
            GeneralDataModel generalData = ExerciseModel.GeneralData;
            FormDataModel.opts.system = generalData.unidades ? appConfig.SI : appConfig.FPS;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            dynamic image = api.Get(
                "/polea",
                ("system", FormDataModel.opts.system),
                ("diameter", FormDataModel.diametro.ToString()),
                ("inclination_degree", FormDataModel.inclinacion.ToString()),
                ("orientation", FormDataModel.energia),
                ("direction", generalData.sentidoGiro),
                ("name", ExerciseModel.GetNextComponentLetter())
            );
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            dynamic[] torqueValues = { FormDataModel.potencia, generalData.numeroVuelta, data.torque };
            dynamic[] descompositionTorque = { data.torque, data.radio, FormDataModel.relacionTension, data.T_1, data.T_2, data.fuerzaTension };
            dynamic[] tangentialDescomposition = { data.inclinacion, data.fuerzaTension, data.fuerzaZ, data.fuerzaY };

            Pdf pdf = new();
            pdf.uuid = ExerciseModel.Uuid;
            pdf.title = FormDataModel.title;
            pdf.images = image;
            pdf.opts = FormDataModel.opts;
            pdf.torque_values = new List<dynamic>(torqueValues);
            pdf.descomposition_torque = new List<dynamic>(descompositionTorque);
            pdf.tangential_descomposition = new List<dynamic>(tangentialDescomposition);

            return pdf;
        }

        private PoleaCalculateModel CalculateComponent()
        {
            GeneralDataModel generalData = ExerciseModel.GeneralData;

            double peso = FormDataModel.peso;
            double inclinacion = FormDataModel.inclinacion;
            double radio = generalData.unidades ? (FormDataModel.diametro / 2) / 1000 : FormDataModel.diametro / 2;
            int constante = generalData.unidades ? appConfig.CONSTANTE_TORQUE_SI : appConfig.CONSTANTE_TORQUE_FPS;
            double torque = constante * (FormDataModel.potencia / generalData.numeroVuelta);

            if (
                (generalData.sentidoGiro == "Horario" && FormDataModel.energia == "Recibe") ||
                (generalData.sentidoGiro == "Antihorario" && FormDataModel.energia == "Consume"))
            {
                inclinacion = (-inclinacion * Math.PI) / 180;
            }

            if (
                (generalData.sentidoGiro == "Antihorario" && FormDataModel.energia == "Recibe") ||
                (generalData.sentidoGiro == "Horario" && FormDataModel.energia == "Consume"))
            {
                inclinacion = (inclinacion * Math.PI) / 180;
            }

            double T_2 = torque / (radio * (FormDataModel.relacionTension - 1));
            double T_1 = FormDataModel.relacionTension * T_2;
            double fuerzaTension = T_1 + T_2;
            double fuerzaZ = Math.Round(fuerzaTension * Math.Cos(inclinacion), 3);
            double fuerzaY = Math.Round(fuerzaTension * Math.Sin(inclinacion), 3);
            // TODO: mover esta logica al cs de la page
            //mainHandler.datosPolea.fuerzaFinalZ.Add(fuerzaZ);
            //mainHandler.datosPolea.fuerzaFinalY.Add(fuerzaY - FormDataModel.peso);

            return new PoleaCalculateModel
            {
                radio = radio,
                torque = torque,
                T_1 = T_1,
                T_2 = T_2,
                fuerzaTension = fuerzaTension,
                inclinacion = inclinacion,
                fuerzaZ = fuerzaZ,
                fuerzaY = fuerzaY
            };
        }
    }
}
