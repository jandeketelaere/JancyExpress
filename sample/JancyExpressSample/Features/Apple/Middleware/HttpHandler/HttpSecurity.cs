﻿using JancyExpress;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace JancyExpressSample.Features.Apple.Middleware.HttpHandler
{
    public class HttpSecurity : IHttpHandlerMiddleware
    {
        public Task Handle(HttpRequest httpRequest, HttpResponse httpResponse, RouteData routeData, HttpHandlerDelegate next)
        {
            //do some security related stuff on http level
            return next();
        }
    }
}