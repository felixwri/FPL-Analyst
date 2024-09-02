namespace Manager.Server
{
    public class Fetch
    {
        public static async Task<string> Get(string url)
        {
            using HttpClient client = new();
            string apiUrl = url;


            HttpResponseMessage response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                return data;
            }
            else
            {
                return "None";
            }
        }
    }
}