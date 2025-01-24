using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DraftFever.UI
{
    public class UIAnimationCollection : UIAnimation
    {
        [SerializeField]
        protected UIAnimation[] animations;

        [SerializeField]
        protected float _delayBetweenComponents;

        public override void InterruptAnimation()
        {
            if (animations != null)
            {
                foreach (UIAnimation animation in animations)
                {
                    animation.InterruptAnimation();
                }
            }
        }

        public override void PlayAnimation(UnityAction onFinishedAction = null)
        {
            if (gameObject.activeInHierarchy)
                StartCoroutine(ExecuteForwards(onFinishedAction));
        }

        public override void PlayAnimationReversed(UnityAction onFinishedAction = null)
        {
            if (gameObject.activeInHierarchy)
                StartCoroutine(ExecuteBackwards(onFinishedAction));
        }

        public override void ResetAnimation()
        {
            if (animations != null)
            {
                foreach (UIAnimation animation in animations)
                {
                    animation.ResetAnimation();
                }
            }
        }

        IEnumerator ExecuteForwards(UnityAction onFinishedAction)
        {
            if (animations != null)
            {
                int animationsFinished = 0;
                void AllAnimationsFinished()
                {
                    animationsFinished++;

                    if (animationsFinished == animations.Length)
                        onFinishedAction?.Invoke();
                }
                for (int i = 0; i < animations.Length; i++)
                {
                    UIAnimation animation = animations[i];

                    if (animation.gameObject.activeInHierarchy)
                        animation.PlayAnimation(AllAnimationsFinished);

                    if (_delayBetweenComponents > 0)
                        yield return new WaitForSeconds(_delayBetweenComponents);
                }
            }
            yield return null;
        }

        IEnumerator ExecuteBackwards(UnityAction onFinishedAction)
        {
            if (animations != null)
            {
                int animationsFinished = 0;
                void AllAnimationsFinished()
                {
                    animationsFinished++;

                    if (animationsFinished == animations.Length)
                        onFinishedAction?.Invoke();
                }

                for (int i = animations.Length - 1; i >= 0; i--)
                {
                    if (animations[i].gameObject.activeInHierarchy)
                        animations[i].PlayAnimationReversed(AllAnimationsFinished);

                    if (_delayBetweenComponents > 0)
                        yield return new WaitForSeconds(_delayBetweenComponents);
                }
            }
            yield return null;
        }
    }
}