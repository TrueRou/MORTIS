using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//待开发，未被用到

public class BoneNode 
{
    public Transform transform;
    public int startJoint; //开始关节点
    public int endJoint; //结束关节点
    public Quaternion rotation;
    public List<BoneNode> childboneNodes = new List<BoneNode>();

    
    // 初始化方法
    public void Initialize(Transform transform, int startJoint, int endJoint)
    {
        this.transform = transform;
        this.startJoint = startJoint;
        this.endJoint = endJoint;
        this.rotation = Quaternion.identity;
    }
    // 添加子骨骼节点
    public void AddChild(BoneNode childNode)
    {
        childboneNodes.Add(childNode);
    }

    public virtual Vector3 getDirection(Vector3[] jointPositions,int startjoint, int endjoint)
    {
        // 计算从开始关节点到结束关节点的方向向量
        // 获取起点和终点的关节点位置
        Vector3 startPosition = jointPositions[startJoint];
        Vector3 endPosition = jointPositions[endJoint];
        Vector3 direction = endPosition - startPosition;
        return direction;
    }

    // 计算骨骼的位置和旋转
    public  Quaternion CalculateBoneRotation(Vector3[] jointPositions, Quaternion parentRotation)
    {
        // 计算从开始关节点到结束关节点的方向向量
        // 获取起点和终点的关节点位置
        //     
        Vector3 direction = getDirection(jointPositions, startJoint, endJoint);
        if (direction != Vector3.zero)
        {
            // 根据方向向量和骨骼的横向向量计算旋转
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, direction);
            return Quaternion.Slerp(rotation, parentRotation * targetRotation, 0.5f);
        }
        else
        {
            return parentRotation;
        }
    }

    // 更新骨骼的位置和旋转
    public void UpdateBone(Vector3[] jointPositions, Quaternion parentRotation)
    {
        rotation = CalculateBoneRotation(jointPositions, parentRotation);
        transform.rotation = rotation;

        if (childboneNodes.Count > 0)
        {
            // 更新每个子节点
            foreach (BoneNode boneNode in childboneNodes)
            {
                boneNode.UpdateBone(jointPositions, this.rotation);
            }
        }
        else
        {
            Debug.LogWarning($"endjoint[{endJoint}]has No childbone nodes found");
        }
    }
    // 从根节点开始更新整个骨骼树
    public  void UpdateFromRoot( Vector3[] jointPositions)
    {
        if (this != null)
        {
            this.UpdateBone(jointPositions, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No RootBone");
        }
    }
    
}
