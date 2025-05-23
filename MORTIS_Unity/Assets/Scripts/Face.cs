using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour
{
    [System.Serializable]
    public class FaceData
    {
        public string result_type;
        public List<Vector3> face_landmarks;
    }


    public float smoothFactor = 1.0f;

    public float exFactor = 100f;

    public FaceData facelm;

    //脖子
    public Transform Neck;
    //头
    public Transform Head;

    private Animator animator;

    private void BoneBinding()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("(Face)Animator component is not assigned.");
            return;
        }
        //脖子
        Neck = animator.GetBoneTransform(HumanBodyBones.Neck);

        // 头
        Head = animator.GetBoneTransform(HumanBodyBones.Head);
    }

    //头部旋转
    private void UpdateHeadRotation()
    {
        // 获取各个关键点的位置
        Vector3 right_face = facelm.face_landmarks[220];  // 右眼的外侧角
        Vector3 left_face = facelm.face_landmarks[440];   // 左眼的外侧角
        Vector3 eyebrows_between = facelm.face_landmarks[9]; // 眉毛中间
        Vector3 chin = facelm.face_landmarks[152];       // 下巴位置

        // 计算 forward（前方向），从眉毛中点指向下巴
        // Vector3 forward = (chin - eyebrows_between).normalized;
        Vector3 up = (chin - eyebrows_between).normalized;

        // 计算 up（上方向），从左眼到右眼的叉乘可以确定头部上方向
        Vector3 rightToLeftEye = (right_face - left_face).normalized;
        Vector3 forward = Vector3.Cross(rightToLeftEye, up).normalized;

        // 使用 forward 和 up 创建一个目标 Quaternion
        Quaternion targetRotation = Quaternion.LookRotation(up, forward);

        // 加入初始旋转 10°（例如 Y 轴旋转）
        Quaternion initialRotation = Quaternion.Euler(90, 0, 0);
        targetRotation = targetRotation * initialRotation;

        Head.rotation = Quaternion.Slerp(Head.rotation, targetRotation, Time.smoothDeltaTime * smoothFactor);
    }
    void Start()
    {
        BoneBinding();
    }

    void Update()
    {
        UpdateHeadRotation();
    }
}
