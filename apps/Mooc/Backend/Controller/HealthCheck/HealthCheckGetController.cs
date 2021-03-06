namespace CodelyTv.Apps.Mooc.Backend.Controller.HealthCheck
{
    using Microsoft.AspNetCore.Mvc;
    using Shared.Domain;

    [Route("health-check")]
    public class HealthCheckGetController : Controller
    {
        private readonly IRandomNumberGenerator _generator;

        public HealthCheckGetController(IRandomNumberGenerator generator)
        {
            _generator = generator;
        }

        [HttpGet]
        [Produces("application/json")]
        public IActionResult Index()
        {
            return Ok(new {moocBackend = "ok", rand = _generator.Generate()});
        }
    }
}