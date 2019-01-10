using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Iogurt.UI
{
    public class NumericField : Selectable, ISubmitHandler, IUsesTooltip
    {
        [SerializeField]
        RadialField RadialFieldPrefab;
        [SerializeField]
        TextMeshProUGUI ValueText;

        RadialField m_radialFieldInstance;

        public void OnSubmit(BaseEventData eventData)
        {
            var go = this.ShowTooltip(RadialFieldPrefab.gameObject);
            var radialField = go.GetComponent<RadialField>();

            radialField.OnValueChanged.AddListener(() =>
            {
                ValueText.text = radialField.value.ToString();
            });

            radialField.OnConfirm.AddListener(() =>
            {
                ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.selectHandler);
            });

            radialField.Select();
        }
    }
}

