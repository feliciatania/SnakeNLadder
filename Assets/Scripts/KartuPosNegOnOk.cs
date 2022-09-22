using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartuPosNegOnOk : MonoBehaviour
{
    public GameManager GM;

    public void onButtonOKClick()
    {
        GM.onButtonOk();
    }
}
