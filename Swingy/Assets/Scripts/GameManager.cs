using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Application.LoadLevel(Application.loadedLevel);
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
        return colorChoices;
    }
}
