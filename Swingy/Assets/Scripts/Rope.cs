using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    #region Procedurally Generated Variables
    // TODO: Make private
    [Space]
    [Header("Procedurally Generated Variables")]

    [Tooltip("Length of rope vertically")]
    public float length = 1;

    [Tooltip("Location of the point location the hinge is attached to")]
    public Vector2 hingePoint;

    [Tooltip("Force modifier for swing movement")]
    public float forceModifier = 1;

    // Max angle should be made negative in code when moving away from objective
    [Tooltip("Cutoff angle (from bottom) for applying force")]
    public float maxAngle = 25;

    // Min angle should be made negative in code when moving towards the objective
    [Tooltip("Starting angle to begin applying force on downswing")]
    public float minAngle = 25;

    #endregion

    private Rigidbody2D rb;

    // TODO: Add functionality for moving ropes, both swinging and point moving



    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Player Inputs

    // Add force to the rope to swing it to the left (backward)
    public void ForceLeft()
    {
        if (this.rb.rotation < minAngle && this.rb.rotation > -maxAngle)
        {
            this.rb.AddForce(Vector2.left * forceModifier);
        }
    }

    // Add force to the rope to swing it to the right (forward)
    public void ForceRight()
    {
        if (this.rb.rotation > -minAngle && this.rb.rotation < maxAngle)
        {
            this.rb.AddForce(Vector2.right * forceModifier);
        }
    }

    #endregion


    #region Getters & Setters
    public float GetLength()
    {
        return this.length;
    }

    public void SetLength(float len)
    {
        this.length = len;
        // TODO: Update length
    }

    public Vector2 GetHingePoint()
    {
        return this.hingePoint;
    }

    public void SetHingePoint(Vector2 pos)
    {
        this.hingePoint = pos;
        this.gameObject.GetComponentInParent<Transform>().position = pos;
    }

    public float GetForceModifier()
    {
        return forceModifier;
    }

    public void SetForceModifier(float force)
    {
        this.forceModifier = force;
    }

    public float GetMaxAngle()
    {
        return this.maxAngle;
    }

    public void SetMaxAngle(float angle)
    {
        this.maxAngle = angle;
    }

    public float GetMinAngle()
    {
        return this.minAngle;
    }

    public void SetMinAngle(float angle)
    {
        this.minAngle = angle;
    }


    #endregion

}
