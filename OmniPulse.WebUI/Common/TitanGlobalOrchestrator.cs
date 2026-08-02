using Microsoft.Extensions.Hosting;
                using Microsoft.Extensions.Logging;
                using Dapr.Client;
                using MediatR;
                using System;
                using System.Threading;
                using System.Threading.Tasks;
                using OmniPulse.Entities.Events;

                namespace OmniPulse.WebUI.Common;

                using Microsoft.Extensions.Logging;

public class TitanGlobalOrchestrator : BackgroundService
{
    private readonly DaprClient _daprClient;
    private readonly IMediator _mediator;
    private readonly ILogger<TitanGlobalOrchestrator> _logger;

    public TitanGlobalOrchestrator(
        DaprClient daprClient, 
        IMediator mediator, 
        ILogger<TitanGlobalOrchestrator> logger)
    {
        _daprClient = daprClient;
        _mediator = mediator;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Titan Global Orchestrator is awakening...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // [HIGH_GLOBAL_ORCHESTRATION_SCAN_START]

                var clusterState = await _daprClient.GetMetadataAsync(stoppingToken);
                
                await _mediator.Publish(new ClusterStateSynchronizedEvent { 
                    State = clusterState,
                    Timestamp = DateTime.UtcNow 
                }, stoppingToken);

                // [HIGH_GLOBAL_ORCHESTRATION_SCAN_END]

            }
            catch (Exception ex)
            {
                _logger.LogWarning($"[DAPR_OFFLINE]: Sidecar connection refused or pending. Orchestrator is in waiting mode. Details: {ex.Message}");
            }

            await Task.Delay(5000, stoppingToken);
        }
    }
}