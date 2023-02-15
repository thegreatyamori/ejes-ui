using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjesUI.Models
{
    public class SegundaIteracionModel
    {
        public double factorCondicionSuperficial { get; set; }
        public double factorTemperatura{ get; set; }
        public double factorConcentradorEsfuerzoFlexion { get; set; }
        public double factorConcentradorEsfuerzoTorsion { get; set; }
        public double moduloYoung { get; set; }
    }
}
