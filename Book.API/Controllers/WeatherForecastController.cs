using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.API.Message;
using Microsoft.AspNetCore.Authorization;
using Book.IService;
using Microsoft.AspNetCore.Http;

namespace Book.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController :BaseController
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUserService userService;
        private readonly IHttpContextAccessor contextAccessor;

        public WeatherForecastController(IUserService user,IHttpContextAccessor accessor,ILogger<WeatherForecastController> logger):base(user,accessor)
        {
            _logger = logger;
            userService = user;
            contextAccessor = accessor;
        }

        [HttpGet(Name = "weathers")]
        [Authorize(Policy = "BookPolicy")]
        public ActionResult weathers()
        {
            var rng = new Random();
            var list= Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToList();
            return Success(list);
        }
    }
}
