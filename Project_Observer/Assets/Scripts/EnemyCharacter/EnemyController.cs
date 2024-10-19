using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region Variables

    [Header("Enemy Data"), SerializeField]
    GameObject enemyMesh;
    CapsuleCollider collider;
    Rigidbody rb;

    #endregion

    #region Start Functions

    void Start() { }

    #endregion

    #region Update Functions

    void Update() { }

    #endregion

    #region Event Handlers

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "LightZone")
        {
            Debug.Log($"OBSERVER IN LIGHT");
            enemyMesh.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "LightZone")
        {
            Debug.Log($"OBSERVER OUTSIDE LIGHT");
            enemyMesh.SetActive(true);
        }
    }

    #endregion

    #region Base Functions



    #endregion
}
