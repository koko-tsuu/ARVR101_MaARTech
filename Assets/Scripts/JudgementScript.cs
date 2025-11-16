using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class JudgementScript : MonoBehaviour
{

    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private Light light;
    // Start is called before the first frame update
    [SerializeField] private GameObject loofahPrefab;
    private GameObject loofah;
    void Start()
    {
        particleSystem.Pause();
        loofah = Instantiate(loofahPrefab, ImageTracker.instance.originalPos.position + new UnityEngine.Vector3(0, 0.2f, 0), UnityEngine.Quaternion.identity);

        loofah.GetComponent<SphereCollider>().isTrigger = true;
        loofah.GetComponent<Rigidbody>().useGravity = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "loofah")
        {
            Debug.Log("Loofah went in");
            StartCoroutine(FadeLightRoutine());
            StartCoroutine(PlayThenPauseParticleSystem(3f));
            
        }
    }

    void OnDestroy()
    {
        Destroy(loofah);
    }

    IEnumerator FadeLightRoutine()
    {
        // Fade 0 → 3
        yield return StartCoroutine(FadeIntensity(0f, 3f, 2f));

        // Fade 3 → 0
        yield return StartCoroutine(FadeIntensity(3f, 0f, 2f));
    }

    IEnumerator FadeIntensity(float from, float to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            light.intensity = Mathf.Lerp(from, to, t);

            yield return null; // wait for next frame
        }

        light.intensity = to; // ensure final value is exact
    }

     IEnumerator PlayThenPauseParticleSystem(float duration)
    {
        float elapsed = 0f;

        particleSystem.Play();
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            yield return null; // wait for next frame
        }

        particleSystem.Pause();

    }



    // upon loofah entering the collision enter
}
