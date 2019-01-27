using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeParticleManager : MonoBehaviour
{

    public ParticleSystem impact;
    private int maxRope;
    private int currRope;
    private List<Color> colorChoices;
    private bool needsChoices = true;

    // Start is called before the first frame update
    void Start()
    {
        maxRope = Procedural.GetMaxRopes();
        currRope = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spawnRopeParticle(Vector2 pos)
    {
        if (needsChoices)
        {
            colorChoices = GameManager.GetColorChoices();
            needsChoices = false;
        }
        ++currRope;
        if (currRope > maxRope)
        {
            currRope = maxRope;
        }
        float ratio = (float)currRope / (float)maxRope;

        ParticleSystem particle = Instantiate(impact, pos, Quaternion.identity);

        // 3 will be the max number of colors chosen by the player
        int rand = Random.Range(0, 3);
        Color chosenColor = colorChoices[rand];
        chosenColor.a = 1;
        particle.startColor = chosenColor;


        particle.Play();
        Destroy(particle.gameObject, particle.main.duration + 1);
    }
}
