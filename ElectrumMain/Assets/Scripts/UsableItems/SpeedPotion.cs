using UnityEngine;

public class SpeedPotion : Potion
{
    public float speedingUpRate = 1.3f;

    public override void Effect()
    {
        Player.speedScale = speedingUpRate; //брать все с объекта
        Player.prevUseSpeedPotionTime = Time.time;
        Player.needChangeSpeedScale = true;        
    }
}
