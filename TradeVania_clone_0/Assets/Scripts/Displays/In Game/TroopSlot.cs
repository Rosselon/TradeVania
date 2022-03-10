using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TroopSlot : MonoBehaviour, IPointerDownHandler 
{
    //Is this troop selected 
    public bool isSelected = false;

    [SerializeField] public int id;
    [SerializeField] public int leftToSpawn = 5;
    [SerializeField] private TMP_Text leftRToSpawnText;

    [SerializeField] private TroopSelectionUI tsui = null;

    private void Start()
    {
        UpdateLTSText();
    }

    public void UpdateLTSText()
    {
        leftRToSpawnText.text = leftToSpawn.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        tsui.troopSelected(this);
    }
}
