using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using HDTween;
using UnityEngine;

public class CompositeTween : ITween
{
    public bool HasCancellationRequested { get; private set; }
    public bool WasCancelled { get; private set; }
    private readonly bool _stopOnAnyTweenCancel;
    private readonly List<ITween> _tweens = new List<ITween>();

    public CompositeTween(bool stopOnAnyTweenCancel = true)
    {
        _stopOnAnyTweenCancel = stopOnAnyTweenCancel;
    }

    public CompositeTween AddTween(ITween tween)
    {
        _tweens.Add(tween);
        
        return this;
    }

    public ITween SetEase(AnimationCurve curve)
    {
        foreach (var tween in _tweens)
        {
            tween.SetEase(curve);
        }
        
        return this;
    }

    public ITween SetDelay(float delay)
    {
        foreach (var tween in _tweens)
        {
            tween.SetDelay(delay);
        }
        
        return this;
    }

    public async UniTask ExecuteAsync(CancellationToken cancellationToken)
    {
        List<UniTask> tasks = new List<UniTask>();

        foreach (var tween in _tweens)
        {
            if (HasCancellationRequested || cancellationToken.IsCancellationRequested)
            {
                WasCancelled = true;
                break;
            }

            UniTask task = ExecuteTweenAsync(tween, cancellationToken);
            tasks.Add(task);
        }

        await UniTask.WhenAll(tasks);

        if (HasCancellationRequested || cancellationToken.IsCancellationRequested)
        {
            WasCancelled = true;
        }
    }

    private async UniTask ExecuteTweenAsync(ITween tween, CancellationToken cancellationToken)
    {
        await tween.ExecuteAsync(cancellationToken);

        if (tween.WasCancelled && _stopOnAnyTweenCancel)
        {
            WasCancelled = true;
            Cancel();
        }
    }

    public void Cancel()
    {
        HasCancellationRequested = true;
        WasCancelled = true;
        
        foreach (var tween in _tweens)
        {
            tween.Cancel();
        }
    }
}