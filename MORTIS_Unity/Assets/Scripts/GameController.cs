using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
  
    public void RefreshGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 重新加载当前场景
    }
    private bool isPaused = false;

  
    public void Pause()
    {
        if (isPaused)
        {
            Time.timeScale = 1f; // 恢复游戏
        }
        else
        {
            Time.timeScale = 0f; // 暂停游戏
        }
        isPaused = !isPaused;
    }
    
    public SkinnedMeshRenderer skinnedMeshRenderer;  // 需要控制的 SkinnedMeshRenderer
    public int tongueBlendShapeIndex = 63;  
    public float blendSpeed = 5f;  // 舌头变化的速度

    private float currentBlendShapeValue = 0f;
    private bool isSpacePressed = false;

    void Update()
    {
        // 检测空格键是否按下
        if (Input.GetKey(KeyCode.Space) && !isSpacePressed)
        {
            // 空格键刚按下时开始增加 BlendShape
            StartCoroutine(ChangeBlendShape(100f));  
            isSpacePressed = true;
        }
        else if (!Input.GetKey(KeyCode.Space) && isSpacePressed)
        {
            // 空格键松开时开始减少 BlendShape
            StartCoroutine(ChangeBlendShape(0f));  
            isSpacePressed = false;
        }
    }

    // 协程用来平滑过渡 BlendShape 的值
    private IEnumerator ChangeBlendShape(float targetValue)
    {
        float startValue = currentBlendShapeValue;
        float timeElapsed = 0f;

        // 逐渐改变 BlendShape 值直到达到目标值
        while (timeElapsed < 1f)
        {
            timeElapsed += Time.deltaTime * blendSpeed;
            currentBlendShapeValue = Mathf.Lerp(startValue, targetValue, timeElapsed);
            skinnedMeshRenderer.SetBlendShapeWeight(tongueBlendShapeIndex, currentBlendShapeValue);
            yield return null;
        }

        // 确保最终值是精确的
        currentBlendShapeValue = targetValue;
        skinnedMeshRenderer.SetBlendShapeWeight(tongueBlendShapeIndex, currentBlendShapeValue);
    }
}
