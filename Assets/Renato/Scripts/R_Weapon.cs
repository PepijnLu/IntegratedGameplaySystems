using UnityEngine;

[CreateAssetMenu(fileName = "Weapon")]
public class R_Weapon : ScriptableObject
{
    public GameObject weapon;
    public string Name = "";
    public bool isActive = false;
    public bool isAttacking = false;
    public float attackRate = 2f;
    public float nextTimeToAttack = 0f;

    public void Spawn(Transform _position) 
    {    
        GameObject newWeapon = Instantiate(weapon, _position.position, weapon.transform.rotation);
        newWeapon.transform.SetParent(_position);
    }




    // Might use this method for other purposes
    // public bool InRange()
    // {
    //     // Get the collider from the weapon
    //     if (weapon.TryGetComponent<CircleCollider2D>(out var collider)) return false;

    //     // Do something
    //     Collider2D hitCollider = Physics2D.OverlapCircle(weapon.transform.position, collider.radius);
    //     if (hitCollider != null)
    //     {
    //         Debug.Log($"Made contact with: {hitCollider.name}");
    //         return true;
    //     }

    //     if (!hitCollider.CompareTag("Player"))
    //     {
    //         Transform player = hitCollider.transform;

    //         if (player != null)
    //         {
    //             Debug.Log($"Made contact with the {player.gameObject.name}");
    //             return true;
    //         }

    //         return false;
    //     }

    //     return false;
    // }
}
