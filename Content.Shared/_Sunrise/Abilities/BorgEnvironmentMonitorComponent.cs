using Robust.Shared.GameObjects;
using Robust.Shared.Serialization;

namespace Content.Shared._Sunrise.Abilities;

[RegisterComponent]
public sealed class BorgEnvironmentMonitorComponent : Component
{
    [DataField] public TimeSpan UpdateInterval = TimeSpan.FromSeconds(1);

    [DataField] public float MinPressure = 50f;
    [DataField] public float MaxPressure = 300f;

    [DataField] public float MinTemperature = 260f;
    [DataField] public float MaxTemperature = 360f;
    [DataField] public float MinOxygenPartialPressure = 16f;

    [DataField] public ProtoId<AlertPrototype> HighPressureAlert = "HighPressure";
    [DataField] public ProtoId<AlertPrototype> LowPressureAlert = "LowPressure";
    [DataField] public ProtoId<AlertPrototype> TemperatureAlert = "Temperature";
    [DataField] public ProtoId<AlertPrototype> LowOxygenAlert = "LowOxygen";

}