using Microsoft.AspNetCore.Mvc;
using PetFamily.Application;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SharedVO;
using PetFamily.Domain.VolunteerContext;
using PetFamily.Domain.VolunteerContext.VolunteerVO;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]

public class VolunteersController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<GetAllVolunteersResponce>> Get()
    { 
        var title = Title.Create("VolunteersTitle").Value;
        var description = Description.Create("VolunteersDescription").Value;
        var responce = new GetAllVolunteersResponce(Guid.NewGuid(), title, description);
        
        return Ok(new List<GetAllVolunteersResponce>([responce, responce, responce]));
    } 
}