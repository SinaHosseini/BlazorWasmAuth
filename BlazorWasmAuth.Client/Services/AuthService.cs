using Microsoft.JSInterop;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;

    public AuthService(HttpClient http, IJSRuntime js)
    {
        _http = http;
        _js = js;
    }

    public async Task<UserInfoModel?> GetCurrentUserAsync()
    {
        var token = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");
        if (string.IsNullOrWhiteSpace(token))
            return null;

        var handler = new JwtSecurityTokenHandler();

        try
        {
            var jwt = handler.ReadJwtToken(token);
            var id = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var name = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (!string.IsNullOrEmpty(id) && int.TryParse(id, out var uid))
            {
                return new UserInfoModel
                {
                    UserId = uid,
                    Username = name ?? ""
                };
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine("خطا در خواندن توکن: " + ex.Message);
            return null;
        }
    }

}
