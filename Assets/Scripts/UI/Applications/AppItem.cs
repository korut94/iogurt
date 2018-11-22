using System;

namespace Iogurt.Applications
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AppItem : Attribute
    {
        string m_description;
        string m_title;

        public string description { get { return m_description; } }
        public string title { get { return m_title; } }

        public AppItem(string title, string description = null)
        {
            m_title = title;
            m_description = description;
        }
    }
}

