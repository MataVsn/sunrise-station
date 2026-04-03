using Content.Shared.Actions;
using Robust.Shared.Localization;

namespace Content.Shared._Sunrise.Abilities;

public sealed class BorgEnvironmentMonitorSystem : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<BorgEnvironmentMonitorComponent, GetVerbsEvent<ActivationVerb>>(OnGetActivationVerbs);
    }

    private void OnGetActivationVerbs(EntityUid uid, BorgEnvironmentMonitorComponent component, GetVerbsEvent<ActivationVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract)
            return;

        ActivationVerb verb = new()
        {
            Text = Loc.GetString("borg-env-mon-verb"),
            Act = () => MonitorEnvironment(uid, args.User),
        };
        args.Verbs.Add(verb);
    }

    private void MonitorEnvironment(EntityUid uid, EntityUid user)
    {
        _popup.PopupEntity(Loc.GetString("borg-env-mon-message"), uid, user);
    }
}