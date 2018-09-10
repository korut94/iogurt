using Iogurt.UI;
using UnityEngine;

namespace Iogurt.Menus
{
    public class VRTouchMenu : MonoBehaviour, ITouchHandler, ITouchPressHandler, ITouchReleaseHandler, ITouchScrollHandler
    {
        [SerializeField]
        GameObject PointerPrefab;

        Vector3[]   m_corners = new Vector3[4];
        GameObject  m_pointer;
        
        public void OnTouchPress()
        {
            m_pointer.SetActive(true);
        }

        public void OnTouchRelease()
        {
            m_pointer.SetActive(false);
        }

        public void OnTouchScroll(float speed, Vector2 direction)
        {
            Debug.Log(speed + " " + direction);
        }

        public void OnTouch(Vector2 relativePosition)
        {
            RectTransform pointer = m_pointer.transform as RectTransform;
            pointer.localPosition = MapToRect(relativePosition);
        }

        void Awake()
        {
            m_pointer = Instantiate(PointerPrefab, transform);
            // Don't show the cursor until the touch event is occured
            m_pointer.SetActive(false);

            (transform as RectTransform).GetLocalCorners(m_corners);
        }

        Vector3 MapToRect(Vector3 relativePosition)
        {
            var x = Mathf.Lerp(m_corners[0].x, m_corners[2].x, Mathf.InverseLerp(-1f, 1f, relativePosition.x));
            var y = Mathf.Lerp(m_corners[0].y, m_corners[2].y, Mathf.InverseLerp(-1f, 1f, relativePosition.y));
            return new Vector3(x, y);
        }
    }
}

