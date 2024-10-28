using System.Threading.Tasks;
using UnityEngine;

public class ObserverEnemy : BaseEnemy
{
    #region Variables

    [Header("Enemy Data"), SerializeField]
    GameObject enemyMesh;
    CapsuleCollider collider;
    Rigidbody rb;

    #endregion

    #region Start Functions

    void Start()
    {
        StartSanityDrain(1500);
    }

    #endregion

    #region Update Functions

    void Update()
    {
        LookAtPlayer();
        DistanceCheck();
    }

    #endregion

    #region Event Handlers

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "LightZone")
        {
            Debug.Log($"OBSERVER IN LIGHT");
            enemyMesh.SetActive(false);
            enemyDead = true;
            StopSanityDrain();
        }
    }

    // void OnTriggerExit(Collider other)
    // {
    //     if (other.gameObject.tag == "LightZone")
    //     {
    //         Debug.Log($"OBSERVER OUTSIDE LIGHT");
    //         enemyMesh.SetActive(true);
    //     }
    // }

    #endregion

    #region Base Functions



    #endregion
}
