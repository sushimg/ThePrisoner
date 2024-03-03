using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    public GameObject text;
    public Image notebook;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<character>() != null)
        {
            text.SetActive(true);

            if (Input.GetKey(KeyCode.R))
            {
                notebook.gameObject.SetActive(true);
            }
            else if (Input.GetKeyUp(KeyCode.R))
            {
                notebook.gameObject.SetActive(false);
            }
        }

    }
    void OnTriggerExit(Collider other)
    {
        notebook.gameObject.SetActive(false);

        text.SetActive(false);
    }
}
