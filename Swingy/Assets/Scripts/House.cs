using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{

    [SerializeField]
    private MeshRenderer backWindowRenderer;

    [SerializeField]
    private MeshRenderer frontWindowRenderer;

    [SerializeField]
    private ParticleSystem chimney;

    [SerializeField]
    private ParticleSystem fireworks;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init(int score)
    {
        List<Color> choices = GameManager.GetColorChoices();
        if (score >= Procedural.GetLevel1Ropes())
        {
            backWindowRenderer.material.SetColor("_FlowColor", choices[0]);
        }
        
        if (score >= Procedural.GetLevel1Ropes() + Procedural.GetLevel2Ropes())
        {
            frontWindowRenderer.material.SetColor("_FlowColor", choices[1]);
        }

        if (score >= Procedural.GetMaxRopes())
        {
            var main = chimney.main;
            main.startColor = new Color(choices[2].r, choices[2].g, choices[2].b, 1.0f);
        }

    }
}
