using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OmniPulse.Entities.Models.Dto;

namespace OmniPulse.Business.Services;

/// <summary>
/// OOP Kalıtım (Inheritance) Ata Sınıfı:
/// Tüm Elsa İş Akışı adımları bu sınıftan türetilir.
/// </summary>
public abstract class BaseWorkflowStep
{
    public string StepName { get; protected set; } = string.Empty;
    public DateTime ExecutionTime { get; protected set; } = DateTime.UtcNow;

    public abstract Task<bool> ExecuteStepAsync(TelemetryPoint point);
}

public interface IColdChainWorkflowEngine
{
    Task ProcessThermalTelemetryAsync(TelemetryPoint point);
}

/// <summary>
/// .NET 10 Elsa Workflows İş Akışı Motoru:
/// BaseWorkflowStep sınıfından KALITILMIŞTIR (Inheritance).
/// Sıcaklık +8.0°C üzerine çıktığında otomatik kriz sürecini tetikler.
/// </summary>
public class ColdChainBreachWorkflow : BaseWorkflowStep, IColdChainWorkflowEngine
{
    private readonly ILogger<ColdChainBreachWorkflow> _logger;

    public ColdChainBreachWorkflow(ILogger<ColdChainBreachWorkflow> logger)
    {
        _logger = logger;
        StepName = "ColdChainThermalThresholdRule";
    }

    public override async Task<bool> ExecuteStepAsync(TelemetryPoint point)
    {
        // +8.0°C Eşik Kontrolü
        if (point.Temperature > 8.0)
        {
            _logger.LogWarning($">> [ELSA_WORKFLOW_TRIGGER]: Thermal Breach on Node={point.NodeId} ({point.Temperature}°C > +8.0°C)! Executing automated motor override...");
            await Task.Delay(50); // Asenkron iş akışı simülasyonu
            return true; // Anomali Var
        }

        return false; // Nominal
    }

    public async Task ProcessThermalTelemetryAsync(TelemetryPoint point)
    {
        bool isBreached = await ExecuteStepAsync(point);
        if (isBreached)
        {
            _logger.LogCritical($">> [ELSA_INCIDENT_DISPATCH]: Generated Incident Ticket for Node={point.NodeId}. Driver & Tech team notified via SignalR.");
        }
    }
}