using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class objectHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public ParticleSystem destroyEffect;
    public GameObject[] fragments;

    bool x = false;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Mathf.Clamp(health, 0, maxHealth);

        if (health <= 0 && !x)
        {
            Fragmentation();
            x = true;
        }

    }

    void Fragmentation()
    {
        ParticleSystem destroyClone;
        destroyClone = Instantiate(destroyEffect, transform.position, transform.rotation);
        destroyClone.Play();

        GameObject[] parts = new GameObject[fragments.Length];

        for (int i = 0; i < fragments.Length; i++)
        {
            parts[i] = Instantiate(fragments[i], transform.position, transform.rotation);
            parts[i].AddComponent<Rigidbody>();
            parts[i].GetComponent<Rigidbody>().mass = 0.1f;
            parts[i].AddComponent<BoxCollider>();

            Destroy(parts[i], 35f);
        }

        Destroy(gameObject);
        Destroy(destroyClone.gameObject, 10f);
    }
}
