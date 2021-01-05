using UnityEngine;

public class Weapon: Item
{
    [SerializeField] protected JoystickSecond joystickSecond;
    [SerializeField] protected float timeBtwStrikes;
    protected LayerMask enemiesLayer;
    protected float attackRange;
    public int number = 1;
    public bool isEquiped; 
    public int damage;
    protected float lastStrikeTime;
    public virtual void Strike()
    {

    }
   
}
