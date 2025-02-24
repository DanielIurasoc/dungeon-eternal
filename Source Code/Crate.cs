using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Fighter
{
    protected override void death() {
        base.death();
        Destroy(gameObject);
    }
}
