




# **Mathematical Formulas Used in the Code:**

---

### **1. Formula for Initializing the Intermediate Rotation Matrix**
Used to align the initial rotations of model joints with the input data.

#### **Calculation of Intermediate Rotation Matrix**
For each joint, the intermediate matrix for the initial rotation is represented as:
$$
Q_{\text{mid}} = Q_{\text{init}}^{-1} \cdot Q_{\text{target}}
$$
Where:
- $ Q_{\text{init}} $: The quaternion representing the initial rotation of the joint.
- $ Q_{\text{target}} $: The target direction quaternion calculated based on the joint position.

---

### **2. Joint Update Formula**

The rotation of each joint per frame is determined by the following formula:
$$
Q_{\text{current}} = Q_{\text{look}} \cdot Q_{\text{mid}}^{-1}
$$
Where:
- $ Q_{\text{look}} = \text{Quaternion.LookRotation}(\mathbf{forward}, \mathbf{up}) $: Used to calculate the quaternion for the target direction.
- $ Q_{\text{mid}} $: The intermediate matrix used to align the joint's coordinate system.

#### **Target Direction Calculation**
The target direction $ \mathbf{forward} $ is typically calculated based on the difference in positions of the joints, for example:
$$
\mathbf{forward} = \mathbf{p}_{\text{end}} - \mathbf{p}_{\text{start}}
$$
- $ \mathbf{p}_{\text{start}} $: The position of the starting joint.
- $ \mathbf{p}_{\text{end}} $: The position of the end joint.

#### **Custom Reference Direction**
For some joints (e.g., hands or feet), an additional normal vector calculation is used to determine the reference direction $ \mathbf{up} $:
$$
\mathbf{up} = \text{TriangleNormal}(\mathbf{p}_1, \mathbf{p}_2, \mathbf{p}_3)
$$
Where:
- $ \text{TriangleNormal} $: The normal vector of a triangle, calculated as:
  $$
  \mathbf{n} = \frac{(\mathbf{p}_1 - \mathbf{p}_2) \times (\mathbf{p}_1 - \mathbf{p}_3)}{\|(\mathbf{p}_1 - \mathbf{p}_2) \times (\mathbf{p}_1 - \mathbf{p}_3)\|}
  $$

---

### **3. Root Node Position Update Formula**

The root node position (`root.position`) is adjusted based on the input 3D posture height and model height:
$$
\mathbf{p}_{\text{root}} = \mathbf{p}_{\text{hip}} \cdot \left( \frac{h_{\text{model}}}{h_{\text{input}}} \right)
$$
Where:
- $ \mathbf{p}_{\text{hip}} $: The hip position in the input data.
- $ h_{\text{model}} $: The total leg height of the model, calculated as:
  $$
  h_{\text{model}} = \frac{\|\mathbf{p}_{\text{lhip}} - \mathbf{p}_{\text{lknee}}\| + \|\mathbf{p}_{\text{lknee}} - \mathbf{p}_{\text{lfoot}}\|}{2} + \frac{\|\mathbf{p}_{\text{rhip}} - \mathbf{p}_{\text{rknee}}\| + \|\mathbf{p}_{\text{rknee}} - \mathbf{p}_{\text{rfoot}}\|}{2}
  $$
- $ h_{\text{input}} $: The total leg height in the input data, calculated similarly.

---

### **4. Joint Position Calculation Formula**

The positions of certain joints are derived from other joints, for example:
- Neck position:
  $$
  \mathbf{p}_{\text{neck}} = \frac{\mathbf{p}_{\text{lshoulder}} + \mathbf{p}_{\text{rshoulder}}}{2}
  $$
- Hip position:
  $$
  \mathbf{p}_{\text{hips}} = \frac{\mathbf{p}_{\text{lhip}} + \mathbf{p}_{\text{rhip}}}{2}
  $$
- Upper abdomen position:
  $$
  \mathbf{p}_{\text{abdomen}} = \frac{\mathbf{p}_{\text{hips}} + \mathbf{p}_{\text{neck}}}{2}
  $$

---

### **5. Normal Vector and Orientation**

#### **Normal Vector Calculation**
Normal vectors are used to ensure stable reference directions for joint rotations, for example:
$$
\mathbf{n} = \frac{(\mathbf{p}_1 - \mathbf{p}_2) \times (\mathbf{p}_1 - \mathbf{p}_3)}{\|(\mathbf{p}_1 - \mathbf{p}_2) \times (\mathbf{p}_1 - \mathbf{p}_3)\|}
$$

#### **Joint Orientation Calculation**
For joints like the hands or feet, the rotation is determined by both the target direction and the normal vector:
$$
Q_{\text{current}} = \text{Quaternion.LookRotation}(\mathbf{forward}, \mathbf{up}) \cdot Q_{\text{mid}}^{-1}
$$

---

### **Core Summary**

1. **Rotation Formula**:
   $$
   Q_{\text{current}} = Q_{\text{look}} \cdot Q_{\text{mid}}^{-1}
   $$

2. **Direction and Reference Calculation**:
    - $ \mathbf{forward} = \mathbf{p}_{\text{end}} - \mathbf{p}_{\text{start}} $
    - $ \mathbf{up} = \text{TriangleNormal}(\mathbf{p}_1, \mathbf{p}_2, \mathbf{p}_3) $

3. **Root Position Adjustment**:
   $$
   \mathbf{p}_{\text{root}} = \mathbf{p}_{\text{hip}} \cdot \left( \frac{h_{\text{model}}}{h_{\text{input}}} \right)
   $$

