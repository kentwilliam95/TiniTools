using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using DG.Tweening;

public class DoRotationAction : DoTweenAction
{
    public override Tween GenerateDotween(Component component)
    {
        Tween tween = null;
        RectTransform rt = component.GetComponent<RectTransform>();
        Transform transform = component.GetComponent<Transform>();

        if (rt == null && transform == null)
            return tween;

        switch (data.isUsingFrom)
        {
            case true:
                if (rt != null)
                    tween = rt.DORotate(data.endValue, data.duration).From(data.fromValue).SetEase(data.ease);
                else if (transform != null)
                    tween = transform.DORotate(data.endValue, data.duration).From(data.fromValue).SetEase(data.ease);
                break;

            case false:
                if (rt != null)
                    tween = rt.DORotate(data.endValue, data.duration).SetEase(data.ease);
                else if (transform != null)
                    tween = transform.DORotate(data.endValue, data.duration).SetEase(data.ease);
                break;
        }


        return tween;
    }

    public override void OnEditorGUI(ref Rect rect)
    {
#if UNITY_EDITOR
        rect.y += EditorGUIUtility.singleLineHeight + 10;

        EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "World Rotation");
        rect.y += EditorGUIUtility.singleLineHeight;

        data.isUsingFrom = EditorGUI.Toggle(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Is Using From", data.isUsingFrom);
        rect.y += EditorGUIUtility.singleLineHeight;

        data.endValue = EditorGUI.Vector3Field(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "End Value", data.endValue);
        rect.y += EditorGUIUtility.singleLineHeight;

        if (!data.isUsingFrom)
            return;
        data.fromValue = EditorGUI.Vector3Field(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "From Value", data.fromValue);
#endif
    }

    public override void OnEditorGUILayout()
    {
#if UNITY_EDITOR
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("World Rotation");
        data.endValue = EditorGUILayout.Vector3Field("End Value", data.endValue);
        data.fromValue = EditorGUILayout.Vector3Field("From Value", data.fromValue);
#endif
    }
}