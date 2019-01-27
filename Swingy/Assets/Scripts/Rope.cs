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
    private SpriteRenderer spriteR;



    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        hj = this.GetComponent<HingeJoint2D>();
        parent = this.gameObject.GetComponentInParent<ParentRope>();
        originalPosition = this.gameObject.transform.position;
        spriteR = this.gameObject.GetComponent<SpriteRenderer>();

        LengthChange();


    }

    // Update is called once per frame
    void Update()
    {

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

    public void destroyCollider(){
        StartCoroutine(FadeAway());
        Destroy(gameObject.GetComponent<PolygonCollider2D>());
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

    private IEnumerator FadeAway()
    {
        Color startColor = spriteR.color;
        float elapsedTime = 0f;
        float time = 1f;


        while (elapsedTime < time)
        {
            spriteR.color = Color.Lerp(startColor, new Color(0f, 0f, 0f, 0f), elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
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

    public Vector2 GetVelocity(){
        return rb.velocity;
    }

    public float GetAngularVelocity(){
        return rb.angularVelocity;
    }

    #endregion

}
