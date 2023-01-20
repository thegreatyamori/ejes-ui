using System;
using System.Collections.Generic;


namespace EjesUI.Models
{
    public class EngraneFormDataModel: FormDataModel
    {
        public string tipo { get; set; }
        public double ubicacion { get; set; }
        public string energia { get; set; }
        public double peso { get; set; }
        public double diametro { get; set; }
        public double potencia { get; set; }
        public double presion { get; set; }
        public double helice { get; set; }
        public double inclinacion { get; set; }
        public string direccionFuerzaAxial { get; set; }
    }

    public class EngraneCalculateModel: CalculateModel
    {
        public double inclinacionTangencial { get; set; }
        public double fuerzaTangencial { get; set; }
        public double fuerzaTangencialZ { get; set; }
        public double fuerzaTangencialY { get; set; }
        public double inclinacionRadial { get; set; }
        public double fuerzaRadial { get; set; }
        public double fuerzaRadialZ { get; set; }
        public double fuerzaRadialY { get; set; }
        public double fuerzaZ { get; set; }
        public double fuerzaY { get; set; }
    }

    public class EngraneHelicoidalCalculateModel : EngraneCalculateModel
    {
        public double fuerzaAxial { get; set; }
        public double momento { get; set; }
        public double momentoZ { get; set; }
        public double momentoY { get; set; }
        public double anguloTransversalPresion { get; set; }
    }

    public class EngraneConicoCalculateModel : EngraneCalculateModel
    {
        public double fuerzaAxial { get; set; }
        public double momento { get; set; }
        public double momentoZ { get; set; }
        public double momentoY { get; set; }
    }
}
