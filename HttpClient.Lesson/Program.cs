using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

namespace HttpClient.Lesson
{


    internal class Program
    {

        private static readonly System.Net.Http.HttpClient Client = new System.Net.Http.HttpClient();

        static void Main(string[] args)
        {
            GetStringAsync("");
        }
        public static async Task<string> GetStringAsync(string uri)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await Client.SendAsync(requestMessage);
            return await response.Content.ReadAsStringAsync();
        }
    }
    
}
