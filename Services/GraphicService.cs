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

        public dynamic Generate()
        {
            var components = ExerciseModel.Components;
            List<string> nombre = new();
            List<string> energia = new();
            List<string> direccionFuerzaAxial = new();
            List<double> peso = new();
            List<double> ubicacion = new();
            List<double> fuerzaZ = new();
            List<double> fuerzaY = new();
            List<double> momentoZ = new();
            List<double> momentoY = new();
            List<double> torque = new();

            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].FormData.GetType() == typeof(RodamientoFormDataModel)) {
                    RodamientoFormDataModel formData = (RodamientoFormDataModel) components[i].FormData;

                    nombre.Add(formData.title.Split(" ")[1]);
                    ubicacion.Add(formData.ubicacion);
                    energia.Add(formData.tipo);
                    peso.Add(0);
                    direccionFuerzaAxial.Add("");
                    direccionFuerzaAxial.Add("");
                    fuerzaZ.Add(0);
                    fuerzaY.Add(0);
                    momentoY.Add(0);
                    momentoY.Add(0);
                    torque.Add(0);
                } else {
                    nombre.Add(components[i].FormData.title.Split(" ")[1]);
                    ubicacion.Add(components[i].FormData.ubicacion);
                    peso.Add(-components[i].FormData.peso);
                    energia.Add(components[i].FormData.energia);
                    torque.Add(components[i].Calculate.torque);
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
            }
            List<int> posicionRodamiento = new();

            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].FormData.GetType() == typeof(RodamientoFormDataModel))
                    posicionRodamiento.Add(i);
            }


            Graphic graphicData = new()
            {
                uuid = ExerciseModel.Uuid,
                sistema = ExerciseModel.GeneralData.unidades ? appConfig.SI : appConfig.FPS,
                sentidoEje = ExerciseModel.GeneralData.sentidoGiro,
                posicionRodamientoUno = posicionRodamiento[0],
                posicionRodamientoDos = posicionRodamiento[1],
                nombre = nombre,
                ubicacion = ubicacion,
                fuerzaZ = fuerzaZ,
                fuerzaY = fuerzaY,
                direccionFuerzaAxial = direccionFuerzaAxial,
                peso = peso,
                momentoZ = momentoZ,
                momentoY = momentoY,
                energia = energia,
                torque = torque
            };

            //Graphic graphicData = new Graphic
            //{
            //    uuid = ExerciseModel.Uuid,
            //    sistema = appConfig.FPS,
            //    sentidoEje = "Horario",
            //    posicionRodamientoUno = 0,
            //    posicionRodamientoDos = 5,
            //    nombre = new List<string> { "A", "B", "C", "D", "E", "F" },
            //    ubicacion = new List<double> { 0, 4, 6, 6, 4, 4 },
            //    fuerzaZ = new List<double> { 0, 437.5, -74.74, 0, 341, 0 },
            //    fuerzaY = new List<double> { 0, 159.24, -278.91, 393.13, 196.84, 0 },
            //    direccionFuerzaAxial = new List<string> { "", "", "", "", "", "" }, // mal
            //    peso = new List<double> { 0, -5, -18, -13, -13, 0 },
            //    momentoZ = new List<double> { 0, 0, 0, 0, 0, 0 },
            //    momentoY = new List<double> { 0, 0, 0, 0, 0, 0 },
            //    energia = new List<string> { "Empuje", "Consume", "Recibe", "Consume", "Consume", "Empuje" },
            //    torque = new List<double> { 0, 656.25, 1443.75, 393.75, 393.75, 0 }
            //};
            string payloadGraphic = JsonConvert.SerializeObject(graphicData);
            dynamic? graphics = api.Post("/generate-graficos", payloadGraphic);
            return graphics;
        }
    }
}
