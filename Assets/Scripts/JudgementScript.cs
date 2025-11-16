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
        Debug.Log("judgement room pos: " + this.transform.position);
        
        loofah = Instantiate(loofahPrefab, ImageTracker.instance.originalPos.position, UnityEngine.Quaternion.identity);
        loofah.transform.parent = this.gameObject.transform;
        loofah.GetComponent<LoofahScript>().loofahPrefab = loofahPrefab;

        Destroy(loofah.GetComponent<SphereCollider>());
        Destroy(loofah.GetComponent<Rigidbody>());
    }

    void OnEnable()
    {
        particleSystem.Stop();
        light.intensity = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision hit");
        if(other.gameObject.tag == "loofah")
        {
            Debug.Log("Loofah went in");
            StartCoroutine(FadeLightRoutine());
            StartCoroutine(PlayThenPauseParticleSystem(1.5f));

            Destroy(other.gameObject);
            
        }
    }

    IEnumerator FadeLightRoutine()
    {
        // Fade 0 → 3
        yield return StartCoroutine(FadeIntensity(0f, 3f, 1f));

        // Fade 3 → 0
        yield return StartCoroutine(FadeIntensity(3f, 0f, 1f));
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

        particleSystem.Stop();

    }



}
