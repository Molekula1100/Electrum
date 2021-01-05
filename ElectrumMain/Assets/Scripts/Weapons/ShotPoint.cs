using UnityEngine;

public class ShotPoint : MonoBehaviour
{
    private WeaponTypes weaponType;
    GameObject parent;
    private void Start()
    {
        parent = transform.parent.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        SetRange(true);
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        SetRange(false);
    }

    private void SetRange(bool state)
    {
        switch (weaponType)
        {
            case WeaponTypes.Staff:
                parent.GetComponent<Staff>().isMeleeRange = state;
                break;
            case WeaponTypes.Wand:
                parent.GetComponent<Wand>().isMeleeRange = state;
                break;

            default:
                break;
        }
    }
}
