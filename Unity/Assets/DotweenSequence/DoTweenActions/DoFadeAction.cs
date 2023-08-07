using DG.Tweening;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class DoFadeAction : DoTweenAction
{
    public override Tween GenerateDotween(Component component)
    {
        Tween tween = null;
        var image = component.GetComponent<Image>();
        var canvasGroup = component.GetComponent<CanvasGroup>();

        switch (data.isUsingFrom)
        {
            case true:
                if (image)
                    tween = image.DOFade(data.endOpacity, data.duration).From(data.fromOpacity).SetEase(data.ease);
                else if (canvasGroup)
                    tween = canvasGroup.DOFade(data.endOpacity, data.duration).From(data.fromOpacity).SetEase(data.ease);
                break;

            case false:
                if (image)
                    tween = image.DOFade(data.endOpacity, data.duration).SetEase(data.ease);
                else if (canvasGroup)
                    tween = canvasGroup.DOFade(data.endOpacity, data.duration).SetEase(data.ease);
                break;
        }

        return tween;
    }

    public override void OnEditorGUI(ref Rect rect)
    {
#if UNITY_EDITOR
        rect.y += EditorGUIUtility.singleLineHeight + 10;

        EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Do Fade");
        rect.y += EditorGUIUtility.singleLineHeight;

        data.isUsingFrom = EditorGUI.Toggle(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Is Using From", data.isUsingFrom);
        rect.y += EditorGUIUtility.singleLineHeight;

        data.endOpacity = EditorGUI.FloatField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "End Value", data.endOpacity);
        rect.y += EditorGUIUtility.singleLineHeight;

        if (!data.isUsingFrom)
            return;

        data.fromOpacity = EditorGUI.FloatField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "From Value", data.fromOpacity);
#endif
    }

    public override void OnEditorGUILayout()
    {
#if UNITY_EDITOR
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Do Fade");
        data.endOpacity = EditorGUILayout.FloatField("End Value", data.endOpacity);
        data.fromOpacity = EditorGUILayout.FloatField("From Value", data.fromOpacity);
#endif
    }
}