﻿global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using MediatR;
global using Innermost.IdempotentCommand;
global using System.Net;
global using Innermost.LogLife.Infrastructure;
global using TypeExtensions;
global using Innermost.LogLife.Domain.AggregatesModels.LifeRecordAggregate;
global using AutoMapper;
global using EventBusCommon;
global using Innermost.LogLife.Domain.AggregatesModels.LifeRecordAggregate.Entities;
global using Innermost.IdempotentCommand.Infrastructure.Repositories;
global using Autofac;
global using System.Reflection;
global using Innermost.LogLife.API.Application.Behaviors;
global using CommonService;
global using IntegrationEventRecord;
global using Microsoft.EntityFrameworkCore.Design;
global using System.ComponentModel.DataAnnotations;
global using System.Diagnostics.CodeAnalysis;
global using Dapper;
global using MySqlConnector;
global using Innermost.LogLife.Domain.Events;
global using Autofac.Extensions.DependencyInjection;
global using CommonService.IdentityService;
global using EventBusCommon.Abstractions;
global using EventBusServiceBus;
global using HealthChecks.UI.Client;
global using Innemost.LogLife.API.Infrastructure.AutofacModules;
global using Innemost.LogLife.API.Queries;
global using Innermost.LogLife.API.Application.Filter;
global using Innermost.LogLife.API.Application.IntegrationEvents;
global using Innermost.LogLife.API.Services.IntegrationEventServices;
global using Innermost.LogLife.Infrastructure.Repositories;
global using IntegrationEventRecord.Services;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Diagnostics.HealthChecks;
global using Microsoft.OpenApi.Models;
global using System.IdentityModel.Tokens.Jwt;
global using Innermost.LogLife.API;
global using Serilog;
global using Microsoft.AspNetCore;
global using System.Runtime.Serialization;
global using System.Text.Json.Serialization;
global using Innermost.LogLife.Domain.Events.LifeRecordEvents;
global using Innermost.LogLife.API.Application.Commands;
global using TagS.Microservices.Client.Models;
global using Innemost.LogLife.API.Queries.Model;
