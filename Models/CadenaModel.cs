namespace EjesUI.Models
{
    public class CadenaFormDataModel: FormDataModel
    {
        public double potencia {get;set;}
        public double diametro {get;set;}
        public double inclinacion {get;set;}
    }

    public class CadenaCalculateModel: CalculateModel
    {
        public double inclinacion { get; set; }
        public double fuerzaTangencial { get; set; }
        public double fuerzaTangencialZ { get; set; }
        public double fuerzaTangencialY { get; set; }
    }
}
