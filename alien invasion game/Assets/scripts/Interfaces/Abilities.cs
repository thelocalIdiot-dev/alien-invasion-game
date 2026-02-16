using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Abilities
{
    public bool unlocked {  get; set; }
    public bool hasEnergyEffect {  get; set; }

    public void EnergyEffect(float duration)
    {

    }

    public void upgrade(UpGradeSO UPGSO)
    {
        // ────── GUN ────── //
        //type 0 - damage
        //type 1 - cooldown
        //type 2 - kb
        // ────── BOMB ───── //
        //type 0 - damage
        //type 1 - cooldown
        // ─── Chain saw ─── //
        //type 0 - damage
        //type 1 - cooldown
        //type 2 - power
    }
}
