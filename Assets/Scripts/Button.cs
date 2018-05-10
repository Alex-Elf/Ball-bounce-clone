using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
    bool ff = false;
    public void FastForward()
    {
        Time.timeScale =(ff)? 3: 1;
        ff = !ff;
    }
}
