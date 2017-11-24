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
    public class TracesController : Controller
    {
        private readonly ITraceRepository _traceRepository;
        private readonly IOriginsRepository _originsRepository;
        private readonly ILogger _logger;
        private readonly IMemoryCache _cache;

        // costructor
        public TracesController(ITraceRepository traceRepository, IOriginsRepository originsRepository, ILogger<TracesController> logger, IMemoryCache cache)
        {
            _traceRepository = traceRepository;    
            _originsRepository = originsRepository;
            _logger = logger;
            _cache = cache;
        }

        // GET api/traces        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation(0, "Get all Trace");

            if(!ModelState.IsValid)
                return BadRequest();

            var traces = await _traceRepository.GetAllAsync();        

            if(traces == null || traces.Count() == 0)    
                return NotFound();
            else
                return this.Ok(traces);
        }

        // GET api/traces/myapp/2017-1-1/2017-1-31
        [HttpGet("{origin}/{fromDate}/{toDate}")]
        public async Task<IActionResult> Get(string origin, DateTime fromDate, DateTime toDate)
        {
            _logger.LogInformation(0, "Get trace by range");

            if(!ModelState.IsValid)
                return BadRequest();

            var traces = await _traceRepository.GetByRangeAsync(origin, fromDate, toDate);        

            if(traces == null || traces.Count() == 0)    
                return NotFound();
            else
                return this.Ok(traces);
        }

        // GET api/traces/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            _logger.LogInformation(0, "Get Trace by id: {0}", id);

            if(!ModelState.IsValid)
                return BadRequest();

            var trace = await _traceRepository.GetByIdAsync(id);

            if(trace == null)
                return this.NotFound();
            else
                return this.Ok(trace);
        }

        // POST api/traces
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Trace t)
        {
            if(!ModelState.IsValid)
                return BadRequest();
            
            var trace = _traceRepository.Add(t);
            await _traceRepository.SaveAsync();

            await EnsureOriginAsync(t);

            return CreatedAtAction(nameof(Get), new { TraceId = trace.TraceId}, trace);
        }

        // PUT api/traces
        [HttpPut()]
        public async Task<IActionResult> Put([FromBody]Trace t)
        {
            if(!ModelState.IsValid)
                return BadRequest();
            
            Trace trace = await _traceRepository.UpdateAsync(t);
            await _traceRepository.SaveAsync();

            await EnsureOriginAsync(t);

            return this.Ok(trace);
        }

        // DELETE api/traces/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if(!ModelState.IsValid)
                return BadRequest();
            
            bool res = await _traceRepository.DeleteAsync(id);
            await _traceRepository.SaveAsync();

            if(res)
                return Ok();
            else
                return NotFound();
        }

        private async Task EnsureOriginAsync(Trace t)
        {
            try
            {
                TraceOrigin traceOrigin;

                // try from cache
                if(!_cache.TryGetValue(t.Origin, out traceOrigin))
                {
                    _logger.LogDebug("Origin NOT in cache");

                    // try from db
                    traceOrigin = await _originsRepository.GetByNameAsync(t.Origin);

                    if(traceOrigin == null)
                    {
                        _logger.LogDebug("Origin NOT in db");

                        // save data in database
                        TraceOrigin origin = new TraceOrigin(){ Origin = t.Origin };

                        traceOrigin = _originsRepository.Add(origin);
                        await _originsRepository.SaveAsync();

                        _logger.LogDebug("Origin ADDED in db");
                    }
                    else
                    {
                        _logger.LogDebug("Origin in db");
                    }

                    // set cache options
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        // keep in cache for this time, reset time if accessed
                        .SetSlidingExpiration(TimeSpan.FromSeconds(5));

                    // save data in cache
                    _cache.Set(traceOrigin.Origin, traceOrigin, cacheEntryOptions);

                    _logger.LogDebug("Origin ADDED in cache");
                }
                else
                {
                    _logger.LogDebug("Origin in cache");
                }
                
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
            }            
        }
        
    }
}
