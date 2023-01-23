using System;
using System.Collections.Generic;
using System.Text;

namespace EjesUI.Models
{
    public class Graphic
    {
        public string uuid { get; set; }
        public string sistema { get; set; }
        public string sentidoEje { get; set; }
        public int posicionRodamientoUno { get; set; }
        public int posicionRodamientoDos { get; set; }
        public List<string> nombreArray { get; set; }
        public List<double> ubicacionArray { get; set; }
        public List<double> fuerzaZArray { get; set; }
        public List<double> fuerzaYArray { get; set; }
        public List<string> direccionFuerzaAxialArray { get; set; }
        public List<double> pesoArray { get; set; }
        public List<double> momentoZArray { get; set; }
        public List<double> momentoYArray { get; set; }
        public List<string> energiaArray { get; set; }
        public List<double> torqueArray { get; set; }
    }
}
