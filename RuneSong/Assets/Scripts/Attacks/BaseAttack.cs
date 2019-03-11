using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseAttack : MonoBehaviour {

    public string attackName; // name
    public string attackDescription;
    public float attackDamage; // Base Damage 15, melee lvl 10 stamina 35 = based damage + stamina + lvl
    public float attackCost; // Mana Cost
}
