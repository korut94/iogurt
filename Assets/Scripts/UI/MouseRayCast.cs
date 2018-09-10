using UnityEngine;
using UnityEngine.EventSystems;

namespace Iogurt.UI
{
    public class MouseRayCast : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        Vector3[]               m_corners = new Vector3[4];
        Vector2                 m_lastPosition;
        ITouchHandler           m_touch;
        ITouchPressHandler      m_touchPress;
        ITouchReleaseHandler    m_touchRelease;
        ITouchScrollHandler     m_touchScroll;

        public void OnDrag(PointerEventData eventData)
        {
            PointerMoved(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            PointerExpired();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (m_touchPress != null)
                m_touchPress.OnTouchPress();

            PointerMoved(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            PointerExpired();
        }

        void Awake()
        {
            m_touch = GetComponent<ITouchHandler>();
            m_touchPress = GetComponent<ITouchPressHandler>();
            m_touchRelease = GetComponent<ITouchReleaseHandler>();
            m_touchScroll = GetComponent<ITouchScrollHandler>();

            (transform as RectTransform).GetLocalCorners(m_corners);
        }

        void PointerExpired()
        {
            if (m_touchRelease != null)
                m_touchRelease.OnTouchRelease();
        }

        void PointerMoved(PointerEventData eventData)
        {
            Vector2 localPosition;
            Vector2 currentPosition;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform as RectTransform, eventData.position, eventData.pressEventCamera, out localPosition
            );

            currentPosition.x = Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(m_corners[0].x, m_corners[2].x, localPosition.x));
            currentPosition.y = Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(m_corners[0].y, m_corners[2].y, localPosition.y));

            if (m_touch != null)
                m_touch.OnTouch(currentPosition);

            // Possible starting scroll moment
            if (m_lastPosition == Vector2.zero)
                m_lastPosition = currentPosition;

            var delta = m_lastPosition - currentPosition;
            var speed = delta.magnitude / Time.deltaTime;

            if (speed > 5f)
            {
                var direction = (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) ? Vector2.right * Mathf.Sign(delta.x) : Vector2.up * Mathf.Sign(delta.y);

                if (m_touchScroll != null)
                    m_touchScroll.OnTouchScroll(speed, direction);
            }

            m_lastPosition = currentPosition;
        }
    }
}
