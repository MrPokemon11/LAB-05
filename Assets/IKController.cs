using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class IKController : MonoBehaviour
{
    //code shamelessly adapted from https://docs.unity3d.com/Manual/InverseKinematics.html 

    protected Animator animator;

    public bool ikActive = false;
    public Transform rightHandTarget = null;
    public Transform leftHandTarget = null;
    public Transform lookObj = null;
    public Transform leftFootTarget = null;
    public Transform rightFootTarget = null;

    [Range(0, 1)] public float LeftHandWeight = 1f;
    [Range(0, 1)] public float RightHandWeight = 1f;
    [Range(0, 1)] public float LeftFootWeight = 0f;
    [Range(0, 1)] public float RightFootWeight = 0f;

    public GameObject leftFoot = null;
    public GameObject rightFoot = null;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if (animator)
        {

            //if the IK is active, set the position and rotation directly to the goal.
            if (ikActive)
            {

                // Set the look target position, if one has been assigned
                if (lookObj != null)
                {
                    animator.SetLookAtWeight(1);
                    animator.SetLookAtPosition(lookObj.position);
                }

                // Set the right hand target position and rotation, if one has been assigned
                if (rightHandTarget != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, RightHandWeight);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, RightHandWeight);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
                }

                // Set the left hand target position and rotation, if one has been assigned
                if (leftHandTarget != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, LeftHandWeight);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, LeftHandWeight);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
                }

                // Set the right foot target position and rotation, if one has been assigned
                if (rightFootTarget != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, RightFootWeight);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, RightFootWeight);
                    animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootTarget.position);
                    animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootTarget.rotation);
                }

                // Set the left foot target position and rotation, if one has been assigned
                if (leftFootTarget != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, LeftFootWeight);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, LeftFootWeight);
                    animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootTarget.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootTarget.rotation);
                }

            }

            //if the IK is not active, set the position and rotation of the limbs and head back to the original position
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetLookAtWeight(0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (leftFoot != null )
        {
            
            RaycastHit hit;
            if (Physics.Raycast(transform.position + 0.25f * Vector3.up, transform.TransformDirection(Vector3.down), out hit, 0.5f))
            {
                LeftFootWeight = 0.15f;
            }
            else
            {
                LeftFootWeight = 0f;
            }
        }

        if(rightFoot != null)
        {

            RaycastHit hit;
            if (Physics.Raycast(transform.position + 0.25f * Vector3.up, transform.TransformDirection(Vector3.down), out hit, 0.5f))
            {
                RightFootWeight = 0.15f;
            }
            else
            {
                RightFootWeight = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Part1Chair" || other.name == "Climb1" || other.name == "Climb2"|| other.name == "Barrel")
        {
            ikActive = true;
            Transform[] targets = other.gameObject.GetComponentsInChildren<Transform>();
            leftHandTarget = targets[1];
            rightHandTarget = targets[2];
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Part1Chair" )
        {
            ikActive = false;
            leftHandTarget = null;
            rightHandTarget = null;
        } else if (other.name == "Climb1" || other.name == "Climb2" || other.name == "Barrel")
        {
            leftHandTarget = null;
            rightHandTarget = null;
        }
    }
}
