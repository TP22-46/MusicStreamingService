using MusicStreamingService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MusicStreamingService.Controller;

[ApiController]
[Route("api/[controller]")]
public class MoodsController : ControllerBase
{
        private readonly AppDbContext _db;

        public MoodsController(AppDbContext db)
        {
            _db = db;
        }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var moods = await _db.Moods
        .OrderBy(m => m.Name)
        .ToListAsync();
        return Ok(moods);
    }
}
