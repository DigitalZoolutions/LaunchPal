using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Serialization;
using LaunchPal.ExternalApi.SpaceNews.JsonObject;
using Newtonsoft.Json;

namespace LaunchPal.ExternalApi.SpaceNews.Request
{
    internal static class HttpCaller
    {
        /// <summary>
        /// Makes a HTTP GET to LaunchLibrary requesting data with REST API Url
        /// </summary>
        /// <typeparam name="T">Type of object expected to be returned</typeparam>
        /// <param name="url">The REST API Url to fetch the data</param>
        /// <returns>Object specified by T</returns>
        internal static async Task<T> GetSpaceNews<T>(string url) where T : NewsBase
        {
            // The API call
            T result;
            using (var client = new HttpClient())
            {
                // Prepare settings for the API-call
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.Clear();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.103 Safari/537.36");

                // Get response from the API and process it
                var requestTask = client.GetAsync(url);
                var response = Task.Run(() => requestTask).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Reading the response and deserialize it to expected object
                    var content = await response.Content.ReadAsStringAsync();

                    var serializer = new XmlSerializer(typeof(T));
                    using (var reader = new StringReader(content))
                    {
                        result = (T)serializer.Deserialize(reader);
                    }
                }
                else
                {
                    var error = JsonConvert.DeserializeObject<Error>(await response.Content.ReadAsStringAsync());

                    if (error.Cod != 200)
                    {
                        throw new HttpRequestException(error.Message);
                    }
                    else
                    {
                        throw CreateExceptionFromResponseErrors(response);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Makes a HTTP POST to LaunchPal posting data with REST API Url
        /// </summary>
        /// <typeparam name="T">Type of object expected to be returned</typeparam>
        /// <param name="url">The REST API Url to fetch the data</param>
        /// <returns>Object specified by T</returns>
        internal static async Task<bool> PostLaunchpalApi<T>(string url)
        {
            // The API call
            using (var client = new HttpClient())
            {
                // Prepare settings for the API-call
                client.BaseAddress = new Uri("http://launchpal.digitalzoolutions.com/api/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.Clear();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.103 Safari/537.36");

                // Get response from the API and process it
                var response = await client.GetAsync(url);

                return response.IsSuccessStatusCode;
            }
        }

        private static Exception CreateExceptionFromResponseErrors(HttpResponseMessage response)
        {
            var httpErrorObject = response.Content.ReadAsStringAsync().Result;

            // Create an anonymous object to use as the template for deserialization:
            var anonymousErrorObject =
                new { message = "", ModelState = new Dictionary<string, string[]>() };

            // Deserialize:
            var deserializedErrorObject =
                JsonConvert.DeserializeAnonymousType(httpErrorObject, anonymousErrorObject);

            // Now wrap into an exception which best fullfills the needs of your application:
            var ex = new HttpRequestException();

            // Sometimes, there may be Model Errors:
            if (deserializedErrorObject.ModelState != null)
            {
                var errors =
                    deserializedErrorObject.ModelState
                                            .Select(kvp => string.Join(". ", kvp.Value));
                for (int i = 0; i < errors.Count(); i++)
                {
                    // Wrap the errors up into the base Exception.Data Dictionary:
                    ex.Data.Add(i, errors.ElementAt(i));
                }
            }
            // Othertimes, there may not be Model Errors:
            else
            {
                var error =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(httpErrorObject);
                foreach (var kvp in error)
                {
                    // Wrap the errors up into the base Exception.Data Dictionary:
                    ex.Data.Add(kvp.Key, kvp.Value);
                }
            }
            return ex;
        }
    }
}
