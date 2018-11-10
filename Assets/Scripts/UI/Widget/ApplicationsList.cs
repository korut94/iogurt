using Iogurt.Applications;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Iogurt.UI
{
    public sealed class ApplicationsList : AbstractWidget, IUsesListOfApplications
    {
        [SerializeField]
        ApplicationLink ApplicationLinkPrefab;

        public IEnumerable<Type> applications { get; set; }

        void Start()
        {
            foreach (var application in applications)
            {
                var applicationInfo = application.GetCustomAttributes(typeof(ApplicationItem), false);
                if (applicationInfo.Length == 0)
                    Debug.LogWarning("The " + application.Name + " does not provide any relative informations about its application. Skipped.");
                else
                {
                    var item = applicationInfo.First() as ApplicationItem;
                    var go = Instantiate(ApplicationLinkPrefab, transform);
                    var link = go.GetComponent<ApplicationLink>();

                    link.title = item.title;
                }
            }
        }
    }
}

