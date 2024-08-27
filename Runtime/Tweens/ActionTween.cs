using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ActionTween : ITween
{
    public bool WasCancelled { get; private set; }
    private readonly Action _action;
    private float _delay = 0f;
    private CancellationTokenSource _cts;

    public ActionTween(Action action)
    {
        _action = action;
    }

    public ITween SetDelay(float delay)
    {
        _delay = delay;
        return this;
    }

    public ITween SetEase(AnimationCurve curve)
    {
        return this;
    }

    public async UniTask ExecuteAsync(CancellationToken cancellationToken)
    {
        ResetState();
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        if (_delay > 0)
        {
            await UniTask.Delay((int)(_delay * 1000), cancellationToken: _cts.Token);
        }

        if (_action == null || _cts.Token.IsCancellationRequested)
        {
            WasCancelled = true;
            
            return;
        }

        _action.Invoke();
    }

    public void Cancel()
    {
        WasCancelled = true;
        _cts?.Cancel();
    }

    private void ResetState()
    {
        WasCancelled = false;
    }
}