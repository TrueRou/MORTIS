using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [System.Serializable]
    public class HandsData
    {
        public List<HandData> hands;
    }

    [System.Serializable]
    public class HandData
    {
        public string result_type;
        public int handedness;
        public List<Vector3> hand_world_landmarks;
    }

    public HandsData handlm;

    Animator animator;

    public bool usinglerp;
    float interpolationFactor = 0.3f;

    [Header("Right Hand's joints")]

    // Right wrist
    public Transform RightWrist;

    // Index finger
    public Transform IndexFinger1_R;
    public Transform IndexFinger2_R;
    public Transform IndexFinger3_R;

    // Little finger
    public Transform LittleFinger1_R;
    public Transform LittleFinger2_R;
    public Transform LittleFinger3_R;

    // Middle finger
    public Transform MiddleFinger1_R;
    public Transform MiddleFinger2_R;
    public Transform MiddleFinger3_R;

    // Ring finger
    public Transform RingFinger1_R;
    public Transform RingFinger2_R;
    public Transform RingFinger3_R;

    // Thumb
    public Transform Thumb0_R;
    public Transform Thumb1_R;
    public Transform Thumb2_R;

    [Header("Left Hand's joints")]

    // Left wrist
    public Transform LeftWrist;

    // Index finger
    public Transform IndexFinger1_L;
    public Transform IndexFinger2_L;
    public Transform IndexFinger3_L;

    // Little finger
    public Transform LittleFinger1_L;
    public Transform LittleFinger2_L;
    public Transform LittleFinger3_L;

    // Middle finger
    public Transform MiddleFinger1_L;
    public Transform MiddleFinger2_L;
    public Transform MiddleFinger3_L;

    // Ring finger
    public Transform RingFinger1_L;
    public Transform RingFinger2_L;
    public Transform RingFinger3_L;

    // Thumb
    public Transform Thumb0_L;
    public Transform Thumb1_L;
    public Transform Thumb2_L;

    [Header("Right Hand's AlignmentsMatrix")]

    // Right wrist
    public Quaternion align_RightWrist;

    // Index finger
    public Quaternion align_IndexFinger1_R;
    public Quaternion align_IndexFinger2_R;
    public Quaternion align_IndexFinger3_R;

    // Little finger
    public Quaternion align_LittleFinger1_R;
    public Quaternion align_LittleFinger2_R;
    public Quaternion align_LittleFinger3_R;

    // Middle finger
    public Quaternion align_MiddleFinger1_R;
    public Quaternion align_MiddleFinger2_R;
    public Quaternion align_MiddleFinger3_R;

    // Ring finger
    public Quaternion align_RingFinger1_R;
    public Quaternion align_RingFinger2_R;
    public Quaternion align_RingFinger3_R;

    // Thumb
    public Quaternion align_Thumb0_R;
    public Quaternion align_Thumb1_R;
    public Quaternion align_Thumb2_R;

    [Header("Left Hand's AlignmentsMatrix")]

    // Left wrist
    public Quaternion align_LeftWrist;

    // Index finger
    public Quaternion align_IndexFinger1_L;
    public Quaternion align_IndexFinger2_L;
    public Quaternion align_IndexFinger3_L;

    // Little finger
    public Quaternion align_LittleFinger1_L;
    public Quaternion align_LittleFinger2_L;
    public Quaternion align_LittleFinger3_L;

    // Middle finger
    public Quaternion align_MiddleFinger1_L;
    public Quaternion align_MiddleFinger2_L;
    public Quaternion align_MiddleFinger3_L;

    // Ring finger
    public Quaternion align_RingFinger1_L;
    public Quaternion align_RingFinger2_L;
    public Quaternion align_RingFinger3_L;

    // Thumb
    public Quaternion align_Thumb0_L;
    public Quaternion align_Thumb1_L;
    public Quaternion align_Thumb2_L;


    //绑骨骼
    private void BoneBinding()
    {

        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("(Hand)Animator is not assigned!");
            return;
        }

        // Left Hand
        LeftWrist = animator.GetBoneTransform(HumanBodyBones.LeftHand);
        IndexFinger1_L = animator.GetBoneTransform(HumanBodyBones.LeftIndexProximal);
        IndexFinger2_L = animator.GetBoneTransform(HumanBodyBones.LeftIndexIntermediate);
        IndexFinger3_L = animator.GetBoneTransform(HumanBodyBones.LeftIndexDistal);
        LittleFinger1_L = animator.GetBoneTransform(HumanBodyBones.LeftLittleProximal);
        LittleFinger2_L = animator.GetBoneTransform(HumanBodyBones.LeftLittleIntermediate);
        LittleFinger3_L = animator.GetBoneTransform(HumanBodyBones.LeftLittleDistal);
        MiddleFinger1_L = animator.GetBoneTransform(HumanBodyBones.LeftMiddleProximal);
        MiddleFinger2_L = animator.GetBoneTransform(HumanBodyBones.LeftMiddleIntermediate);
        MiddleFinger3_L = animator.GetBoneTransform(HumanBodyBones.LeftMiddleDistal);
        RingFinger1_L = animator.GetBoneTransform(HumanBodyBones.LeftRingProximal);
        RingFinger2_L = animator.GetBoneTransform(HumanBodyBones.LeftRingIntermediate);
        RingFinger3_L = animator.GetBoneTransform(HumanBodyBones.LeftRingDistal);
        Thumb0_L = animator.GetBoneTransform(HumanBodyBones.LeftThumbProximal);
        Thumb1_L = animator.GetBoneTransform(HumanBodyBones.LeftThumbIntermediate);
        Thumb2_L = animator.GetBoneTransform(HumanBodyBones.LeftThumbDistal);

        // Right Hand
        RightWrist = animator.GetBoneTransform(HumanBodyBones.RightHand);
        IndexFinger1_R = animator.GetBoneTransform(HumanBodyBones.RightIndexProximal);
        IndexFinger2_R = animator.GetBoneTransform(HumanBodyBones.RightIndexIntermediate);
        IndexFinger3_R = animator.GetBoneTransform(HumanBodyBones.RightIndexDistal);
        LittleFinger1_R = animator.GetBoneTransform(HumanBodyBones.RightLittleProximal);
        LittleFinger2_R = animator.GetBoneTransform(HumanBodyBones.RightLittleIntermediate);
        LittleFinger3_R = animator.GetBoneTransform(HumanBodyBones.RightLittleDistal);
        MiddleFinger1_R = animator.GetBoneTransform(HumanBodyBones.RightMiddleProximal);
        MiddleFinger2_R = animator.GetBoneTransform(HumanBodyBones.RightMiddleIntermediate);
        MiddleFinger3_R = animator.GetBoneTransform(HumanBodyBones.RightMiddleDistal);
        RingFinger1_R = animator.GetBoneTransform(HumanBodyBones.RightRingProximal);
        RingFinger2_R = animator.GetBoneTransform(HumanBodyBones.RightRingIntermediate);
        RingFinger3_R = animator.GetBoneTransform(HumanBodyBones.RightRingDistal);
        Thumb0_R = animator.GetBoneTransform(HumanBodyBones.RightThumbProximal);
        Thumb1_R = animator.GetBoneTransform(HumanBodyBones.RightThumbIntermediate);
        Thumb2_R = animator.GetBoneTransform(HumanBodyBones.RightThumbDistal);

        Debug.Log("All Hand's bones are successfully bound.");
    }

    private void InitAlignmentsMatrix()
    {
        //Right hand normal
        Vector3 forward_R = TriangleNormal(RightWrist.position, IndexFinger1_R.position, LittleFinger1_R.position);
        //Left hand normal
        Vector3 forward_L = TriangleNormal(LeftWrist.position, IndexFinger1_L.position, LittleFinger1_L.position);

        // Right Hand Alignment
        align_RightWrist = Quaternion.Inverse(RightWrist.rotation) * Quaternion.LookRotation(RightWrist.position - MiddleFinger1_R.position, forward_R);

        align_IndexFinger1_R = Quaternion.Inverse(IndexFinger1_R.rotation) * Quaternion.LookRotation(IndexFinger1_R.position - IndexFinger2_R.position, forward_R);
        align_IndexFinger2_R = Quaternion.Inverse(IndexFinger2_R.rotation) * Quaternion.LookRotation(IndexFinger2_R.position - IndexFinger3_R.position, forward_R);
        align_IndexFinger3_R = Quaternion.Inverse(IndexFinger3_R.rotation) * Quaternion.LookRotation(-1f * IndexFinger3_R.up, forward_R);

        align_LittleFinger1_R = Quaternion.Inverse(LittleFinger1_R.rotation) * Quaternion.LookRotation(LittleFinger1_R.position - LittleFinger2_R.position, forward_R);
        align_LittleFinger2_R = Quaternion.Inverse(LittleFinger2_R.rotation) * Quaternion.LookRotation(LittleFinger2_R.position - LittleFinger3_R.position, forward_R);
        align_LittleFinger3_R = Quaternion.Inverse(LittleFinger3_R.rotation) * Quaternion.LookRotation(-1f * LittleFinger3_R.up, forward_R);

        align_MiddleFinger1_R = Quaternion.Inverse(MiddleFinger1_R.rotation) * Quaternion.LookRotation(MiddleFinger1_R.position - MiddleFinger2_R.position, forward_R);
        align_MiddleFinger2_R = Quaternion.Inverse(MiddleFinger2_R.rotation) * Quaternion.LookRotation(MiddleFinger2_R.position - MiddleFinger3_R.position, forward_R);
        align_MiddleFinger3_R = Quaternion.Inverse(MiddleFinger3_R.rotation) * Quaternion.LookRotation(-1f * MiddleFinger3_R.up, forward_R);

        align_RingFinger1_R = Quaternion.Inverse(RingFinger1_R.rotation) * Quaternion.LookRotation(RingFinger1_R.position - RingFinger2_R.position, forward_R);
        align_RingFinger2_R = Quaternion.Inverse(RingFinger2_R.rotation) * Quaternion.LookRotation(RingFinger2_R.position - RingFinger3_R.position, forward_R);
        align_RingFinger3_R = Quaternion.Inverse(RingFinger3_R.rotation) * Quaternion.LookRotation(-1f * RingFinger3_R.up, forward_R);

        align_Thumb0_R = Quaternion.Inverse(Thumb0_R.rotation) * Quaternion.LookRotation(Thumb0_R.position - Thumb1_R.position, forward_R);
        align_Thumb1_R = Quaternion.Inverse(Thumb1_R.rotation) * Quaternion.LookRotation(Thumb1_R.position - Thumb2_R.position, forward_R);
        align_Thumb2_R = Quaternion.Inverse(Thumb2_R.rotation) * Quaternion.LookRotation(-1f * Thumb2_R.up, forward_R);

        // Left Hand Alignment
        align_LeftWrist = Quaternion.Inverse(LeftWrist.rotation) * Quaternion.LookRotation(LeftWrist.position - MiddleFinger1_L.position, forward_L);

        align_IndexFinger1_L = Quaternion.Inverse(IndexFinger1_L.rotation) * Quaternion.LookRotation(IndexFinger1_L.position - IndexFinger2_L.position, forward_L);
        align_IndexFinger2_L = Quaternion.Inverse(IndexFinger2_L.rotation) * Quaternion.LookRotation(IndexFinger2_L.position - IndexFinger3_L.position, forward_L);
        align_IndexFinger3_L = Quaternion.Inverse(IndexFinger3_L.rotation) * Quaternion.LookRotation(-1f * IndexFinger3_L.up, forward_L);

        align_LittleFinger1_L = Quaternion.Inverse(LittleFinger1_L.rotation) * Quaternion.LookRotation(LittleFinger1_L.position - LittleFinger2_L.position, forward_L);
        align_LittleFinger2_L = Quaternion.Inverse(LittleFinger2_L.rotation) * Quaternion.LookRotation(LittleFinger2_L.position - LittleFinger3_L.position, forward_L);
        align_LittleFinger3_L = Quaternion.Inverse(LittleFinger3_L.rotation) * Quaternion.LookRotation(-1f * LittleFinger3_L.up, forward_L);

        align_MiddleFinger1_L = Quaternion.Inverse(MiddleFinger1_L.rotation) * Quaternion.LookRotation(MiddleFinger1_L.position - MiddleFinger2_L.position, forward_L);
        align_MiddleFinger2_L = Quaternion.Inverse(MiddleFinger2_L.rotation) * Quaternion.LookRotation(MiddleFinger2_L.position - MiddleFinger3_L.position, forward_L);
        align_MiddleFinger3_L = Quaternion.Inverse(MiddleFinger3_L.rotation) * Quaternion.LookRotation(-1f * MiddleFinger3_L.up, forward_L);

        align_RingFinger1_L = Quaternion.Inverse(RingFinger1_L.rotation) * Quaternion.LookRotation(RingFinger1_L.position - RingFinger2_L.position, forward_L);
        align_RingFinger2_L = Quaternion.Inverse(RingFinger2_L.rotation) * Quaternion.LookRotation(RingFinger2_L.position - RingFinger3_L.position, forward_L);
        align_RingFinger3_L = Quaternion.Inverse(RingFinger3_L.rotation) * Quaternion.LookRotation(-1f * RingFinger3_L.up, forward_L);

        align_Thumb0_L = Quaternion.Inverse(Thumb0_L.rotation) * Quaternion.LookRotation(Thumb0_L.position - Thumb1_L.position, forward_L);
        align_Thumb1_L = Quaternion.Inverse(Thumb1_L.rotation) * Quaternion.LookRotation(Thumb1_L.position - Thumb2_L.position, forward_L);
        align_Thumb2_L = Quaternion.Inverse(Thumb2_L.rotation) * Quaternion.LookRotation(-1f * Thumb2_L.up, forward_L);

        Debug.Log("InitAlignmentsMatrix calculation has been Done");
    }

    //calculate normal by 3 points
    private Vector3 TriangleNormal(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        Vector3 vector1 = pointA - pointB;
        Vector3 vector2 = pointA - pointC;

        Vector3 normal = Vector3.Cross(vector1, vector2);
        normal.Normalize();
        return normal;
    }

    private void UpdateHand()
    {
        if (handlm == null)
        {
            Debug.LogError("handlm or its landmarks are not properly initialized.");
            return;
        }

        foreach (var handData in handlm.hands)
        {
            // Right hand
            if (handData.handedness == 0)
            {
                // Calculate forward vector
                Vector3 forward_R = TriangleNormal(handData.hand_world_landmarks[0], handData.hand_world_landmarks[5], handData.hand_world_landmarks[17]);

                // Wrist rotation
                RightWrist.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[5] - handData.hand_world_landmarks[0], forward_R) * Quaternion.Inverse(align_RightWrist);

                // // Fingers rotation (Index, Middle, Ring, Pinky, Thumb)
                IndexFinger1_R.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[6] - handData.hand_world_landmarks[5], forward_R) * Quaternion.Inverse(align_IndexFinger1_R);
                IndexFinger2_R.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[7] - handData.hand_world_landmarks[6], forward_R) * Quaternion.Inverse(align_IndexFinger2_R);
                IndexFinger3_R.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[8] - handData.hand_world_landmarks[7], forward_R) * Quaternion.Inverse(align_IndexFinger3_R);

                MiddleFinger1_R.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[10] - handData.hand_world_landmarks[9], forward_R) * Quaternion.Inverse(align_MiddleFinger1_R);
                MiddleFinger2_R.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[11] - handData.hand_world_landmarks[10], forward_R) * Quaternion.Inverse(align_MiddleFinger2_R);
                MiddleFinger3_R.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[12] - handData.hand_world_landmarks[11], forward_R) * Quaternion.Inverse(align_MiddleFinger3_R);

                RingFinger1_R.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[14] - handData.hand_world_landmarks[13], forward_R) * Quaternion.Inverse(align_RingFinger1_R);
                RingFinger2_R.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[15] - handData.hand_world_landmarks[14], forward_R) * Quaternion.Inverse(align_RingFinger2_R);
                RingFinger3_R.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[16] - handData.hand_world_landmarks[15], forward_R) * Quaternion.Inverse(align_RingFinger3_R);

                LittleFinger1_R.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[18] - handData.hand_world_landmarks[17], forward_R) * Quaternion.Inverse(align_LittleFinger1_R);
                LittleFinger2_R.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[19] - handData.hand_world_landmarks[18], forward_R) * Quaternion.Inverse(align_LittleFinger2_R);
                LittleFinger3_R.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[20] - handData.hand_world_landmarks[19], forward_R) * Quaternion.Inverse(align_LittleFinger3_R);

                Thumb0_R.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[2] - handData.hand_world_landmarks[1], forward_R) * Quaternion.Inverse(align_Thumb0_R);
                Thumb1_R.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[3] - handData.hand_world_landmarks[2], forward_R) * Quaternion.Inverse(align_Thumb1_R);
                Thumb2_R.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[4] - handData.hand_world_landmarks[3], forward_R) * Quaternion.Inverse(align_Thumb2_R);
            }
            // Left hand
            else if (handData.handedness == 1)
            {
                // Calculate forward vector
                Vector3 forward_L = TriangleNormal(handData.hand_world_landmarks[0], handData.hand_world_landmarks[5], handData.hand_world_landmarks[17]);

                // Wrist rotation
                LeftWrist.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[5] - handData.hand_world_landmarks[0], forward_L) * Quaternion.Inverse(align_LeftWrist);

                // // Fingers rotation (Index, Middle, Ring, Pinky, Thumb)
                IndexFinger1_L.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[6] - handData.hand_world_landmarks[5], forward_L) * Quaternion.Inverse(align_IndexFinger1_L);
                IndexFinger2_L.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[7] - handData.hand_world_landmarks[6], forward_L) * Quaternion.Inverse(align_IndexFinger2_L);
                IndexFinger3_L.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[8] - handData.hand_world_landmarks[7], forward_L) * Quaternion.Inverse(align_IndexFinger3_L);

                MiddleFinger1_L.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[10] - handData.hand_world_landmarks[9], forward_L) * Quaternion.Inverse(align_MiddleFinger1_L);
                MiddleFinger2_L.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[11] - handData.hand_world_landmarks[10], forward_L) * Quaternion.Inverse(align_MiddleFinger2_L);
                MiddleFinger3_L.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[12] - handData.hand_world_landmarks[11], forward_L) * Quaternion.Inverse(align_MiddleFinger3_L);

                RingFinger1_L.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[14] - handData.hand_world_landmarks[13], forward_L) * Quaternion.Inverse(align_RingFinger1_L);
                RingFinger2_L.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[15] - handData.hand_world_landmarks[14], forward_L) * Quaternion.Inverse(align_RingFinger2_L);
                RingFinger3_L.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[16] - handData.hand_world_landmarks[15], forward_L) * Quaternion.Inverse(align_RingFinger3_L);

                LittleFinger1_L.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[18] - handData.hand_world_landmarks[17], forward_L) * Quaternion.Inverse(align_LittleFinger1_L);
                LittleFinger2_L.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[19] - handData.hand_world_landmarks[18], forward_L) * Quaternion.Inverse(align_LittleFinger2_L);
                LittleFinger3_L.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[20] - handData.hand_world_landmarks[19], forward_L) * Quaternion.Inverse(align_LittleFinger3_L);

                Thumb0_L.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[2] - handData.hand_world_landmarks[1], forward_L) * Quaternion.Inverse(align_Thumb0_L);
                Thumb1_L.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[3] - handData.hand_world_landmarks[2], forward_L) * Quaternion.Inverse(align_Thumb1_L);
                Thumb2_L.rotation = Quaternion.LookRotation(handData.hand_world_landmarks[4] - handData.hand_world_landmarks[3], forward_L) * Quaternion.Inverse(align_Thumb2_L);
            }
            else
            {
                Debug.Log("Handedness is not properly initialized.");
            }
        }
    }

    void Start()
    {
        BoneBinding();
        InitAlignmentsMatrix();
    }


    void Update()
    {
        UpdateHand();
    }
}
