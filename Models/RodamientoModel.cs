namespace EjesUI.Models
{
    public class RodamientoFormDataModel
    {
        public string title {get;set;}
        public Opts opts {get;set;}
        public string tipo {get;set;}
        public double ubicacion { get;set;}
    }

    public class RodamientoCalculateModel
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
