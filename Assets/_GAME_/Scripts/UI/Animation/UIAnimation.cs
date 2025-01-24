using DG.Tweening;
using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class UIAnimation : MonoBehaviour
{
    [Serializable]
    public class UIAnimationSettings
    {
        public float duration;

        [Header("Looping")]
        public bool useLoops = false;

        [ShowIf("useLoops")]
        public LoopType loopType = LoopType.Restart;

        [ShowIf("useLoops")]
        public int loopsCount;

        [ShowIf("useLoops")]
        public float delayBetweenLoop;

    }
    public abstract void PlayAnimation(UnityAction onFinishedAction = null);

    public abstract void PlayAnimationReversed(UnityAction onFinishedAction = null);

    public abstract void ResetAnimation();

    public abstract void InterruptAnimation();
}