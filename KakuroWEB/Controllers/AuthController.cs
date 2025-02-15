using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KakuroWEB.Controllers;

public class AuthController : Controller
{
    private readonly HttpClient _httpClient;
    
    public AuthController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string name, string email, string password)
    {
        var apiUrl = "https://kakuroapi-gqgwb0b4excwbxg5.canadaeast-01.azurewebsites.net/users/exists";
        var requestBody = new
        {
            Name = name,
            Email = email,
            Password = password
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(apiUrl, jsonContent);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
            string userId = responseData.id;

            // Store user ID and login status in session
            HttpContext.Session.SetString("UserId", userId);
            HttpContext.Session.SetString("IsLoggedIn", "true");

            // Redirect to Dashboard
            return RedirectToAction("Dashboard", "Home");
        }
        else
        {
            try
            {
                var errorResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                string errorMessage = errorResponse?.title ?? "Invalid name, email or password.";
            
                ModelState.AddModelError("", errorMessage);
            }
            catch
            {
                ModelState.AddModelError("", "Login failed. Please try again.");
            }

            return View();
        }
    }

    
    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // Clear session data
        return RedirectToAction("Login");
    }
    
    [HttpGet("Login")]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost("Login")]
    [Route("Auth/Register")]
    public async Task<IActionResult> Register(string name, string email, string password, string confirmPassword)
    {
        if (password != confirmPassword)
        {
            ModelState.AddModelError("Password", "Passwords do not match.");
            return View();
        }

        var apiUrl = "https://kakuroapi-gqgwb0b4excwbxg5.canadaeast-01.azurewebsites.net/users/register";
        var requestBody = new
        {
            Name = name,
            Email = email,
            Password = password
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(apiUrl, jsonContent);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
            string userId = responseData.id;

            // Store user ID in session
            HttpContext.Session.SetString("UserId", userId);
            HttpContext.Session.SetString("IsLoggedIn", "true");

            // Redirect to Dashboard
            return RedirectToAction("Dashboard", "Home");
        }
        else
        {
            try
            {
                var errorResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                string errorMessage = errorResponse?.title ?? "An error occurred during registration.";
            
                ModelState.AddModelError("", errorMessage);
            }
            catch
            {
                ModelState.AddModelError("", "Registration failed. Please try again.");
            }

            return View();
        }
    }
    
    public IActionResult ReRegister()
    {
        return View();
    }
}