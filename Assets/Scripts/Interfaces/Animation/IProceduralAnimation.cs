using RSG;
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace Iogurt.Animation
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProceduralAnimation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IPromise    Start();
        /// <summary>
        /// 
        /// </summary>
        void        Stop();
    }

    public static class IProceduralAnimationMethods
    {
        public static IEnumerator ExecAnimation(this IProceduralAnimation instance, Action<float> generateFrame, float time)
        {
            var elapsedTime = 0.0f;

            while (elapsedTime < time)
            {
                generateFrame(elapsedTime);
                elapsedTime += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }
        }
    }
}

