using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destroy", 3.5f);
    }

    private void Destroy()
    {
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}