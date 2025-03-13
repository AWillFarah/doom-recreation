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
        }
        
    }
    
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            print(weaponType.weaponType);
            if (reFired)
            {
                float offset = Random.Range(-weaponType.offsetMax, weaponType.offsetMax);
                Quaternion forwardOffset = Quaternion.AngleAxis(offset, new Vector3(0, 1, 0));
                offsetVector = forwardOffset * transform.forward;  
                
            }
            else offsetVector = transform.forward;
            Physics.Raycast(transform.position, offsetVector, out RaycastHit hit, weaponType.range);
            print("firing");
            //For debugging
            Vector3 forward = offsetVector * weaponType.range;
            Debug.DrawRay(transform.position, forward, Color.red, 1);
            // Our first shot will always be accurate, however if we keep the mouse down our shots will be
            // Innacurate
            reFired = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            reFired = false;
        }
    }
}
