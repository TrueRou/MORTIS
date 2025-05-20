using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour
{
    
    [System.Serializable]
    public class FaceData_landmark
    {
        public string result_type;
        public List<Vector3> face_landmarks;
    }
    
    [System.Serializable]
    public class FaceData_blendshape
    {
        public string result_type;
        public List<BlendShape> face_blendshape;
       
    }
    [System.Serializable]
    public class BlendShape
    {
        public string name;
        public float score;
    }

    public bool usinglerp;//是否使用表情插值
    
    public float smoothFactor = 1.0f;//头部旋转插值速度，平滑因子
    
    public float lerpSpeed = 35f;  // 控制表情插值的速度，可以根据需要调整

    public float exFactor = 100f;//扩大因子
    
    public FaceData_landmark facelm;//脸部landmark
    public FaceData_blendshape facebs;//脸部blendshape
    
    public SkinnedMeshRenderer blendshapes;//blendshape修改
    
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
        Vector3 up=( chin-eyebrows_between).normalized;
    
        // 计算 up（上方向），从左眼到右眼的叉乘可以确定头部上方向
        Vector3 rightToLeftEye = (right_face - left_face).normalized;
        Vector3 forward = Vector3.Cross(rightToLeftEye, up).normalized;
    
        // 使用 forward 和 up 创建一个目标 Quaternion
        Quaternion targetRotation = Quaternion.LookRotation(up, forward);
    
        
    
        // 加入初始旋转 10°（例如 Y 轴旋转）
        Quaternion initialRotation = Quaternion.Euler(90, 0, 0);
        targetRotation = targetRotation * initialRotation;
        
      
    
        
        // 使用 Slerp 平滑地插值到目标旋转
        // 控制旋转速度，值越大旋转越快
        Head.rotation = Quaternion.Slerp(Head.rotation, targetRotation, Time.smoothDeltaTime* smoothFactor);
      
    
        Debug.Log("head rotation");
    }

    
    

