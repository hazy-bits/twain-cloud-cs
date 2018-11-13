namespace HazyBits.Twain.Cloud.Telemetry
{
    public interface IContextExtender
    {
        void Extend(TelemetryContext context);
    }
}
