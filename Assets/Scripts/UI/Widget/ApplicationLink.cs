using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Iogurt.UI
{
    public class ApplicationLink : MonoBehaviour
    {
        public Sprite icon
        {
            set
            {
                var iconTransform = transform.Find("Icon");

                if (iconTransform == null)
                    Debug.LogError("No Icon element was found in the current object");
                else
                    iconTransform.GetComponent<Image>().sprite = value;
            }
        }

        public string title
        {
            set
            {
                var textTransform = transform.Find("Title");

                if (textTransform == null)
                    Debug.LogError("No Text element was found in the current object");
                else
                    textTransform.gameObject.GetComponent<TextMeshProUGUI>().SetText(value);
            }
        }
    }
}
