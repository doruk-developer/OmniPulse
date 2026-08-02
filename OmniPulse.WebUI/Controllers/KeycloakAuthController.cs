using Microsoft.AspNetCore.Mvc;
using System;
using OmniPulse.WebUI.Common.Security;

namespace OmniPulse.WebUI.Controllers;

[ApiController]
[Route("api/auth")]
public class KeycloakAuthController : ControllerBase
{
    private readonly QuantumSafeMinter _quantumSafeMinter;

    public KeycloakAuthController()
    {
        _quantumSafeMinter = new QuantumSafeMinter();
    }

    /// <summary>
    /// POST /api/auth/token
    /// Keycloak OIDC Token alma ve Zero-Trust el sıkışma simülasyonu.
    /// Kapsülleme (Encapsulation): Token claims ve kuantum korumalı imza arka plana gizlenir.
    /// </summary>
    [HttpPost("token")]
    public IActionResult IssueOidcToken([FromQuery] string username = "Doruk_Avgın_Apex")
    {
        var payload = $"{username}:{Guid.NewGuid():N}:OIDC_TENANT_SHARED";
        var quantumSignature = _quantumSafeMinter.MintQuantumResistantSignature(payload);

        return Ok(new
        {
            access_token = $"eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.omnipulse.{Guid.NewGuid():N}",
            token_type = "Bearer",
            expires_in = 3600,
            user = username,
            titan_node_identity = "NODE-TITAN-DEFENSE-01",
            quantum_signature = quantumSignature,
            security_policy = "ZERO_TRUST_ENCAPSULATED"
        });
    }
}