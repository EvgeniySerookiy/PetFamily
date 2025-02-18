using PetFamily.Domain.SharedVO;

namespace PetFamily.Application;

public record GetAllVolunteersResponce(Guid id, Title title, Description description);