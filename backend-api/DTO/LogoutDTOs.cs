
using System.ComponentModel.DataAnnotations;
namespace WeatherAPI.DTOs;

public class LogoutDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}