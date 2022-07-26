using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Movie_search.Model;

namespace Movie_search.Logic
{
    public class IMDBReader
    {
        private const string URL = "https://imdb-api.com/en/API/";
        private const string key = "k_aqzsw8np/";//"k_ehc0sd58/";
        private const string search = "SearchMovie/";
        private const string title = "Title/";
        private const string trailer = "YouTubeTrailer/";
        private const string rating = "/Ratings,";
        private HttpClient client = new HttpClient();
        
        public IMDBReader()
        {
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public List<Movie> FindByTitle(string title)
        {
            HttpResponseMessage response = client.GetAsync(search+key+title).Result;
            if (response.IsSuccessStatusCode)
            {
                //var dataObjects = response.Content.ReadAsStringAsync().Result;
                //JObject jsonString = JObject.Parse(dataObjects);
                JObject jsonString = Parse(response);
                List<Movie> movies = new List<Movie>();
                foreach( var movie in jsonString.SelectToken("results"))
                {
                    movies.Add(JsonSerializer.Deserialize<Movie>(movie.ToString()));
                }
                return movies;
            }
            else
                return null;

        }
        public Movie ExtendedInfoByID(string id)
        {
            Movie movie;
            HttpResponseMessage response = client.GetAsync(title + key + id + rating).Result;
            if (response.IsSuccessStatusCode)
                movie = JsonSerializer.Deserialize<Movie>(Parse(response).ToString());
            else
                return null;
            response = client.GetAsync(trailer + key + id).Result;
            if (response.IsSuccessStatusCode)
            {
                JObject jsonString = Parse(response);
                 movie.videoUrl= jsonString.SelectToken("videoUrl").ToString();
                return movie;
            }
            else
                return null;
        }
        private JObject Parse(HttpResponseMessage response)
        {
            var dataObjects = response.Content.ReadAsStringAsync().Result;
            return JObject.Parse(dataObjects);
        }
    }
}
