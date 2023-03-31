using Task3.Models;
using Task3.Services;
using Microsoft.AspNetCore.Mvc;

namespace Task3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogsController : ControllerBase
{
    private readonly LogsService _logsService;
    
    public LogsController(LogsService logsService)
    {
        _logsService = logsService;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Log>>> GetAsync() =>
        await _logsService.GetAsync();
    
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Log>> GetAsync(string id)
    {
        var log = await _logsService.GetAsync(id);
        if (log == null)
            return NotFound();
        return log;
    }
    
    [HttpPost]
    public async Task<ActionResult<Log>> CreateAsync(Log log)
    {
        await _logsService.CreateAsync(log);
        return Ok("Log " + log.Id + " has been created!");
    }
    
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> UpdateAsync(string id, Log log)
    {
        var logFromDb = await _logsService.GetAsync(id);
        if (logFromDb == null)
            return NotFound();
        log.Id = logFromDb.Id;
        await _logsService.UpdateAsync(id, log);
        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var log = await _logsService.GetAsync(id);
        if (log is null)
        {
            return NotFound();
        }

        await _logsService.RemoveAsync(id);
        return NoContent();

    }

}