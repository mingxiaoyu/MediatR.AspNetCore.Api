using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MediatR.AspNetCore.Mvc
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(AuthorizationFilter))]
    public class MediatRController<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IMediator _mediator;

        public MediatRController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TResponse> SendAsync(TRequest request)
        {
            return await _mediator.Send<TResponse>(request);
        }
    }
}
