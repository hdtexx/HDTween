using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface ITween
{
    ITween SetEase(AnimationCurve curve);
    ITween SetDelay(float delay);
    UniTask ExecuteAsync(CancellationToken cancellationToken);
    void Cancel();
    bool WasCancelled { get; }
}