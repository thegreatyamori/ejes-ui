namespace EjesUI.Models
{
    public class ComponentModel
    {
        public FormDataModel FormData { get; set; }
        public CalculateModel Calculate { get; set; }
    }

    public class FormDataModel
    {
        public string title { get; set; }
        public Opts opts { get; set; }
        public double ubicacion { get; set; }
        public double peso { get; set; }
        public string direccionFuerzaAxial { get; set; }
        public string energia { get; set; }
    }

    public class CalculateModel
    {
        public double radio { get; set; }
        public double torque { get; set; }
    }
}
