using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HDTween
{
    public class RotateOffsetToTween : ITween
    {
        public bool WasCancelled { get; private set; }
        private readonly Transform _target;
        private readonly Quaternion _offsetRotation;
        private readonly float _duration;
        private readonly bool _unscaled;
        private readonly bool _local;
        private float _elapsedTime = 0f;
        private float _delay = 0f;
        private Quaternion _startRotation;
        private Quaternion _endRotation;
        private AnimationCurve _curve = EaseType.Linear;
        private CancellationTokenSource _cts;

        public RotateOffsetToTween(Transform target, Quaternion offsetRotation, float duration, bool unscaled = false, bool local = false)
        {
            _target = target;
            _offsetRotation = offsetRotation;
            _duration = duration;
            _unscaled = unscaled;
            _local = local;
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
                ApplyFinalState();
                return;
            }

            while (_elapsedTime < _duration)
            {
                if (_cts.Token.IsCancellationRequested || _target == null)
                {
                    WasCancelled = true;
                    return;
                }

                float delta = _unscaled ? Time.unscaledDeltaTime : Time.deltaTime;
                _elapsedTime += delta;
                float percent = _elapsedTime / _duration;

                if (_target != null)
                {
                    Quaternion newRotation = Quaternion.SlerpUnclamped(_startRotation, _endRotation, _curve.Evaluate(percent));
                    if (_local)
                    {
                        _target.localRotation = newRotation;
                    }
                    else
                    {
                        _target.rotation = newRotation;
                    }
                }

                await UniTask.Yield(PlayerLoopTiming.Update, _cts.Token);
            }
        }

        private void ApplyFinalState()
        {
            if (_target != null)
            {
                if (_local)
                {
                    _target.localRotation = _endRotation;
                }
                else
                {
                    _target.rotation = _endRotation;
                }
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

            if (_target != null)
            {
                _startRotation = _local ? _target.localRotation : _target.rotation;
                _endRotation = _startRotation * _offsetRotation;
            }
            else
            {
                WasCancelled = true;
            }
        }
    }
}