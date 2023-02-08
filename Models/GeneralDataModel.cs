using System;

namespace EjesUI.Models
{
    public class GeneralDataModel
    {
        public double numeroVuelta { get; set; }
        public double confiabilidad { get; set; }
        public double limiteFluencia { get; set; }
        public double limiteMaximaFractura { get; set; }
        public double factorSeguridad { get; set; }
        public double coeficienteGlobal { get; set; }
        public double factorConcentradorEsfuerzoFlexion { get; set; }
        public double factorConcentradorEsfuerzoTorsion { get; set; }
        public string sentidoGiro { get; set; }
        public bool unidades { get; set; }
    }
}
