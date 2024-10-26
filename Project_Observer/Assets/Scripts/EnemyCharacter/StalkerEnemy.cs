using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class StalkerEnemy : BaseEnemy
{
    #region Variables

    [Header("Enemy Data"), SerializeField]
    GameObject enemyMesh;

    [SerializeField]
    GameObject decoyMesh;
    CapsuleCollider collider;
    Rigidbody rb;
    NavMeshAgent agent;

    bool exposed = false;

    [SerializeField]
    Animator animator;

    #endregion

    #region Start Functions

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        // PlayerCharacter.OnSpotlightTurnedOff += ToggleEnemyOff; // Throws an error??
        PlayerCharacter.OnSpotlightTurnedOff += TriggerToggle;
    }

    void Start()
    {
        StartFollowing();
    }

    #endregion

    #region Update Functions

    void Update()
    {
        LookAtPlayer();

        if (canFollow)
            FollowPlayer();
        else
            agent.ResetPath();
    }

    #endregion

    #region Event Handlers

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter");

        if (other.gameObject.tag == "LightZone")
            ToggleEnemy(true);
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger Exit");

        if (other.gameObject.tag == "LightZone")
            ToggleEnemy(false);
    }

    #endregion

    #region Base Functions

    protected override void LookAtPlayer()
    {
        base.LookAtPlayer();
    }

    protected override void FollowPlayer()
    {
        base.FollowPlayer();

        agent.SetDestination(PlayerCharacter.Instance.transform.position);
    }

    async void StartFollowing()
    {
        await Task.Delay(3000);

        canFollow = true;
        animator.SetBool("frozen", false);
        animator.SetBool("startedWalking", true);
    }

    void ToggleEnemy(bool value)
    {
        if (value)
        {
            Debug.Log($"STALKER IN LIGHT");

            enemyMesh.SetActive(false);
            decoyMesh.SetActive(true);

            animator.SetBool("frozen", !exposed);
            canFollow = false;
        }
        else
        {
            Debug.Log($"STALKER OUT OF LIGHT");

            decoyMesh.SetActive(false);
            enemyMesh.SetActive(true);

            animator.SetBool("frozen", !exposed);
            canFollow = true;
        }
    }

    void TriggerToggle() => ToggleEnemyOff();

    async void ToggleEnemyOff()
    {
        await Task.Delay(800);

        Debug.Log($"Light Turned Off");

        decoyMesh.SetActive(false);
        enemyMesh.SetActive(true);

        animator.SetBool("frozen", !exposed);
        canFollow = true;
    }

    #endregion
}
