
public class ApiService
{
    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;

    public ApiService(HttpClient httpClient, IConfiguration configuration)
    {
        _client = httpClient;
        _configuration = configuration;
    }

    

    public async Task<HttpResponseMessage> PostResponse(string path, dynamic model)
    {
        var json = JsonConvert.SerializeObject(model);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var key = _configuration.GetSection("AppSettings:ReportApi").Value;

        var response = await _client.PostAsync(key + path, data);
        return response;
    }



    public async Task<HttpResponseMessage> PutResponse(string path, dynamic model)
    {
        var json = JsonConvert.SerializeObject(model);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var key = _configuration.GetSection("AppSettings:ReportApi").Value;

        var response = await _client.PutAsync(key + path, data);
        return response;
    }
    public async Task<HttpResponseMessage> GetResponse(string path)
    {
        var key = _configuration.GetSection("AppSettings:ReportApi").Value;

        var response = await _client.GetAsync(new Uri(key + path));
        return response;
    }
    public async Task<HttpResponseMessage> DeleteResponse(string path)
    {
        var key = _configuration.GetSection("AppSettings:ReportApi").Value;

        var response = await _client.DeleteAsync(key + path);
        return response;
    }



}
