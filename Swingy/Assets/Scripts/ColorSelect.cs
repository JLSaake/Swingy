using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelect : MonoBehaviour
{
    const int NUM_COLORS = 8;
    [SerializeField]
    private List<GameObject> panels;

    private List<Material> flowMats = new List<Material>();

    // Forgive my dear aunt sally for parallel arrays
    // The idea is bearable because the panel array is set once and only once

    [SerializeField]
    private List<Color> panelColors;

    // TODO: UI element to interpolate each panel to these
    [SerializeField]
    private List<Color> colorBlindPanelColors;

    // State management
    // TODO: debug mode that selects colors and gets into the game
    private bool cb; // colorblind
    private int selectIndex = 0; 
    private int lastDirection = 0;

    void Start()
    {
        // 1:1 match the panels with a panel color
        for (int i = 0; i < panels.Count; i++)
        {
            // Set color in the material
            Material m = panels[i].GetComponent<MeshRenderer>().material;
            m.SetColor("_FlowColor", panelColors[i]);
            m.SetColor("_CBFlowColor", colorBlindPanelColors[i]);
            flowMats.Add(m);
        }
        StartCoroutine(lerpColor(0.0f,1.0f,0));
    }

    void Update()
    {
        // Check for colorblind mode
        if (Input.GetKeyDown(KeyCode.C)) 
        {
            if (cb)
            {
                foreach (Material m in flowMats)
                {
                    m.SetFloat("_CB", 1.0f);
                }
            }
            else 
            {
                foreach (Material m in flowMats)
                {
                    m.SetFloat("_CB", 0.0f);
                }
            }
            cb = !cb;
        }

        if (Input.GetAxisRaw("Horizontal") != 0f)
        {
            cycleSelection((int)(Input.GetAxisRaw("Horizontal")));
        }
        flowMats[selectIndex].SetFloat("_MyTime", flowMats[selectIndex].GetFloat("_MyTime") + Time.deltaTime);

        lastDirection = (int)Input.GetAxisRaw("Horizontal");

    }

    private void cycleSelection(int input)
    {
        if (input != lastDirection)
        {
            selectIndex = (selectIndex + input) % NUM_COLORS;
            if (selectIndex < 0)
            {
                selectIndex = NUM_COLORS - 1;
            }

            // Lerp into the flow state
            StartCoroutine(lerpColor(0.0f,1.0f, selectIndex));
        }
    }

    private IEnumerator lerpColor(float start, float end, int index)
    {
        float alpha = 0.0f;
        do {
            alpha += Time.deltaTime * 2;
            flowMats[index].SetFloat("_FlowActive", alpha);
            Debug.Log(alpha);
            yield return null;
        } while (alpha < end);
    }
}
