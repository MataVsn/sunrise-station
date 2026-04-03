using Robust.Shared.GameObjects;
using Robust.Shared.Serialization;

namespace Content.Shared._Sunrise.Abilities;

[RegisterComponent]
public sealed class BorgEnvironmentMonitorComponent : Component
{
    [DataField("range")]
    public float Range = 10f;

    [DataField("updateInterval")]
    public TimeSpan UpdateInterval = TimeSpan.FromSeconds(1);

    [DataField]
    public ProtoId<AlertPrototype> HighPressureAlert = "HighPressure";

    [DataField]
    public ProtoId<AlertPrototype> LowPressureAlert = "LowPressure";

    [DataField]
    public ProtoId<AlertPrototype> TemperatureAlert = "Temperature";
}