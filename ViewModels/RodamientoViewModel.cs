﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EjesUI.Helpers;
using EjesUI.Models;
using EjesUI.Services;
using System;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using Wpf.Ui.Common.Interfaces;

namespace EjesUI.ViewModels
{
    public partial class RodamientoViewModel : ObservableObject, INavigationAware
    {
        private ApiService api;
        private AppConfig appConfig;
        private PdfService pdf;

        [ObservableProperty]
        private string _filenamePath = string.Empty;
        [ObservableProperty]
        private BitmapImage _rodamientoImg = new();
        [ObservableProperty]
        private bool _downloadPDFRodamientoButton = false;
        [ObservableProperty]
        private RodamientoFormDataModel _formDataModel = new();
        [ObservableProperty]
        private bool _testRodamientoToggle = false;

        private int position = 1;

        public RodamientoViewModel()
        {
            this.api = new ApiService();
            this.appConfig = new AppConfig();
            this.pdf = new PdfService();
        }

        public void OnNavigatedTo()
        {
            DownloadPDFRodamientoButton = false;
            RodamientoImg = ImageProcessor.SetDefaultImage();
        }

        public void OnNavigatedFrom()
        {
        }

        [RelayCommand]
        public void OnClickPDFButton()
        {
            pdf.Download(FilenamePath);
            Process.Start("explorer.exe", this.appConfig.DefaultDownloadPath);
        }

        [RelayCommand]
        public void OnClickSaveButton()
        {
            // var generalData = new GeneralDataModel();
            // if (TestRodamientoToggle)
            // {
            //     // Ejemplo 1
            //     //generalData.numeroVuelta = 480;
            //     //generalData.confiabilidad = 2;
            //     //generalData.limiteFluencia = 2;
            //     //generalData.limiteMaximaFractura = 4;
            //     //generalData.factorSeguridad = 5;
            //     //generalData.coeficienteGlobal = 6;
            //     //generalData.factorConcentradorEsfuerzoFlexion = 7;
            //     //generalData.factorConcentradorEsfuerzoTorsion = 8;
            //     //generalData.sentidoGiro = "Horario";
            //     //generalData.uuid = "uuid";
            //     //generalData.sistemaUnidades = "FPS";

            //     // Ejemplo 2
            //     //generalData.numeroVuelta = 1000;
            //     //generalData.confiabilidad = 1; //Falta dato
            //     //generalData.limiteFluencia = 700;
            //     //generalData.limiteMaximaFractura = 600;
            //     //generalData.factorSeguridad = 3;
            //     //generalData.coeficienteGlobal = 0.6;
            //     //generalData.factorConcentradorEsfuerzoFlexion = 1.6;
            //     //generalData.factorConcentradorEsfuerzoTorsion = 1.4;
            //     //generalData.sentidoGiro = "Horario";
            //     //generalData.uuid = "uuid";
            //     //generalData.sistemaUnidades = "SI";

            //     // Ejemplo 3
            //     generalData.numeroVuelta = 500;
            //     generalData.confiabilidad = 1; //Falta dato
            //     generalData.limiteFluencia = 2;
            //     generalData.limiteMaximaFractura = 3;
            //     generalData.factorSeguridad = 4;
            //     generalData.coeficienteGlobal = 5;
            //     generalData.factorConcentradorEsfuerzoFlexion = 6;
            //     generalData.factorConcentradorEsfuerzoTorsion = 7;
            //     generalData.sentidoGiro = "Antihorario";
            //     generalData.uuid = "uuid";
            //     generalData.sistemaUnidades = "FPS";
            // }

            // PopulateFormData();

            // RodamientoCalculateModel calculateData = CalculateComponent(generalData);

            // Pdf payload = BuildData(calculateData, generalData);

            // BitmapImage img = ImageProcessor.ProcessImage(payload.images?.frontal.Value);

            // string filename = pdf.Generate(payload);

            // FilenamePath = filename;
            // RodamientoImg = img;
            // DownloadPDFRodamientoButton = true;
        }

