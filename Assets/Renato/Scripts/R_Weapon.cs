using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]
public class R_Weapon : ScriptableObject, IIdentifiable, IAttackable, IAttackableAbility
{
    public enum WeaponType { SWORD, SPEAR, STAFF }
    public WeaponType weaponType;
    public GameObject weaponPrefab;
    public GameObject normalAbilityPrefab;
    public GameObject NormalAbilityPrefab 
    {
        get => normalAbilityPrefab;
        private set => normalAbilityPrefab = value;
    }

    public GameObject newWeapon;
    public GameObject Obj 
    {
        get => newWeapon;
        private set => newWeapon = value;
    }

    public Sprite icon;
    public Sprite Icon 
    {
        get => icon;
        private set => icon = value;
    }
    
    public new string name = "";
    public string Name 
    {
        get => name;
        private set => name = value;
    }
    
    [Header("Booleans")]
    public bool isAdded = false;
    public bool IsAdded 
    {
        get => isAdded;
        set => isAdded = value;
    }

    public bool isGrabable = true;
    public bool IsGrabable 
    {
        get => isGrabable;
        set => isGrabable = value;
    }

    public bool isActive = false;
    public bool isSelectedInInventory = false;
    public bool isSelectedInUsable = false;
    public bool isAttacking = false;

    [Header("Weapon Parameters")]
    public Vector2 combatStartPosition = new();
    public Vector2 CombatStartPosition 
    {
        get => combatStartPosition;
        private set => combatStartPosition = value;
    }

    public Vector2 combatEndPosition = new();
    public Vector2 CombatEndPosition 
    {
        get => combatEndPosition;
        private set => combatEndPosition = value;
    }

    public Vector3 combatStartEulerRotation = new();
    public Vector3 CombatStartRotation 
    {
        get => combatStartEulerRotation;
        private set => combatStartEulerRotation = value;
    }

    public Vector3 combatEndEulerRotation = new();
    public Vector3 CombatEndRotation 
    {
        get => combatEndEulerRotation;
        private set => combatEndEulerRotation = value;
    }

    public float attackRate = 2f;
    public float AttackRate 
    {
        get => attackRate;
        private set => attackRate = value;
    }

    public float abilityFireSpeed = 4f;
    public float AbilityFireSpeed 
    {
        get => abilityFireSpeed;
        private set => abilityFireSpeed = value;
    }

    public float abilityLifeTime = 2f;
    public float AbilityLifeTime 
    {
        get => abilityLifeTime;
        private set => abilityLifeTime = value;
    }

    [HideInInspector]
    public Quaternion combatStartRotation;
    [HideInInspector]
    public Quaternion combatEndRotation;


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

        combatStartRotation = Quaternion.Euler(combatStartEulerRotation);
        combatEndRotation = Quaternion.Euler(combatEndEulerRotation);
    }
}


public interface IAttackable
{
    Vector2 CombatStartPosition { get; }
    Vector2 CombatEndPosition { get; }
    Vector3 CombatStartRotation { get; }
    Vector3 CombatEndRotation { get; }
    float AttackRate { get; }
}

public interface IAttackableAbility 
{
    GameObject NormalAbilityPrefab { get; }
    float AbilityFireSpeed { get; }
    float AbilityLifeTime { get; } 
}
