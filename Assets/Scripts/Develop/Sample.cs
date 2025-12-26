using SymphonyFrameWork.System;
using UnityEngine;

public class Sample : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform pos = ServiceLocator.GetInstance<Transform>();
        Debug.Log(pos.name);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
