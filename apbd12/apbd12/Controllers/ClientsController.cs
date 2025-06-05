using apbd12.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd12.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ClientsController: Controller
{
    private readonly IDbService _dbService;

    public ClientsController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _dbService.DeleteClient(id);
            return Ok($"Client with {id} deleted");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}