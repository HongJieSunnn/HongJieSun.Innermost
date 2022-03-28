﻿global using Autofac;
global using Autofac.Extensions.DependencyInjection;
global using TagS.Microservices.Server.AutofacExtensions;
global using TagS.Microservices.Server.Microsoft.AspNetCore.Http;
global using TagS.Microservices.Server.Microsoft.DependencyInjection;
global using MediatR;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using TagS.Microservices.Server.Commands;
global using CommonService.IdentityService;
global using Innermost.IdempotentCommand;
global using TagS.Microservices.Server.Models;
global using MongoDB.Driver.GeoJsonObjectModel;
global using TagS.Microservices.Client.Models;
global using TagS.Microservices.Server.Queries.Models;