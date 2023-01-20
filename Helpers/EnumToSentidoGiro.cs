using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjesUI.Helpers
{
    internal class EnumToSentidoGiro: ObservableCollection<string>
    {
        public EnumToSentidoGiro()
        {
            Add("Horario");
            Add("Antihorario");
        }
    }
}
