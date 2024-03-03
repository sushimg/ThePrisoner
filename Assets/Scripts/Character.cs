using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class character : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;

    [Header("Movement")]
    public float characterSpeed;
    public float maxCharacterSpeed;

    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity = 2;

    public Camera cam;
    public float camChaseSpeed;

    public bool isWalking;

    [Header("Audio")]
    public AudioSource footsteps;

    [Header("Holding/Carring")]
    public bool isHolding = false;
    public string carryingObjectName = "";

    //Throwing
    float elapsedTime = 0f;
    float maxPressTime = 1f;

    float throwingPower;
    float maxThrowingPower = 750;

    [Header("Objects")]
    public GameObject bomb;
    public GameObject wood;
    public GameObject barrel;
    public GameObject nuclear;

    [Header("Prefabs")]
    public GameObject bombPrefab;
    public GameObject woodPrefab;
    public GameObject barrelPrefab;
    public GameObject nuclearPrefab;

    // Start is called before the first frame update
    void Start()
    {
        characterSpeed = maxCharacterSpeed;

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        float x, y;

        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(y, 0, -x).normalized;
        rb.transform.position += movement * characterSpeed;

        Vector3 direction = new Vector3(Input.GetAxisRaw("Vertical"), 0f, -Input.GetAxisRaw("Horizontal")).normalized;
        if (direction.magnitude >= 0.01f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            isWalking = true;
            footsteps.enabled = true;
        }
        else
        {
            isWalking = false;
            footsteps.enabled = false;
        }

        //Throwing Objects
        if (Input.GetKey(KeyCode.E) && isHolding)
        {
            if (elapsedTime < maxPressTime)
                elapsedTime += Time.deltaTime;

            throwingPower = Mathf.Lerp(0, maxThrowingPower, elapsedTime / maxPressTime);
        }
        else if (Input.GetKeyUp(KeyCode.E) && isHolding)
            Throw(carryingObjectName);
        else if (!isHolding)
            elapsedTime = 0;

        //Animator
        if (isWalking)
            anim.SetBool("isWalking", true);
        else
            anim.SetBool("isWalking", false);

        //isHolding Anim
        if (isHolding)
            anim.SetBool("isHolding", true);
        else
            anim.SetBool("isHolding", false);


        //Camera Follow
        float realCamPosY = cam.transform.position.y;
        float realCamPosZ = cam.transform.position.z;
        float targetedCamPosZ;
        float targetedCamPosY = transform.position.y + 15;
        if (Mathf.Abs(transform.position.z) < 210)
            targetedCamPosZ = transform.position.z;
        else
        {
            if (transform.position.z < 0)
                targetedCamPosZ = -210;
            else
                targetedCamPosZ = 210;
        }

        cam.transform.position = new Vector3(cam.transform.position.x, Mathf.Lerp(realCamPosY, targetedCamPosY, camChaseSpeed * Time.deltaTime), Mathf.Lerp(realCamPosZ, targetedCamPosZ, camChaseSpeed * Time.deltaTime));
    }

    GameObject holdingBomb;
    GameObject holdingWood;
    GameObject holdingBarrel;
    GameObject holdingNuclear;

    public void Hold(string objectName)
    {
        switch (objectName)
        {
            case "bomb":
                holdingBomb = Instantiate(bombPrefab, transform.position, transform.rotation);
                holdingBomb.transform.position += transform.forward + (3.5f * transform.up); 
                holdingBomb.transform.parent = transform;
                carryingObjectName = "bomb";
                break;
            case "wood":
                holdingWood = Instantiate(woodPrefab, transform.position, transform.rotation);
                holdingWood.transform.position += transform.forward + (3.5f * transform.up);
                holdingWood.transform.parent = transform;
                carryingObjectName = "wood";
                break;
            case "barrel":
                holdingBarrel = Instantiate(barrelPrefab, transform.position, transform.rotation);
                holdingBarrel.transform.position += transform.forward + (3.5f * transform.up);
                holdingBarrel.transform.parent = transform;
                carryingObjectName = "barrel";
                break;
            case "nuclear":
                holdingNuclear = Instantiate(nuclearPrefab, transform.position, transform.rotation);
                holdingNuclear.transform.position += transform.forward + (3.5f * transform.up);
                holdingNuclear.transform.parent = transform;
                carryingObjectName = "nuclear";
                break;
        }

        StartCoroutine(holdingStarted());
    }

    IEnumerator holdingStarted()
    {
        yield return new WaitForSeconds(0.3f);
        isHolding = true;
    }

    Quaternion rot = Quaternion.Euler(-45, 0, 0); //throwing angle

    void Throw(string objects)
    {
        switch (objects)
        {
            case "bomb":
                GameObject throwingBomb;
                throwingBomb = Instantiate(bomb, transform.position + transform.forward + (3.5f * transform.up), transform.rotation);
                carryingObjectName = "";
                throwingBomb.GetComponent<Rigidbody>().AddForce(rot * transform.forward * throwingPower, ForceMode.Acceleration);
                objectScript.isHold = false; 
                StartCoroutine(throwingBomb.GetComponent<bomb>().Triggered());
                Destroy(holdingBomb);
                break;
                case "wood":
                GameObject throwingWood;
                throwingWood = Instantiate(wood, transform.position + transform.forward + (3.5f * transform.up), transform.rotation);
                carryingObjectName = "";
                throwingWood.GetComponent<Rigidbody>().AddForce(rot * transform.forward * throwingPower, ForceMode.Acceleration);
                objectScript.isHold = false;
                Destroy(holdingWood);
                break;
                case "barrel":
                GameObject throwingBarrel;
                throwingBarrel = Instantiate(barrel, transform.position + transform.forward + (3.5f * transform.up), transform.rotation);
                carryingObjectName = "";
                throwingBarrel.GetComponent<Rigidbody>().AddForce(rot * transform.forward * throwingPower, ForceMode.Acceleration);
                StartCoroutine(throwingBarrel.GetComponent<bomb>().Triggered());
                objectScript.isHold = false;
                Destroy(holdingBarrel);
                break;
                case "nuclear":
                GameObject throwingNuclear;
                throwingNuclear = Instantiate(nuclear, transform.position + transform.forward + (3.5f * transform.up), transform.rotation);
                carryingObjectName = "";
                throwingNuclear.GetComponent<Rigidbody>().AddForce(rot * transform.forward * throwingPower, ForceMode.Acceleration);
                StartCoroutine(throwingNuclear.GetComponent<nuclear>().Triggered());
                objectScript.isHold = false;
                Destroy(holdingNuclear);
                break;
        }

        StartCoroutine(holdingEnded());
    }
    IEnumerator holdingEnded()
    {
        yield return new WaitForSeconds(0.3f);
        isHolding = false;
    } 
}
