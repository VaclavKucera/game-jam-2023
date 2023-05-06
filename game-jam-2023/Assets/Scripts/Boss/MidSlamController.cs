using UnityEngine;

public class MidSlamController : MonoBehaviour
{
    private GameObject arm;
    private GameObject fist;
    private BossMainAnimator bossMainAnimator;

    // Start is called before the first frame update
    void Start()
    {
        arm = transform.GetChild(0).gameObject;
        fist = transform.GetChild(1).gameObject;
        bossMainAnimator = transform.parent.parent.GetComponent<BossMainAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 parentPosition = transform.parent.position;
            Vector2 direction = mousePosition - parentPosition;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Activate(angle);
        }
    }

    public void Activate(float angle)
    {
        if (bossMainAnimator != null)
        {
            bossMainAnimator.LookAtAngle(angle + 90);
        }

        ActivateChildScripts(arm);
        ActivateChildScripts(fist);
    }

    private void ActivateChildScripts(GameObject child)
    {
        // Activate SpriteCycler script
        SpriteCycler spriteCycler = child.GetComponent<SpriteCycler>();
        if (spriteCycler != null)
        {
            spriteCycler.active = true;
        }

        // Activate FistSlamController script
        FistSlamController fistSlamController = child.GetComponent<FistSlamController>();
        if (fistSlamController != null)
        {
            fistSlamController.Activate();
        }
    }
}