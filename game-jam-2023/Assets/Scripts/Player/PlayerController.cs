using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}