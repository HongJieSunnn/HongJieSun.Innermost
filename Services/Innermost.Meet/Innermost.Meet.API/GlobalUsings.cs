﻿global using Autofac;
global using Autofac.Extensions.DependencyInjection;
global using EventBusCommon;
global using EventBusCommon.Abstractions;
global using Innermost.Meet.API.Application.IntegrationEventHandles.IntegrationEvents;
global using Innermost.Meet.Domain.Repositories;
global using System.Reflection;
global using ILoggerExtensions;
global using Innermost.Meet.API.Queries.SharedLifeRecordQueries.Models;
global using CommonService;
global using CommonService.IdentityService;
global using MongoDB.Driver;
global using MongoDB.Driver.Linq;
global using Innermost.Meet.Infrastructure;
global using Innermost.Meet.Domain.AggregatesModels.SharedLifeRecordAggregate;
global using MediatR;
global using Innermost.Meet.API.Application.Commands.SharedLifeRecordAggregate;
global using Innermost.Meet.API.Application.Commands.UserSocialContactAggregate;
global using System.ComponentModel.DataAnnotations;
global using Innermost.IdempotentCommand;
global using CommonIdentityService.IdentityService;
global using Innermost.Meet.Domain.Events.SharedLifeRecordEvents;
global using CommonService.Exceptions;
global using Innermost.Meet.Domain.AggregatesModels.UserSocialContactAggregate.Entities;
global using Innermost.Meet.Domain.AggregatesModels.UserSocialContactAggregate.Enumerations;
global using Innermost.Meet.API.Queries.SocialContactQueries.Models;
global using Innermost.Meet.API.Queries.StatueQueries.Models;
