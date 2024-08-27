using System.Threading;
using Cysharp.Threading.Tasks;
using HDTween;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColorToTween : ITween
{
    public bool WasCancelled { get; private set; }
    private readonly Graphic _graphic;
    private readonly SpriteRenderer _spriteRenderer;
    private readonly Light _light;
    private readonly Material _material;
    private readonly float _duration;
    private readonly bool _unscaled;
    private float _elapsedTime = 0f;
    private float _delay = 0f;
    private Color _startColor;
    private readonly Color _targetColor;
    private AnimationCurve _curve = EaseType.Linear;
    private CancellationTokenSource _cts;

    public ChangeColorToTween(Graphic graphic, Color targetColor, float duration, bool unscaled = false)
    {
        _graphic = graphic;
        _startColor = graphic.color;
        _targetColor = targetColor;
        _duration = duration;
        _unscaled = unscaled;
    }

    public ChangeColorToTween(SpriteRenderer spriteRenderer, Color targetColor, float duration, bool unscaled = false)
    {
        _spriteRenderer = spriteRenderer;
        _startColor = spriteRenderer.color;
        _targetColor = targetColor;
        _duration = duration;
        _unscaled = unscaled;
    }

    public ChangeColorToTween(Light light, Color targetColor, float duration, bool unscaled = false)
    {
        _light = light;
        _startColor = light.color;
        _targetColor = targetColor;
        _duration = duration;
        _unscaled = unscaled;
    }

    public ChangeColorToTween(Material material, Color targetColor, float duration, bool unscaled = false)
    {
        _material = material;
        _startColor = material.color;
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

        if (IsTargetValid() == false)
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
            if (_cts.Token.IsCancellationRequested || IsTargetValid() == false)
            {
                WasCancelled = true;
                
                return;
            }

            float delta = _unscaled ? Time.unscaledDeltaTime : Time.deltaTime;
            _elapsedTime += delta;
            float percent = _elapsedTime / _duration;
            Color newColor = Color.LerpUnclamped(_startColor, _targetColor, _curve.Evaluate(percent));
            ApplyColor(newColor);

            await UniTask.Yield(PlayerLoopTiming.Update, _cts.Token);
        }
    }

    private void ApplyFinalState()
    {
        Color finalColor = Color.LerpUnclamped(_startColor, _targetColor, _curve.Evaluate(1.0f));
        ApplyColor(finalColor);
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

        if (_graphic != null)
        {
            _startColor = _graphic.color;
        }
        else if (_spriteRenderer != null)
        {
            _startColor = _spriteRenderer.color;
        }
        else if (_light != null)
        {
            _startColor = _light.color;
        }
        else if (_material != null)
        {
            _startColor = _material.color;
        }
        else
        {
            WasCancelled = true;
        }
    }

    private bool IsTargetValid()
    {
        return _graphic != null || _spriteRenderer != null || _light != null || _material != null;
    }

    private void ApplyColor(Color color)
    {
        if (_graphic != null)
        {
            _graphic.color = color;
        }
        else if (_spriteRenderer != null)
        {
            _spriteRenderer.color = color;
        }
        else if (_light != null)
        {
            _light.color = color;
        }
        else if (_material != null)
        {
            _material.color = color;
        }
    }
}