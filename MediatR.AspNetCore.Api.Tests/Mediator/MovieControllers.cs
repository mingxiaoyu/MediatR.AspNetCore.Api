using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using MediatR.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR.AspNetCore.Api.Tests.Models;

namespace MediatR.AspNetCore.Api.Tests.Mediator
{

    #region GetAllRequest
    [MediatRController("MediatR", Method = HttpMethodEnum.Get)]
    public class GetAllRequest : IRequest<IEnumerable<Movie>>
    {
    }

    public class GetAllRequestHandler : IRequestHandler<GetAllRequest, IEnumerable<Movie>>
    {
        private readonly IMediator _mediator;

        public GetAllRequestHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<IEnumerable<Movie>> Handle(GetAllRequest request, CancellationToken cancellationToken)
        {
            var list = FakesMovie.GetMovies();
            return Task.FromResult<IEnumerable<Movie>>(list);
        }
    }
    #endregion

    #region GetRequest
    [MediatRController("MediatR", "{Id}", Method = HttpMethodEnum.Get)]
    public class GetRequest : IRequest<Movie>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class GetRequestHandler : IRequestHandler<GetRequest, Movie>
    {
        private readonly IMediator _mediator;

        public GetRequestHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<Movie> Handle(GetRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult<Movie>(FakesMovie.GetMovie(request.Id));
        }
    }
    #endregion

    #region PostRequest
    [MediatRController("MediatR", Method = HttpMethodEnum.Post)]
    public class PostRequest : Movie, IRequest<Movie>
    {

    }

    public class PostRequestHandler : IRequestHandler<PostRequest, Movie>
    {
        public PostRequestHandler()
        {
        }

        public Task<Movie> Handle(PostRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult<Movie>(request);
        }
    }
    #endregion

    #region PutRequest
    [MediatRController("MediatR", Method = HttpMethodEnum.Put)]
    public class PutRequest : Movie, IRequest<Movie>
    {
    }

    public class PutRequestHandler : IRequestHandler<PutRequest, Movie>
    {

        public PutRequestHandler()
        {

        }

        public Task<Movie> Handle(PutRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult<Movie>(request);
        }
    }
    #endregion

    #region DeleteRequest
    [MediatRController("MediatR", "{Id}", Method = HttpMethodEnum.Delete)]

    public class DeleteRequest : IRequest
    {
        public int Id { get; set; }
    }
    public class DeleteRequestHandler : IRequestHandler<DeleteRequest, Unit>
    {

        public DeleteRequestHandler()
        {

        }

        public Task<Unit> Handle(DeleteRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult<Unit>(Unit.Value);
        }
    }

    #endregion
}
