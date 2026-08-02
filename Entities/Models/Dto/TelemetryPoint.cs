namespace OmniPulse.Entities.Models.Dto;

public class TelemetryPoint // <- 'public' kelimesi mutlaka olmalı
{
    public string NodeId { get; set; } = string.Empty;
    public string VehiclePlate { get; set; } = string.Empty;
    public double Temperature { get; set; }
    public double Speed { get; set; }
    public double Humidity { get; set; }
    public bool IsDoorOpen { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string MeasurementName { get; set; } = "thermal_telemetry";
}