using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace HttpClientSample
{
    public class SaveStudenteResource
    {
        public string CorsoId { get; set; }
        public string Name { get; set; }
      
    }
    public class Studente
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CorsoId { get; set; }
        public virtual Corso? Corso { get; set; }

    }
    public class Corso
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DatePublished { get; set; }
        public virtual List<Studente>? Students { get; set; }
    }

    class Program
    {
        static HttpClient client = new HttpClient();

        static void ShowProduct(Studente product)
        {
            Console.WriteLine($"Name: {product.Name}\tCorsoId: " +
                $"{product.CorsoId}");
        }

        static async Task<Uri> CreateStudenteAsync(SaveStudenteResource product)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/University/", product);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        static async Task<Studente> GetStudentAsync(string Name)
        {
            Studente product = null;
            HttpResponseMessage response = await client.GetAsync($"api/University/Students/{Name}");
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<Studente>();
            }
            return product;
        }

        static async Task<Studente> UpdateStudenteAsync(SaveStudenteResource studenteRsc)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"api/University/{studenteRsc.Name}", studenteRsc);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            var  studente = await response.Content.ReadAsAsync<Studente>();
            return studente;
        }

        static async Task<HttpStatusCode> DeleteStudenteAsync(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                $"api/University/{id}");
            return response.StatusCode;
        }

        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            // Update port # in the following line.
             client.BaseAddress = new Uri("https://localhost:5001/");
             client.DefaultRequestHeaders.Accept.Clear();
             client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Create a new product
                SaveStudenteResource studenteRsc = new SaveStudenteResource
                {
                    Name = "Gizmo"                   
                };

                //var url = await CreateStudenteAsync(studenteRsc);
                //Console.WriteLine($"Created at {url}");

                // Get the studente
                Studente studente = await GetStudentAsync("Bruno");
                ShowProduct(studente);

                // Update the studente
                Console.WriteLine("Updating price...");
                  studente.Name = "Anna";
                Studente std =  await UpdateStudenteAsync( new SaveStudenteResource() { Name = studente.Name });

                // Get the updated studente
                studente = await GetStudentAsync(std.Name);
                ShowProduct(studente);

                // Delete the studente
                var statusCode = await DeleteStudenteAsync(studente.Id);
                Console.WriteLine($"Deleted (HTTP Status = {(int)statusCode})");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}