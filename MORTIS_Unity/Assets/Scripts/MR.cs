using System.Collections;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;

public class MR : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PXR_Manager.EnableVideoSeeThrough = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
