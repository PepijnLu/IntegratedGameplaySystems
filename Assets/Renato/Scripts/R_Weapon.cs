using UnityEngine;

[CreateAssetMenu(fileName = "Weapon")]
public class R_Weapon : ScriptableObject, IIdentifiable
{
    public GameObject weapon;
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

    public bool isActive = false;
    public bool isAttacking = false;
    public float attackRate = 2f;
    public float nextTimeToAttack = 0f;

    public bool IsAdded 
    {
        get => isAdded;
        set => isAdded = value;
    }
    
    public bool isAdded = false;
    public bool selected = false;

    public void Instantiate(Transform _position) 
    {    
        GameObject newWeapon = Instantiate(weapon, _position.position, weapon.transform.rotation);
        newWeapon.name = Name;
        newWeapon.transform.SetParent(_position);
    }
}