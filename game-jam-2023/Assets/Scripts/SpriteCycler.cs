using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCycler : MonoBehaviour
{
    public float FPS;
    public bool active;

    private List<GameObject> childObjects;
    private float timer;
    private int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        childObjects = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++)
        {
            childObjects.Add(transform.GetChild(i).gameObject);
            childObjects[i].SetActive(false);
        }

        timer = 0;
        currentIndex = 0;

        if (childObjects.Count > 0 && active)
        {
            childObjects[currentIndex].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (active && childObjects.Count > 0)
        {
            timer += Time.deltaTime;
            if (timer >= 1 / FPS)
            {
                timer = 0;
                childObjects[currentIndex].SetActive(false);
                currentIndex = (currentIndex + 1) % childObjects.Count;
                childObjects[currentIndex].SetActive(true);
            }
        }
    }
}