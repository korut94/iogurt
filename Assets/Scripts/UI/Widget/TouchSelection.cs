﻿using DigitalRubyShared;
using Iogurt.Input.Touch;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Iogurt.UI
{
    public sealed class TouchSelection : MonoBehaviour, IUsesTapGesture, IUsesLongPressGesture
    {
        [SerializeField]
        GameObject CursorPrefab;
        [SerializeField]
        float VisibleTime = 0.1f;

        GameObject              m_cursor;
        List<RaycastResult>     m_prevHits = new List<RaycastResult>();
        GraphicRaycaster        m_raycaster;
        
        public void LongPressGesture(LongPressGestureRecognizer gesture)
        {
            var pointerEventData = new PointerEventData(EventSystem.current) {
                position = Camera.main.WorldToScreenPoint((m_cursor.transform as RectTransform).localPosition)
            };

            if (gesture.State == GestureRecognizerState.Began)
            {
                m_cursor.SetActive(true);
                (m_cursor.transform as RectTransform).localPosition = BoundScreenRectToLocalRect(new Vector2(gesture.FocusX, gesture.FocusY));
            }
            else if (gesture.State == GestureRecognizerState.Executing)
            {
                (m_cursor.transform as RectTransform).localPosition = BoundScreenRectToLocalRect(new Vector2(gesture.FocusX, gesture.FocusY));

                var results = new List<RaycastResult>();
                m_raycaster.Raycast(pointerEventData, results);

                ExecutePointerEnter(results.Where(hit => m_prevHits.FindIndex(other => hit.gameObject == other.gameObject) == -1), pointerEventData);
                ExecutePointerExit(m_prevHits.Where(hit => results.FindIndex(other => hit.gameObject == other.gameObject) == -1), pointerEventData);

                m_prevHits = results;
            }
            else if (gesture.State == GestureRecognizerState.Ended)
            {
                ExecutePointerExit(m_prevHits, pointerEventData);

                m_cursor.SetActive(false);
                m_prevHits.Clear();
            }
        }

        public void TapGesture(TapGestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Ended)
            {
                (m_cursor.transform as RectTransform).localPosition = BoundScreenRectToLocalRect(new Vector2(gesture.FocusX, gesture.FocusY));
                StartCoroutine(FlashCursor());

                var results = new List<RaycastResult>();
                var pointerEventData = new PointerEventData(EventSystem.current)
                {
                    position = Camera.main.WorldToScreenPoint((m_cursor.transform as RectTransform).localPosition)
                };

                m_raycaster.Raycast(pointerEventData, results);
                ExecutePointerSubmit(results, pointerEventData);
            }
        }

        Vector2 BoundScreenRectToLocalRect(Vector2 position)
        {
            var area = transform as RectTransform;
            var halfRectWidth = area.rect.width / 2f;
            var halfRectHeight = area.rect.height / 2f;

            var x = Mathf.Lerp(-halfRectWidth, halfRectWidth, Mathf.InverseLerp(0f, Screen.width, position.x));
            var y = Mathf.Lerp(-halfRectHeight, halfRectHeight, Mathf.InverseLerp(0f, Screen.height, position.y));

            return new Vector2(x, y);
        }

        void ExecuteEvent<T>(IEnumerable<RaycastResult> hits, BaseEventData eventData, ExecuteEvents.EventFunction<T> functor) where T : IEventSystemHandler
        {
            foreach (var hit in hits) {
                ExecuteEvents.Execute(hit.gameObject, eventData, functor);
            }
        }

        void ExecutePointerSubmit(IEnumerable<RaycastResult> hits, PointerEventData eventData)
        {
            ExecuteEvent(hits, eventData, ExecuteEvents.submitHandler);
        }

        void ExecutePointerEnter(IEnumerable<RaycastResult> hits, PointerEventData eventData)
        {
            ExecuteEvent(hits, eventData, ExecuteEvents.pointerEnterHandler);
        }

        void ExecutePointerExit(IEnumerable<RaycastResult> hits, PointerEventData eventData)
        {
            ExecuteEvent(hits, eventData, ExecuteEvents.pointerExitHandler);
        }

        IEnumerator FlashCursor()
        {
            m_cursor.SetActive(true);
            yield return new WaitForSeconds(VisibleTime);
            m_cursor.SetActive(false);
        }

        void Awake()
        {
            m_cursor = Instantiate(CursorPrefab, transform);
            m_cursor.SetActive(false);

            m_raycaster = GetComponentInParent<GraphicRaycaster>();
        }
    }
}