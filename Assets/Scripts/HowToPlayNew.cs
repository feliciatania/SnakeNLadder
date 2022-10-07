using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayNew : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnButtonNext()
    {
        GameInstance.onStart?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
