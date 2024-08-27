using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HDTween
{
    public class ChangeShaderColorToTween : ITween
    {
        public bool WasCancelled { get; private set; }
        private readonly string _shaderKey;
        private readonly float _duration;
        private readonly bool _unscaled;
        private float _elapsedTime = 0f;
        private float _delay = 0f;
        private readonly Material _material;
        private UnityEngine.Color _startColor;
        private readonly UnityEngine.Color _targetColor;
        private AnimationCurve _curve = EaseType.Linear;
        private CancellationTokenSource _cts;

        public ChangeShaderColorToTween(Material material, string shaderKey, UnityEngine.Color targetColor, float duration, bool unscaled = false)
        {
            _material = material;
            _shaderKey = shaderKey;
            _startColor = material.GetColor(shaderKey);
            _targetColor = targetColor;
            _duration = duration;
            _unscaled = unscaled;
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

            if (_material == null)
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
                if (_cts.Token.IsCancellationRequested || _material == null)
                {
                    WasCancelled = true;
                
                    return;
                }

                float delta = _unscaled ? Time.unscaledDeltaTime : Time.deltaTime;
                _elapsedTime += delta;
                float percent = _elapsedTime / _duration;
                UnityEngine.Color newColor = UnityEngine.Color.LerpUnclamped(_startColor, _targetColor, _curve.Evaluate(percent));

                if (_material != null)
                {
                    _material.SetColor(_shaderKey, newColor);
                }

                await UniTask.Yield(PlayerLoopTiming.Update, _cts.Token);
            }
        }

        private void ApplyFinalState()
        {
            if (_material != null)
            {
                UnityEngine.Color finalColor = UnityEngine.Color.LerpUnclamped(_startColor, _targetColor, _curve.Evaluate(1.0f));
                _material.SetColor(_shaderKey, finalColor);
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

            if (_material != null)
            {
                _startColor = _material.GetColor(_shaderKey);
            }
            else
            {
                WasCancelled = true;
            }
        }
    }
}