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
    private HingeJoint2D hj;
    private ParentRope parent;
    private Vector3 originalPosition;
    // TODO: Add functionality for moving ropes, both swinging and point moving



    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        hj = this.GetComponent<HingeJoint2D>();
        parent = this.gameObject.GetComponentInParent<ParentRope>();
        originalPosition = this.gameObject.transform.position;

        LengthChange();


    }

    // Update is called once per frame
    void Update()
    {
        // Temp Changes
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            this.SetLength(length - 0.2f);
        } else
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            this.SetLength(length + 0.2f);
        }

    }


    #region Player Inputs

    // Add force to the rope to swing it to the left (backward)
    public void ForceLeft(float axisMagnitude)
    {
        if (this.rb.rotation < minAngle && this.rb.rotation > -maxAngle)
        {
            this.rb.AddForce(Vector2.left * forceModifier * Mathf.Abs(axisMagnitude));
        }
    }

    // Add force to the rope to swing it to the right (forward)
    public void ForceRight(float axisMagnitude)
    {
        if (this.rb.rotation > -minAngle && this.rb.rotation < maxAngle)
        {
            this.rb.AddForce(Vector2.right * forceModifier * Mathf.Abs(axisMagnitude));
        }
    }

    #endregion

    #region Helper Functions

    private void LengthChange()
    {
        parent.ChangeScale(1);
        this.gameObject.transform.position = originalPosition;

        parent.ChangeScale(length);
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x,
                                                         this.gameObject.transform.position.y + hj.connectedAnchor.y,
                                                         this.gameObject.transform.position.z);
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
        LengthChange();
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
