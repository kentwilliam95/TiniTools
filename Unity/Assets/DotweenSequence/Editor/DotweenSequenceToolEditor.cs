using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.DOTweenEditor;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;
using UnityEditorInternal;

[CustomEditor(typeof(DotweenSequenceTools))]
public class DotweenSequenceToolEditor : Editor
{
    private Dictionary<System.Type, DotweenActionID[]> _mapComponentAndActions = new Dictionary<System.Type, DotweenActionID[]>()
    {
        { typeof(Transform) , new DotweenActionID[]{
            DotweenActionID.DoMove, DotweenActionID.DoLocalMove, DotweenActionID.DoRotate, DotweenActionID.DoLocalRotate,
            DotweenActionID.DoJump, DotweenActionID.DoScale
        } },

        { typeof(RectTransform), new DotweenActionID[]{
            DotweenActionID.DoMove, DotweenActionID.DoLocalMove, DotweenActionID.DoRotate, DotweenActionID.DoLocalRotate,
            DotweenActionID.DoJump, DotweenActionID.DoScale
        } },

        { typeof(UnityEngine.UI.Image) , new DotweenActionID[]{
            DotweenActionID.DoMove, DotweenActionID.DoLocalMove, DotweenActionID.DoRotate, DotweenActionID.DoLocalRotate,
            DotweenActionID.DoScale, DotweenActionID.DoFade
        } },

        { typeof(CanvasGroup) , new DotweenActionID[]{
            DotweenActionID.DoMove, DotweenActionID.DoLocalMove, DotweenActionID.DoRotate, DotweenActionID.DoLocalRotate,
            DotweenActionID.DoJump, DotweenActionID.DoScale, DotweenActionID.DoFade
        } },
    };

    private Sequence sequence;
    private ReorderableList reorderableList;
    private Vector3 initialPosition;
    private Vector3 initialRotation;
    private Vector3 initialScale;

    void OnEnable()
    {
        var tools = target as DotweenSequenceTools;
        initialPosition = tools.transform.localPosition;
        initialRotation = tools.transform.localRotation.eulerAngles;
        initialScale = tools.transform.localScale;

        var headerData = serializedObject.FindProperty("headerData");
        var animationData = headerData.FindPropertyRelative("animationData");
        reorderableList = new ReorderableList(serializedObject, animationData, true, true, true, true);
        reorderableList.elementHeight = 230;
        reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            rect.height = 0f;
            SubEditorGUI(tools.headerData.animationData[index], ref rect);
        };
    }

    private void OnDisable()
    {
        ResetTransform();
    }

    private void ResetTransform()
    {
        var tools = target as DotweenSequenceTools;
        tools.transform.localPosition = initialPosition;
        tools.transform.localRotation = Quaternion.Euler(initialRotation);
        tools.transform.localScale = initialScale;
    }

    public override void OnInspectorGUI()
    {
        var tools = target as DotweenSequenceTools;
        Debug.Log(GUI.enabled);
        HeadEditorGUI(tools);

        serializedObject.ApplyModifiedProperties();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Play Animation"))
        {
            ResetTransform();

            DOTweenEditorPreview.Stop();
            sequence.Complete();
            sequence = DotweenAnimationFactory.GenerateSequence(tools.headerData);
            
            DOTweenEditorPreview.PrepareTweenForPreview(sequence);
            DOTweenEditorPreview.Start();
        }

        if (GUILayout.Button("Stop"))
        {
            sequence.Complete();
            DOTweenEditorPreview.Stop();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(25);
        base.OnInspectorGUI();
    }

    public void HeadEditorGUI(DotweenSequenceTools instance)
    {
        serializedObject.Update();

        instance.headerData.isRepeat = EditorGUILayout.Toggle("Is Animation repeat", instance.headerData.isRepeat);
        instance.headerData.isGenerateAutomatically = EditorGUILayout.Toggle("Is GenerateAutomatically ", instance.headerData.isGenerateAutomatically);
        instance.headerData.isPaused = EditorGUILayout.Toggle("Is Pause ", instance.headerData.isPaused);

        var headerData = serializedObject.FindProperty("headerData");

        var serializedObjectOnStart = headerData.FindPropertyRelative("onStart");
        EditorGUILayout.PropertyField(serializedObjectOnStart);

        var serializedObjectOnEnd = headerData.FindPropertyRelative("onComplete");
        EditorGUILayout.PropertyField(serializedObjectOnEnd);
        
        if (GUILayout.Button("Fill out timing"))
        {
            var totalAnimationDuration = 0f;
            for (int i = 0; i < instance.headerData.animationData.Length; i++)
            {
                var data = instance.headerData.animationData[i];
                data.startAt = totalAnimationDuration;
                totalAnimationDuration += data.duration;
            }
        }
        reorderableList.DoLayoutList();
    }

    public void SubEditorGUI(DotweenData data, ref Rect rect)
    {
        data.component = EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Target", data.component, typeof(Component)) as Component;
        System.Type componentType = data.component.GetType();
        rect.height = 0;

        if (componentType == typeof(Component))
            return;

        var actions = _mapComponentAndActions[componentType];
        var selectedActionID = data.selectedDotweenActionID;
        var stringActions = actions.Select(x => x.ToString()).ToArray();

        var stringKey = selectedActionID.ToString();
        int selectedPopup = 0;
        for (int i = 0; i < stringActions.Length; i++)
        {
            if (stringActions[i] == stringKey)
            {
                selectedPopup = i;
            }
        }

        rect.y += EditorGUIUtility.singleLineHeight + 10;
        var popupValue = EditorGUI.Popup(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Actions", selectedPopup, stringActions);
        var convertBackToEnum = (DotweenActionID)(int)actions[popupValue];
        data.selectedDotweenActionID = convertBackToEnum;

        rect.y += EditorGUIUtility.singleLineHeight + 10;
        data.ease = (Ease)EditorGUI.EnumPopup(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Ease", data.ease);

        rect.y += EditorGUIUtility.singleLineHeight + 10;
        data.duration = EditorGUI.FloatField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Duration", data.duration);

        rect.y += EditorGUIUtility.singleLineHeight + 10;
        data.startAt = EditorGUI.FloatField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Start at", data.startAt);

        var action = DotweenAnimationFactory.GetDotweenAction(data.selectedDotweenActionID);
        action.SetData(data);
        action.OnEditorGUI(ref rect);
    }

    public void SubEditorGUILayout(DotweenData data)
    {
        data.component = EditorGUILayout.ObjectField("Target", data.component, typeof(Component)) as Component;
        System.Type componentType = data.component.GetType();

        var actions = _mapComponentAndActions[componentType];
        var selectedActionID = data.selectedDotweenActionID;
        var stringActions = actions.Select(x => x.ToString()).ToArray();

        var stringKey = selectedActionID.ToString();
        int selectedPopup = 0;
        for (int i = 0; i < stringActions.Length; i++)
        {
            if (stringActions[i] == stringKey)
            {
                selectedPopup = i;
            }
        }

        var popupValue = EditorGUILayout.Popup("Actions", selectedPopup, stringActions);
        var convertBackToEnum = (DotweenActionID)(int)actions[popupValue];
        data.selectedDotweenActionID = convertBackToEnum;

        data.ease = (Ease)EditorGUILayout.EnumPopup("Ease", data.ease);
        data.duration = EditorGUILayout.FloatField("Duration", data.duration);
        var action = DotweenAnimationFactory.GetDotweenAction(data.selectedDotweenActionID);
        action.SetData(data);
        action.OnEditorGUILayout();
    }
}