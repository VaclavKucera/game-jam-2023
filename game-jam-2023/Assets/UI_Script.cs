using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Script : MonoBehaviour
{
    public Slider BossHPSlider;
    public Slider HPSlider;
    public Slider EnduranceSlider;

    public BossController bossController;
    public PlayerController playerController;
    public Movement movement;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        movement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        bossController = GameObject.FindGameObjectWithTag("BossController").GetComponent<BossController>();
        
        BossHPSlider.maxValue = BossController.totalHealth;
        HPSlider.maxValue = playerController.health;
        EnduranceSlider.maxValue = movement.maxStamina;

    }

    private void Update()
    {
        BossHPSlider.value = bossController.currentHealth;
        HPSlider.value = playerController.health;
        EnduranceSlider.value = movement.stamina;
    }
}
