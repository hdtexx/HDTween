using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using HDTween;

public class TweenManager
{
    private readonly List<ITween> _activeTweens = new List<ITween>();

    private static TweenManager _instance;

    private TweenManager()
    {
    }

    public static TweenManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new TweenManager();
            }

            return _instance;
        }
    }
    
    public ITween StartTween(ITween tween)
    {
        _activeTweens.Add(tween);
        tween.ExecuteAsync(CancellationToken.None).Forget();
        
        return tween;
    }

    public ITween StartTween(ITween tween, int repeatCount)
    {
        if (repeatCount == 0)
        {
            return null;
        }

        _activeTweens.Add(tween);
        RunTweenWithRepeats(tween, repeatCount).Forget();
        
        return tween;
    }

    private async UniTask RunTweenWithRepeats(ITween tween, int repeatCount)
    {
        int currentRepeat = 0;

        while (repeatCount < 0 || currentRepeat < repeatCount)
        {
            await tween.ExecuteAsync(CancellationToken.None);

            if (repeatCount > 0)
            {
                currentRepeat++;
            }

            if (tween is SequenceTween sequence && sequence.HasCancellationRequested)
            {
                break;
            }

            if (tween is CompositeTween composite && composite.HasCancellationRequested)
            {
                break;
            }
        }

        _activeTweens.Remove(tween);
    }


    public void CancelTween(ITween tween)
    {
        if (tween == null)
        {
            return;
        }

        if (_activeTweens.Contains(tween) == false)
        {
            return;
        }
        
        tween.Cancel();
        _activeTweens.Remove(tween);
    }

    public void CancelAllTweens()
    {
        foreach (var tween in _activeTweens)
        {
            tween.Cancel();
        }

        _activeTweens.Clear();
    }
}