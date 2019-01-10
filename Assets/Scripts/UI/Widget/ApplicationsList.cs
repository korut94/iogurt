﻿using Iogurt.Applications;
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

        Selectable m_firstToolLink;

        public IEnumerable<Type> applications { get; set; }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
        }

        protected override void Start()
        {
            base.Start();

            Selectable prevSelectable = null;

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
                    link.onClick.AddListener(() => { this.LoadTool(tool); });

                    var nav = link.navigation;
                    nav.mode = Navigation.Mode.Explicit;

                    if (prevSelectable == null)
                        m_firstToolLink = link;
                    else
                    {
                        var preNav = prevSelectable.navigation;

                        nav.selectOnUp = prevSelectable;
                        preNav.selectOnDown = link;

                        prevSelectable.navigation = preNav;
                    }

                    link.navigation = nav;
                    prevSelectable = link;
                }
            }
        }
    }
}

