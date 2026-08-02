using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using OmniPulse.Business.Services;
using OmniPulse.Entities.Models.Dto;

namespace OmniPulse.WebUI.Controllers;

[ApiController]
[Route("api/telemetry")]
public class TelemetryStreamController : ControllerBase
{
    private readonly ITelemetryChannel _telemetryChannel;

    public TelemetryStreamController(ITelemetryChannel telemetryChannel)
    {
        _telemetryChannel = telemetryChannel;
    }

    /// <summary>
    /// POST /api/telemetry/ingest
    /// Sahadaki frigorifik araçlardan gelen tekil telemetri paketini Channel<T> tamponuna yazar.
    /// UI thread'i asla bloklanmaz (Zero-Locking).
    /// </summary>
    [HttpPost("ingest")]
    public async Task<IActionResult> IngestPoint([FromBody] TelemetryPoint point)
    {
        if (point == null) return BadRequest("Telemetry point payload is empty.");

        point.Timestamp = DateTime.UtcNow;
        await _telemetryChannel.WriteTelemetryAsync(point);

        return Ok(new { status = "QUEUED_IN_CHANNEL", node = point.NodeId, timestamp = point.Timestamp });
    }

    /// <summary>
    /// POST /api/telemetry/simulate-burst
    /// Test Butonları İçin: Bellek kanalına anında N adet yüksek frekanslı veri paketi pompalar.
    /// </summary>
    [HttpPost("simulate-burst")]
    public async Task<IActionResult> SimulateBurst([FromQuery] int count = 100)
    {
        var random = new Random();

        for (int i = 0; i < count; i++)
        {
            var point = new TelemetryPoint
            {
                NodeId = $"NODE-TR-06-{(i % 3) + 1:D2}",
                VehiclePlate = $"06 APEX {(i % 3) + 1:D2}",
                Temperature = Math.Round(2.5 + (random.NextDouble() * 2.0), 2), // +2.5°C - +4.5°C
                Speed = random.Next(70, 95),
                Humidity = random.Next(40, 60),
                IsDoorOpen = false,
                Timestamp = DateTime.UtcNow
            };

            await _telemetryChannel.WriteTelemetryAsync(point);
        }

        return Ok(new { status = "BURST_INJECTED", count = count, target_channel = "UnboundedChannel<TelemetryPoint>" });
    }
}