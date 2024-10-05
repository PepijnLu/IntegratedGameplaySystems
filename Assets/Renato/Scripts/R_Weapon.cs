using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]
public class R_Weapon : ScriptableObject, IIdentifiable
{
    public GameObject weaponPrefab;
    public GameObject newWeapon;

    public GameObject Obj 
    {
        get => newWeapon;
        private set => newWeapon = value;
    }

    public Sprite icon;
    public new string name = "";
    
    public Sprite Icon 
    {
        get => icon;
        private set => icon = value;
    }
    
    public string Name 
    {
        get => name;
        private set => name = value;
    }

    [Header("Combat")]
    public NormalAttack normalAttack;
    // public SuperAttack superAttack;
    public List<ScriptableObject> abilities;
    
    [Header("Boolean")]
    public bool isActive = false;
    public bool IsAdded 
    {
        get => isAdded;
        set => isAdded = value;
    }
    
    public bool isAdded = false;
    public bool isSelectedInInventory = false;
    public bool isSelectedInUsable = false;
    public bool isAttacking = false;

    public bool IsGrabable 
    {
        get => isGrabable;
        set => isGrabable = value;
    }

    public bool isGrabable = true;

    [Header("Int & Float")]
    public float attackRate = 2f;
    public float nextTimeToAttack = 0f;


    public void InstantiateAtStart(Transform _position) 
    {    
        newWeapon = Instantiate(weaponPrefab, _position.position, weaponPrefab.transform.rotation);
        newWeapon.name = Name;
        newWeapon.transform.SetParent(_position); 
    
        isSelectedInInventory = false;
        isSelectedInUsable = false;
        isAdded = false;
        isActive = false;
        isAttacking = false;
        isGrabable = true;
        nextTimeToAttack = 0f;
    }
}