using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using UnityEngine.UI;

public class DotweenAnimationFactory
{
    public static Tween CreateTween(DotweenData data)
    {
        var action = GetDotweenAction(data.selectedDotweenActionID);
        action.SetData(data);
        Tween t = action.GenerateDotween(data.component);
        t.Pause();
        return t;
    }

    public static Sequence GenerateSequence(DotweenHeaderData data)
    {
        var sequence = DOTween.Sequence();
        sequence.SetLoops(data.isRepeat ? -1 : 1);
        sequence.SetAutoKill(data.isAutoKill);

        for (int i = 0; i < data.animationData.Length; i++)
        {
            var subData = data.animationData[i];
            var tween = CreateTween(subData);
            sequence.Insert(subData.startAt, tween);
        }

        return sequence;
    }

    public static DoTweenAction GetDotweenAction(DotweenActionID actionID)
    {
        DoTweenAction action = null;
        switch (actionID)
        {
            case DotweenActionID.DoMove:
                action = new DoMoveAction();
                break;

            case DotweenActionID.DoLocalMove:
                action = new DoLocalMoveAction();
                break;

            case DotweenActionID.DoLocalRotate:
                action = new DoLocalRotationAction();
                break;

            case DotweenActionID.DoRotate:
                action = new DoRotationAction();
                break;

            case DotweenActionID.DoScale:
                action = new DoScaleAction();
                break;

            case DotweenActionID.DoFade:
                action = new DoFadeAction();
                break;
        }
        return action;
    }
}