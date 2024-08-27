using System.Threading;
using Cysharp.Threading.Tasks;
using HDTween;
using UnityEngine;

public class ConstantRotateTween : ITween
{
    private Transform _target;
    private Vector3 _rotationSpeeds = new Vector3(0, 0, 1);
    private bool _unscaled;
    private bool _local;
    private float _elapsedTime = 0f;
    private float _duration;
    private float _delay = 0f;
    private AnimationCurve _curve = EaseType.Linear;
    private CancellationTokenSource _cts;

    public bool WasCancelled { get; private set; }

    public ConstantRotateTween(Transform target, float duration, bool unscaled = false, bool local = false)
    {
        _target = target;
        _duration = duration;
        _unscaled = unscaled;
        _local = local;
    }

    public ConstantRotateTween SetRotationSpeed(Vector3 rotationSpeeds)
    {
        _rotationSpeeds = rotationSpeeds;
        
        return this;
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
            float evaluatedPercent = _curve.Evaluate(_elapsedTime / _duration);
            Vector3 rotationStep = _rotationSpeeds * delta * evaluatedPercent;

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

    private void ApplyFinalState()
    {
        Vector3 finalRotation = _rotationSpeeds * _duration * _curve.Evaluate(1.0f);

        if (_local)
        {
            _target.localRotation *= Quaternion.Euler(finalRotation);
        }
        else
        {
            _target.rotation *= Quaternion.Euler(finalRotation);
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