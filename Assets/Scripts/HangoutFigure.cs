using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangoutFigure : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs = new List<GameObject>();
    private List<GameObject> instantiatedObjects = new List<GameObject>();
    private Animator animator;
    private bool hasAnimationPlayed = false;

    // [SerializeField] private ParticleSystem particleSystem;
    // Start is called before the first frame update
    
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
    }



    public void AnimateComingUpWithIdea()
    {
        
        if (!hasAnimationPlayed)
        {
            hasAnimationPlayed = true;
            animator.SetTrigger("isIdea");
            StartCoroutine(WaitForAnimationToFinish());
            StartCoroutine(SpawnAndEnableGravity());
            // insert coroutine to spawn and particle effect
        }
    }

    IEnumerator WaitForAnimationToFinish()
    {

        yield return new WaitForSeconds(8.4f);

        animator.ResetTrigger("isIdea");
        hasAnimationPlayed = false;
        
    }

    IEnumerator SpawnAndEnableGravity()
    {
        yield return new WaitForSeconds(3.5f);
        instantiatedObjects.Add(Instantiate(prefabs[Random.Range(0, prefabs.Count)], new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 0.5f, this.gameObject.transform.position.z), Quaternion.identity));
       
       // enable gravity(?)
    }



    IEnumerator SpawnAndAddParticleEffect()
    {
        yield return null;
    }
}
