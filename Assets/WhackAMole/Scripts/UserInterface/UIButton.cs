using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WhackAMole
{
    public class UIButton : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            AudioController.Instance.PlaySFX(SFXData.ID.Button);
        }
    }
}
