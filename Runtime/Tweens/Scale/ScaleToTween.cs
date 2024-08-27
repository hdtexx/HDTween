using System.Threading;
using Cysharp.Threading.Tasks;
using HDTween;
using UnityEngine;

public class ScaleToTween : ITween
{
    public bool WasCancelled { get; private set; }
    private readonly Transform _target;
    private readonly Vector3 _scaleMultiplier;
    private readonly float _duration;
    private readonly bool _unscaled;
    private float _elapsedTime = 0f;
    private float _delay = 0f;
    private Vector3 _startScale;
    private Vector3 _targetScale;
    private AnimationCurve _curve = EaseType.Linear;
    private CancellationTokenSource _cts;

    public ScaleToTween(Transform target, Vector3 scaleMultiplier, float duration, bool unscaled = false)
    {
        _target = target;
        _startScale = target.localScale;
        _scaleMultiplier = scaleMultiplier;
        _targetScale = Vector3.Scale(_startScale, scaleMultiplier);
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
                _target.localScale = Vector3.LerpUnclamped(_startScale, _targetScale, _curve.Evaluate(percent));
            }

            await UniTask.Yield(PlayerLoopTiming.Update, _cts.Token);
        }
    }

    private void ApplyFinalState()
    {
        if (_target != null)
        {
            _target.localScale = Vector3.LerpUnclamped(_startScale, _targetScale, _curve.Evaluate(1.0f));
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
            _startScale = _target.localScale;
            _targetScale = new Vector3(
                _startScale.x != 0 ? _startScale.x * _scaleMultiplier.x : _scaleMultiplier.x,
                _startScale.y != 0 ? _startScale.y * _scaleMultiplier.y : _scaleMultiplier.y,
                _startScale.z != 0 ? _startScale.z * _scaleMultiplier.z : _scaleMultiplier.z
            );
        }
        else
        {
            WasCancelled = true;
        }
    }
}