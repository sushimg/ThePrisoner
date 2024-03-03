using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class comicScene : MonoBehaviour
{
    public GameObject comic1;
    public GameObject comic2;
    public GameObject text;
    public static int counter = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && counter == 0)
        {
            comic1.SetActive(false);
            comic2.SetActive(true);
            StartCoroutine(addCounter());
        }
        else if (Input.GetKeyDown(KeyCode.Space) && counter == 1)
        {
            comic2.SetActive(false);
            text.SetActive(false);
            StartCoroutine(addCounter());
        }

    }

    IEnumerator addCounter()
    {
        yield return new WaitForSeconds(0.5f);
        counter++;
    }
}
