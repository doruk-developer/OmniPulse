using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using OmniPulse.Entities.Models.Dto;

namespace OmniPulse.Business.Services;

public interface ITelemetryChannel
{
    ValueTask WriteTelemetryAsync(TelemetryPoint point, CancellationToken cancellationToken = default);
    IAsyncEnumerable<TelemetryPoint> ReadTelemetryStreamAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// .NET 8 System.Threading.Channels kullanan yüksek performanslı, kilitlenmesiz (zero-locking)
/// bellek içi telemetri tamponlama servisi.
/// </summary>
public class TelemetryChannel : ITelemetryChannel
{
    private readonly Channel<TelemetryPoint> _channel;

    public TelemetryChannel()
    {
        // Unbounded (Sınırsız) veya Bounded bellek kanalı. UI/Kullanıcı thread'ini asla bloklamaz.
        var options = new UnboundedChannelOptions
        {
            SingleReader = true,  // Tek arka plan işleyicisi (BackgroundWorker) okur
            SingleWriter = false  // Birden fazla API/Sensör isteği aynı anda yazabilir
        };

        _channel = Channel.CreateUnbounded<TelemetryPoint>(options);
    }

    public async ValueTask WriteTelemetryAsync(TelemetryPoint point, CancellationToken cancellationToken = default)
    {
        await _channel.Writer.WriteAsync(point, cancellationToken);
    }

    public IAsyncEnumerable<TelemetryPoint> ReadTelemetryStreamAsync(CancellationToken cancellationToken = default)
    {
        return _channel.Reader.ReadAllAsync(cancellationToken);
    }
}