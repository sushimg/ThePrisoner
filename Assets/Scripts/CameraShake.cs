using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeIntensity = 0.1f;
    public float shakeDuration = 0.5f;

    private Vector3 originalPosition;

    public void ShakeCamera()
    {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        originalPosition = transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            // Generate a random offset for camera position
            Vector3 offset = new Vector3(Random.Range(-shakeIntensity, shakeIntensity),
                                         Random.Range(-shakeIntensity, shakeIntensity),
                                         0f);

            // Apply the offset to the camera position
            transform.position = originalPosition + offset;

            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Reset camera position to the original position after the shake
        transform.position = originalPosition;
    }
}