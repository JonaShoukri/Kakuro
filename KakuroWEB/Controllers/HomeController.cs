using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using KakuroWEB.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using KakuroWEB.Models;


namespace KakuroWEB.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }
    
    [AuthorizeUser]
    public IActionResult Dashboard()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }
    
    [AuthorizeUser]
    public IActionResult PlayGame()
    {
        return View();
    }
    
    [AuthorizeUser]
    public async Task<IActionResult> DaysPlayed()
    {
        var userId = HttpContext.Session.GetString("UserId");

        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var httpClient = new HttpClient();
        var apiUrl = $"https://kakuroapi-gqgwb0b4excwbxg5.canadaeast-01.azurewebsites.net/users/{userId}/games";

        var response = await httpClient.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode)
        {
            return View(new Dictionary<string, int>());
        }

        var json = await response.Content.ReadAsStringAsync();

        var games = JsonSerializer.Deserialize<List<Game>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Parse date strings to DateTime safely
        var gamesGroupedByDate = games!
            .Where(g => DateTime.TryParseExact(g.Date, "dd/MM/yy", null, System.Globalization.DateTimeStyles.None, out _))
            .GroupBy(g => DateTime.ParseExact(g.Date, "dd/MM/yy", null))
            .ToDictionary(g => g.Key.ToString("dd/MM/yy"), g => g.Count());

        return View(gamesGroupedByDate);
    }


    
    [AllowAnonymous]
    public async Task<IActionResult> Leaderboard()
    {
        string apiUrl = "https://kakuroapi-gqgwb0b4excwbxg5.canadaeast-01.azurewebsites.net/users/leaderboard";
    
        using (var client = new HttpClient())
        {
            var jsonContent = new StringContent("{\"Name\": \"John Doe\", \"Email\": \"john.doe@example.com\", \"Password\": \"securepassword123\"}", System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync(apiUrl, jsonContent);
        
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
            
                // Debug log to check raw response data
                _logger.LogInformation("API Response: " + responseContent);
            
                try
                {
                    var leaderboard = JsonSerializer.Deserialize<LeaderboardModel>(responseContent);
                
                    if (leaderboard == null || leaderboard.List == null)
                    {
                        _logger.LogWarning("Deserialization returned null leaderboard.");
                        leaderboard = new LeaderboardModel();  // Ensure we have a valid object to pass to the view
                    }

                    return View(leaderboard);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error deserializing leaderboard data: " + ex.Message);
                    return View("Error");
                }
            }
            else
            {
                _logger.LogError("Failed to fetch leaderboard data: " + response.StatusCode);
                return View("Error");
            }
        }
    }

    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    [AuthorizeUser]
    public async Task<IActionResult> DailyGame()
    {
        var httpClient = new HttpClient();
        var apiUrl = "https://kakuroapi-gqgwb0b4excwbxg5.canadaeast-01.azurewebsites.net/users/dailygame";

        var response = await httpClient.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode)
        {
            return View("Error"); // Or some error page
        }

        var json = await response.Content.ReadAsStringAsync();

        var gameData = JsonSerializer.Deserialize<DailyGameResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var viewModel = new PlayGameViewModel
        {
            RowSums = gameData.Row,
            ColumnSums = gameData.Column
        };

        return View(viewModel);
    }

}