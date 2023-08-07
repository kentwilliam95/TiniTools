using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public enum DotweenActionID
{
    DoMove,
    DoLocalMove,
    DoRotate,
    DoLocalRotate,
    DoJump,
    DoAnchorPos,
    DoScale,
    DoFade,
}

public class DotweenSequenceTools : MonoBehaviour
{
    [field: SerializeField] public Component Component { get; set; }

    [field: SerializeField] public DotweenData Data { get; private set; }

    public DotweenHeaderData headerData;
    private Sequence sequence;

    void OnDestroy()
    {
        sequence?.Kill();
        sequence = null;
    }

    void Start()
    {
        if (headerData.isGenerateAutomatically)
            Initialize();
    }

    public void Initialize()
    {
        sequence = DotweenAnimationFactory.GenerateSequence(headerData);
        if (headerData.isPaused)
            sequence.Pause();
        else
            Play();
    }

    public void Play()
    {
        sequence?.Restart();
    }

    public void Pause()
    {        
        sequence?.Pause();
    }
}

[System.Serializable]
public class DotweenHeaderData
{
    public float animationDuration;
    public bool isAutoKill = true;
    public bool isGenerateAutomatically;
    public bool isPaused = false;

    public UnityEvent onStart;
    public UnityEvent onComplete;
    public bool isRepeat;
    public int animationCount;
    public DotweenData[] animationData;
}

[System.Serializable]
public class DotweenData
{
    public Component component;
    public DotweenActionID selectedDotweenActionID;

    public Vector3 endValue;

    public bool isUsingFrom;
    public Vector3 fromValue;

    public float duration;

    public float startAt;

    public Ease ease;

    public RotateMode rotateMode;

    public float endOpacity;
    public float fromOpacity;
}