// ### 从0开始排序的BlendShapes名称(模型)：
// 0. vrc.blink_left
// 1. vrc.blink_right
// 2. vrc.lowerlid_left
// 3. vrc.lowerlid_right
// 4. vrc.v_aa
// 5. vrc.v_ch
// 6. vrc.v_dd
// 7. vrc.v_e
// 8. vrc.v_ff
// 9. vrc.v_ih
// 10. vrc.v_kk
// 11. vrc.v_nn
// 12. vrc.v_oh
// 13. vrc.v_ou
// 14. vrc.v_pp
// 15. vrc.v_rr
// 16. vrc.v_sil
// 17. vrc.v_ss
// 18. vrc.v_th
// 19. Fcl_ALL_Neutral
// 20. Fcl_ALL_Angry
// 21. Fcl_ALL_Fun
// 22. Fcl_ALL_Joy
// 23. Fcl_ALL_Sorrow
// 24. Fcl_ALL_Surprised
// 25. Fcl_BRW_Angry
// 26. Fcl_BRW_Fun
// 27. Fcl_BRW_Joy
// 28. Fcl_BRW_Sorrow
// 29. Fcl_BRW_Surprised
// 30. Fcl_EYE_Angry
// 31. Fcl_EYE_Close
// 32. Fcl_EYE_Close_L
// 33. Fcl_EYE_Close_R
// 34. Fcl_EYE_Fun
// 35. Fcl_EYE_Sorrow
// 36. Fcl_EYE_Surprised
// 37. Fcl_EYE_Spread
// 38. Fcl_EYE_Iris_Hide
// 39. Fcl_EYE_Highlight_Hide
// 40. Fcl_EYE_Look_UP
// 41. Fcl_EYE_Look_DOWN
// 42. Fcl_EYE_Look_LEFT
// 43. Fcl_EYE_Look_RIGHT
// 44. Fcl_MTH_Close
// 45. Fcl_MTH_Up
// 46. Fcl_MTH_Down
// 47. Fcl_MTH_Angry
// 48. Fcl_MTH_Small
// 49. Fcl_MTH_Large
// 50. Fcl_MTH_Neutral
// 51. Fcl_MTH_Fun
// 52. Fcl_MTH_Joy
// 53. Fcl_MTH_Sorrow
// 54. Fcl_MTH_Surprised
// 55. Fcl_MTH_SkinFung
// 56. Fcl_MTH_SkinFung_R
// 57. Fcl_MTH_SkinFung_L
// 58. Fcl_MTH_A
// 59. Fcl_MTH_I
// 60. Fcl_MTH_U
// 61. Fcl_MTH_E
// 62. Fcl_MTH_O
// 63. Fcl_MTH_Tongue_Out
// 64. Fcl_HA_Hide
// 65. Fcl_HA_Short
// 66. Fcl_HA_Short_Up
// 67. Hide_Body_Under_Clothes_TOP
// 68. Hide_Body_Under_Clothes_BOTTOM


   
// mediapipe blendshape数据：

    // 0. _neutral  
    // 1. browDownLeft  
    // 2. browDownRight  
    // 3. browInnerUp  
    // 4. browOuterUpLeft  
    // 5. browOuterUpRight  
    // 6. cheekPuff  
    // 7. cheekSquintLeft  
    // 8. cheekSquintRight  
    // 9. eyeBlinkLeft  
    // 10. eyeBlinkRight  
    // 11. eyeLookDownLeft  
    // 12. eyeLookDownRight  
    // 13. eyeLookInLeft  
    // 14. eyeLookInRight  
    // 15. eyeLookOutLeft  
    // 16. eyeLookOutRight  
    // 17. eyeLookUpLeft  
    // 18. eyeLookUpRight  
    // 19. eyeSquintLeft  
    // 20. eyeSquintRight  
    // 21. eyeWideLeft  
    // 22. eyeWideRight  
    // 23. jawForward  
    // 24. jawLeft  
    // 25. jawOpen  
    // 26. jawRight  
    // 27. mouthClose  
    // 28. mouthDimpleLeft  
    // 29. mouthDimpleRight  
    // 30. mouthFrownLeft  
    // 31. mouthFrownRight  
    // 32. mouthFunnel  
    // 33. mouthLeft  
    // 34. mouthLowerDownLeft  
    // 35. mouthLowerDownRight  
    // 36. mouthPressLeft  
    // 37. mouthPressRight  
    // 38. mouthPucker  
    // 39. mouthRight  
    // 40. mouthRollLower  
    // 41. mouthRollUpper  
    // 42. mouthShrugLower  
    // 43. mouthShrugUpper  
    // 44. mouthSmileLeft  
    // 45. mouthSmileRight  
    // 46. mouthStretchLeft  
    // 47. mouthStretchRight  
    // 48. mouthUpperUpLeft  
    // 49. mouthUpperUpRight  
    // 50. noseSneerLeft  
    // 51. noseSneerRight
    
    // mediapipe blendshape数据与模型BlendShape名称的对应关系：

    
    private void UpdateFaceBlendshapes()
    {
       
    
        // 检查SkinnedMeshRenderer是否存在
        if (blendshapes == null || blendshapes.sharedMesh == null)
        {
            Debug.Log("SkinnedMeshRenderer 或 Mesh 丢失！");
            return;
        }
        
        
        // 更新脸部blendshape
        // 使用扩大因子
        // 更新脸部blendshape（每个score值乘上exFactor）
        blendshapes.SetBlendShapeWeight(19, facebs.face_blendshape[0].score * exFactor);  // Fcl_ALL_Neutral
        blendshapes.SetBlendShapeWeight(29, facebs.face_blendshape[3].score * exFactor);  // brow上挑
        
        
        blendshapes.SetBlendShapeWeight(25, facebs.face_blendshape[1].score * exFactor+facebs.face_blendshape[2].score * exFactor);  // brow下沉
       
       
        blendshapes.SetBlendShapeWeight(0, facebs.face_blendshape[9].score * 150f);  // vrc.blink_left
        blendshapes.SetBlendShapeWeight(1, facebs.face_blendshape[10].score * 150f);  // vrc.blink_right
        
        blendshapes.SetBlendShapeWeight(40,facebs.face_blendshape[17].score*120f+facebs.face_blendshape[18].score*120f);//look_up=LookUpRight +LookUpLeft
        blendshapes.SetBlendShapeWeight(41,facebs.face_blendshape[11].score*120f+facebs.face_blendshape[12].score*120f);//Look_down=LookdownRight +LookdownLeft
        blendshapes.SetBlendShapeWeight(42,facebs.face_blendshape[11].score*120f+facebs.face_blendshape[17].score*120f);//loolleft=LookUpLeft+LookdownLeft
        blendshapes.SetBlendShapeWeight(43,facebs.face_blendshape[12].score*120f+facebs.face_blendshape[18].score*120f);//lookright=LookdownRight+LookUpRight
        
      
        blendshapes.SetBlendShapeWeight(37, facebs.face_blendshape[21].score * 400f+facebs.face_blendshape[22].score * 400f);  // Fcl_EYE_Spread
        
        blendshapes.SetBlendShapeWeight(44, facebs.face_blendshape[42].score * 200f);  // Fcl_MTH_Close(闭嘴)
        blendshapes.SetBlendShapeWeight(50, facebs.face_blendshape[42].score * 200f);  // Fcl_MTH_Close(闭嘴)
        
        
        blendshapes.SetBlendShapeWeight(51, facebs.face_blendshape[44].score * 120f+facebs.face_blendshape[45].score * 120f);  // Fcl_MTH_Fun（闭嘴笑）
        blendshapes.SetBlendShapeWeight(52, facebs.face_blendshape[48].score * exFactor+facebs.face_blendshape[49].score * exFactor);//张嘴笑
        
        blendshapes.SetBlendShapeWeight(62, facebs.face_blendshape[38].score * 50f);//O形嘴巴
        blendshapes.SetBlendShapeWeight(53, facebs.face_blendshape[42].score * 150f);  // sorrow
       
      
        blendshapes.SetBlendShapeWeight(47, facebs.face_blendshape[25].score * exFactor*0.7f);
        
       
        
    }

        private void UpdateFaceBlendshapes_lerp()
{
    // 检查SkinnedMeshRenderer是否存在
    if (blendshapes == null || blendshapes.sharedMesh == null)
    {
        Debug.Log("SkinnedMeshRenderer 或 Mesh 丢失！");
        return;
    }

    

    // 更新脸部blendshape，使用插值平滑过渡
    float targetWeight;

    // Fcl_ALL_Neutral
    targetWeight = facebs.face_blendshape[0].score * exFactor;
    blendshapes.SetBlendShapeWeight(19, Mathf.Lerp(blendshapes.GetBlendShapeWeight(19), targetWeight, Time.deltaTime * lerpSpeed));

    // brow上挑
    targetWeight = facebs.face_blendshape[3].score * exFactor;
    blendshapes.SetBlendShapeWeight(29, Mathf.Lerp(blendshapes.GetBlendShapeWeight(29), targetWeight, Time.deltaTime * lerpSpeed));

    // brow下沉
    targetWeight = (facebs.face_blendshape[1].score + facebs.face_blendshape[2].score) * exFactor;
    blendshapes.SetBlendShapeWeight(25, Mathf.Lerp(blendshapes.GetBlendShapeWeight(25), targetWeight, Time.deltaTime * lerpSpeed));
    //眨眼
    blendshapes.SetBlendShapeWeight(0, facebs.face_blendshape[9].score * 150f);  // vrc.blink_left
    blendshapes.SetBlendShapeWeight(1, facebs.face_blendshape[10].score * 150f);  // vrc.blink_right
    //眼球
    blendshapes.SetBlendShapeWeight(40,facebs.face_blendshape[17].score*120f+facebs.face_blendshape[18].score*120f);//look_up=LookUpRight +LookUpLeft
    blendshapes.SetBlendShapeWeight(41,facebs.face_blendshape[11].score*120f+facebs.face_blendshape[12].score*120f);//Look_down=LookdownRight +LookdownLeft
    blendshapes.SetBlendShapeWeight(42,facebs.face_blendshape[11].score*120f+facebs.face_blendshape[17].score*120f);//loolleft=LookUpLeft+LookdownLeft
    blendshapes.SetBlendShapeWeight(43,facebs.face_blendshape[12].score*120f+facebs.face_blendshape[18].score*120f);//lookright=LookdownRight+LookUpRight

    // Fcl_EYE_Spread
    targetWeight = (facebs.face_blendshape[21].score * 400f + facebs.face_blendshape[22].score * 400f);
    blendshapes.SetBlendShapeWeight(37, Mathf.Lerp(blendshapes.GetBlendShapeWeight(37), targetWeight, Time.deltaTime * lerpSpeed));

    // Fcl_MTH_Close (闭嘴)
    targetWeight = facebs.face_blendshape[42].score * 200f;
    blendshapes.SetBlendShapeWeight(44, Mathf.Lerp(blendshapes.GetBlendShapeWeight(44), targetWeight, Time.deltaTime * lerpSpeed));
    blendshapes.SetBlendShapeWeight(50, Mathf.Lerp(blendshapes.GetBlendShapeWeight(50), targetWeight, Time.deltaTime * lerpSpeed));

    // Fcl_MTH_Fun (闭嘴笑)
    targetWeight = (facebs.face_blendshape[44].score * 120f + facebs.face_blendshape[45].score * 120f);
    blendshapes.SetBlendShapeWeight(51, Mathf.Lerp(blendshapes.GetBlendShapeWeight(51), targetWeight, Time.deltaTime * lerpSpeed));

    // 张嘴笑
    targetWeight = (facebs.face_blendshape[48].score * exFactor + facebs.face_blendshape[49].score * exFactor);
    blendshapes.SetBlendShapeWeight(52, Mathf.Lerp(blendshapes.GetBlendShapeWeight(52), targetWeight, Time.deltaTime * lerpSpeed));

    // O形嘴巴
    targetWeight = facebs.face_blendshape[38].score * 50f;
    blendshapes.SetBlendShapeWeight(62, Mathf.Lerp(blendshapes.GetBlendShapeWeight(62), targetWeight, Time.deltaTime * lerpSpeed));

    // sorrow
    targetWeight = facebs.face_blendshape[42].score * 150f;
    blendshapes.SetBlendShapeWeight(53, Mathf.Lerp(blendshapes.GetBlendShapeWeight(53), targetWeight, Time.deltaTime * lerpSpeed));

    // 张嘴部blendshape
    targetWeight = facebs.face_blendshape[25].score * exFactor * 0.7f;
    blendshapes.SetBlendShapeWeight(47, Mathf.Lerp(blendshapes.GetBlendShapeWeight(47), targetWeight, Time.deltaTime * lerpSpeed));
}



            
        
    


    // 获取BlendShape信息的函数
    public void GetBlendShapes(SkinnedMeshRenderer skinnedMeshRenderer)
    {
        if (skinnedMeshRenderer == null)
        {
            Debug.LogError("SkinnedMeshRenderer为空，无法获取BlendShape信息！");
            return;
        }

        Mesh mesh = skinnedMeshRenderer.sharedMesh;

        // 获取模型中所有的BlendShape
        int blendShapeCount = mesh.blendShapeCount;
        Debug.Log($"共有 {blendShapeCount} 个BlendShape:");

        for (int i = 0; i < blendShapeCount; i++)
        {
            string blendShapeName = mesh.GetBlendShapeName(i);
            float blendShapeWeight = skinnedMeshRenderer.GetBlendShapeWeight(i);
            Debug.Log($"BlendShape {i}: {blendShapeName}, 初始权重: {blendShapeWeight}");
        }
    }
    void Start()
    {
        // // 获取所有BlendShape信息
        // if ( blendshapes != null)
        // {
        // GetBlendShapes( blendshapes);
       
        // }
        // else
        // {
        //     Debug.LogError("请在Unity编辑器中将SkinnedMeshRenderer赋值给该脚本！");
        // }
        BoneBinding();
        
        

    }

    

    // Update is called once per frame
    void Update()
    {
        // Time.timeScale = 1;
        //更新头部旋转
        UpdateHeadRotation();
        if (usinglerp)
        {
            UpdateFaceBlendshapes_lerp();//更新脸部blendshape(表情)
        }
        else
        {
            UpdateFaceBlendshapes();//更新脸部blendshape(表情)
        }
        
        
    }
}
