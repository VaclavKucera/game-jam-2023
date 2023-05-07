using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int health = 1000;
    public Movement Movement;

    public void Start()
    {
        Movement = gameObject.GetComponent<Movement>();
    }

    public void TakeDamage(int damage)
    {
        if (Movement.dashing)
            return;

        health -= damage;

        if (health <= 0)
        {
            SceneManager.LoadScene("Ded");
        }
    }
}