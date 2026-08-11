using InfluxDB.Client;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using OmniPulse.Business.Services;
using OmniPulse.Business.StateMachines;
using OmniPulse.DataAccess;
using OmniPulse.Entities.Common;   
using OmniPulse.Entities.Models.Dto;
using OmniPulse.WebUI.Common;       
using OmniPulse.WebUI.Common.Agents;
using OmniPulse.WebUI.Common.Hubs;
using OmniPulse.WebUI.Common.Middleware;
using OmniPulse.WebUI.Common.Security;
using OmniPulse.WebUI.Common.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddMemoryCache();
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddSwaggerGen();
builder.Services.AddDaprClient();

builder.Services.AddHostedService<TitanGlobalOrchestrator>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(BaseEntity).Assembly));

builder.Services.AddDbContext<OmniPulseDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApexConnection")));

builder.Services.AddScoped<OmniPulse.DataAccess.Repositories.IUnitOfWork, OmniPulse.DataAccess.Repositories.UnitOfWork>();
builder.Services.AddScoped<IOmniPulseService, OmniPulseManager>();

// [ZERO_TRUST_SECURITY_REGISTRATION]
builder.Services.AddScoped<IAuthorizationHandler, TitanZeroTrustHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("TitanManagerOnly", policy =>
        policy.Requirements.Add(new TitanZeroTrustRequirement()));
});

// [TELEMETRY_SERVICES_REGISTRATION]
builder.Services.AddSingleton<ITelemetryChannel, TelemetryChannel>();
builder.Services.AddHostedService<TelemetryIngestionWorker>();

// [INFLUXDB_CLIENT_REGISTRATION]
builder.Services.AddSingleton<IInfluxDBClient>(sp =>
    new InfluxDBClient(
        builder.Configuration["InfluxDb:Url"],
        builder.Configuration["InfluxDb:Token"]
    ));

// [ELSA_WORKFLOW_SERVICES_REGISTRATION]
builder.Services.AddScoped<IColdChainWorkflowEngine, ColdChainBreachWorkflow>();

// [MCP_AGENT_SERVICES_REGISTRATION]
builder.Services.AddScoped<IMcpAgent, ThermalAnomalyAgent>();
builder.Services.AddScoped<IMcpAgent, SpatialDriftAgent>();

builder.Services.AddMassTransit(x =>
{
    // StateMachine ve Saga State kaydı
    x.AddSagaStateMachine<OmniPulseStateMachine, OmniPulseSagaState>()
     .InMemoryRepository();

    x.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

AppServiceProvider.Instance = app.Services; 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OmniPulse TITAN_APEX v3.0"));
}

app.UseStaticFiles();
app.UseRouting();
app.UseCors(p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseMiddleware<TitanTenantMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllerRoute(name: "default", pattern: "{controller=OmniPulse}/{action=Index}/{id?}");

// [SIGNALR_HUB_MAPPING]
app.MapHub<TelemetryHub>("/hubs/telemetry");

app.MapControllers();

app.Run();
