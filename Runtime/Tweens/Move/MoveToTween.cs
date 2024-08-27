using System.Threading;
using Cysharp.Threading.Tasks;
using HDTween;
using UnityEngine;

public class MoveToTween : ITween
{
    public bool WasCancelled { get; private set; }
    private readonly Transform _target;
    private readonly float _duration;
    private readonly bool _unscaled;
    private readonly bool _local;
    private float _elapsedTime = 0f;
    private float _delay = 0f;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private AnimationCurve _curve = EaseType.Linear;
    private CancellationTokenSource _cts;

    public MoveToTween(Transform target, Vector3 endPosition, float duration, bool unscaled = false, bool local = false)
    {
        _target = target;
        _local = local;
        _startPosition = _local ? target.localPosition : target.position;
        _endPosition = endPosition;
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
                Vector3 newPosition = Vector3.LerpUnclamped(_startPosition, _endPosition, _curve.Evaluate(percent));
                if (_local)
                {
                    _target.localPosition = newPosition;
                }
                else
                {
                    _target.position = newPosition;
                }
            }

            await UniTask.Yield(PlayerLoopTiming.Update, _cts.Token);
        }
    }

    private void ApplyFinalState()
    {
        if (_target != null)
        {
            Vector3 finalPosition = Vector3.LerpUnclamped(_startPosition, _endPosition, _curve.Evaluate(1.0f));

            if (_local)
            {
                _target.localPosition = finalPosition;
            }
            else
            {
                _target.position = finalPosition;
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
            _startPosition = _local ? _target.localPosition : _target.position;
        }
        else
        {
            WasCancelled = true;
        }
    }
}