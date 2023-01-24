namespace EjesUI.Models
{
    public class ComponentModel
    {
        public FormDataModel FormData { get; set; }
        public CalculateModel? Calculate { get; set; }
    }

    public class FormDataModel
    {
        private string _energia = "Energía";
        private string _direccionFuerzaAxial = "Dir. F. Axial";
        public string title { get; set; }
        public Opts opts { get; set; }
        public double ubicacion { get; set; }
        public double peso { get; set; }
        public string direccionFuerzaAxial {
            get { return _direccionFuerzaAxial; }
            set { _direccionFuerzaAxial = value; }
        }
        public string energia
        {
            get { return _energia; }
            set { _energia = value; }
        }
    }

    public class CalculateModel
    {
        public double radio { get; set; }
        public double torque { get; set; }
    }
}
