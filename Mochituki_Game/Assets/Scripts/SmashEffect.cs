using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashEffect : SingletonMonoBehaviourFast<SmashEffect>
{
    public GameObject SmashParticle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Smash()
    {
        Destroy(Instantiate(SmashParticle, this.transform.position, Quaternion.identity), 3f);
    }
}
