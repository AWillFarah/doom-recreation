
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;




public class Weapon : MonoBehaviour
{
    [Header("Inscribed")] 
    public bool isPlayer;
    [SerializeField] private Transform weaponOrigin;
    
    [Header("Dynamic")] 
    [Tooltip("Setting this manually while playing does not work properly")] public WeaponSO weaponType;
    public eWeaponType currentWeapon;
    private bool reFired = false; // This is used for accuracy calculations
    private Vector3 offsetVector;
    
    private bool fireCooldown = false;

    void Start()
    {
        if(isPlayer) ChangeWeapon(eWeaponType.pistol);
        if(weaponOrigin == null) weaponOrigin = transform;
    }

    public void ChangeWeapon(eWeaponType weaponToSwitchTo)
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
            case eWeaponType.fireball:
                weaponType = Resources.Load<WeaponSO>("WeaponSOs/fireball");
                break;
        }
        // Calculating how our fireCooldown. We have the shots per second thanks to decinos video
        
    }
    
    void Update()
    {
        // Player only stuff
        if(!isPlayer) return;
        // Firing
        if (Input.GetMouseButton(0))
        {
           FireShot(); 
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

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeWeapon(eWeaponType.fireball);
        }
    }

    public void FireShot()
    {
        if(fireCooldown) return;


        if (weaponType.useProjectile)
        {
            GameObject projectileGO = Instantiate(weaponType.projectilePrefab, weaponOrigin.position, Quaternion.identity);
            Rigidbody rb = projectileGO.GetComponent<Rigidbody>();
            rb.velocity = weaponOrigin.forward * weaponType.projectileSpeed;
        }
        for (int i = 0; i < weaponType.numberOfShots; i++)
        {
            if (reFired || weaponType.weaponType == eWeaponType.shotgun)
            {
                float offset = Random.Range(-weaponType.offsetMax, weaponType.offsetMax);
                Quaternion forwardOffset = Quaternion.AngleAxis(offset, new Vector3(0, 1, 0));
                offsetVector = forwardOffset * weaponOrigin.forward;  
                
            }
            else offsetVector = weaponOrigin.forward;
            Physics.Raycast(weaponOrigin.position, offsetVector, out RaycastHit hit, weaponType.range);
                
            //For debugging
            Vector3 forward = offsetVector * weaponType.range;
            Debug.DrawRay(weaponOrigin.position, forward, Color.red, 1);  
        }
            
        // Our first shot will always be accurate, however if we keep the mouse down our shots will be
        // Innacurate
        reFired = true;
        if(!fireCooldown) fireCooldown = true;
        Invoke("ShotRefresh", weaponType.rateOfFire);
    }
    void ShotRefresh()
    {
        fireCooldown = false;
    }
}
