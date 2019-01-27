using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelect : MonoBehaviour
{
    const int NUM_COLORS = 8;
    [SerializeField]
    private List<GameObject> panels;

    // Selling cache coherency 10mil GE
    private List<Material> flowMats = new List<Material>();
    private List<Animator> flowAnims = new List<Animator>();

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
    private bool[] selectedPanels = new bool[8];

    void Start()
    {
        // 1:1 match the panels with a panel color
        for (int i = 0; i < panels.Count; i++)
        {
            // Set color in the material
            Material m = panels[i].GetComponent<MeshRenderer>().material;
            flowAnims.Add(panels[i].transform.parent.gameObject.GetComponent<Animator>());
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

        if (Input.GetButtonDown("Jump")) 
        {
            // Play the animation for selecting the color
            flowAnims[selectIndex].Play("Pick");
            // Play a cool sound effect for selecting

            // Prevent picker from cycling into this color again
            selectedPanels[selectIndex] = true; 

            // Check if we're at three colors in the GameManager
            if (GameManager.AddColor(cb ? colorBlindPanelColors[selectIndex] : panelColors[selectIndex]) == 3)
            {
                // fade this out, move on
                Debug.Log("Nope!");
            }
            else
            {
                lastDirection = 0;
                cycleSelection(1);
            }
            
        }

        flowMats[selectIndex].SetFloat("_MyTime", flowMats[selectIndex].GetFloat("_MyTime") + Time.deltaTime);
        // We'll keep animating the selected ones too
        for (int i = 0; i < selectedPanels.Length; i++)
        {
            if (selectedPanels[i])
            {
                flowMats[i].SetFloat("_MyTime", flowMats[i].GetFloat("_MyTime") + Time.deltaTime);
            }
        }

        lastDirection = (int)Input.GetAxisRaw("Horizontal");

    }

    private void cycleSelection(int input)
    {
        if (input != lastDirection)
        {
            do {
                selectIndex = (selectIndex + input) % NUM_COLORS;
                if (selectIndex < 0)            
                {
                    selectIndex = NUM_COLORS - 1;
                } 
            } while (selectedPanels[selectIndex]);

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
            yield return null;
        } while (alpha < end);
    }
}
