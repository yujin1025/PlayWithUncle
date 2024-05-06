using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : UIComponent
{
    public bool cookieIN = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        cookieIN = true;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        cookieIN = false;
    }
}
