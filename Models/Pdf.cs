using System;
using System.Collections.Generic;
using System.Text;

namespace EjesUI.Models
{
    public class Pdf
    {
        public string title { get; set; }
        public string uuid { get; set; }
        public dynamic? images { get; set; }
        public Opts opts { get; set; }
        public List<dynamic> torque_values { get; set; }
        public List<dynamic?> general_forces { get; set; }
        public List<dynamic> tangential_descomposition { get; set; }
        public List<dynamic> radial_descomposition { get; set; }
        public List<dynamic> axial_descomposition { get; set; }
        public List<dynamic> descomposition_z { get; set; }
        public List<dynamic> descomposition_y { get; set; }
        public List<dynamic> descomposition_torque { get; set; }
        public List<dynamic> moment_descomposition { get; set; }
        public List<dynamic> angulo_transversal_presion { get; set; }

        public Pdf() { }
    }

    public class Opts
    {
        public string type { get; set; }
        public object? subtype { get; set; }
        public string? system { get; set; }
    }
}
