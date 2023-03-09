
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LineRenderer lr;
    [SerializeField] private Vector3 grapplePoint;
    [SerializeField] private Vector3 drawGrapplePoint;
    [SerializeField] private Rigidbody connectedBody;
    [SerializeField] private Transform grappleTransfrom, Camera, Player;

    [Header("Input Settings")]
    [SerializeField] private bool holdGrapple;
    [SerializeField] private KeyCode grapple;

    [Header("Grapple Settings"), Space(2)]
    [SerializeField] private LayerMask grappleMask;
    [SerializeField] private float maxRange = 50f;
    [SerializeField] private Vector2 minMaxGrappleDist = new Vector2(0.8f, 0.25f);
    [SerializeField] private float spring = 4.5f;
    [SerializeField] private float upwardForce = 4.5f;
    [SerializeField] private float damper = 7f;
    [SerializeField] private float massScale = 4.5f;
    [SerializeField] private float grappleSpeedMultiplier;
    [SerializeField] private float grappleDrag;

    private SpringJoint joint;
    
    private Rigidbody rb;
    
    private float distFromPoint, yDiff;

    private bool isGrappling, grappleInput;

    bool canGrapple;
    
    void Start() {
        rb = Player.GetComponent<Rigidbody>();
    }

    void Update()
    {
        switch (holdGrapple)
        {
            case true:
                if (Input.GetKeyDown(grapple))
                {
                    grappleInput = true;
                    StartGrapple();
                }
                else if (Input.GetKeyUp(grapple))
                {
                    grappleInput = false;
                    StopGrapple();
                }
                break;

            case false:
                if (Input.GetKeyDown(grapple))
                {
                    grappleInput = !grappleInput;
                }
                if (grappleInput)
                {
                    if (canGrapple)
                    {
                        StartGrapple();
                        canGrapple = false;
                    }
                }
                else
                {
                    StopGrapple();
                }
                break;
        }

        PlayerController.grapplemultiplier = grappleSpeedMultiplier;
        PlayerController.grappleDrag = grappleDrag;
    }

    void LateUpdate() { DrawRope(); }

    void ConfigureJoint(SpringJoint joint)
    {
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = grapplePoint;
        joint.connectedBody = connectedBody;

        distFromPoint = Vector3.Distance(Player.position, grapplePoint);
        yDiff = grapplePoint.y - Player.position.y;

        joint.minDistance = distFromPoint * minMaxGrappleDist.x;
        joint.maxDistance = distFromPoint * minMaxGrappleDist.y;

        joint.spring = spring;
        joint.damper = damper;
        joint.massScale = massScale;
    }

    Transform hitTransform = null;
    void StartGrapple()
    {
        isGrappling = true;
        PlayerController.grappling = isGrappling;

        RaycastHit hit;

        if (Physics.Raycast(Camera.position, Camera.forward, out hit, maxRange, grappleMask))
        {
            if (hit.transform.GetComponent<Rigidbody>() != null)
            {
                grapplePoint = Vector3.zero;
                connectedBody = hit.transform.GetComponent<Rigidbody>();
                hitTransform = hit.transform;
            }
            else
            {
                grapplePoint = hit.point;
                connectedBody = null;
                hitTransform = null;
            }


            joint = Player.gameObject.AddComponent<SpringJoint>();
            ConfigureJoint(joint);

            rb.AddForce(Player.up * upwardForce * distFromPoint/(yDiff > 0f ? yDiff: 1.0f), ForceMode.Impulse);

            lr.positionCount = 2;
        }

        if (hitTransform != null)
            drawGrapplePoint = hitTransform.position;
        else
            drawGrapplePoint = hit.point;
    }

    void StopGrapple()
    {
        PlayerController.grappling = false;
        isGrappling = false;
        canGrapple = true;

        lr.positionCount = 0;

        Destroy(joint);
    }

    void DrawRope()
    {
        if (hitTransform != null)
            drawGrapplePoint = hitTransform.position;

        if (joint == null) return;

        lr.SetPosition(0, grappleTransfrom.position);
        lr.SetPosition(1, drawGrapplePoint);
    }
}
