using System.Threading;
using Cysharp.Threading.Tasks;
using HDTween;
using UnityEngine;

public class LookAtTween : ITween
{
    public bool WasCancelled { get; private set; }
    private readonly Transform _target;
    private readonly Transform _lookAtTransform;
    private readonly Vector3 _lookAtPoint;
    private readonly bool _useTransform;
    private readonly float _duration;
    private readonly bool _unscaled;
    private readonly bool _local;
    private bool _restrictX = false;
    private bool _restrictY = false;
    private bool _restrictZ = false;
    private float _elapsedTime = 0f;
    private float _delay = 0f;
    private AnimationCurve _curve = EaseType.Linear;
    private CancellationTokenSource _cts;

    public LookAtTween(Transform target, Transform lookAtTarget, float duration, bool unscaled = false, bool local = false)
    {
        _target = target;
        _lookAtTransform = lookAtTarget;
        _duration = duration;
        _unscaled = unscaled;
        _local = local;
        _useTransform = true;
    }

    public LookAtTween(Transform target, Vector3 lookAtPoint, float duration, bool unscaled = false, bool local = false)
    {
        _target = target;
        _lookAtPoint = lookAtPoint;
        _duration = duration;
        _unscaled = unscaled;
        _local = local;
        _useTransform = false;
    }

    public LookAtTween SetAxisRestriction(params Axis[] restrictedAxes)
    {
        _restrictX = false;
        _restrictY = false;
        _restrictZ = false;

        foreach (var axis in restrictedAxes)
        {
            switch (axis)
            {
                case Axis.X:
                    _restrictX = true;
                    break;
                case Axis.Y:
                    _restrictY = true;
                    break;
                case Axis.Z:
                    _restrictZ = true;
                    break;
            }
        }

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

        if (_target == null || (!_useTransform && _lookAtTransform == null))
        {
            WasCancelled = true;
            
            return;
        }

        Vector3 lookAtPosition = _useTransform ? _lookAtTransform.position : _lookAtPoint;

        if (_duration <= 0)
        {
            ApplyFinalState(lookAtPosition);
            
            return;
        }

        while (_elapsedTime < _duration)
        {
            if (_cts.Token.IsCancellationRequested || _target == null || (!_useTransform && _lookAtTransform == null))
            {
                WasCancelled = true;
                
                return;
            }

            float delta = _unscaled ? Time.unscaledDeltaTime : Time.deltaTime;
            _elapsedTime += delta;
            float percent = _elapsedTime / _duration;

            Vector3 direction = lookAtPosition - _target.position;

            if (_restrictX) direction.x = 0;
            if (_restrictY) direction.y = 0;
            if (_restrictZ) direction.z = 0;

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Quaternion currentRotation = _local ? _target.localRotation : _target.rotation;
            Quaternion newRotation = Quaternion.Slerp(currentRotation, lookRotation, _curve.Evaluate(percent));

            if (_local)
            {
                _target.localRotation = newRotation;
            }
            else
            {
                _target.rotation = newRotation;
            }

            await UniTask.Yield(PlayerLoopTiming.Update, _cts.Token);
        }
    }

    private void ApplyFinalState(Vector3 lookAtPosition)
    {
        Vector3 direction = lookAtPosition - _target.position;

        if (_restrictX) direction.x = 0;
        if (_restrictY) direction.y = 0;
        if (_restrictZ) direction.z = 0;

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        if (_local)
        {
            _target.localRotation = lookRotation;
        }
        else
        {
            _target.rotation = lookRotation;
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