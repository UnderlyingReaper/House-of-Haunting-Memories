using System.Collections.Generic;
using UnityEngine;

public class InitialItems : MonoBehaviour
{
    [SerializeField] private List<InventoryItem> invItemsList;


    private void Start()
    {
        if(invItemsList == null || invItemsList.Count == 0) return;
        
        foreach(InventoryItem item in invItemsList)
        {
            InventoryManager.Instance.AddItem(item);
        }
    }
}
