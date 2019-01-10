using DigitalRubyShared;
using Iogurt.Input.Touch;
using Iogurt.UI;
using Iogurt.UI.Applications;
using RSG;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor.Experimental.EditorVR;
using UnityEditor.Experimental.EditorVR.Menus;

namespace Iogurt
{
    public sealed class IogurtMainMenu : MonoBehaviour,
        IMenu, INavigationSystem, ITooltipHandler, IUsesCurrentApplication, IPanGesture, ITapGesture
    {
        [SerializeField]
        Transform ApplicationRect;
        [SerializeField]
        Transform TooltipRect;
        [SerializeField]
        float VerticalSensibility;

        int m_currVStep = 0;

        public Bounds localBounds { get; private set; }

        public MenuHideFlags menuHideFlags
        {
            get { return gameObject.activeSelf ? 0 : MenuHideFlags.Hidden; }
            set { gameObject.SetActive(value == 0); }
        }

        public GameObject menuContent { get { return gameObject; } }
        public int priority { get { return 1; } }

        public void OnPanGesture(PanGestureRecognizer gesture)
        {
            var g = gesture as PanGestureRecognizerAdapter;
            var currentGO = EventSystem.current.currentSelectedGameObject;

            if (g.State == GestureRecognizerState.Ended)
            {
                m_currVStep = 0;
                ExecuteEvents.Execute(currentGO, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
            }
            else
            {
                var pointerEventData = new PointerEventData(EventSystem.current)
                {
                    position = new Vector2(g.FocusX, g.FocusY)
                };

                ExecuteEvents.Execute(currentGO, pointerEventData, ExecuteEvents.pointerDownHandler);

                var vStep = Mathf.FloorToInt(g.DeltaY / VerticalSensibility);

                if (vStep > m_currVStep)
                {
                    var axisEventData = new AxisEventData(EventSystem.current)
                    {
                        moveDir = MoveDirection.Up
                    };
                    ExecuteEvents.Execute(currentGO, axisEventData, ExecuteEvents.moveHandler);
                }
                else if (vStep < m_currVStep)
                {
                    var axisEventData = new AxisEventData(EventSystem.current)
                    {
                        moveDir = MoveDirection.Down
                    };
                    ExecuteEvents.Execute(currentGO, axisEventData, ExecuteEvents.moveHandler);
                }

                m_currVStep = vStep;
            }
        }

        public void OnTapGesture(TapGestureRecognizer gesture)
        {
            ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }

        public IApplication LoadApplication(IApplication prefab)
        {
            var go = Instantiate(prefab.gameObject, ApplicationRect);
            go.transform.SetAsFirstSibling();
            go.SetActive(false);

            return go.GetComponent<IApplication>();
        }

        public IPromise ShowApplication(IApplication application)
        {
            var currentApp = this.CurrentApplication();

            if (currentApp != null)
                currentApp.gameObject.SetActive(false);

            application.gameObject.transform.SetAsLastSibling();
            application.gameObject.SetActive(true);

            if (application.root != null)
                application.root.Select();
            else
                Debug.LogWarning("No root widget selected on the application " + application.gameObject.name);
            

            return Promise.Resolved();
        }

        public void CloseTooltip(GameObject tooltip)
        {
            tooltip.SetActive(false);
            Destroy(tooltip);
        }

        public GameObject ShowTooltip(GameObject tooltip)
        {
            var go = Instantiate(tooltip, TooltipRect);
            var rect = go.transform as RectTransform;

            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 1f);

            return go;
        }
    }
}

