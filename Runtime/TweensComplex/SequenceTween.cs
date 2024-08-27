using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SequenceTween : ITween
{
    public bool HasCancellationRequested { get; private set; }
    public bool WasCancelled { get; private set; } 
    private readonly bool _stopOnAnyTweenCancel;
    private readonly List<ITween> _tweens = new List<ITween>();

    public SequenceTween(bool stopOnAnyTweenCancel = true)
    {
        _stopOnAnyTweenCancel = stopOnAnyTweenCancel;
    }

    public SequenceTween AddTween(ITween tween)
    {
        _tweens.Add(tween);
        
        return this;
    }

    public ITween SetEase(AnimationCurve curve)
    {
        foreach (ITween tween in _tweens)
        {
            tween.SetEase(curve);
        }
        
        return this;
    }

    public ITween SetDelay(float delay)
    {
        foreach (ITween tween in _tweens)
        {
            tween.SetDelay(delay);
        }
        
        return this;
    }

    public async UniTask ExecuteAsync(CancellationToken cancellationToken)
    {
        foreach (ITween tween in _tweens)
        {
            if (HasCancellationRequested || cancellationToken.IsCancellationRequested)
            {
                WasCancelled = true;
                break;
            }

            await tween.ExecuteAsync(cancellationToken);

            if (HasCancellationRequested || cancellationToken.IsCancellationRequested)
            {
                WasCancelled = true;
                break;
            }

            if (tween.WasCancelled && _stopOnAnyTweenCancel)
            {
                WasCancelled = true;
                break;
            }
        }
    }

    public void Cancel()
    {
        HasCancellationRequested = true;
        WasCancelled = true;
        
        foreach (ITween tween in _tweens)
        {
            tween.Cancel();
        }
    }
}