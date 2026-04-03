using Content.Shared.Alert;
using Content.Shared.Atmos;
using Robust.Shared.GameObjects;
using Robust.Shared.Timing;

namespace Content.Shared._Sunrise.Abilities;

public sealed class BorgEnvironmentMonitorSystem : EntitySystem
{
    [Dependency] private readonly AlertsSystem _alerts = default!;
    [Dependency] private readonly AtmosphereSystem _atmos = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Update(float frameTime)
    {
        var query = EntityQueryEnumerator<BorgEnvironmentMonitorComponent>();

        while (query.MoveNext(out var uid, out var comp))
        {
            if (!_timing.IsFirstTimePredicted)
                continue;

            if (!TryComp<TransformComponent>(uid, out var xform))
                continue;

            if (!comp.NextUpdate.TryGetValue(uid, out var next))
                next = TimeSpan.Zero;

            if (next > _timing.CurTime)
                continue;

            comp.NextUpdate[uid] = _timing.CurTime + comp.UpdateInterval;

            UpdateEnvironment(uid, comp, xform);
        }
    }

    private void UpdateEnvironment(EntityUid uid, BorgEnvironmentMonitorComponent comp, TransformComponent xform)
    {
        var mixture = _atmos.GetTileMixture(xform.Coordinates);

        if (mixture == null)
            return;

        var pressure = mixture.Pressure;
        var temp = mixture.Temperature;

        // ---- Давление ----
        var lowPressure = pressure < comp.MinPressure;
        var highPressure = pressure > comp.MaxPressure;

        _alerts.SetAlert(uid, comp.LowPressureAlert, lowPressure);
        _alerts.SetAlert(uid, comp.HighPressureAlert, highPressure);

        // ---- Температура ----
        var badTemp = temp < comp.MinTemperature || temp > comp.MaxTemperature;

        _alerts.SetAlert(uid, comp.TemperatureAlert, badTemp);

        // ---- Кислород ----
        var oxygen = mixture.GetMoles(Gas.Oxygen);
        var totalMoles = mixture.TotalMoles;

        float oxygenRatio = 0f;

        if (totalMoles > 0)
            oxygenRatio = oxygen / totalMoles;

        var oxygenPartialPressure = oxygenRatio * pressure;

        var lowOxygen = oxygenPartialPressure < comp.MinOxygenPartialPressure;

        _alerts.SetAlert(uid, comp.LowOxygenAlert, lowOxygen);
    }
}