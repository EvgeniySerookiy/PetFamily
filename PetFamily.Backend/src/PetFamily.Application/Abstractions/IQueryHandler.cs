using PetFamily.Application.Dtos;
using PetFamily.Application.Models;

namespace PetFamily.Application.Abstractions;

public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery
{
    Task<PagedList<PetDto>> Handle(
        TQuery query, 
        CancellationToken cancellationToken = default);
}