using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MoveOffsetToAxisEasingTween : ITween
{
    public bool WasCancelled { get; private set; }
    private readonly Transform _target;
    private readonly Vector3 _offset;
    private readonly float _duration;
    private readonly bool _unscaled;
    private readonly bool _local;
    private float _elapsedTime = 0f;
    private float _delay = 0f;
    private AnimationCurve _curveX = EaseType.Linear;
    private AnimationCurve _curveY = EaseType.Linear;
    private AnimationCurve _curveZ = EaseType.Linear;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private CancellationTokenSource _cts;

    public MoveOffsetToAxisEasingTween(Transform target, Vector3 offset, float duration, bool unscaled = false, bool local = false)
    {
        _target = target;
        _offset = offset;
        _duration = duration;
        _unscaled = unscaled;
        _local = local;
    }

    public MoveOffsetToAxisEasingTween SetEaseX(AnimationCurve curve)
    {
        _curveX = curve;
        
        return this;
    }

    public MoveOffsetToAxisEasingTween SetEaseY(AnimationCurve curve)
    {
        _curveY = curve;
        
        return this;
    }

    public MoveOffsetToAxisEasingTween SetEaseZ(AnimationCurve curve)
    {
        _curveZ = curve;
        
        return this;
    }

    public ITween SetEase(AnimationCurve curve)
    {
        _curveX = curve;
        _curveY = curve;
        _curveZ = curve;
        
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
                Vector3 newPosition = _local ? _target.localPosition : _target.position;
                newPosition.x = Mathf.LerpUnclamped(_startPosition.x, _endPosition.x, _curveX.Evaluate(percent));
                newPosition.y = Mathf.LerpUnclamped(_startPosition.y, _endPosition.y, _curveY.Evaluate(percent));
                newPosition.z = Mathf.LerpUnclamped(_startPosition.z, _endPosition.z, _curveZ.Evaluate(percent));

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
            Vector3 finalPosition = _endPosition;
            finalPosition.x = Mathf.LerpUnclamped(_startPosition.x, _endPosition.x, _curveX.Evaluate(1.0f));
            finalPosition.y = Mathf.LerpUnclamped(_startPosition.y, _endPosition.y, _curveY.Evaluate(1.0f));
            finalPosition.z = Mathf.LerpUnclamped(_startPosition.z, _endPosition.z, _curveZ.Evaluate(1.0f));

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
            _endPosition = _startPosition + _offset;
        }
        else
        {
            WasCancelled = true;
        }
    }
}