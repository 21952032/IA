using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChasingState : MonoBehaviour
{
    public Collider myCollider;
    public GameObject chasingVisual;
    public MonoBehaviour wanderingState;
    private StateMachine stateMachine;
    private EnemyController controller;
    // Start is called before the first frame update
    private void OnEnable()
    {
        myCollider.enabled = true;
        stateMachine = GetComponent<StateMachine>();
        controller = GetComponent<EnemyController>();
        Invoke(nameof(PlayerNotCaught), 20f);
        chasingVisual.SetActive(true);
    }
    private void OnDisable()
    {
        CancelInvoke();
        myCollider.enabled = false;
        chasingVisual.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            controller.enabled = false;
            Invoke(nameof(ResetScene), 3f);
        }
    }

    private void PlayerNotCaught()
    {
        stateMachine.ChangeCurrentStatus(wanderingState);
    }

    private void ResetScene()
    {
        SceneManager.LoadScene("Pathfinding");
    }
}
