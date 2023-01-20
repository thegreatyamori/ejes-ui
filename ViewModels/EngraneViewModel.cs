using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EjesUI.Helpers;
using EjesUI.Models;
using EjesUI.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using Wpf.Ui.Common.Interfaces;

namespace EjesUI.ViewModels
{
    public partial class EngraneViewModel : ObservableObject, INavigationAware
    {
        private ApiService api;
        private AppConfig appConfig;
        private PdfService pdf;

        [ObservableProperty]
        private string  _filenamePath = string.Empty;
        [ObservableProperty]
        private BitmapImage _engraneFrontalImg = new BitmapImage();
        [ObservableProperty]
        private BitmapImage _engraneLateralImg = new BitmapImage();
        [ObservableProperty]
        private bool _downloadPDFEngraneButton = false;
        [ObservableProperty]
        private EngraneFormDataModel _formDataModel = new EngraneFormDataModel();
        [ObservableProperty]
        private bool _testEngraneToggle = false;

        public EngraneViewModel()
        {
            this.api = new ApiService();
            this.appConfig = new AppConfig();
            this.pdf = new PdfService();
        }

        public void OnNavigatedTo()
        {
            DownloadPDFEngraneButton = false;
            EngraneFrontalImg = ImageProcessor.SetDefaultImage("https://via.placeholder.com/345x143.png");
            EngraneLateralImg = ImageProcessor.SetDefaultImage("https://via.placeholder.com/345x143.png");
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
            EngraneLateralImg = img;
            DownloadPDFEngraneButton = true;
        }

