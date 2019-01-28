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
    private List<ParticleSystem> fireworks;

    [SerializeField]
    private List<ParticleSystem> explosions;

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
        if (score >= Procedural.GetMaxRopes() * (1f/3f))
        {
            backWindowRenderer.material.SetColor("_FlowColor", choices[0]);
        }
        
        if (score >= Procedural.GetMaxRopes() * (2f/3f)) 
        {
            frontWindowRenderer.material.SetColor("_FlowColor", choices[1]);
        }

        if (score >= Procedural.GetMaxRopes())
        {
            for (int i = 0; i < fireworks.Count; i++)
            {
                Gradient grad = new Gradient();
                grad.SetKeys( new GradientColorKey[] {new GradientColorKey(GameManager.GetColorChoices()[i], 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(100.0f, 0.3f) } );
                var col = fireworks[i].colorOverLifetime;
                col.color = grad;
                fireworks[i].gameObject.SetActive(true);
            }

            for (int i = 0; i < explosions.Count; i++)
            {
                Gradient grad = new Gradient();
                grad.SetKeys( new GradientColorKey[] {new GradientColorKey(GameManager.GetColorChoices()[i], 0.3f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.3f), new GradientAlphaKey(0.0f, 1.0f) } );
                var col = explosions[i].colorOverLifetime;
                col.color = grad;
                explosions[i].gameObject.SetActive(true);
            }

            var main = chimney.main;
            main.startColor = new Color(choices[2].r, choices[2].g, choices[2].b, 1.0f);
        }

    }
}
