using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPerformar : MonoBehaviour
{
    private float t = 1;
    private HardLight2D hl;

    // Start is called before the first frame update
    void Start()
    {
        hl = GetComponent<HardLight2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("‚ ‚ ‚ ");
        
        //if(hl.Range >= 0) hl.Range -= Time.deltaTime * 3;
        //if(hl.Intensity >= 0) hl.Intensity -= Time.deltaTime * 5;

        hl.Color = new Color(hl.Color.r, hl.Color.g, hl.Color.b, t / 2);

        t -= Time.deltaTime * 1.5f;

        if (t < 0) Destroy(this.gameObject);
    }
}
