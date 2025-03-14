using KakuroWEB.Models;
using System.Text;
using System.Text.Json;

namespace KakuroWEB.Services;

public class UserService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _baseUrl = "https://kakuroapi-gqgwb0b4excwbxg5.canadaeast-01.azurewebsites.net";
    }
    
    public async Task<UserResponse> RegisterUserAsync(UserRegistartionRequest request)
    {
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync($"{_baseUrl}/users/register", content);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<UserResponse>(responseJson, new JsonSerializerOptions {PropertyNameCaseInsensitive = true});
    }

    public async Task<UserResponse> CheckUserExistsAsync(UserRegistartionRequest request)
    {
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync($"{_baseUrl}/users/exists", content);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<UserResponse>(responseJson, new JsonSerializerOptions {PropertyNameCaseInsensitive = true});
    }
}