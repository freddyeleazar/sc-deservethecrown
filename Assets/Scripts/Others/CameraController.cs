using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public List<CinemachineVirtualCamera> cameras;
    public Transform player;
    
    private void Awake()
    {
        player = FindObjectOfType<PlayerMovementController>().gameObject.transform;
        cameras = GetComponentsInChildren<CinemachineVirtualCamera>().ToList();
        cameras.ForEach(t => { t.Follow = player; t.LookAt = player; });
    }
}
