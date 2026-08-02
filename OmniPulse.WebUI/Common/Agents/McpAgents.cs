using System.Threading.Tasks;
using OmniPulse.WebUI.Controllers;

namespace OmniPulse.WebUI.Common.Agents;

// [MCP_POLYMORPHISM_INTERFACE]
/// <summary>
/// OOP ÇOK BİÇİMLİLİK (POLYMORPHISM) ARAYÜZÜ:
/// Tüm Agentic AI modelleri bu arayüzü uygular. Controller hangi ajanın 
/// çalıştığını bilmez, sadece EvaluatePolymorphicAsync() metodunu çağırır.
/// </summary>
public interface IMcpAgent
{
    string IntentType { get; }
    Task<double> EvaluatePolymorphicAsync(ReasoningRequest request);
}

// [MCP_POLYMORPHISM_CONCRETE_1]
public class ThermalAnomalyAgent : IMcpAgent
{
    public string IntentType => "THERMAL_DRIFT_ANALYSIS";

    public async Task<double> EvaluatePolymorphicAsync(ReasoningRequest request)
    {
        // Polymorphic Davranış 1: Termal (Sıcaklık) sapma analizi mantığı
        await Task.Delay(120); // LLM / Model yanıt gecikmesi simülasyonu
        return 98.85; // %98.85 anomali güven skoru
    }
}

// [MCP_POLYMORPHISM_CONCRETE_2]
public class SpatialDriftAgent : IMcpAgent
{
    public string IntentType => "SPATIAL_ROUTE_ANALYSIS";

    public async Task<double> EvaluatePolymorphicAsync(ReasoningRequest request)
    {
        // Polymorphic Davranış 2: Rota (GPS) sapma analizi mantığı
        await Task.Delay(85);
        return 82.40; // %82.40 rota sapma güven skoru
    }
}