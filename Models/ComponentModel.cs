namespace EjesUI.Models
{
    public class ComponentModel
    {
        public FormDataModel FormData { get; set; }
        public CalculateModel Calculate { get; set; }
    }

    public class FormDataModel: IFormDataModel
    {
        public string title { get; set; }
        public Opts opts { get; set; }
    }

    public class CalculateModel
    {
        public double radio { get; set; }
        public double torque { get; set; }
    }

    public interface IFormDataModel
    {
        public string title { get; set; }
        public Opts opts { get; set; }
    }
}
