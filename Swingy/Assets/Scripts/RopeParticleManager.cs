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
            // Temp
            colorChoices.Add(Color.green);
            colorChoices.Add(Color.blue);
            colorChoices.Add(Color.grey);
            needsChoices = false;
        }
        float newSize = 0;
        //++currRope;
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
        if (ratio >=0 && ratio < 0.3f)
        {
            chosenColor.a = 0.5f;
        } else 
        if (ratio >= 0.3f && ratio < 0.6f)
        {
            chosenColor.a = 0.75f;
        } else
        if (ratio >= 0.6f)
        {
            chosenColor.a = 1f;
        }
        particle.startColor = chosenColor;


        particle.Play();
        Destroy(particle.gameObject, particle.main.duration + 1);
    }
}
