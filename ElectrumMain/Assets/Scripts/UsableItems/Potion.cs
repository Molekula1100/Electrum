using UnityEngine;

public class Potion : UsableItem
{

    private GameObject DescrText;
    void Start()
    {
        DescrText = gameObject.transform.GetChild(0).gameObject;
        DescrText.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.name == Player.uniqName)
        {
            DescrText.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.name == Player.uniqName)
        {
            DescrText.SetActive(false);
        }
    }
}
