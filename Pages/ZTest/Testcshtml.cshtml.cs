using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Sellasist_Optima.Pages.ZTest
{
    public class TestcshtmlModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        // Komunikat do pokazania w widoku (np. o wyniku)
        public string ResultMessage { get; set; }

        // Zdefiniuj tutaj swój adres bazowy API i token
        private const string API_BASE = "http://localhost:6462";
        private const string BEARER_TOKEN =
"jwROrpIaort4R-A4lwAoPHdZ_WbxfJA6bF6WPJQubXW6Oiy3Tso8pzvOvF-nuJgfu20jcBrLEXTpmdly8vVRAYkpQY8qzCY81HEyeg4Wy6NDCTx7UcYRi77Ki-35iR-VjFAAonY0m4UNmgXw44S1tNzybtNoFav7i4qLNTKqGzOJz3lE2ks-ik5a-X2h3oXcTj4uQhhpSzjhpINk9VZ1chbMwGGvALKShRZY-lomOCLbsZkOZsejVnSUkdCpQK9cow76u8ikjAFnVRQzrEoEraknmUFuBxfTgUcbNMB27d7tM7HJXTYYEZ00439KxNVx";
        public TestcshtmlModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public void OnGet()
        {
            // Nie robimy nic przy GET
        }

        // Metoda wywo³ywana po klikniêciu przycisku "Utwórz dokument"
        public async Task<IActionResult> OnPostCreateDocAsync()
        {
            using (var client = _clientFactory.CreateClient())
            {
                // 1) Autoryzacja w Comarch Optima
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", BEARER_TOKEN);

                var loginUrl = $"{API_BASE}/api/LoginOptima?companyName=FIRMA_DEMO";
                var loginResponse = await client.PostAsync(loginUrl, null);
                if (!loginResponse.IsSuccessStatusCode)
                {
                    ResultMessage = $"B³¹d logowania. Kod: {loginResponse.StatusCode}";
                    return Page();
                }

                // 2) Tworzenie dokumentu Zamówienie Sprzeda¿y (typ = 308)
                var docUrl = $"{API_BASE}/api/Documents";

                // Przyk³adowe dane zamówienia
                var docData = new
                {
                    type = 308,                        // Zamówienie sprzeda¿y
                    foreignNumber = "ZAMOWIENIE_Aplikacji",
                    calculatedOn = 1,                  // 1 = netto
                    paymentMethod = "przelew",
                    currency = "PLN",
                    description = "Zamówienie utworzone przez Razor Page",
                    status = 1,                        // 1 = do edycji
                    sourceWarehouseId = 1,             // ID magazynu
                    documentIssueDate = "2025-01-05T00:00:00",  // data wystawienia

                    // Dane kontrahenta
                    payer = new
                    {
                        code = "!NIEOKREŒLONY!",
                        name1 = "!NIEOKREŒLONY!"
                    },

                    // Pozycje (lista)
                    elements = new[]
                    {
                        new {
                            code = "GERALT Z RIVII",
                            quantity = 5,
                            unitNetPrice = 150.00,    // Ustawiamy cenê NETTO = 150
                           //TotalNetValue = (quantity * unitNetPrice),
                            vatRate = 23.0,
                            setCustomValue = false  // Aby Optima nie nadpisywa³a naszej ceny
                        }
                    }
                };

                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(docData),
                    Encoding.UTF8,
                    "application/json"
                );

                var createResponse = await client.PostAsync(docUrl, jsonContent);
                if (!createResponse.IsSuccessStatusCode)
                {
                    ResultMessage = $"B³¹d podczas tworzenia dokumentu. Kod: {createResponse.StatusCode}";
                    // (Opcjonalnie) wyloguj siê przed return
                    await LogoutOptima(client);
                    return Page();
                }

                // Odczyt odpowiedzi
                var responseString = await createResponse.Content.ReadAsStringAsync();
                ResultMessage = $"Dokument utworzony poprawnie! {responseString}";

                // 3) Wylogowanie z Optima (opcjonalnie)
                await LogoutOptima(client);
            }

            // Powrót na stronê z komunikatem
            return Page();
        }

        // Metoda pomocnicza do wylogowania
        private async Task LogoutOptima(HttpClient client)
        {
            var logoutUrl = $"{API_BASE}/api/LogoutOptima";
            var logoutResponse = await client.PostAsync(logoutUrl, null);
            // Mo¿esz obs³u¿yæ b³¹d wylogowania w razie potrzeby
        }
    }
}
