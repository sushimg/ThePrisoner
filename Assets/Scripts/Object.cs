using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class objectScript : MonoBehaviour
{
    public string objectName;


    public static bool isHold = false;

    private void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<character>() != null)
            if (Input.GetKeyDown(KeyCode.E) && !other.GetComponent<character>().isHolding && !isHold)
            {
                other.GetComponent<character>().Hold(objectName);
                isHold = true;
                Destroy(transform.parent.gameObject);
            }
    }
}
