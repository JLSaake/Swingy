using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    private Vector2 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.eulerAngles = Vector3.zero;
    }

    public void SetOffset(Vector2 off)
    {
        offset = off;
    }

    public void Move(Vector2 pos)
    {
        this.transform.position = new Vector3(pos.x + offset.x, pos.y + offset.y, this.transform.position.z);
    }

    public IEnumerator MoveToRope(Vector3 ropePosition)
    {
        Vector3 startPosition = this.transform.position;
        Vector3 destination = ropePosition + new Vector3(offset.x, offset.y, this.transform.position.z);
        float elapsedTime = 0f;
        float time = .5f;

        while (elapsedTime < time)
        {
            this.transform.position = Vector3.Lerp(startPosition, destination, elapsedTime / time); ;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

}
