using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waitfor2sec : MonoBehaviour
{
    public IEnumerator PauseGame()
    {
        yield return new WaitForSeconds(2f);

    }

}
