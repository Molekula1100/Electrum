using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Healthbar : MonoBehaviour
{
    public List<Image> hearts = new List<Image>();
    
    public void Set(float health)
    {
        foreach (Image heart in hearts)
        {
            heart.gameObject.SetActive(false);
        }
        for(int i = 0; i < health; i++)
        {
            hearts[i].gameObject.SetActive(true);
        }
    }
}
