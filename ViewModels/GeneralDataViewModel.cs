using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using EjesUI.Models;

namespace EjesUI.ViewModels
{
    public partial class GeneralPageViewModel : ObservableObject, INavigationAware
    {
        [ObservableProperty]
        private bool _testGeneralDataToggle = false;

        public void OnNavigatedTo()
        {
        }

        public void OnNavigatedFrom()
        {
        }

        [RelayCommand]
        public void OnClickGuardarGeneralData()
        {
            SetTestData();
        }

        private void SetTestData()
        {
            string component = "engrane";
            int ejercicio = 1;

            GeneralDataModel generalData = ExerciseModel.GeneralData;

            if (TestGeneralDataToggle && component == "engrane" && ejercicio == 1)
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
                generalData.sistemaUnidades = "FPS";
                return;
            }

            if (TestGeneralDataToggle && component == "engrane" && ejercicio == 2)
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
                generalData.sistemaUnidades = "SI";
                return;
            }

            if (TestGeneralDataToggle && component == "engrane" && ejercicio == 3)
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
                generalData.sistemaUnidades = "FPS";
                return;
            }

            if (TestGeneralDataToggle && component == "engrane" && ejercicio == 4)
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
                generalData.sistemaUnidades = "FPS";
                return;
            }

            if (TestGeneralDataToggle && component == "polea" && ejercicio == 1)
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
                generalData.sistemaUnidades = "FPS";
                return;
            }

            if (TestGeneralDataToggle && component == "polea" && ejercicio == 2)
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
                generalData.sistemaUnidades = "SI";
                return;
            }

            if (TestGeneralDataToggle && component == "polea" && ejercicio == 3)
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
                generalData.sistemaUnidades = "FPS";
                return;
            }

            if (TestGeneralDataToggle && component == "cadena" && ejercicio == 1)
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
                generalData.sistemaUnidades = "FPS";
                return;
            }

            if (TestGeneralDataToggle && component == "cadena" && ejercicio == 2)
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
                generalData.sistemaUnidades = "FPS";
                return;
            }
        }
    }
}
