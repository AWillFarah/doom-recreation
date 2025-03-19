
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;


public enum eWeaponType
{
    none, // default or no weapon
    pistol, 
    shotgun, // multiple shots simultaneously
    
}

public class Weapon : MonoBehaviour
{
    
    
    [Header("Dynamic")] 
    [Tooltip("Setting this manually while playing does not work properly")] public WeaponSO weaponType;
    public eWeaponType currentWeapon;
    private bool reFired = false; // This is used for accuracy calculations
    private Vector3 offsetVector;
    
    private bool fireCooldown = false;

    void Start()
    {
        
        ChangeWeapon(eWeaponType.pistol);
    }

    void ChangeWeapon(eWeaponType weaponToSwitchTo)
    {
        currentWeapon = weaponToSwitchTo;
        switch (weaponToSwitchTo)
        {
            case eWeaponType.none:
                weaponType = Resources.Load<WeaponSO>("WeaponSOs/fist");
                break;
            case eWeaponType.pistol:
                weaponType = Resources.Load<WeaponSO>("WeaponSOs/pistol");  
                break;
            case eWeaponType.shotgun:
                weaponType = Resources.Load<WeaponSO>("WeaponSOs/shotgun"); 
                break;
        }
        // Calculating how our fireCooldown. We have the shots per second thanks to decinos video
        
    }
    
    void Update()
    {
        // Firing
        if (Input.GetMouseButton(0))
        {
            if(fireCooldown) return;
            // Doom has a mechanic where the first shot will always be accurate
            

            for (int i = 0; i < weaponType.numberOfShots; i++)
            {
                if (reFired || weaponType.weaponType == eWeaponType.shotgun)
                {
                    float offset = Random.Range(-weaponType.offsetMax, weaponType.offsetMax);
                    Quaternion forwardOffset = Quaternion.AngleAxis(offset, new Vector3(0, 1, 0));
                    offsetVector = forwardOffset * transform.forward;  
                
                }
                else offsetVector = transform.forward;
                Physics.Raycast(transform.position, offsetVector, out RaycastHit hit, weaponType.range);
                
                //For debugging
                Vector3 forward = offsetVector * weaponType.range;
                Debug.DrawRay(transform.position, forward, Color.red, 1);  
            }
            
            // Our first shot will always be accurate, however if we keep the mouse down our shots will be
            // Innacurate
            reFired = true;
            if(!fireCooldown) fireCooldown = true;
            Invoke("ShotRefresh", weaponType.rateOfFire);
            
        }
        if (Input.GetMouseButtonUp(0))
        {
            reFired = false;
        }
        
        
        // Switching weapons
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeWeapon(eWeaponType.none);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeWeapon(eWeaponType.pistol);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeWeapon(eWeaponType.shotgun);
        }
    }

    void ShotRefresh()
    {
        fireCooldown = false;
    }
}
