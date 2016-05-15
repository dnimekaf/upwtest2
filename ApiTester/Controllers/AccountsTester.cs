using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ApiTester.Controllers
{
    public class AccountsTester
    {
        public class Account
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }
        }

        public static async Task RunAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:62669/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = await client.GetAsync("api/Accounts/get/1");
                if (response.IsSuccessStatusCode)
                {
                    Account account = await response.Content.ReadAsAsync<Account>();
                    Console.WriteLine("{0}\t {1}\t{2}", account.Id, account.Name, account.Description);
                }
                else
                {
                    Console.WriteLine("No success");
                }

                // HTTP POST
                var gizmo = new Account() { Name = "Dima", Description = "hello world", Id = 10 };
                response = await client.PostAsJsonAsync("api/accounts/post/", gizmo);
                if (response.IsSuccessStatusCode)
                {
                    Uri gizmoUrl = response.Headers.Location;

                    // HTTP PUT
                    gizmo.Name = "Vasya"; // Update price
                    response = await client.PutAsJsonAsync(gizmoUrl, gizmo);

                    // HTTP DELETE
                    response = await client.DeleteAsync(gizmoUrl);
                }
                else
                {
                    Console.WriteLine("No success {0}", response.ReasonPhrase);
                }
            }
        }
    }
}
