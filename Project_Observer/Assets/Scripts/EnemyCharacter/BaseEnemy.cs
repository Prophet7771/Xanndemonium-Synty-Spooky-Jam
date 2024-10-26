using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    #region Variables

    [Header("Parent Data"), SerializeField]
    protected GameObject playerObject;
    protected float psycheDamage = 20f;
    protected bool canFollow = false;

    #endregion

    #region Start Functions

    private void OnEnable()
    {
        // playerObject = PlayerController.Instance.gameObject;
    }

    #endregion

    #region Update Functions



    #endregion

    #region Base Functions

    void DoDamage() { }

    protected virtual void FollowPlayer()
    {
        transform.LookAt(playerObject.transform.position);
    }

    #endregion
}
