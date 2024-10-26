using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

public class StalkerEnemy : BaseEnemy
{
    #region Variables

    [Header("Enemy Data"), SerializeField]
    GameObject enemyMesh;
    CapsuleCollider collider;
    Rigidbody rb;
    NavMeshAgent agent;

    bool exposed = false;

    [SerializeField]
    Animator animator;

    #endregion

    #region Start Functions

    void Start() { }

    #endregion

    #region Update Functions

    void Update()
    {
        if (canFollow)
            FollowPlayer();
    }

    #endregion

    #region Event Handlers

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "LightZone")
        {
            Debug.Log($"STALKER IN LIGHT");
            animator.SetBool("frozen", !exposed);
            canFollow = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "LightZone")
        {
            Debug.Log($"STALKER IN LIGHT");
            animator.SetBool("frozen", !exposed);
            canFollow = true;
        }
    }

    #endregion

    #region Base Functions

    protected override void FollowPlayer()
    {
        base.FollowPlayer();

        agent.Move(playerObject.transform.position);
    }

    #endregion
}
