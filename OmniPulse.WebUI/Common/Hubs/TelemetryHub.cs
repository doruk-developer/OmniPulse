using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using OmniPulse.Entities.Models.Dto;

namespace OmniPulse.WebUI.Common.Hubs;

/// <summary>
/// .NET 10 SignalR Hub: Ingestion worker tarafından kanaldan okunan telemetri
/// verilerini tarayıcıdaki Chart.js grafiğine ve Dashboard'a canlı yayınlar.
/// </summary>
public class TelemetryHub : Hub
{
    public async Task BroadcastTelemetryPoint(TelemetryPoint point)
    {
        await Clients.All.SendAsync("ReceiveTelemetryPoint", point);
    }
}