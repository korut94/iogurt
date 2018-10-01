using Iogurt.Animation;
using RSG;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Iogurt.UI.Animation
{
    public sealed class HorizontalSlideMenuAnimation : MenuNavigator, IProceduralAnimation
    {
        [SerializeField]
        ScrollRect TargetRect;

        [SerializeField]
        float TimeToSwitch = 0.0f;

        Coroutine   m_coroutine;
        Promise     m_currentSlideAnimation;
        float       m_step = 0.0f; 

        public override IPromise Next()
        {
            m_step = 1.0f / (TargetRect.content.childCount - 1);
            return Start();
        }

        public override IPromise Previous()
        {
            m_step = -1.0f / (TargetRect.content.childCount - 1);
            return Start();
        }

        public IPromise Start()
        {
            var promise = new Promise();
            m_coroutine = StartCoroutine(SlideMenuHorizontally(m_step, promise));

            m_currentSlideAnimation = promise;

            return promise;
        }

        public void Stop()
        {
            StopCoroutine(m_coroutine);
            m_currentSlideAnimation.Reject(new AnimationAbortException());
        }

        IEnumerator SlideMenuHorizontally(float step, Promise handler)
        {
            var startPosition = TargetRect.horizontalNormalizedPosition;
            var endPosition = startPosition + step;

            yield return this.ExecAnimation(elapsedTime =>
            {
                var progress = elapsedTime / TimeToSwitch;

                TargetRect.horizontalNormalizedPosition = Mathf.Lerp(startPosition, endPosition, progress);
                handler.ReportProgress(progress);
            }, TimeToSwitch);

            TargetRect.horizontalNormalizedPosition = endPosition;
            handler.Resolve();
        }
    }
}
