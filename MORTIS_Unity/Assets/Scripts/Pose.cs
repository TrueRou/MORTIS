using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Pose : MonoBehaviour
{
    [System.Serializable]
    public class PoseData
    {
        public string result_type;
        public List<Vector3> landmarks;
    }

    public PoseData poselm;
    public float smoothFactor = 0.2f;
    public bool isSitting = false;
    Animator animator;

    [Header("joints")]
    // 骨骼
    // 髋部
    public Transform Hips;

    // 左侧
    public Transform Left_leg;   // 大腿
    public Transform Left_knee;  // 膝盖
    public Transform Left_ankle; // 脚踝
    public Transform Left_toe;   // 脚尖

    // 右侧
    public Transform Right_leg;   // 大腿
    public Transform Right_knee;  // 膝盖
    public Transform Right_ankle; // 脚踝
    public Transform Right_toe;   // 脚尖

    //脊椎
    public Transform Spine;
    public Transform Chest;
    public Transform Upper_Chest;
    public Transform Neck;
    //头
    public Transform Head;
    //肩膀——大臂——小臂——手肘：
    //右
    public Transform Right_Shoulder;
    public Transform Right_arm;
    public Transform Right_elbow;
    public Transform Right_wrist;
    public Transform IndexFinger1_R;
    public Transform LittleFinger1_R;
    //左
    public Transform Left_Shoulder;
    public Transform Left_arm;
    public Transform Left_elbow;
    public Transform Left_wrist;
    public Transform IndexFinger1_L;
    public Transform LittleFinger1_L;
    //对齐旋转矩阵
    // 髋部
    public Quaternion align_Hips;

    // 左侧
    public Quaternion align_Left_leg;   // 大腿
    public Quaternion align_Left_knee;  // 膝盖
    public Quaternion align_Left_ankle; // 脚踝
    public Quaternion align_Left_toe;   // 脚尖

    // 右侧
    public Quaternion align_Right_leg;   // 大腿
    public Quaternion align_Right_knee;  // 膝盖
    public Quaternion align_Right_ankle; // 脚踝
    public Quaternion align_Right_toe;   // 脚尖

    // 脊椎
    public Quaternion align_Spine;
    public Quaternion align_Chest;
    public Quaternion align_Upper_Chest;
    public Quaternion align_Neck;

    // 头
    public Quaternion align_Head;

    // 肩膀——大臂——小臂——手肘：
    // 右
    public Quaternion align_Right_Shoulder;
    public Quaternion align_Right_arm;
    public Quaternion align_Right_elbow;
    public Quaternion align_Right_wrist;
    public Quaternion align_IndexFinger1_R;

    // 左
    public Quaternion align_Left_Shoulder;
    public Quaternion align_Left_arm;
    public Quaternion align_Left_elbow;
    public Quaternion align_Left_wrist;
    public Quaternion align_IndexFinger1_L;

    private void BoneBinding()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("(Pose)Animator component is not assigned.");
            return;
        }

        Hips = animator.GetBoneTransform(HumanBodyBones.Hips);

        // 左侧
        Left_leg = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
        Left_knee = animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
        Left_ankle = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        Left_toe = animator.GetBoneTransform(HumanBodyBones.LeftToes);

        // 右侧
        Right_leg = animator.GetBoneTransform(HumanBodyBones.RightUpperLeg);
        Right_knee = animator.GetBoneTransform(HumanBodyBones.RightLowerLeg);
        Right_ankle = animator.GetBoneTransform(HumanBodyBones.RightFoot);
        Right_toe = animator.GetBoneTransform(HumanBodyBones.RightToes);

        // 脊椎
        Spine = animator.GetBoneTransform(HumanBodyBones.Spine);
        Chest = animator.GetBoneTransform(HumanBodyBones.Chest);
        Upper_Chest = animator.GetBoneTransform(HumanBodyBones.UpperChest);
        Neck = animator.GetBoneTransform(HumanBodyBones.Neck);

        // 头
        Head = animator.GetBoneTransform(HumanBodyBones.Head);

        // 右侧肩膀——大臂——小臂——手腕
        Right_Shoulder = animator.GetBoneTransform(HumanBodyBones.RightShoulder);
        Right_arm = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
        Right_elbow = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
        Right_wrist = animator.GetBoneTransform(HumanBodyBones.RightHand);
        IndexFinger1_R = animator.GetBoneTransform(HumanBodyBones.RightIndexProximal);
        LittleFinger1_R = animator.GetBoneTransform(HumanBodyBones.RightLittleProximal);

        // 左侧肩膀——大臂——小臂——手腕
        Left_Shoulder = animator.GetBoneTransform(HumanBodyBones.LeftShoulder);
        Left_arm = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
        Left_elbow = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
        Left_wrist = animator.GetBoneTransform(HumanBodyBones.LeftHand);
        IndexFinger1_L = animator.GetBoneTransform(HumanBodyBones.LeftIndexProximal);
        LittleFinger1_L = animator.GetBoneTransform(HumanBodyBones.LeftLittleProximal);

        Debug.Log("All Pose's bones are successfully bound.");
    }

    private void InitAlignmentsMatrix()
    {
        //身体朝向
        Vector3 forward = TriangleNormal(Hips.position, Left_leg.position, Right_leg.position);

        //Right hand normal
        Vector3 forward_R = TriangleNormal(Right_wrist.position, IndexFinger1_R.position, LittleFinger1_R.position);
        //Left hand normal
        Vector3 forward_L = TriangleNormal(Left_wrist.position, IndexFinger1_L.position, LittleFinger1_L.position);

        // Root (髋部)
        align_Hips = Quaternion.Inverse(Hips.rotation) * Quaternion.LookRotation(forward);

        // 躯干
        align_Spine = Quaternion.Inverse(Spine.rotation) * Quaternion.LookRotation(Spine.position - Upper_Chest.position, forward);
        align_Chest = Quaternion.Inverse(Chest.rotation) * Quaternion.LookRotation(Chest.position - Upper_Chest.position, forward);
        align_Upper_Chest = Quaternion.Inverse(Upper_Chest.rotation) * Quaternion.LookRotation(Upper_Chest.position - Neck.position, forward);
        align_Neck = Quaternion.Inverse(Neck.rotation) * Quaternion.LookRotation(Neck.position - Head.position, forward);

        // 头部
        align_Head = Quaternion.Inverse(Head.rotation) * Quaternion.LookRotation(Head.position - Neck.position);

        // 左臂
        align_Left_Shoulder = Quaternion.Inverse(Left_Shoulder.rotation) * Quaternion.LookRotation(Left_Shoulder.position - Left_arm.position, forward);
        align_Left_arm = Quaternion.Inverse(Left_arm.rotation) * Quaternion.LookRotation(Left_arm.position - Left_elbow.position, forward);
        align_Left_elbow = Quaternion.Inverse(Left_elbow.rotation) * Quaternion.LookRotation(Left_elbow.position - Left_wrist.position, forward);
        // Left Hand Alignment
        align_Left_wrist = Quaternion.Inverse(Left_wrist.rotation) * Quaternion.LookRotation(Left_wrist.position - IndexFinger1_L.position, forward_L);

        // 右臂
        align_Right_Shoulder = Quaternion.Inverse(Right_Shoulder.rotation) * Quaternion.LookRotation(Right_Shoulder.position - Right_arm.position, forward);
        align_Right_arm = Quaternion.Inverse(Right_arm.rotation) * Quaternion.LookRotation(Right_arm.position - Right_elbow.position, forward);
        align_Right_elbow = Quaternion.Inverse(Right_elbow.rotation) * Quaternion.LookRotation(Right_elbow.position - Right_wrist.position, forward);
        // Right Hand Alignment
        align_Right_wrist = Quaternion.Inverse(Right_wrist.rotation) * Quaternion.LookRotation(Right_wrist.position - IndexFinger1_R.position, forward_R);

        // 左腿
        align_Left_leg = Quaternion.Inverse(Left_leg.rotation) * Quaternion.LookRotation(Left_leg.position - Left_knee.position, forward);
        align_Left_knee = Quaternion.Inverse(Left_knee.rotation) * Quaternion.LookRotation(Left_knee.position - Left_ankle.position, forward);
        align_Left_ankle = Quaternion.Inverse(Left_ankle.rotation) * Quaternion.LookRotation(Left_ankle.position - Left_toe.position, Left_knee.position - Left_ankle.position);

        // 右腿
        align_Right_leg = Quaternion.Inverse(Right_leg.rotation) * Quaternion.LookRotation(Right_leg.position - Right_knee.position, forward);
        align_Right_knee = Quaternion.Inverse(Right_knee.rotation) * Quaternion.LookRotation(Right_knee.position - Right_ankle.position, forward);
        align_Right_ankle = Quaternion.Inverse(Right_ankle.rotation) * Quaternion.LookRotation(Right_ankle.position - Right_toe.position, Right_knee.position - Right_ankle.position);

        Debug.Log("InitAlignmentsMatrix calculation has been Done");
    }


    private Vector3 TriangleNormal(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        Vector3 vector1 = pointA - pointB;
        Vector3 vector2 = pointA - pointC;

        Vector3 normal = Vector3.Cross(vector1, vector2);
        normal.Normalize();
        return normal;
    }

    private void UpdatePose()
    {
        if (poselm == null)
        {
            Debug.LogError("poselm or its landmarks are not properly initialized.");
            return;
        }

        Vector3 forward;
        if (isSitting)
        {
            forward = Hips.forward;
        }
        else
        {
            // mediapipe中forward计算
            forward = TriangleNormal((poselm.landmarks[23] + poselm.landmarks[24]) / 2f, (poselm.landmarks[23] + poselm.landmarks[12]) / 50f, (poselm.landmarks[24] + poselm.landmarks[11]) / 50f);

            // 使用插值平滑骨骼的旋转更新
            Hips.rotation = Quaternion.Slerp(Hips.rotation, Quaternion.LookRotation(forward) * Quaternion.Inverse(align_Hips), smoothFactor);

            // right leg
            Right_leg.rotation = Quaternion.Slerp(Right_leg.rotation, Quaternion.LookRotation(poselm.landmarks[26] - poselm.landmarks[24], forward) * Quaternion.Inverse(align_Right_leg), smoothFactor);
            Right_knee.rotation = Quaternion.Slerp(Right_knee.rotation, Quaternion.LookRotation(poselm.landmarks[28] - poselm.landmarks[26], forward) * Quaternion.Inverse(align_Right_knee), smoothFactor);

            // left leg
            Left_leg.rotation = Quaternion.Slerp(Left_leg.rotation, Quaternion.LookRotation(poselm.landmarks[25] - poselm.landmarks[23], forward) * Quaternion.Inverse(align_Left_leg), smoothFactor);
            Left_knee.rotation = Quaternion.Slerp(Left_knee.rotation, Quaternion.LookRotation(poselm.landmarks[27] - poselm.landmarks[25], forward) * Quaternion.Inverse(align_Left_knee), smoothFactor);
        }

        Vector3 Headpos = (poselm.landmarks[8] + poselm.landmarks[7]) / 2f;
        Vector3 Neckpos = (poselm.landmarks[12] + poselm.landmarks[11]) / 2f;

        Neck.rotation = Quaternion.Slerp(Neck.rotation, Quaternion.LookRotation(Headpos - Neckpos, forward) * Quaternion.Inverse(align_Neck), smoothFactor);
        // chest
        Chest.rotation = Quaternion.Slerp(Chest.rotation, Quaternion.LookRotation((poselm.landmarks[12] + poselm.landmarks[11]) / 2f - (poselm.landmarks[24] + poselm.landmarks[23]) / 2f, forward) * Quaternion.Inverse(align_Chest), smoothFactor);

        // right arm
        Right_arm.rotation = Quaternion.Slerp(Right_arm.rotation, Quaternion.LookRotation(poselm.landmarks[14] - poselm.landmarks[12], forward) * Quaternion.Inverse(align_Right_arm), smoothFactor);
        Right_elbow.rotation = Quaternion.Slerp(Right_elbow.rotation, Quaternion.LookRotation(poselm.landmarks[16] - poselm.landmarks[14], forward) * Quaternion.Inverse(align_Right_elbow), smoothFactor);

        // left arm
        Left_arm.rotation = Quaternion.Slerp(Left_arm.rotation, Quaternion.LookRotation(poselm.landmarks[13] - poselm.landmarks[11], forward) * Quaternion.Inverse(align_Left_arm), smoothFactor);
        Left_elbow.rotation = Quaternion.Slerp(Left_elbow.rotation, Quaternion.LookRotation(poselm.landmarks[15] - poselm.landmarks[13], forward) * Quaternion.Inverse(align_Left_elbow), smoothFactor);
    }

    void Start()
    {
        BoneBinding();
        InitAlignmentsMatrix();
    }

    void FixedUpdate()
    {
        UpdatePose();
    }
}
