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
        public List<string> nombre { get; set; }
        public List<double> ubicacion { get; set; }
        public List<double> fuerzaZ { get; set; }
        public List<double> fuerzaY { get; set; }
        public List<string> direccionFuerzaAxial { get; set; }
        public List<double> peso { get; set; }
        public List<double> momentoZ { get; set; }
        public List<double> momentoY { get; set; }
        public List<string> energia { get; set; }
        public List<double> torque { get; set; }
    }
}
