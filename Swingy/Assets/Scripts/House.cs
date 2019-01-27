using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{

    [SerializeField]
    private List<int> thresholds = new List<int>();

    [SerializeField]
    private MeshRenderer backWindowRenderer;

    [SerializeField]
    private MeshRenderer frontWindowRenderer;

    [SerializeField]
    private ParticleSystem chimney;

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
        if (score > thresholds[0])
        {
            backWindowRenderer.material.SetColor("_FlowColor", choices[0]);
        }
        
        if (score > thresholds[1])
        {
            frontWindowRenderer.material.SetColor("_FlowColor", choices[1]);
        }

        if (score > thresholds[2])
        {
            var main = chimney.main;
            main.startColor = new Color(choices[2].r, choices[2].g, choices[2].b, 1.0f);
        }

    }
}
