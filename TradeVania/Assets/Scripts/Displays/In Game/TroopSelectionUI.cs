using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TroopSelectionUI : MonoBehaviour
{
    // List of clickable images
    [SerializeField] public TroopSlot[] troopSlots = new TroopSlot[3];

    private void Start()
    {
        SetLeftToSpawn(GameObject.FindObjectOfType<Stats>().ps);    
    }

    public void troopSelected(TroopSlot troopSlot)
    {
        // Reset the troop slots
        foreach(TroopSlot slot in troopSlots)
        {
            slot.GetComponent<Image>().color = Color.white;
            slot.isSelected = false;
        }

        // Set colour of selected troop item
        troopSlot.GetComponent<Image>().color = Color.green;
        
        // Set is selected of selected troop item
        troopSlot.isSelected = true;
    }

    public void SetLeftToSpawn(PlayerStats stats)
    {
        for(int x = 0; x < 3; x ++)
        {
            troopSlots[x].leftToSpawn = stats.numTroops[x];
            troopSlots[x].UpdateLTSText();
        }
    }
}
