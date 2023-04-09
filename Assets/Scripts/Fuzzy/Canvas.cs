using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : MonoBehaviour
{
    public GameObject enemy;
    private HealthController healthController;

    private void Start()
    {
        healthController = enemy.GetComponent<HealthController>();
    }
    public void HitEnemy()
    {
        healthController.TakeDamage();
    }
}
