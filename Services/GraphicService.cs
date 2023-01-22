using EjesUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjesUI.Services
{
    public class GraphicService
    {
        private ApiService api;
        private readonly AppConfig appConfig;

        public GraphicService()
        {
            this.api = new ApiService();
            this.appConfig = new AppConfig();
        }

        public string Isometrico()
        {
            var components = ExerciseModel.Components;
            List<string> nombre = new();
            List<double> distancia = new();
            List<double> peso = new();
            List<string> energia = new();
            List<double> fuerzaZ = new();
            List<double> fuerzaY = new();
            List<string> direccionFuerzaAxial = new();
            List<double> momentoZ = new();
            List<double> momentoY = new();

            for (int i = 0; i < components.Count; i++)
            {
                nombre.Add(components[i].FormData.title.Split(" ")[1]);
                distancia.Add(components[i].FormData.ubicacion);
                peso.Add(components[i].FormData.peso);
                energia.Add(components[i].FormData.energia);
                if (components[i].Calculate.GetType() == typeof(CadenaCalculateModel))
                {
                    CadenaCalculateModel formData = (CadenaCalculateModel) components[i].Calculate;
                    fuerzaZ.Add(formData.fuerzaTangencialZ);
                    fuerzaY.Add(formData.fuerzaTangencialY);
                    direccionFuerzaAxial.Add("");
                    momentoZ.Add(0);
                    momentoY.Add(0);
                }
                if (components[i].Calculate.GetType() == typeof(PoleaCalculateModel))
                {
                    PoleaCalculateModel formData = (PoleaCalculateModel) components[i].Calculate;
                    fuerzaZ.Add(formData.fuerzaZ);
                    fuerzaY.Add(formData.fuerzaY);
                    direccionFuerzaAxial.Add("");
                    momentoZ.Add(0);
                    momentoY.Add(0);
                }
                if (components[i].Calculate.GetType() == typeof(EngraneCalculateModel))
                {
                    EngraneCalculateModel formData = (EngraneCalculateModel) components[i].Calculate;
                    fuerzaZ.Add(formData.fuerzaZ);
                    fuerzaY.Add(formData.fuerzaY);
                    direccionFuerzaAxial.Add("");
                    momentoZ.Add(0);
                    momentoY.Add(0);
                }
                if (components[i].Calculate.GetType() == typeof(EngraneHelicoidalCalculateModel))
                {
                    EngraneHelicoidalCalculateModel formData = (EngraneHelicoidalCalculateModel) components[i].Calculate;
                    fuerzaZ.Add(formData.fuerzaZ);
                    fuerzaY.Add(formData.fuerzaY);
                    direccionFuerzaAxial.Add(components[i].FormData.direccionFuerzaAxial);
                    momentoZ.Add(formData.momentoZ);
                    momentoY.Add(formData.momentoY);
                }
                if (components[i].Calculate.GetType() == typeof(EngraneConicoCalculateModel))
                {
                    EngraneConicoCalculateModel formData = (EngraneConicoCalculateModel) components[i].Calculate;
                    fuerzaZ.Add(formData.fuerzaZ);
                    fuerzaY.Add(formData.fuerzaY);
                    direccionFuerzaAxial.Add(components[i].FormData.direccionFuerzaAxial);
                    momentoZ.Add(formData.momentoZ);
                    momentoY.Add(formData.momentoY);
                }
            }
            List<dynamic> arrayFinal = new()
            {
                nombre,
                distancia,
                fuerzaZ,
                fuerzaY,
                peso,
                direccionFuerzaAxial,
                momentoZ,
                momentoY,
                energia
            };
            string payloadGraphic = JsonConvert.SerializeObject(arrayFinal);
            dynamic? graphics = api.Post("/isometrico", payloadGraphic);
            return graphics;
        }
    }
}
