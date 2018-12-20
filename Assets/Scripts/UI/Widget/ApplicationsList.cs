using Iogurt.Applications;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Iogurt.UI
{
    public sealed class ApplicationsList : Selectable, IUsesListOfApplications, ILoadTool, IUsesApplicationIcon
    {
        [SerializeField]
        GameObject ApplicationLinkPrefab;

        public IEnumerable<Type> applications { get; set; }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
        }

        protected override void Start()
        {
            base.Start();

            foreach (var tool in applications)
            {
                var applicationInfo = tool.GetCustomAttributes(typeof(AppItem), false);
                if (applicationInfo.Length == 0)
                {
                    Debug.LogWarning("The " + tool.Name + " does not provide any relative informations about its application. Skipped.");
                } 
                else 
                {
                    var item = applicationInfo.First() as AppItem;
                    var go = Instantiate(ApplicationLinkPrefab, transform);
                    var link = go.GetComponent<ApplicationLink>();

                    link.title = item.title;
                    link.icon = this.GetApplicationIcon(tool);
                    // link.onClick.AddListener(() => { this.LoadTool(tool); });

                    var nav = link.navigation;
                    nav.mode = Navigation.Mode.Vertical;
                    link.navigation = nav;
                }
            }
        }
    }
}

