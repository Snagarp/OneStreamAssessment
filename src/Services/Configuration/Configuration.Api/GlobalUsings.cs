//2023 (c) TD Synnex - All Rights Reserved.


global using Autofac.Extensions.DependencyInjection;
global using Autofac;
global using HealthChecks.UI.Client;
global using MediatR;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Diagnostics.HealthChecks;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Server.Kestrel.Core;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Diagnostics.HealthChecks;
global using Microsoft.OpenApi.Models;
global using Serilog;
global using Swashbuckle.AspNetCore.SwaggerGen;
global using System.Data.Common;
global using System.Net;
global using System.Reflection;
global using Streamone.Integrations.BuildingBlocks.EventBus.Abstractions;
global using Streamone.Integrations.BuildingBlocks.EventBus;
global using Streamone.Integrations.BuildingBlocks.IntegrationEventLogEF;

global using Streamone.Integrations.BuildingBlocks.EventBus.EventBusServiceBus;
global using StreamOne.Integrations.BuildingBlocks.IntegrationEventLogEF.Services;
global using Configuration.Infrastructure;
global using Configuration.API.Infrastructure.Filters;
global using Configuration.Application.Behaviors;
