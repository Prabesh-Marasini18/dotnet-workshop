using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProductAPI;

namespace WeatherAPI.Controllers;

[ApiController]
[Route("/")]
public class HomeController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly MyInfoOptions _myInfoOptions;

    public HomeController(IConfiguration configuration, IOptions<MyInfoOptions> myInfoOptions)
    {
        _configuration = configuration;
        _myInfoOptions = myInfoOptions.Value;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            message = "Welcome to the Weather API! Use the /weatherforecast endpoint to get the weather forecast."
        });
    }

    [HttpGet("myinfo/configuration")]
    public IActionResult GetMyInfoUsingConfiguration()
    {
        var myInfo = _configuration.GetSection(MyInfoOptions.SectionName);

        return Ok(new
        {
            Name = myInfo["Name"],
            Age = int.TryParse(myInfo["Age"], out var age) ? age : 0,
            Address = myInfo["Address"]
        });
    }

    [HttpGet("myinfo/options")]
    public IActionResult GetMyInfoUsingOptions()
    {
        return Ok(_myInfoOptions);
    }
}