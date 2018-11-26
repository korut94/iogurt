using Iogurt.Animation;
using Iogurt.UI.Applications;
using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Iogurt.UI
{
    public sealed class HorizontalAppsSlider : AbstractAppsNavigator, 
        IProceduralAnimation, IUsesLoadedApplications, IUsesCurrentApplication
    {
        [SerializeField]
        float TimeToSwitch = 0.0f;

        Coroutine   m_coroutine;
        Promise     m_currentSlideAnimation;
        float       m_from;
        float       m_to;

        public IList<IApplication> LoadedApplications { get; set; }

        public override IPromise Next()
        {
            return Start();
        }

        public override IPromise Previous()
        {
            return Start();
        }

        public override IPromise SpawnApplication(GameObject app)
        {
            m_from = scrollRect.horizontalNormalizedPosition;
            m_to = 1f;
            return Start();
        }

        public IPromise Start()
        {
            var promise = new Promise();
            m_coroutine = StartCoroutine(SlideMenuHorizontally(m_from, m_to, promise));

            m_currentSlideAnimation = promise;

            return promise;
        }

        public void Stop()
        {
            StopCoroutine(m_coroutine);
            m_currentSlideAnimation.Reject(new AnimationAbortException());
        }

        IEnumerator SlideMenuHorizontally(float from, float to, Promise handler)
        {
            scrollRect.horizontalNormalizedPosition = from;
            // Required to update the scrollview position
            yield return new WaitForEndOfFrame();

            yield return this.ExecAnimation(elapsedTime =>
            {
                var progress = elapsedTime / TimeToSwitch;

                scrollRect.horizontalNormalizedPosition = Mathf.Lerp(from, to, progress);
                handler.ReportProgress(progress);
            }, TimeToSwitch);

            scrollRect.horizontalNormalizedPosition = to;

            handler.Resolve();
        }
    }
}
