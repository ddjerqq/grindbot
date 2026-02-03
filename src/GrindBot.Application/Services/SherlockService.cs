using System.Net.Http.Json;
using System.Text.Json.Serialization;
using GrindBot.Domain.Common;

namespace GrindBot.Application.Services;

public sealed class SherlockService(HttpClient httpClient)
{
    private static string SherlockUrl => "SHERLOCK_URL".FromEnv();
    private static string SherlockPassword => "SHERLOCK_PASSWORD".FromEnv();

    public async Task<List<SherlockResponse>> Investigate(string username, CancellationToken ct = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, SherlockUrl.Replace(":username", username));
        request.Headers.Add("Authorization", SherlockPassword);
        
        var response = await httpClient.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadFromJsonAsync<List<SherlockResponse>>(ct);
        return result ?? [];
    }
}

public sealed record SherlockResponse(
    [property: JsonPropertyName("username")] string Username,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("url_main")] string UrlMain,
    [property: JsonPropertyName("url_user")] string UrlUser,
    [property: JsonPropertyName("exists")] string Exists,
    [property: JsonPropertyName("http_status")] string HttpStatus,
    [property: JsonPropertyName("response_time_s")] string ResponseTimeS);