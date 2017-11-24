using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Models;
using Repository;

namespace TraceService.Controllers
{
    [Route("api/[controller]")]
    public class OriginsController : Controller
    {
        private readonly IOriginsRepository _originsRepository;
        private readonly ILogger _logger;
        private readonly IMemoryCache _cache;

        // costructor
        public OriginsController(IOriginsRepository originsRepository, ILogger<TracesController> logger, IMemoryCache cache)
        {
            _originsRepository = originsRepository;    
            _logger = logger;
            _cache = cache;
        }
        
        // GET api/origins
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation(0, "Get all Trace Origins");

            if(!ModelState.IsValid)
                return BadRequest();

            var origins = await _originsRepository.GetAllAsync();        

            if(origins == null || origins.Count() == 0)    
                return NotFound();
            else
                return this.Ok(origins);
        }

    }
}