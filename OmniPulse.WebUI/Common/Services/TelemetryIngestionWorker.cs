using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OmniPulse.Business.Services;
using OmniPulse.Entities.Models.Dto;
using OmniPulse.WebUI.Common.Hubs;

namespace OmniPulse.WebUI.Common.Services;

public class TelemetryIngestionWorker : BackgroundService
{
    private readonly ITelemetryChannel _telemetryChannel;
    private readonly IHubContext<TelemetryHub> _hubContext;
    private readonly ILogger<TelemetryIngestionWorker> _logger;

    public TelemetryIngestionWorker(
        ITelemetryChannel telemetryChannel,
        IHubContext<TelemetryHub> hubContext,
        ILogger<TelemetryIngestionWorker> logger)
    {
        _telemetryChannel = telemetryChannel;
        _hubContext = hubContext;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(">> [TELEMETRY_WORKER]: .NET 10 Channel<T> Ingestion Engine Active.");

        try
        {
            await foreach (var point in _telemetryChannel.ReadTelemetryStreamAsync(stoppingToken))
            {
                // InfluxDB Batch Write Simülasyonu
                _logger.LogDebug($">> [INFLUXDB_FLUSH]: Node={point.NodeId} | Temp={point.Temperature}°C");

                // SignalR Hub üzerinden ön yüze canlı yayın
                await _hubContext.Clients.All.SendAsync("ReceiveTelemetryPoint", point, cancellationToken: stoppingToken);

                await Task.Yield();
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation(">> [TELEMETRY_WORKER]: Ingestion worker stopped gracefully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ">> [TELEMETRY_WORKER]: Error processing channel stream.");
        }
    }
}