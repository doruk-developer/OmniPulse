using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using OmniPulse.Business.Services;
using OmniPulse.Entities.Models.Dto;

namespace OmniPulse.WebUI.Controllers;

[ApiController]
[Route("api/workflows")]
public class WorkflowsController : ControllerBase
{
    private readonly IColdChainWorkflowEngine _workflowEngine;

    public WorkflowsController(IColdChainWorkflowEngine workflowEngine)
    {
        _workflowEngine = workflowEngine;
    }

    /// <summary>
    /// POST /api/workflows/trigger-thermal-rule
    /// Sınır ihlali olan bir sıcaklık verisi göndererek Elsa iş akışını manuel tetikler.
    /// </summary>
    [HttpPost("trigger-thermal-rule")]
    public async Task<IActionResult> TriggerThermalRule([FromBody] TelemetryPoint point)
    {
        if (point == null) point = new TelemetryPoint { NodeId = "NODE-TR-06-03", Temperature = 9.4 };

        await _workflowEngine.ProcessThermalTelemetryAsync(point);

        return Ok(new
        {
            status = "ELSA_WORKFLOW_EXECUTED",
            rule = "ColdChainThermalThresholdRule",
            node = point.NodeId,
            temperature = point.Temperature,
            action_taken = point.Temperature > 8.0 ? "COOLER_MOTOR_OVERRIDE_DISPATCHED" : "NOMINAL_NO_ACTION"
        });
    }
}