        private void PopulateFormData()
        {
            FormDataModel.title = "Engrane";
            FormDataModel.opts = new Opts
            {
                type = "engrane",
                subtype = FormDataModel.tipo
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
                // FormDataModel.tipo = "Conico";
                // FormDataModel.opts.subtype = "Conico";
                // FormDataModel.ubicacion = 0;
                // FormDataModel.energia = "Consume";
                // FormDataModel.peso = 0;
                // FormDataModel.diametro = 70;
                // FormDataModel.potencia = 3.191;
                // FormDataModel.presion = 20;
                // FormDataModel.inclinacion = 270;
                // FormDataModel.helice = 25;
                // FormDataModel.direccionFuerzaAxial = "Derecha";

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
        }

        private Pdf BuildData(EngraneCalculateModel data)
        {
            GeneralDataModel generalData = ExerciseModel.GeneralData;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            dynamic images = api.Get(
                "/engrane",
                ("system", generalData.sistemaUnidades),
                ("diameter", FormDataModel.diametro.ToString()),
                ("inclination_degree", FormDataModel.inclinacion.ToString()),
                ("orientation", FormDataModel.energia),
                ("type", FormDataModel.tipo.ToString()),
                ("direction", generalData.sentidoGiro),
                ("name", ExerciseModel.GetNextComponentLetter())
            );
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            dynamic[] torqueValues = {};
            dynamic?[] generalForces = {};
            dynamic[] tangentialDescomposition = {};
            dynamic[] radialDescomposition = {};
            dynamic[] axialDescomposition = {};
            dynamic[] descompositionZ = {};
            dynamic[] descompositionY = {};
            dynamic[] descomposition_torque = {};
            dynamic[] momentDescomposition = {};
            dynamic[] anguloTransversalPresion = {};
            FormDataModel.opts.system = generalData.sistemaUnidades;

            //if (FormDataModel.tipo.Equals("Recto"))
            //{
            //    torqueValues = { FormDataModel.potencia, generalData.numeroVuelta, data.torque };
            //    generalForces = { data.torque, data.fuerzaTangencial, data.fuerzaRadial, data.radio, FormDataModel.presion, null };
            //    tangentialDescomposition = { data.inclinacionTangencial, data.fuerzaTangencial, data.fuerzaTangencialZ, data.fuerzaTangencialY };
            //    radialDescomposition = { data.inclinacionRadial, data.fuerzaRadial, data.fuerzaRadialZ, data.fuerzaRadialY };
            //    descompositionZ = { data.fuerzaRadialZ, data.fuerzaTangencialZ, data.fuerzaZ };
            //    descompositionY = { data.fuerzaRadialY, data.fuerzaTangencialY, data.fuerzaY };
            //}

            //if (FormDataModel.tipo.Equals("Helicoidal"))
            //{
            //    torqueValues = { FormDataModel.potencia, generalData.numeroVuelta, data.torque };
            //    generalForces = { data.torque, data.fuerzaTangencial, data.fuerzaRadial, data.radio, FormDataModel.presion, FormDataModel.helice };
            //    axialDescomposition = { null /* TODO: verificar a que corresponde f*/, null, data.fuerzaTangencial, data.fuerzaAxial };
            //    momentDescomposition = { data.radio, FormDataModel.inclinacion, data.fuerzaAxial, data.momento, data.momentoZ, data.momentoY };
            //    anguloTransversalPresion = { FormDataModel.presion, FormDataModel.helice, data.anguloTransversalPresion };
            //}

            //if (FormDataModel.tipo.Equals("Conico"))
            //{
            //    torqueValues = { FormDataModel.potencia, generalData.numeroVuelta, data.torque };
            //    generalForces = { data.torque, data.fuerzaTangencial, data.fuerzaRadial, data.radio, FormDataModel.presion, FormDataModel.helice };
            //    axialDescomposition = { null /* TODO: verificar a que corresponde f*/, FormDataModel.helice, data.fuerzaTangencial, data.fuerzaAxial };
            //    momentDescomposition = { data.radio, FormDataModel.inclinacion, data.fuerzaAxial, data.momento, data.momentoZ, data.momentoY };
            //}

            Pdf pdf = new Pdf();
            pdf.uuid = ExerciseModel.Uuid;
            pdf.title = FormDataModel.title;
            pdf.images = images;
            pdf.opts = FormDataModel.opts;
            pdf.torque_values = new List<dynamic>(torqueValues);
            pdf.general_forces = new List<dynamic?>(generalForces);
            pdf.tangential_descomposition = new List<dynamic>(tangentialDescomposition);
            pdf.radial_descomposition = new List<dynamic>(radialDescomposition);
            pdf.axial_descomposition = new List<dynamic>(axialDescomposition);
            pdf.descomposition_z = new List<dynamic>(descompositionZ);
            pdf.descomposition_y = new List<dynamic>(descompositionY);
            pdf.descomposition_torque = new List<dynamic>(descomposition_torque);
            pdf.moment_descomposition = new List<dynamic>(momentDescomposition);
            pdf.angulo_transversal_presion = new List<dynamic>(anguloTransversalPresion);

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

            int constante = (generalData.sistemaUnidades == "SI") ? appConfig.CONSTANTE_TORQUE_SI : appConfig.CONSTANTE_TORQUE_FPS;
            double torque = constante * (FormDataModel.potencia / generalData.numeroVuelta);
            double radio = (generalData.sistemaUnidades == "SI") ? (FormDataModel.diametro / 2) / 1000 : FormDataModel.diametro / 2;
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
            // TODO: mover esta logica al cs de la page
            //mainHandler.datoEngranes.fuerzaFinalZ.Add(fuerzaZ);
            //mainHandler.datoEngranes.fuerzaFinalY.Add(fuerzaY - FormDataModel.peso);

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

            int constante = (generalData.sistemaUnidades == "SI") ? appConfig.CONSTANTE_TORQUE_SI : appConfig.CONSTANTE_TORQUE_FPS;
            double torque = constante * (FormDataModel.potencia / generalData.numeroVuelta);
            double radio = (generalData.sistemaUnidades == "SI") ? (FormDataModel.diametro / 2) / 1000 : FormDataModel.diametro / 2;
            double fuerzaTangencial = torque / radio;
            double anguloTransversalPresion = Math.Atan(Math.Tan(FormDataModel.presion * Math.PI / 180) / Math.Cos(FormDataModel.helice * Math.PI / 180));
            double fuerzaRadial = fuerzaTangencial * Math.Tan(anguloTransversalPresion);
            double fuerzaAxial = fuerzaTangencial * Math.Tan(FormDataModel.helice);
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
            double momentoY = momento * Math.Cos(inclinacionTangencial);
            // TODO: mover esta logica al cs de la page
            //mainHandler.datoEngranes.fuerzaFinalZ.Add(fuerzaZ);
            //mainHandler.datoEngranes.fuerzaFinalY.Add(fuerzaY - FormDataModel.peso);
            //mainHandler.datoEngranes.momentoFinalZ.Add(momentoZ);
            //mainHandler.datoEngranes.momentoFinalY.Add(momentoY);

            return new EngraneHelicoidalCalculateModel
            {
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

            int constante = (generalData.sistemaUnidades == "SI") ? appConfig.CONSTANTE_TORQUE_SI : appConfig.CONSTANTE_TORQUE_FPS;
            double torque = constante * (FormDataModel.potencia / generalData.numeroVuelta);
            double radio = (generalData.sistemaUnidades == "SI") ? (FormDataModel.diametro / 2) / 1000 : FormDataModel.diametro / 2;
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
            double momentoY = momento * Math.Cos(inclinacionTangencial);
            // TODO: mover esta logica al cs de la page
            //mainHandler.datoEngranes.fuerzaFinalZ.Add(fuerzaZ);
            //mainHandler.datoEngranes.fuerzaFinalY.Add(fuerzaY - FormDataModel.peso);
            //mainHandler.datoEngranes.momentoFinalZ.Add(momentoZ);
            //mainHandler.datoEngranes.momentoFinalY.Add(momentoY);

            return new EngraneConicoCalculateModel
            {
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
    }
}
