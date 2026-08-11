using System;
using System.Threading;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
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
    private readonly IInfluxDBClient _influxDbClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<TelemetryIngestionWorker> _logger;

    public TelemetryIngestionWorker(
        ITelemetryChannel telemetryChannel,
        IHubContext<TelemetryHub> hubContext,
        IInfluxDBClient influxDbClient,
        IConfiguration configuration,
        ILogger<TelemetryIngestionWorker> logger)
    {
        _telemetryChannel = telemetryChannel;
        _hubContext = hubContext;
        _influxDbClient = influxDbClient;
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(">> [TELEMETRY_WORKER]: .NET 10 Channel<T> Ingestion Engine Active.");

        var bucket = _configuration["InfluxDb:Bucket"] ?? "telemetry_fleet_v1";
        var org = _configuration["InfluxDb:Org"] ?? "ApexOmni";

        try
        {
            await foreach (var point in _telemetryChannel.ReadTelemetryStreamAsync(stoppingToken))
            {
                // 1. Gerçek InfluxDB PointData Nesnesi Oluşturma
                var pointData = PointData.Measurement("thermal_telemetry")
                    .Tag("node_id", point.NodeId)
                    .Tag("vehicle_plate", point.VehiclePlate)
                    .Field("temperature", point.Temperature)
                    .Field("speed", point.Speed)
                    .Timestamp(DateTime.UtcNow, WritePrecision.Ns);

                // 2. InfluxDB 3.0'a Gerçek Asenkron Fiziksel Yazma
                var writeApi = _influxDbClient.GetWriteApiAsync();
                {
                    await writeApi.WritePointAsync(pointData, bucket, org);
                }

                _logger.LogDebug($">> [INFLUXDB_WRITTEN]: Node={point.NodeId} | Temp={point.Temperature}°C to Bucket={bucket}");

                // 3. SignalR Hub üzerinden ön yüze canlı yayın
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