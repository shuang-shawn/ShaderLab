using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleScript : MonoBehaviour
{
    public ParticleSystem fire;
    public ParticleSystem snow;

    public ParticleSystem explosion;

    public ParticleSystem random;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    

    

    // Update is called once per frame
  

    public void Fire(){
        fire.Play();
        snow.Stop();
        explosion.Stop();
        random.Stop();
    }

    public void Snow(){
        snow.Play();
        fire.Stop();
        explosion.Stop();
        random.Stop();
    }
    public void Explosion(){
        explosion.Play();
        snow.Stop();
        fire.Stop();
        random.Stop();
    }
    public void Random(){
        random.Play();
        snow.Stop();
        explosion.Stop();
        fire.Stop();
    }
}
