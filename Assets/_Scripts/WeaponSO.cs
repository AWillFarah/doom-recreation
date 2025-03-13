using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon Stats")]
public class WeaponSO : ScriptableObject
{
    public eWeaponType weaponType;
    public float damageMin = 5f;
    public float damageMax = 15f;
    public float range = 2048f;
    public GameObject ammoType = null;
    public int weaponSlot = 2;
    public float offsetMin = 0f;
    public float offsetMax = 5.6f;
}
