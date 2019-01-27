using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeParticleManager : MonoBehaviour
{

    public ParticleSystem impact;
    private float targetSize;
    private int maxRope;
    private int currRope;
    private List<Color> colorChoices = new List<Color>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnParticle(Vector2 pos)
    {
        float ratio = (float)currRope / (float)maxRope;
        float newSize = 0;
        ++currRope;
        if (currRope > maxRope)
        {
            currRope = maxRope;
        }
        ParticleSystem particle = Instantiate(impact, pos, Quaternion.identity);
        var main = particle.main;

        // 3 will be the max number of colors chosen by the player
        main.startColor = (colorChoices[Random.Range(0, 3)] * ratio);

        if (ratio >= 0f && ratio < 0.30f)
        {
            newSize = targetSize * 0.33f;
        } else
        if (ratio >= 0.30f && ratio < 0.6f) {
            newSize = targetSize * 0.66f;
        } else
        if (ratio >= 0.6f)
        {
            newSize = targetSize;
        }

        main.startSize = newSize;


        particle.Play();
        Destroy(particle.gameObject, particle.main.duration + 1);
    }

    public void SetMaxRopes(int r)
    {
        maxRope = r;
    }
}
