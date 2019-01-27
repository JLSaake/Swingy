using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColorSelect : MonoBehaviour
{
    const int NUM_COLORS = 8;

    public float shrinkRate = 1.0f;

    [SerializeField]
    private List<GameObject> panels;
    [SerializeField]
    private List<Color> panelColors;

    [SerializeField]
    private List<TextMeshPro> labelTMP = new List<TextMeshPro>();

    // TODO: UI element to interpolate each panel to these
    [SerializeField]
    private List<Color> colorBlindPanelColors;

    // Selling cache coherency 10mil GE
    private List<Material> flowMats = new List<Material>();
    private List<Animator> flowAnims = new List<Animator>();

    // State management
    // TODO: debug mode that selects colors and gets into the game
    private bool cb; // colorblind
    private int selectIndex = 0; 
    private int lastDirection = 0;
    private int promptStage = 0;
    private bool[] selectedPanels = new bool[8];

    private List<string> prompts = new List<string>()
    {
        "safety?", "familiar?", "family?", "lol"
    };

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

        if (Input.GetButtonDown("Jump") && promptStage < 3) 
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
                StartCoroutine(lerpPrompt(0, false));
                StartCoroutine(lerpPrompt(1, false));
                StartCoroutine(shrink()); 
            }
            else
            {
                // Lerp the prompt to the next prompt
                promptStage++;
                StartCoroutine(lerpPrompt(1, true));

                // Prep for next input
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

    // Label = 1 or 2
    // twice = 1 -> 0 AND 0 -> 1
    private IEnumerator lerpPrompt(int label, bool twice)
    {
        float alpha = 1.0f;
        TextMeshPro tmp = labelTMP[label];
        do {
            alpha -= Time.deltaTime * shrinkRate;
            tmp.color = new Color(tmp.color.r,tmp.color.g,tmp.color.b,alpha);
            yield return null;
        } while (alpha >= 0.0f);

        if (twice) 
        {
            tmp.text = prompts[promptStage];
            do {
                alpha += Time.deltaTime * shrinkRate;
                tmp.color = new Color(tmp.color.r,tmp.color.g,tmp.color.b,alpha);
                yield return null;
            } while (alpha <= 1.0f);
        }
    }

    private IEnumerator shrink()
    {
        float alpha = 0.0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(0f,8.3f,0f);
        do {
            alpha += Time.deltaTime * shrinkRate * 0.25f;
            transform.position = Vector3.Lerp(startPos,endPos,alpha);
            yield return null;
        } while (alpha <= 1.0f);
        // TODO: use GameManager to yield control to the player
    }
}
