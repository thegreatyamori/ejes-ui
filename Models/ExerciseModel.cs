using EjesUI.Helpers;
using System.Collections.Generic;

namespace EjesUI.Models
{
    public static class ExerciseModel
    {
        public static string Name = "Aún no has empezado un ejercicio";
        public static string Uuid { get; set; }
        public static GeneralDataModel GeneralData = new GeneralDataModel();
        public static List<ComponentModel> Components = new List<ComponentModel>();

        public static string GetNextComponentLetter()
        {
            int nextPosition = ExerciseModel.Components.Count;
            return AlphabetGenerator.GetLetter(nextPosition).ToString();
        }
    }
}
