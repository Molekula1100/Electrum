using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScroll : UsableItem
{
    public override void Effect()
    {
        Vector3 hubPos = GameObject.FindGameObjectWithTag("shop").transform.position;
        GameObject.Find(Player.uniqName).transform.position = 
        new Vector3(hubPos.x, hubPos.y -3f, 1f);
    }
}
