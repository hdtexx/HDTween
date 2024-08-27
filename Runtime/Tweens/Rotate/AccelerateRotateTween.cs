using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HDTween
{
    public class AccelerateRotateTween : ITween
    {
        public bool WasCancelled { get; private set; }
        private readonly Transform _target;
        private readonly Vector3 _rotationSpeeds;
        private readonly bool _unscaled;
        private readonly bool _local;
        private readonly float _duration;
        private readonly float _accelerationDuration;
        private readonly float _decelerationDuration;
        private readonly float _steadyDuration;
        private float _elapsedTime = 0f;
        private float _delay = 0f;
        private AnimationCurve _curve = EaseType.EaseInOutQuad;
        private CancellationTokenSource _cts;

        public AccelerateRotateTween(Transform target, float duration, float accelerationDuration, float decelerationDuration, Vector3 rotationSpeeds, bool unscaled = false, bool local = false)
        {
            _target = target;
            _duration = duration;
            _accelerationDuration = accelerationDuration;
            _decelerationDuration = decelerationDuration;
            _rotationSpeeds = rotationSpeeds;
            _unscaled = unscaled;
            _local = local;
            _steadyDuration = _duration - (_accelerationDuration + _decelerationDuration);
        }

        public ITween SetEase(AnimationCurve curve)
        {
            _curve = curve;
            return this;
        }

        public ITween SetDelay(float delay)
        {
            _delay = delay;
            
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

            if (_target == null)
            {
                WasCancelled = true;
                
                return;
            }

            if (_duration <= 0)
            {
                return;
            }

            if (_accelerationDuration > 0)
            {
                await RotateWithAcceleration(_accelerationDuration, true);
            }

            if (_steadyDuration > 0 && !WasCancelled)
            {
                await RotateAtSteadySpeed(_steadyDuration);
            }

            if (_decelerationDuration > 0 && !WasCancelled)
            {
                await RotateWithAcceleration(_decelerationDuration, false);
            }
        }

        private async UniTask RotateWithAcceleration(float duration, bool accelerating)
        {
            float phaseTime = 0f;
            
            while (phaseTime < duration)
            {
                if (_cts.Token.IsCancellationRequested || _target == null)
                {
                    WasCancelled = true;
                    
                    return;
                }

                float delta = _unscaled ? Time.unscaledDeltaTime : Time.deltaTime;
                phaseTime += delta;
                float percent = phaseTime / duration;
                float evaluatedPercent = _curve.Evaluate(percent);
                float speedFactor = accelerating ? evaluatedPercent : (1f - evaluatedPercent);

                Vector3 rotationStep = _rotationSpeeds * delta * speedFactor;

                if (_local)
                {
                    _target.localRotation *= Quaternion.Euler(rotationStep);
                }
                else
                {
                    _target.rotation *= Quaternion.Euler(rotationStep);
                }

                await UniTask.Yield(PlayerLoopTiming.Update, _cts.Token);
            }
        }

        private async UniTask RotateAtSteadySpeed(float duration)
        {
            float phaseTime = 0f;
            
            while (phaseTime < duration)
            {
                if (_cts.Token.IsCancellationRequested || _target == null)
                {
                    WasCancelled = true;
                    
                    return;
                }

                float delta = _unscaled ? Time.unscaledDeltaTime : Time.deltaTime;
                phaseTime += delta;

                Vector3 rotationStep = _rotationSpeeds * delta;

                if (_local)
                {
                    _target.localRotation *= Quaternion.Euler(rotationStep);
                }
                else
                {
                    _target.rotation *= Quaternion.Euler(rotationStep);
                }

                await UniTask.Yield(PlayerLoopTiming.Update, _cts.Token);
            }
        }

        public void Cancel()
        {
            WasCancelled = true;
            _cts?.Cancel();
        }

        private void ResetState()
        {
            WasCancelled = false;
            _elapsedTime = 0f;
        }
    }
}
