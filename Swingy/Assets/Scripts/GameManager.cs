using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject housePrefab;
    private static List<Color> colorChoices = new List<Color>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Reset"))
        {
            //Application.LoadLevel(Application.loadedLevel);
            SceneManager.LoadScene(Application.loadedLevel);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public static int AddColor(Color c) 
    {
        colorChoices.Add(c);        
        return colorChoices.Count;
    }

    // Here's an interface, its probably easier for you to manage which color you want
    public static List<Color> GetColorChoices()
    {
        if (colorChoices.Count < 3)
        {
            Debug.LogWarning("Color choices were not initialized in the start properly!");
        }
        return colorChoices;
    }

    public static int ChoicesCount()
    {
        return colorChoices.Count;
    }
}
