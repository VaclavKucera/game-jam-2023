using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistSlamController : MonoBehaviour
{
    public float fistRaiseDuration;
    public float fistTopDelay;
    public float fistDropDuration;

    private List<GameObject> childObjects;
    private Coroutine slamRoutine;

    // Start is called before the first frame update
    void Start()
    {
        childObjects = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++)
        {
            childObjects.Add(transform.GetChild(i).gameObject);
            childObjects[i].SetActive(false);
        }

        // Activate();
    }

    public void Activate()
    {
        if (slamRoutine != null)
        {
            StopCoroutine(slamRoutine);
        }
        slamRoutine = StartCoroutine(FistSlamSequence());
    }

    IEnumerator FistSlamSequence()
    {
        // Activate Bottom sprite
        SetActiveChild(2);

        yield return new WaitForSeconds(fistRaiseDuration);

        // Activate mid sprite
        SetActiveChild(0);

        // Wait for fistRaiseDuration then activate top sprite
        yield return new WaitForSeconds(fistRaiseDuration);
        SetActiveChild(1);

        // Wait for fistTopDelay then activate mid sprite
        yield return new WaitForSeconds(fistTopDelay);
        SetActiveChild(0);

        // Wait for fistDropDuration then activate bottom sprite
        yield return new WaitForSeconds(fistDropDuration);
        SetActiveChild(2);

        // Wait for a short delay then deactivate all sprites
        yield return new WaitForSeconds(3);
        DeactivateAllChildren();
    }

    private void SetActiveChild(int index)
    {
        for (int i = 0; i < childObjects.Count; i++)
        {
            childObjects[i].SetActive(i == index);
        }
    }

    private void DeactivateAllChildren()
    {
        for (int i = 0; i < childObjects.Count; i++)
        {
            childObjects[i].SetActive(false);
        }
    }
}