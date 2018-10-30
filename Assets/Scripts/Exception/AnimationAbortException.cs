using System;

namespace Iogurt.Animation
{
    public class AnimationAbortException : Exception
    {
        public AnimationAbortException() {}
        public AnimationAbortException(string message) : base(message) {}
    }
}

