using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform to;

    public Animation panel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay(Collision collision)
    {
        collision.gameObject.transform.position = to.position;
    }
}
