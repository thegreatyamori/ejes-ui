using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using EjesUI.Models;
using Wpf.Ui.Mvvm.Contracts;
using EjesUI.Services;
using System;

namespace EjesUI.ViewModels
{
    public partial class GeneralPageViewModel : ObservableObject, INavigationAware
    {
        private AppConfig appConfig;
        private SnackBarService snackbar;

        [ObservableProperty]
        private bool _testGeneralDataToggle;
        [ObservableProperty]
        private bool _saveGeneralDataButton = true;
        [ObservableProperty]
        private bool _isUnitSystemToggleButtonEnabled = false;
        [ObservableProperty]
        private string _unitSystemContent = "Sistema de Unidades: FPS";
        [ObservableProperty]
        private GeneralDataModel _formDataModel = new GeneralDataModel();

        public GeneralPageViewModel(ISnackbarService snackbarService)
        {
            this.appConfig = new AppConfig();
            this.snackbar = new SnackBarService(snackbarService);
        }

        public void OnNavigatedTo()
        {
        }

        public void OnNavigatedFrom()
        {
        }

        public void OnChangeToggleButton(string system, bool isChecked)
        {
            snackbar.Show("test", system, 2);
            IsUnitSystemToggleButtonEnabled = (bool)isChecked;
            UnitSystemContent = $"Sistema de Unidades: {system}";
        }

        [RelayCommand]
        public void OnClickGuardarGeneralData()
        {
            PopulateFormData();

            SaveGeneralDataButton = false;

            snackbar.Show("Datos Generales", "Datos Añadidos!", 3);
        }

        private void PopulateFormData()
        {
            if (TestGeneralDataToggle)
            {
                string component = "polea";
                int ejercicio = 3;
                SetTestData(component, ejercicio);
                return;
            }

            FormDataModel.unidades = IsUnitSystemToggleButtonEnabled;
            FormDataModel.sentidoGiro = FormDataModel.sentidoGiro.Split(":")[1].Trim();
            ExerciseModel.GeneralData = FormDataModel;
        }

        private void SetTestData(string component, int ejercicio)
        {
            GeneralDataModel generalData = ExerciseModel.GeneralData;

            if (component == "engrane" && ejercicio == 1)
            {
                generalData.numeroVuelta = 480;
                generalData.confiabilidad = 2;
                generalData.limiteFluencia = 2;
                generalData.limiteMaximaFractura = 4;
                generalData.factorSeguridad = 5;
                generalData.coeficienteGlobal = 6;
                generalData.factorConcentradorEsfuerzoFlexion = 7;
                generalData.factorConcentradorEsfuerzoTorsion = 8;
                generalData.sentidoGiro = "Horario";
                generalData.unidades = false;
                return;
            }

            if (component == "engrane" && ejercicio == 2)
            {
                generalData.numeroVuelta = 1000;
                generalData.confiabilidad = 1; //Falta dato
                generalData.limiteFluencia = 700;
                generalData.limiteMaximaFractura = 600;
                generalData.factorSeguridad = 3;
                generalData.coeficienteGlobal = 0.6;
                generalData.factorConcentradorEsfuerzoFlexion = 1.6;
                generalData.factorConcentradorEsfuerzoTorsion = 1.4;
                generalData.sentidoGiro = "Horario";
                generalData.unidades = true;
                return;
            }

            if (component == "engrane" && ejercicio == 3)
            {
                generalData.numeroVuelta = 500;
                generalData.confiabilidad = 1; //Falta dato
                generalData.limiteFluencia = 2;
                generalData.limiteMaximaFractura = 3;
                generalData.factorSeguridad = 4;
                generalData.coeficienteGlobal = 5;
                generalData.factorConcentradorEsfuerzoFlexion = 6;
                generalData.factorConcentradorEsfuerzoTorsion = 7;
                generalData.sentidoGiro = "Antihorario";
                generalData.unidades = false;
                return;
            }

            if (component == "engrane" && ejercicio == 4)
            {
                generalData.numeroVuelta = 1000;
                generalData.confiabilidad = 1; //Falta dato
                generalData.limiteFluencia = 210;
                generalData.limiteMaximaFractura = 380;
                generalData.factorSeguridad = 2;
                generalData.coeficienteGlobal = 1;
                generalData.factorConcentradorEsfuerzoFlexion = 2;
                generalData.factorConcentradorEsfuerzoTorsion = 3;
                generalData.sentidoGiro = "Antihorario";
                generalData.unidades = false;
                return;
            }

            if (component == "polea" && ejercicio == 1)
            {
                generalData.numeroVuelta = 480;
                generalData.confiabilidad = 2;
                generalData.limiteFluencia = 2;
                generalData.limiteMaximaFractura = 4;
                generalData.factorSeguridad = 5;
                generalData.coeficienteGlobal = 6;
                generalData.factorConcentradorEsfuerzoFlexion = 7;
                generalData.factorConcentradorEsfuerzoTorsion = 8;
                generalData.sentidoGiro = "Horario";
                generalData.unidades = false;
                return;
            }

            if (component == "polea" && ejercicio == 2)
            {
                generalData.numeroVuelta = 1000;
                generalData.confiabilidad = 1; //Falta dato
                generalData.limiteFluencia = 700;
                generalData.limiteMaximaFractura = 600;
                generalData.factorSeguridad = 3;
                generalData.coeficienteGlobal = 0.6;
                generalData.factorConcentradorEsfuerzoFlexion = 1.6;
                generalData.factorConcentradorEsfuerzoTorsion = 1.4;
                generalData.sentidoGiro = "Horario";
                generalData.unidades = true;
                return;
            }

            if (component == "polea" && ejercicio == 3)
            {
                generalData.numeroVuelta = 500;
                generalData.confiabilidad = 1; //Falta dato
                generalData.limiteFluencia = 2;
                generalData.limiteMaximaFractura = 3;
                generalData.factorSeguridad = 4;
                generalData.coeficienteGlobal = 5;
                generalData.factorConcentradorEsfuerzoFlexion = 6;
                generalData.factorConcentradorEsfuerzoTorsion = 7;
                generalData.sentidoGiro = "Antihorario";
                generalData.unidades = false;
                return;
            }

            if (component == "cadena" && ejercicio == 1)
            {
                generalData.numeroVuelta = 480;
                generalData.confiabilidad = 2;
                generalData.limiteFluencia = 3;
                generalData.limiteMaximaFractura = 4;
                generalData.factorSeguridad = 5;
                generalData.coeficienteGlobal = 6;
                generalData.factorConcentradorEsfuerzoFlexion = 7;
                generalData.factorConcentradorEsfuerzoTorsion = 8;
                generalData.sentidoGiro = "Horario";
                generalData.unidades = false;
                return;
            }

            if (component == "cadena" && ejercicio == 2)
            {
                generalData.numeroVuelta = 500;
                generalData.confiabilidad = 1; //Falta dato
                generalData.limiteFluencia = 2;
                generalData.limiteMaximaFractura = 3;
                generalData.factorSeguridad = 4;
                generalData.coeficienteGlobal = 5;
                generalData.factorConcentradorEsfuerzoFlexion = 6;
                generalData.factorConcentradorEsfuerzoTorsion = 7;
                generalData.sentidoGiro = "Antihorario";
                generalData.unidades = false;
                return;
            }
        }
    }
}