--- 

Let me know if you'd like further clarification!

# **代码中使用的数学公式：**



---

### **1. 初始化中间旋转矩阵公式**
用于对齐模型关节的初始旋转和输入数据。

#### **计算中间旋转矩阵**
对于每个关节，其初始旋转的中间矩阵表示为：
$$
Q_{\text{mid}} = Q_{\text{init}}^{-1} \cdot Q_{\text{target}}
$$
其中：
- $ Q_{\text{init}}$: 初始状态下关节的旋转四元数。
- $ Q_{\text{target}} $: 基于关节位置计算的目标方向四元数。

---

### **2. 关节更新公式**

每帧中关节的旋转由以下公式决定：
$$
Q_{\text{current}} = Q_{\text{look}} \cdot Q_{\text{mid}}^{-1}
$$
其中：
- $ Q_{\text{look}} = \text{Quaternion.LookRotation}(\mathbf{forward}, \mathbf{up}) $: 用于计算目标方向的四元数。
- $ Q_{\text{mid}} $: 中间矩阵，用于对齐关节坐标系。

#### **目标方向计算**
目标方向 $ \mathbf{forward} $ 通常由关节的位置差计算，例如：
$$
\mathbf{forward} = \mathbf{p}_{\text{end}} - \mathbf{p}_{\text{start}}
$$
- $ \mathbf{p}_{\text{start}} $: 起始关节位置。
- $  \mathbf{p}_{\text{end}} $: 末端关节位置。

#### **自定义参考方向**
对于某些关节（如手或脚），需要额外的法向量计算参考方向 $ \mathbf{up} $：
$$
\mathbf{up} = \text{TriangleNormal}(\mathbf{p}_1, \mathbf{p}_2, \mathbf{p}_3)
$$
其中：
- $ \text{TriangleNormal} $: 三角形法向量计算，公式为：
  $$
  \mathbf{n} = \frac{(\mathbf{p}_1 - \mathbf{p}_2) \times (\mathbf{p}_1 - \mathbf{p}_3)}{\|(\mathbf{p}_1 - \mathbf{p}_2) \times (\mathbf{p}_1 - \mathbf{p}_3)\|}
  $$

---

### **3. 根节点位置更新公式**

根节点位置（`root.position`）基于输入的 3D 姿态高度和模型高度调整：
$$
\mathbf{p}_{\text{root}} = \mathbf{p}_{\text{hip}} \cdot \left( \frac{h_{\text{model}}}{h_{\text{input}}} \right)
$$
其中：
- $ \mathbf{p}_{\text{hip}} $: 输入数据中髋部位置。
- $ h_{\text{model}} $: 模型的腿部总高度，计算为：
  $$
  h_{\text{model}} = \frac{\|\mathbf{p}_{\text{lhip}} - \mathbf{p}_{\text{lknee}}\| + \|\mathbf{p}_{\text{lknee}} - \mathbf{p}_{\text{lfoot}}\|}{2} + \frac{\|\mathbf{p}_{\text{rhip}} - \mathbf{p}_{\text{rknee}}\| + \|\mathbf{p}_{\text{rknee}} - \mathbf{p}_{\text{rfoot}}\|}{2}
  $$
- $ h_{\text{input}} $: 输入数据中腿部总高度，计算方式类似。

---

### **4. 关节位置计算公式**

部分关节位置由其他关节推导，例如：
- 脖子位置：
  $$
  \mathbf{p}_{\text{neck}} = \frac{\mathbf{p}_{\text{lshoulder}} + \mathbf{p}_{\text{rshoulder}}}{2}
  $$
- 臀部位置：
  $$
  \mathbf{p}_{\text{hips}} = \frac{\mathbf{p}_{\text{lhip}} + \mathbf{p}_{\text{rhip}}}{2}
  $$
- 腹部上部位置：
  $$
  \mathbf{p}_{\text{abdomen}} = \frac{\mathbf{p}_{\text{hips}} + \mathbf{p}_{\text{neck}}}{2}
  $$

---

### **5. 法向量与朝向**

#### **法向量计算**
法向量用于确保关节的旋转有稳定的参考方向，例如：
$$
\mathbf{n} = \frac{(\mathbf{p}_1 - \mathbf{p}_2) \times (\mathbf{p}_1 - \mathbf{p}_3)}{\|(\mathbf{p}_1 - \mathbf{p}_2) \times (\mathbf{p}_1 - \mathbf{p}_3)\|}
$$

#### **关节朝向计算**
对于手或脚等关节，其旋转由目标方向和法向量共同决定：
$$
Q_{\text{current}} = \text{Quaternion.LookRotation}(\mathbf{forward}, \mathbf{up}) \cdot Q_{\text{mid}}^{-1}
$$

---

### **核心总结**

1. **旋转公式**:
   $$
   Q_{\text{current}} = Q_{\text{look}} \cdot Q_{\text{mid}}^{-1}
   $$

2. **方向与参考计算**:
    - $ \mathbf{forward} = \mathbf{p}_{\text{end}} - \mathbf{p}_{\text{start}} $
    - $ \mathbf{up} = \text{TriangleNormal}(\mathbf{p}_1, \mathbf{p}_2, \mathbf{p}_3) $

3. **根位置调整**:
   $$
   \mathbf{p}_{\text{root}} = \mathbf{p}_{\text{hip}} \cdot \left( \frac{h_{\text{model}}}{h_{\text{input}}} \right)
   $$

