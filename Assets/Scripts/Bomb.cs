using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class bomb : MonoBehaviour
{
    public Camera cam;

    CameraShake camShake;

    public float force;
    public float damage;
    public float radius;


    public AudioSource explosionSound;
    public ParticleSystem explosion;

    void Start()
    {
        camShake = cam.GetComponent<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Triggered()
    { 
        yield return new WaitForSeconds(1.5f);

        ParticleSystem exp;
        AudioSource expSFX;
        exp = Instantiate(explosion, transform.position, transform.rotation);
        expSFX = Instantiate(explosionSound, transform.position, transform.rotation);
        expSFX.clip = explosionSound.clip;
        exp.Play();
        areaOfEffect();
        expSFX.enabled = true;
        Destroy(gameObject);
        camShake.ShakeCamera();
        Destroy(exp.gameObject, 2f);
        Destroy(expSFX.gameObject, 5f);
    }

    void areaOfEffect()
    {
        // Find all colliders in the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider obj in colliders)
        {
            if (obj.TryGetComponent(out objectHealth healthScript))
                healthScript.health -= damage;

            // Check if the object has a rigidbody
            if (obj.TryGetComponent(out Rigidbody rb))
                rb.AddExplosionForce(force, transform.position, radius);

        }
    }
}
