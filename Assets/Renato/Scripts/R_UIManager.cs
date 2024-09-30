using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class R_UIManager
{
    [SerializeField] private GameObject UI_Inventory;
    [SerializeField] List<GameObject> UI_Slots = new();

    public void InstantiateUISlot(Image _image) 
    {
        // Image image = Instantiate(_image);
    }
}