        private void PopulateFormData()
        {
            // string title = "Rodamiento A";
            // Opts opts = new Opts
            // {
            //     type = "rodamiento",
            //     subtype = null
            // };

            // FormDataModel.title = title;
            // FormDataModel.opts = opts;

            // if (TestRodamientoToggle)
            // {
            //     // Ejemplo 1 - Rodamiento 1
            //     //FormDataModel.peso = 13;
            //     //FormDataModel.diametro = 4;
            //     //FormDataModel.potencia = 3;
            //     //FormDataModel.inclinacion = 90;
            //     //FormDataModel.relacionTension = 3;
            //     //FormDataModel.energia = "Consume";
            //     //FormDataModel.ubicacion = 6;
            //     // Ejemplo 1 - Rodamiento 2
            //     //FormDataModel.peso = 13;
            //     //FormDataModel.diametro = 4;
            //     //FormDataModel.potencia = 3;
            //     //FormDataModel.inclinacion = 30;
            //     //FormDataModel.relacionTension = 3;
            //     //FormDataModel.energia = "Consume";
            //     //FormDataModel.ubicacion = 4;

            //     // Ejemplo 2 - Rodamiento 1
            //     //FormDataModel.peso = 0;
            //     //FormDataModel.diametro = 220;
            //     //FormDataModel.potencia = 8.9484;
            //     //FormDataModel.inclinacion = 10;
            //     //FormDataModel.relacionTension = 2.75;
            //     //FormDataModel.energia = "Recibe";
            //     //FormDataModel.ubicacion = 150;

            //     // Ejemplo 3 - Rodamiento 1
            //     FormDataModel.peso = 0;
            //     FormDataModel.diametro = 5;
            //     FormDataModel.potencia = 12;
            //     FormDataModel.inclinacion = 270;
            //     FormDataModel.relacionTension = 3;
            //     FormDataModel.energia = "Recibe";
            //     FormDataModel.ubicacion = 4;
            //     return;
            // }
        }

//         private Pdf BuildData(RodamientoCalculateModel data, GeneralDataModel generalData)
//         {
// #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
//             dynamic image = api.Get(
//                 "/rodamiento",
//                 ("system", generalData.sistemaUnidades),
//                 ("diameter", FormDataModel.diametro.ToString()),
//                 ("inclination_degree", FormDataModel.inclinacion.ToString()),
//                 ("orientation", FormDataModel.energia),
//                 ("direction", generalData.sentidoGiro),
//                 ("name", position.ToString())
//             );
// #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

//             FormDataModel.opts.system = generalData.sistemaUnidades;
//             dynamic[] torqueValues = { FormDataModel.potencia, generalData.numeroVuelta, data.torque };
//             dynamic[] descompositionTorque = { data.torque, data.radio, FormDataModel.relacionTension, data.T_1, data.T_2, data.fuerzaTension };
//             dynamic[] tangentialDescomposition = { data.inclinacion, data.fuerzaTension, data.fuerzaZ, data.fuerzaY };

//             Pdf pdf = new Pdf();
//             pdf.uuid = generalData.uuid;
//             pdf.title = FormDataModel.title;
//             pdf.images = image;
//             pdf.opts = FormDataModel.opts;
//             pdf.torque_values = new List<dynamic>(torqueValues);
//             pdf.descomposition_torque = new List<dynamic>(descompositionTorque);
//             pdf.tangential_descomposition = new List<dynamic>(tangentialDescomposition);

//             return pdf;
//         }

//         private RodamientoCalculateModel CalculateComponent(GeneralDataModel generalData)
//         {
//             double peso = FormDataModel.peso;
//             double inclinacion = FormDataModel.inclinacion;
//             double radio = (generalData.sistemaUnidades == "SI") ? (FormDataModel.diametro / 2) / 1000 : FormDataModel.diametro / 2;
//             int constante = (generalData.sistemaUnidades == "SI") ? appConfig.CONSTANTE_TORQUE_SI : appConfig.CONSTANTE_TORQUE_FPS;
//             double torque = constante * (FormDataModel.potencia / generalData.numeroVuelta);

//             if (
//                 (generalData.sentidoGiro == "Horario" && FormDataModel.energia == "Recibe") ||
//                 (generalData.sentidoGiro == "Antihorario" && FormDataModel.energia == "Consume"))
//             {
//                 inclinacion = (-inclinacion * Math.PI) / 180;
//             }

//             if (
//                 (generalData.sentidoGiro == "Antihorario" && FormDataModel.energia == "Recibe") ||
//                 (generalData.sentidoGiro == "Horario" && FormDataModel.energia == "Consume"))
//             {
//                 inclinacion = (inclinacion * Math.PI) / 180;
//             }

//             double T_2 = torque / (radio * (FormDataModel.relacionTension - 1));
//             double T_1 = FormDataModel.relacionTension * T_2;
//             double fuerzaTension = T_1 + T_2;
//             double fuerzaZ = Math.Round(fuerzaTension * Math.Cos(inclinacion), 3);
//             double fuerzaY = Math.Round(fuerzaTension * Math.Sin(inclinacion), 3);
//             // TODO: mover esta logica al cs de la page
//             //mainHandler.datosRodamiento.fuerzaFinalZ.Add(fuerzaZ);
//             //mainHandler.datosRodamiento.fuerzaFinalY.Add(fuerzaY - double.Parse(peso));

//             return new RodamientoCalculateModel
//             {
//                 radio = radio,
//                 torque = torque,
//                 T_1 = T_1,
//                 T_2 = T_2,
//                 fuerzaTension = fuerzaTension,
//                 inclinacion = inclinacion,
//                 fuerzaZ = fuerzaZ,
//                 fuerzaY = fuerzaY
//             };
//         }
    }
}
