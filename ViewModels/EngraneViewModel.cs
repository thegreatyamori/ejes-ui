using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EjesUI.Helpers;
using EjesUI.Models;
using EjesUI.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;


namespace EjesUI.ViewModels
{
    public partial class EngraneViewModel : ObservableObject, INavigationAware
    {
        private readonly ApiService api;
        private readonly AppConfig appConfig;
        private readonly PdfService pdf;
        private readonly SnackBarNotifierService snackbar;

        [ObservableProperty]
        private string  _filenamePath = string.Empty;
        [ObservableProperty]
        private BitmapImage _engraneFrontalImg = new();
        [ObservableProperty]
        private BitmapImage _engraneLateralImg = new();
        [ObservableProperty]
        private bool _downloadPDFEngraneButton = false;
        [ObservableProperty]
        private bool _addEngraneButton = true;
        [ObservableProperty]
        private EngraneFormDataModel _formDataModel = new();
        [ObservableProperty]
        private bool _testEngraneToggle = false;
        [ObservableProperty]
        private Visibility _displayHelice = Visibility.Visible;
        [ObservableProperty]
        private string _pesoPH = string.Empty;
        [ObservableProperty]
        private string _diametroPH = string.Empty;
        [ObservableProperty]
        private string _potenciaPH = string.Empty;
        [ObservableProperty]
        private string _inclinacionPH = string.Empty;

        public EngraneViewModel(ISnackbarService snackbarService)
        {
            this.api = new ApiService();
            this.appConfig = new AppConfig();
            this.pdf = new PdfService();
            this.snackbar = new SnackBarNotifierService(snackbarService);
        }

