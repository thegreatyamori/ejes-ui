using System;
using EjesUI.Models;
using Newtonsoft.Json;
using RestSharp;


namespace EjesUI.Services
{
    public class ApiService
    {
        private readonly AppConfig config;

        public ApiService() {
            this.config = new AppConfig();
        }

        public object? Get(string resource, params (string,string)[] queryParams)
        {

            RestClient client = new RestClient(this.config.ApiURL);

            RestRequest request = new RestRequest(resource);
            foreach (var (param, value) in queryParams)
            {
                request.AddQueryParameter(param, value);
            }

            RestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                Console.Write(response.ContentType);
                if (response.ContentType == "application/json")
                {
                    return JsonConvert.DeserializeObject(response.Content);
                }

                return response.RawBytes;
            }

            return null;
        }

        public object? Post(string resource, string json)
        {
            RestClient client = new RestClient(this.config.ApiURL);

            RestRequest request = new RestRequest(resource, Method.Post);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            RestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                Console.Write(response.ContentType);
                return JsonConvert.DeserializeObject(response.Content);
            }

            return null;
        }
    }
}
