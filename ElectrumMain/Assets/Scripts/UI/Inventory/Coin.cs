using UnityEngine;
using UnityEngine.UI;
public class Coin : MonoBehaviour
{
    public void UpdateText()
    {
        GetComponent<Text>().text = Inventory.coins.ToString();
    }
}
