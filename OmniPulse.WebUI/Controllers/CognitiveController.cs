using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OmniPulse.Entities.Events;
using OmniPulse.WebUI.Common.Agents; // Ajanlarımızın Yolu

namespace OmniPulse.WebUI.Controllers;

[Route("Cognitive")]
public class CognitiveController : Controller
{
    private readonly IMediator _mediator;
    private readonly IEnumerable<IMcpAgent> _mcpAgents; // [MCP_POLYMORPHISM_INJECTION]
    private readonly ILogger<CognitiveController> _logger;

    public CognitiveController(
        IMediator mediator,
        IEnumerable<IMcpAgent> mcpAgents,
        ILogger<CognitiveController> logger)
    {
        _mediator = mediator;
        _mcpAgents = mcpAgents;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index() => View();

    [HttpPost("neural/reasoning-chain")]
    public async Task<IActionResult> ExecuteReasoningChain([FromBody] ReasoningRequest request)
    {
        try
        {
            _logger.LogInformation($">> [MCP_SERVER]: Chain_of_Thought sequence initiated: {request.SequenceId}");

            // [MCP_POLYMORPHISM_EXECUTION]
            // Gelen Intent'e göre doğru ajanı dinamik olarak buluyoruz. (Bulamazsa Thermal çalışsın)
            var targetAgent = _mcpAgents.FirstOrDefault(a => a.IntentType == request.Intent)
                              ?? _mcpAgents.FirstOrDefault(a => a.IntentType == "THERMAL_DRIFT_ANALYSIS");

            if (targetAgent == null) return StatusCode(500, "MCP Agent not configured.");

            _logger.LogWarning($">> [AGENT_DISPATCH]: Invoking {targetAgent.GetType().Name} polymorphically...");

            // ÇOK BİÇİMLİLİK ŞOVU: Ajanın türünü bilmeden aynı metodu çağırıyoruz!
            double aiConfidenceScore = await targetAgent.EvaluatePolymorphicAsync(request);

            return Ok(new
            {
                trace_id = Guid.NewGuid(),
                confidence = aiConfidenceScore,
                conclusion = $"Agent [{targetAgent.GetType().Name}] completed reasoning sequence.",
                agent_intent = targetAgent.IntentType
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Reasoning chain broken.");
            return StatusCode(500);
        }
    }
}