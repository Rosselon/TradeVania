using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RangeDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Set the gameobject of the parent of the range indicator sprite
    [SerializeField] private GameObject attackRangeIndicator = null;

    // Occurs when cursor hovers over this script bearing object
    public void OnPointerEnter(PointerEventData eventData)
    {
        attackRangeIndicator.SetActive(true);
    }

    // Occurs when cursor moves off this script bearing object
    public void OnPointerExit(PointerEventData eventData)
    {
        attackRangeIndicator.SetActive(false);
    }
}
