using Microsoft.AspNetCore.Mvc;
using PetFamily.Domain.SharedVO;
using PetFamily.Domain.VolunteerContext;
using PetFamily.Domain.VolunteerContext.VolunteerVO;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]

public class VolunteersController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
    
    [HttpPost]
    public IActionResult Create()
    {
        
        return Ok();
    }
}