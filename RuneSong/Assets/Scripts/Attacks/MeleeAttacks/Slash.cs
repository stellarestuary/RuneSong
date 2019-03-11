using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : BaseAttack {

    public Slash()
    {
        attackName = "Slash";
        attackDescription = "Fast Slash attack with your weapon";
        attackDamage = 10f;
        attackCost = 0;
    }
}
