using System.Collections.Immutable;

namespace AppyNox.Services.Base.Application.MediatR;

public class NoxCommandAction(
    Action action,
    RunType type = RunType.Before,
    int exceptionCode = 998,
    bool suspendOnFailure = true)
{
    internal int ExceptionCode { get; init; } = exceptionCode;
    public int Order { get; private set; }
    public RunType Type { get; init; } = type;
    public bool SuspendOnFailure { get; init; } = suspendOnFailure;
    public Action Action { get; init; } = action;

    public RunStatus Status { get; private set; } = RunStatus.Incomplete;

    internal void SetOrder(int order)
    {
        Order = order;
    }

    internal void SetStatus(RunStatus status)
    { 
        Status = status; 
    }
}

public class NoxCommandExtensions
{
    private readonly IList<NoxCommandAction> _actions = [];

    private int _orderBefore = 0;
    private int _orderAfter = 0;
    public IImmutableList<NoxCommandAction> Actions => _actions.ToImmutableList();

    /// <summary>
    /// B1,B2,B3 ... means actions before the main action
    /// <para>M means main action</para>
    /// <para>A1,A2,A3 ... means actions after the main action</para>
    /// </summary>
    public List<string> RunOrderHistory { get; private set; } = [];

    public NoxCommandExtensions(params NoxCommandAction[] noxCommandActions)
    {
        foreach (var action in noxCommandActions)
        {
            AddAction(action);
        }
    }
    public void AddAction(NoxCommandAction action)
    {
        action.SetOrder(action.Type == RunType.Before ? _orderBefore++ : _orderAfter++);
        _actions.Add(action);
    }
}


/// <summary>
/// Defines when the custom action will be ran. Before the handler or after the handler.
/// </summary>
public enum RunType
{
    Before = 0,
    After = 1
}

public enum RunStatus
{
    Incomplete = 0,
    Finished = 1,
    ExitedWithError = 2
}