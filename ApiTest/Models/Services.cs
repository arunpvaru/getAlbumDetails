using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiTest.Models
{
    public class Services
    {
        private string baseUrl;
        public Services()
        {
            baseUrl = "http://jsonplaceholder.typicode.com/";
        }
        public string GetPhotos()
        {
            string successResult = "";
            var client = new HttpClient();
            string baseUrl = this.baseUrl + "photos";
            HttpResponseMessage response = client.GetAsync(baseUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                successResult = response.Content.ReadAsStringAsync().Result;
            }
            return successResult;
        }

        public string GetAlbums()
        {
            string successResult = "";
            var client = new HttpClient();
            string baseUrl = this.baseUrl + "albums";
            HttpResponseMessage response = client.GetAsync(baseUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                successResult = response.Content.ReadAsStringAsync().Result;

            }
            return successResult;
        }

    }
}
