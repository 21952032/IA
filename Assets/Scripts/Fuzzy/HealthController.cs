using FuzzyLogicPCL;
using FuzzyLogicPCL.FuzzySets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public GameObject player;
    public int health;
    public Text healthText, fuzzy1,fuzzy2;
    private LinguisticVariable distance, strength, damage;
    FuzzySystem healthSystem;

    private void Start()
    {
        healthText.text = health.ToString()+" HP";

        healthSystem = new("Calculadora de Puntos de Salud");

        distance = new LinguisticVariable("Distance", 0, 500000);
        distance.AddValue(new LinguisticValue("Near", new LeftFuzzySet(0, 500000, 30, 50)));
        distance.AddValue(new LinguisticValue("Halfway", new TrapezoidalFuzzySet(0, 500000, 40, 50, 100, 150)));
        distance.AddValue(new LinguisticValue("Far", new RightFuzzySet(0, 500000, 100, 150)));
        healthSystem.addInputVariable(distance);

        strength = new LinguisticVariable("Strength", 0, 100);
        strength.AddValue(new LinguisticValue("Pathetic", new LeftFuzzySet(0, 100, 15, 35)));
        strength.AddValue(new LinguisticValue("Weak", new TrapezoidalFuzzySet(0, 100, 20, 30, 50, 65)));
        strength.AddValue(new LinguisticValue("Mild", new TrapezoidalFuzzySet(0, 100, 50, 70, 85, 90)));
        strength.AddValue(new LinguisticValue("Nice", new RightFuzzySet(0, 100, 75, 100)));
        healthSystem.addInputVariable(strength);

        damage = new LinguisticVariable("Damage", 0, 5);
        damage.AddValue(new LinguisticValue("Low", new LeftFuzzySet(0, 5, 1, 2)));
        damage.AddValue(new LinguisticValue("Regular", new TrapezoidalFuzzySet(0, 5, 1, 2, 3, 4)));
        damage.AddValue(new LinguisticValue("High", new RightFuzzySet(0, 5, 3, 4)));
        healthSystem.addOutputVariable(damage);

        //Rules
        healthSystem.addFuzzyRule("IF Strength IS Pathetic THEN Damage IS Low");
        //Near
        healthSystem.addFuzzyRule("IF Distance IS Near AND Strength IS Weak THEN Damage IS Regular");
        healthSystem.addFuzzyRule("IF Distance IS Near AND Strength IS Mild THEN Damage IS High");
        healthSystem.addFuzzyRule("IF Distance IS Near AND Strength IS Nice THEN Damage IS High");
        //Halfway
        healthSystem.addFuzzyRule("IF Distance IS Halfway AND Strength IS Weak THEN Damage IS Low");
        healthSystem.addFuzzyRule("IF Distance IS Halfway AND Strength IS Mild THEN Damage IS Regular");
        healthSystem.addFuzzyRule("IF Distance IS Halfway AND Strength IS Nice THEN Damage IS High");
        //Far
        healthSystem.addFuzzyRule("IF Distance IS Far AND Strength IS Weak THEN Damage IS Low");
        healthSystem.addFuzzyRule("IF Distance IS Far AND Strength IS Mild THEN Damage IS Low");
        healthSystem.addFuzzyRule("IF Distance IS Far AND Strength IS Nice THEN Damage IS Regular");
    }
    public void TakeDamage()
    {
        float distanceValue = Vector3.Distance(player.transform.position, transform.position);
        float strengthValue = Random.Range(0f, 100f);

        healthSystem.SetInputVariable(distance, distanceValue);
        healthSystem.SetInputVariable(strength, strengthValue);

        health -= (int)(10 *healthSystem.Solve());
        healthText.text = health.ToString() + " HP";
        if (health <= 0)
        {
            SceneManager.LoadScene("Fuzzy");
        }
    }
}
