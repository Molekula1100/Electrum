using UnityEngine;

public class HealPotion : Potion
{
    public  int healingRate = 1;
    public override void Effect()
    {
        Player.playerHealth += healingRate;
        GameObject.Find("healthbar").GetComponent<Healthbar>().Set(Player.playerHealth);
    }
}