        public void OnNavigatedTo()
        {
            DownloadPDFEngraneButton = false;
            AddEngraneButton = true;
            EngraneFrontalImg = ImageProcessor.SetDefaultImage("https://via.placeholder.com/345x143.png");
            EngraneLateralImg = ImageProcessor.SetDefaultImage("https://via.placeholder.com/345x143.png");

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
        public void OnTypeSelectChange()
        {
            snackbar.Show("1", "selected", 3);
        }

        [RelayCommand]
        public void ClearForm()
        {
            ResetForm();
            AddEngraneButton = true;
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
            PopulateFormData();

            EngraneCalculateModel calculateData = CalculateComponent();

            Pdf payload = BuildData(calculateData);

            BitmapImage img = ImageProcessor.ProcessImage(payload.images?.frontal.Value);

            string filename = pdf.Generate(payload);

            FilenamePath = filename;
            EngraneFrontalImg = img;

            if (!string.IsNullOrEmpty(payload.images?.lateral.Value))
            {
                BitmapImage imgL = ImageProcessor.ProcessImage(payload.images?.lateral.Value);
                EngraneLateralImg = imgL;
            }

            ExerciseModel.Components.Add(new ComponentModel
            {
                FormData = FormDataModel,
                Calculate = calculateData
            });

            DownloadPDFEngraneButton = true;
            AddEngraneButton = false;

            snackbar.Show("Ejes", "Componente Añadido!", 3);

        }

        private void PopulateFormData()
        {
            var tipo = FormDataModel.tipo.Split(":")[1].Trim();
            FormDataModel.title = "Engrane " + ExerciseModel.GetNextComponentLetter();
            FormDataModel.opts = new Opts
            {
                type = "engrane",
                subtype = tipo
            };

            if (TestEngraneToggle)
            {
                // Ejemplo 1 - Engrane Recto
                FormDataModel.tipo = "Recto";
                FormDataModel.opts.subtype = "Recto";
                FormDataModel.ubicacion = 4;
                FormDataModel.energia = "Consume";
                FormDataModel.peso = 5;
                FormDataModel.diametro = 3;
                FormDataModel.potencia = 5;
                FormDataModel.presion = 20;
                FormDataModel.inclinacion = 270;
                FormDataModel.helice = 0;
                FormDataModel.direccionFuerzaAxial = "";

                // Ejemplo 2 - Engrane Cónico
                //FormDataModel.tipo = "Conico";
                // FormDataModel.opts.subtype = "Conico";
                // FormDataModel.ubicacion = 0;
                // FormDataModel.energia = "Consume";
                //FormDataModel.peso = 0;
                //FormDataModel.diametro = 140;
                //FormDataModel.potencia = 3.191;
                //FormDataModel.presion = 20;
                //FormDataModel.inclinacion = 180;
                //FormDataModel.helice = 25;
                //FormDataModel.direccionFuerzaAxial = "Derecha";

                // Ejemplo 2 - Engrane Helicoidal
                // FormDataModel.tipo = "Helicoidal";
                // FormDataModel.opts.subtype = "Helicoidal";
                // FormDataModel.ubicacion = 170;
                // FormDataModel.energia = "Consume";
                // FormDataModel.peso = 0;
                // FormDataModel.diametro = 110;
                // FormDataModel.potencia = 5.816;
                // FormDataModel.presion = 20;
                // FormDataModel.inclinacion = 150;
                // FormDataModel.helice = 20;
                // FormDataModel.direccionFuerzaAxial = "Derecha";

                // Ejemplo 3 - Engrane Recto
                // FormDataModel.tipo = "Recto";
                // FormDataModel.opts.subtype = "Recto";
                // FormDataModel.ubicacion = 4;
                // FormDataModel.energia = "Consume";
                // FormDataModel.peso = 6;
                // FormDataModel.diametro = 4;
                // FormDataModel.potencia = 3;
                // FormDataModel.presion = 20;
                // FormDataModel.inclinacion = 90;
                // FormDataModel.helice = 0;
                // FormDataModel.direccionFuerzaAxial = "";

                // Ejemplo 3.1 - Engrane Recto
                // FormDataModel.tipo = "Recto";
                // FormDataModel.opts.subtype = "Recto";
                // FormDataModel.ubicacion = 6;
                // FormDataModel.energia = "Consume";
                // FormDataModel.peso = 6;
                // FormDataModel.diametro = 4;
                // FormDataModel.potencia = 4;
                // FormDataModel.presion = 20;
                // FormDataModel.inclinacion = 330;
                // FormDataModel.helice = 0;
                // FormDataModel.direccionFuerzaAxial = "";

                // Ejemplo 4 - Engrane Recto
                // FormDataModel.tipo = "Recto";
                // FormDataModel.opts.subtype = "Recto";
                // FormDataModel.ubicacion = 180;
                // FormDataModel.energia = "Recibe";
                // FormDataModel.peso = 0;
                // FormDataModel.diametro = 100;
                // FormDataModel.potencia = 43.156;
                // FormDataModel.presion = 20;
                // FormDataModel.inclinacion = 90;
                // FormDataModel.helice = 0;
                // FormDataModel.direccionFuerzaAxial = "";

                // Ejemplo 4 - Engrane Helicoidal
                // FormDataModel.tipo = "Helicoidal";
                // FormDataModel.opts.subtype = "Helicoidal";
                // FormDataModel.ubicacion = 300;
                // FormDataModel.energia = "Consume";
                // FormDataModel.peso = 0;
                // FormDataModel.diametro = 173.22 * 2;
                // FormDataModel.potencia = 43.156;
                // FormDataModel.presion = 20;
                // FormDataModel.inclinacion = 90;
                // FormDataModel.helice = 30;
                // FormDataModel.direccionFuerzaAxial = "Derecha";

                return;
            }

            if (!FormDataModel.direccionFuerzaAxial.Equals("Dir. F. Axial"))
            {
                FormDataModel.direccionFuerzaAxial = FormDataModel.direccionFuerzaAxial.Split(":")[1].Trim();
            } else
            {
                FormDataModel.direccionFuerzaAxial = "";
            }
            FormDataModel.tipo = tipo;
            FormDataModel.energia = FormDataModel.energia.Split(":")[1].Trim();
        }

        private Pdf BuildData(EngraneCalculateModel data)
        {
            GeneralDataModel generalData = ExerciseModel.GeneralData;
            FormDataModel.opts.system = generalData.unidades ? appConfig.SI : appConfig.FPS;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            dynamic images = api.Get(
                "/engrane",
                ("system", FormDataModel.opts.system),
                ("diameter", FormDataModel.diametro.ToString()),
                ("inclination_degree", FormDataModel.inclinacion.ToString()),
                ("orientation", FormDataModel.energia),
                ("type", FormDataModel.tipo.ToString()),
                ("direction", generalData.sentidoGiro),
                ("name", ExerciseModel.GetNextComponentLetter()),
                ("directionFa", FormDataModel.direccionFuerzaAxial.ToString())
            );
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            List<dynamic> torqueValues = new();
            List<dynamic?> generalForces = new();
            List<dynamic> tangentialDescomposition = new();
            List<dynamic> radialDescomposition = new();
            List<dynamic> axialDescomposition = new();
            List<dynamic> descompositionZ = new();
            List<dynamic> descompositionY = new();
            List<dynamic> descomposition_torque = new();
            List<dynamic> momentDescomposition = new();
            List<dynamic> anguloTransversalPresion = new();

            if (FormDataModel.tipo.Equals("Recto"))
            {
                torqueValues = new List<dynamic> { FormDataModel.potencia, generalData.numeroVuelta, data.torque };
                generalForces = new List<dynamic?> { data.torque, data.fuerzaTangencial, data.fuerzaRadial, data.radio, FormDataModel.presion, null };
                tangentialDescomposition = new List<dynamic>{ data.inclinacionTangencial, data.fuerzaTangencial, data.fuerzaTangencialZ, data.fuerzaTangencialY };
                radialDescomposition = new List<dynamic>{ data.inclinacionRadial, data.fuerzaRadial, data.fuerzaRadialZ, data.fuerzaRadialY };
                descompositionZ = new List<dynamic>{ data.fuerzaRadialZ, data.fuerzaTangencialZ, data.fuerzaZ };
                descompositionY = new List<dynamic>{ data.fuerzaRadialY, data.fuerzaTangencialY, data.fuerzaY };
            }

            if (FormDataModel.tipo.Equals("Helicoidal"))
            {
                torqueValues = new List<dynamic> { FormDataModel.potencia, generalData.numeroVuelta, data.torque };
                generalForces = new List<dynamic?> { data.torque, data.fuerzaTangencial, data.fuerzaRadial, data.radio, FormDataModel.presion, FormDataModel.helice };
                tangentialDescomposition = new List<dynamic> { data.inclinacionTangencial, data.fuerzaTangencial, data.fuerzaTangencialZ, data.fuerzaTangencialY };
                radialDescomposition = new List<dynamic> { data.inclinacionRadial, data.fuerzaRadial, data.fuerzaRadialZ, data.fuerzaRadialY };
                descompositionZ = new List<dynamic> { data.fuerzaRadialZ, data.fuerzaTangencialZ, data.fuerzaZ };
                descompositionY = new List<dynamic> { data.fuerzaRadialY, data.fuerzaTangencialY, data.fuerzaY };
                if (data.GetType() == typeof(EngraneHelicoidalCalculateModel))
                {
                    EngraneHelicoidalCalculateModel dataHelicoidal = (EngraneHelicoidalCalculateModel)data;
                    momentDescomposition = new List<dynamic> { data.radio, FormDataModel.inclinacion, dataHelicoidal.fuerzaAxial, dataHelicoidal.momento, dataHelicoidal.momentoZ, dataHelicoidal.momentoY };
                    axialDescomposition = new List<dynamic> { FormDataModel.presion, null, data.fuerzaTangencial, dataHelicoidal.fuerzaAxial };
                    anguloTransversalPresion = new List<dynamic> { FormDataModel.presion, FormDataModel.helice, dataHelicoidal.anguloTransversalPresion };
                }
            }

            if (FormDataModel.tipo.Equals("Conico"))
            {
                torqueValues = new List<dynamic> { FormDataModel.potencia, generalData.numeroVuelta, data.torque };
                generalForces = new List<dynamic?> { data.torque, data.fuerzaTangencial, data.fuerzaRadial, data.radio, FormDataModel.presion, FormDataModel.helice };
                tangentialDescomposition = new List<dynamic> { data.inclinacionTangencial, data.fuerzaTangencial, data.fuerzaTangencialZ, data.fuerzaTangencialY };
                radialDescomposition = new List<dynamic> { data.inclinacionRadial, data.fuerzaRadial, data.fuerzaRadialZ, data.fuerzaRadialY };
                descompositionZ = new List<dynamic> { data.fuerzaRadialZ, data.fuerzaTangencialZ, data.fuerzaZ };
                descompositionY = new List<dynamic> { data.fuerzaRadialY, data.fuerzaTangencialY, data.fuerzaY };

                if (data.GetType() == typeof(EngraneConicoCalculateModel))
                {
                    EngraneConicoCalculateModel dataConico = (EngraneConicoCalculateModel) data;
                    momentDescomposition = new List<dynamic> { data.radio, FormDataModel.inclinacion, dataConico.fuerzaAxial, dataConico.momento, dataConico.momentoZ, dataConico.momentoY };
                    axialDescomposition = new List<dynamic> { FormDataModel.presion, FormDataModel.helice, data.fuerzaTangencial, dataConico.fuerzaAxial };
                }
            }

            Pdf pdf = new()
            {
                uuid = ExerciseModel.Uuid,
                title = FormDataModel.title,
                images = images,
                opts = FormDataModel.opts,
                torque_values = new List<dynamic>(torqueValues),
                general_forces = new List<dynamic?>(generalForces),
                tangential_descomposition = new List<dynamic>(tangentialDescomposition),
                radial_descomposition = new List<dynamic>(radialDescomposition),
                axial_descomposition = new List<dynamic>(axialDescomposition),
                descomposition_z = new List<dynamic>(descompositionZ),
                descomposition_y = new List<dynamic>(descompositionY),
                descomposition_torque = new List<dynamic>(descomposition_torque),
                moment_descomposition = new List<dynamic>(momentDescomposition),
                angulo_transversal_presion = new List<dynamic>(anguloTransversalPresion)
            };
            return pdf;
        }

        private EngraneCalculateModel CalculateComponent()
        {
            if (FormDataModel.tipo.Equals("Recto"))
            {
                return CalculateRectoComponent();
            }

            if (FormDataModel.tipo.Equals("Helicoidal"))
            {
                return CalculateHelicoidalModel();
            }

            return CalculateConicoModel();
        }

        private EngraneCalculateModel CalculateRectoComponent()
        {
            GeneralDataModel generalData = ExerciseModel.GeneralData;

            int constante = generalData.unidades ? appConfig.CONSTANTE_TORQUE_SI : appConfig.CONSTANTE_TORQUE_FPS;
            double torque = constante * (FormDataModel.potencia / generalData.numeroVuelta);
            double radio = generalData.unidades ? (FormDataModel.diametro / 2) / 1000 : FormDataModel.diametro / 2;
            double fuerzaTangencial = torque / radio;
            double fuerzaRadial = fuerzaTangencial * Math.Tan(FormDataModel.presion * Math.PI / 180);

            // Angulos
            double inclinacionTangencial = 0;
            double inclinacionRadial = 0;
            if (
                (generalData.sentidoGiro.Equals("Horario") && FormDataModel.energia.Equals("Recibe")) ||
                (generalData.sentidoGiro.Equals("Antihorario") && FormDataModel.energia.Equals("Consume")))
            {
                inclinacionTangencial = (-(FormDataModel.inclinacion + 90) * Math.PI) / 180;
                inclinacionRadial = (-(FormDataModel.inclinacion + 180) * Math.PI) / 180;
            }

            if (
                (generalData.sentidoGiro.Equals("Antihorario") && FormDataModel.energia.Equals("Recibe")) ||
                (generalData.sentidoGiro.Equals("Horario") && FormDataModel.energia == "Consume"))
            {
                inclinacionTangencial = ((FormDataModel.inclinacion + 90) * Math.PI) / 180;
                inclinacionRadial = ((FormDataModel.inclinacion + 180) * Math.PI) / 180;
            }

            // Fuerzas Z
            double fuerzaTangencialZ = Math.Round(fuerzaTangencial * Math.Cos(inclinacionTangencial), 3);
            double fuerzaRadialZ = Math.Round(fuerzaRadial * Math.Cos(inclinacionRadial), 3);
            double fuerzaZ = fuerzaRadialZ + fuerzaTangencialZ;
            // Fuerzas Y
            double fuerzaTangencialY = Math.Round(fuerzaTangencial * Math.Sin(inclinacionTangencial), 3);
            double fuerzaRadialY = Math.Round(fuerzaRadial * Math.Sin(inclinacionRadial), 3);
            double fuerzaY = fuerzaRadialY + fuerzaTangencialY;

            return new EngraneCalculateModel
            {
                radio = radio,
                torque = torque,
                fuerzaTangencial = fuerzaTangencial,
                fuerzaTangencialZ = fuerzaTangencialZ,
                fuerzaTangencialY = fuerzaTangencialY,
                fuerzaRadial = fuerzaRadial,
                fuerzaRadialZ = fuerzaRadialZ,
                fuerzaRadialY = fuerzaRadialY,
                fuerzaY = fuerzaY,
                fuerzaZ = fuerzaZ,
                inclinacionTangencial = inclinacionTangencial,
                inclinacionRadial = inclinacionRadial
            };
        }

        private EngraneHelicoidalCalculateModel CalculateHelicoidalModel()
        {
            GeneralDataModel generalData = ExerciseModel.GeneralData;

            int constante = generalData.unidades ? appConfig.CONSTANTE_TORQUE_SI : appConfig.CONSTANTE_TORQUE_FPS;
            double torque = constante * (FormDataModel.potencia / generalData.numeroVuelta);
            double radio = generalData.unidades ? (FormDataModel.diametro / 2) / 1000 : FormDataModel.diametro / 2;
            double fuerzaTangencial = torque / radio;
            double anguloTransversalPresion = Math.Atan(Math.Tan(FormDataModel.presion * Math.PI / 180) / Math.Cos(FormDataModel.helice * Math.PI / 180));
            double fuerzaRadial = fuerzaTangencial * Math.Tan(anguloTransversalPresion);
            double fuerzaAxial = fuerzaTangencial * Math.Tan(FormDataModel.helice * Math.PI / 180);
            double momento = fuerzaAxial * radio;

            // Angulos
            double inclinacionTangencial = 0;
            double inclinacionRadial = 0;
            if (
                (generalData.sentidoGiro.Equals("Horario") && FormDataModel.energia.Equals("Recibe")) ||
                (generalData.sentidoGiro.Equals("Antihorario") && FormDataModel.energia.Equals("Consume")))
            {
                inclinacionTangencial = (-(FormDataModel.inclinacion + 90) * Math.PI) / 180;
                inclinacionRadial = (-(FormDataModel.inclinacion + 180) * Math.PI) / 180;
            }

            if (
                (generalData.sentidoGiro.Equals("Antihorario") && FormDataModel.energia.Equals("Recibe")) ||
                (generalData.sentidoGiro.Equals("Horario") && FormDataModel.energia == "Consume"))
            {
                inclinacionTangencial = ((FormDataModel.inclinacion + 90) * Math.PI) / 180;
                inclinacionRadial = ((FormDataModel.inclinacion + 180) * Math.PI) / 180;
            }

            // Fuerzas Z
            double fuerzaTangencialZ = Math.Round(fuerzaTangencial * Math.Cos(inclinacionTangencial), 3);
            double fuerzaRadialZ = Math.Round(fuerzaRadial * Math.Cos(inclinacionRadial), 3);
            double fuerzaZ = fuerzaRadialZ + fuerzaTangencialZ;
            // Fuerzas Y
            double fuerzaTangencialY = Math.Round(fuerzaTangencial * Math.Sin(inclinacionTangencial), 3);
            double fuerzaRadialY = Math.Round(fuerzaRadial * Math.Sin(inclinacionRadial), 3);
            double fuerzaY = fuerzaRadialY + fuerzaTangencialY;
            // Momento
            double momentoZ = momento * Math.Cos(inclinacionTangencial);
            double momentoY = momento * Math.Sin(inclinacionTangencial);
            // TODO: mover esta logica al cs de la page
            //mainHandler.datoEngranes.fuerzaFinalZ.Add(fuerzaZ);
            //mainHandler.datoEngranes.fuerzaFinalY.Add(fuerzaY - FormDataModel.peso);
            //mainHandler.datoEngranes.momentoFinalZ.Add(momentoZ);
            //mainHandler.datoEngranes.momentoFinalY.Add(momentoY);

            return new EngraneHelicoidalCalculateModel
            {
                radio = radio,
                torque = torque,
                fuerzaAxial = fuerzaAxial,
                momento = momento,
                momentoZ = momentoZ,
                momentoY = momentoY,
                anguloTransversalPresion = anguloTransversalPresion,
                inclinacionTangencial = inclinacionTangencial,
                inclinacionRadial = inclinacionRadial,
                fuerzaTangencial = fuerzaTangencial,
                fuerzaTangencialZ = fuerzaTangencialZ,
                fuerzaTangencialY = fuerzaTangencialY,
                fuerzaRadial = fuerzaRadial,
                fuerzaRadialZ = fuerzaRadialZ,
                fuerzaRadialY = fuerzaRadialY,
                fuerzaZ = fuerzaZ,
                fuerzaY = fuerzaY
            };
        }

        private EngraneConicoCalculateModel CalculateConicoModel()
        {
            GeneralDataModel generalData = ExerciseModel.GeneralData;

            int constante = generalData.unidades ? appConfig.CONSTANTE_TORQUE_SI : appConfig.CONSTANTE_TORQUE_FPS;
            double torque = constante * (FormDataModel.potencia / generalData.numeroVuelta);
            double radio = generalData.unidades ? (FormDataModel.diametro / 2) / 1000 : FormDataModel.diametro / 2;
            double fuerzaTangencial = torque / radio;
            double fuerzaRadial = fuerzaTangencial * Math.Tan(FormDataModel.presion * Math.PI / 180) * Math.Cos(FormDataModel.helice * Math.PI / 180);
            double fuerzaAxial = fuerzaTangencial * Math.Tan(FormDataModel.presion * Math.PI / 180) * Math.Sin(FormDataModel.helice * Math.PI / 180);
            double momento = fuerzaAxial * radio;

            // Angulos
            double inclinacionTangencial = 0;
            double inclinacionRadial = 0;
            if (
                (generalData.sentidoGiro.Equals("Horario") && FormDataModel.energia.Equals("Recibe")) ||
                (generalData.sentidoGiro.Equals("Antihorario") && FormDataModel.energia.Equals("Consume")))
            {
                inclinacionTangencial = (-(FormDataModel.inclinacion + 90) * Math.PI) / 180;
                inclinacionRadial = (-(FormDataModel.inclinacion + 180) * Math.PI) / 180;
            }

            if (
                (generalData.sentidoGiro.Equals("Antihorario") && FormDataModel.energia.Equals("Recibe")) ||
                (generalData.sentidoGiro.Equals("Horario") && FormDataModel.energia == "Consume"))
            {
                inclinacionTangencial = ((FormDataModel.inclinacion + 90) * Math.PI) / 180;
                inclinacionRadial = ((FormDataModel.inclinacion + 180) * Math.PI) / 180;
            }

            // Fuerzas Z
            double fuerzaTangencialZ = Math.Round(fuerzaTangencial * Math.Cos(inclinacionTangencial), 3);
            double fuerzaRadialZ = Math.Round(fuerzaRadial * Math.Cos(inclinacionRadial), 3);
            double fuerzaZ = fuerzaRadialZ + fuerzaTangencialZ;
            // Fuerzas Y
            double fuerzaTangencialY = Math.Round(fuerzaTangencial * Math.Sin(inclinacionTangencial), 3);
            double fuerzaRadialY = Math.Round(fuerzaRadial * Math.Sin(inclinacionRadial), 3);
            double fuerzaY = fuerzaRadialY + fuerzaTangencialY;
            // Momento
            double momentoZ = momento * Math.Cos(inclinacionTangencial);
            double momentoY = momento * Math.Sin(inclinacionTangencial);
            // TODO: mover esta logica al cs de la page
            //mainHandler.datoEngranes.fuerzaFinalZ.Add(fuerzaZ);
            //mainHandler.datoEngranes.fuerzaFinalY.Add(fuerzaY - FormDataModel.peso);
            //mainHandler.datoEngranes.momentoFinalZ.Add(momentoZ);
            //mainHandler.datoEngranes.momentoFinalY.Add(momentoY);

            return new EngraneConicoCalculateModel
            {
                radio = radio,
                torque = torque,
                fuerzaAxial = fuerzaAxial,
                momento = momento,
                momentoZ = momentoZ,
                momentoY = momentoY,
                inclinacionTangencial = inclinacionTangencial,
                inclinacionRadial = inclinacionRadial,
                fuerzaTangencial = fuerzaTangencial,
                fuerzaTangencialZ = fuerzaTangencialZ,
                fuerzaTangencialY = fuerzaTangencialY,
                fuerzaRadial = fuerzaRadial,
                fuerzaRadialZ = fuerzaRadialZ,
                fuerzaRadialY = fuerzaRadialY,
                fuerzaZ = fuerzaZ,
                fuerzaY = fuerzaY
            };
        }

        private void ResetForm()
        {
            FormDataModel = new EngraneFormDataModel();
        }
    }
}
