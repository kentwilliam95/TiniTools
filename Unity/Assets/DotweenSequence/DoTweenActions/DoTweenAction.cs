using DG.Tweening;
using UnityEngine;

public abstract class DoTweenAction
{
    protected DotweenData data;
    public abstract Tween GenerateDotween(Component component);
    public abstract void OnEditorGUI(ref Rect rect);
    public abstract void OnEditorGUILayout();
    public virtual void SetData(DotweenData data)
    {
        this.data = data;
    }
}