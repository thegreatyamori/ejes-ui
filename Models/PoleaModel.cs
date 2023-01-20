namespace EjesUI.Models
{
    public class PoleaFormDataModel: FormDataModel
    {
        public double peso {get;set;}
        public double diametro {get;set;}
        public double potencia {get;set;}
        public double inclinacion {get;set;}
        public double relacionTension {get;set;}
        public string energia {get;set;}
        public double ubicacion { get;set;}
    }

    public class PoleaCalculateModel: CalculateModel
    {
        public double radio { get; set; }
        public double torque { get; set; }
        public double T_1 { get; set; }
        public double T_2 { get; set; }
        public double fuerzaTension { get; set; }
        public double inclinacion { get; set; }
        public double fuerzaZ { get; set; }
        public double fuerzaY { get; set; }
    }
}
