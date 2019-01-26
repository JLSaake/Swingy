using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelect : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> panels;

    // Forgive my dear aunt sally for parallel arrays
    // The idea is bearable because the panel array is set once and only once

    [SerializeField]
    private List<Color> panelColors;

    // TODO: UI element to interpolate each panel to these
    [SerializeField]
    private List<Color> colorBlindPanelColors;

    void Start()
    {
        // 1:1 match the panels with a panel color
        for (int i = 0; i < panels.Count; i++)
        {
            // Set color in the material
            panels[i].GetComponent<MeshRenderer>().material.SetColor("_FlowColor", panelColors[i]);
        }
    }

    void Update()
    {
        // Check for the player's "focus"
        // If so, lerp into the flow state
        
        // When we lose focus, lerp into the idle state
    }
}
