using Iogurt.Applications;
using Iogurt.Modules.Injection;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Iogurt.Modules
{
    public sealed class ApplicationDataModule : INested, IInterfaceProvider
    {
        struct ApplicationData
        {
            public Sprite icon;
        }

        readonly Dictionary<Type, ApplicationData> m_applicationsData = new Dictionary<Type, ApplicationData>();

        public ApplicationDataModule()
        {
            IUsesApplicationIconMethods.getApplicationIcon = GetIcon;
        }

        public void ConnectInterface(object target, object userData = null)
        {
            var tool = target as ITool;
            if (tool != null && tool.HasRightSignature())
            {
                if (!m_applicationsData.ContainsKey(tool.GetType()))
                {
                    m_applicationsData[tool.GetType()] = new ApplicationData()
                    {
                        icon = (tool is IApplicationIcon) ? (tool as IApplicationIcon).icon : null
                    };
                }
            }
        }

        public void DisconnectInterface(object target, object userData = null)
        {
            // throw new System.NotImplementedException();
        }

        Sprite GetIcon(Type type)
        {
            ApplicationData data;

            if (m_applicationsData.TryGetValue(type, out data))
            {
                return data.icon;
            }

            return null;
        }
    }
}