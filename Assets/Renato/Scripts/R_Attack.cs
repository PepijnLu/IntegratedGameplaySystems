using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "NormalAttack", menuName = "Combat/Attacks/NormalAttack")]
public class NormalAttack : ScriptableObject, ICombat
{ 
    private bool isCombatInProgress = false;
    public void Attack
    (
        IAttackable attackable, 
        IAttackableAbility attackableAbility, 
        R_WeaponsManager weaponManager, 
        R_GameManager gameManager
    ) 
    {

        isCombatInProgress = true; 

        // Execute combat
        var weapon = weaponManager.activeWeapon;
        switch (weapon.weaponType)
        {
            case R_Weapon.WeaponType.SWORD:
                gameManager.StartCoroutine(ExecuteCombat
                (
                    attackable,
                    weaponManager,
                    gameManager
                ));

                Debug.Log("Execute Sword Attack");
                
            break;

            case R_Weapon.WeaponType.SPEAR:
                gameManager.StartCoroutine(ExecuteCombat
                (
                    attackable,
                    weaponManager,
                    gameManager
                ));
                
                Debug.Log("Execute Spear Attack");

            break;

            case R_Weapon.WeaponType.STAFF:
                StaffAttack
                (
                    attackable,
                    attackableAbility,
                    weaponManager,
                    gameManager
                );
                
                Debug.Log("Execute Magical Staff Attack");

            break;
            
        }
    }
    
    private void StaffAttack
    (
        IAttackable attackable, 
        IAttackableAbility attackableAbility,
        R_WeaponsManager weaponManager, 
        R_GameManager gameManager
    ) 
    {
        gameManager.StartCoroutine(ExecuteCombat
        (
            attackable,
            weaponManager,
            gameManager
        ));

        // Start a coroutine that fire off a fireball or something
        gameManager.StartCoroutine(FireFireball(attackableAbility, weaponManager));
    }

    // Coroutine for firing a fireball or special effect
    private IEnumerator FireFireball(IAttackableAbility attackableAbility, R_WeaponsManager weaponManager)
    {
        // Instantiate the fireball prefab at the wand's position
        GameObject fireball = Instantiate(attackableAbility.NormalAbilityPrefab, weaponManager.activeWeaponGameObject.transform.position + new Vector3(0f, 1.5f, 0f), Quaternion.identity);

        // Set fireball's direction and velocity
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        rb.velocity = weaponManager.activeWeaponGameObject.transform.up * attackableAbility.AbilityFireSpeed; // Adjust fireballSpeed as necessary

        // Optionally add some delay before destroying the fireball after a certain duration
        yield return new WaitForSeconds(attackableAbility.AbilityLifeTime); // fireballLifetime is a float defining how long the fireball stays active
        Destroy(fireball);
    }

    public IEnumerator ExecuteCombat(IAttackable attackable, R_WeaponsManager weaponManager, R_GameManager gameManager)
    {
        GameObject weaponGameObject = weaponManager.activeWeaponGameObject;

        // Convert the Vector3 Euler angles to Quaternion
        Quaternion startRotation = Quaternion.Euler(attackable.CombatStartRotation);
        Quaternion endRotation = Quaternion.Euler(attackable.CombatEndRotation);

        // Start both the movement and rotation to the end position at the same time
        Coroutine moveToEndCoroutine = gameManager.StartCoroutine(TransformLerp
        (
            weaponGameObject, 
            attackable.CombatStartPosition, 
            attackable.CombatEndPosition, 
            attackable.AttackRate
        ));

        Coroutine rotateToEndCoroutine = gameManager.StartCoroutine(RotateLerp
        (
            weaponGameObject,
            startRotation, // Now using Quaternion
            endRotation,   // Now using Quaternion
            attackable.AttackRate 
        ));

        // Wait for both coroutines to finish moving and rotating to the end position
        yield return moveToEndCoroutine;
        yield return rotateToEndCoroutine;

        // Start both the movement and rotation back to the starting position at the same time
        moveToEndCoroutine = gameManager.StartCoroutine(TransformLerp
        (
            weaponGameObject,
            attackable.CombatEndPosition,
            attackable.CombatStartPosition,
            attackable.AttackRate 
        ));

        rotateToEndCoroutine = gameManager.StartCoroutine(RotateLerp
        (
            weaponGameObject, 
            endRotation,   // Now using Quaternion
            startRotation, // Now using Quaternion
            attackable.AttackRate
        ));

        // Wait for both coroutines to finish returning to the starting position
        yield return moveToEndCoroutine;
        yield return rotateToEndCoroutine;

        isCombatInProgress = false;
    }

    private IEnumerator TransformLerp(GameObject obj, Vector2 startPosition, Vector2 endPosition, float duration) 
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration) 
        {
            float t = elapsedTime / duration;
            obj.transform.localPosition = Vector2.Lerp(startPosition, endPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        obj.transform.localPosition = endPosition;
    }

    private IEnumerator RotateLerp(GameObject obj, Quaternion startRotation, Quaternion endRotation, float duration) 
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration) 
        {
            float t = elapsedTime / duration;
            obj.transform.localRotation = Quaternion.Lerp(startRotation, endRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        obj.transform.localRotation = endRotation;
    }
}

// [CreateAssetMenu(fileName = "SuperAttack", menuName = "Combat/Attacks/SuperAttack")]
// public class SuperAttack : ScriptableObject, ICombat
// {
//     public void Attack(R_Weapon weapon)
//     {
//         // Invoke normal attack command
//     }
// }

// [CreateAssetMenu(fileName = "SwordAbility", menuName = "Combat/Abilities/SwordAbility")]
// public class SwordAbility : ScriptableObject, ICombat
// {
//     public void Attack(R_Weapon weapon)
//     {
        
//     }
// }

// [CreateAssetMenu(fileName = "SpearAbility", menuName = "Combat/Abilities/SpearAbility")]
// public class SpearAbility : ScriptableObject, ICombat
// {
//     public void Attack(R_Weapon weapon)
//     {
        
//     }
// }