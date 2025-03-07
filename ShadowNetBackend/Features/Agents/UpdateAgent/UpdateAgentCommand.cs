using MediatR;
using ShadowNetBackend.Common;

namespace ShadowNetBackend.Features.Agents.UpdateAgent;

public record UpdateAgentCommand(
    Guid Id, 
    string FirstName,
    string LastName,
    string? Alias, 
    string? Specialization, 
    ClearanceLevel ClearanceLevel) : IRequest<bool>;
