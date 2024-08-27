using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HDTween
{
    public class PauseTween : ITween
    {
        public bool WasCancelled { get; private set; }
        private readonly float _duration;
        private CancellationTokenSource _cts;

        public PauseTween(float duration)
        {
            _duration = duration;
        }

        public ITween SetEase(AnimationCurve curve)
        {
            return this;
        }

        public ITween SetDelay(float delay)
        {
            return this;
        }

        public async UniTask ExecuteAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        
            try
            {
                await UniTask.Delay((int)(_duration * 1000), cancellationToken: _cts.Token);
            }
            catch (OperationCanceledException)
            {
                WasCancelled = true;
            }
        }

        public void Cancel()
        {
            WasCancelled = true;
            _cts?.Cancel();
        }
    }
}