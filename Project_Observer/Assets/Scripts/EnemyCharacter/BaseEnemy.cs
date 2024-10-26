using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    #region Variables

    [Header("Parent Data"), SerializeField]
    // protected GameObject playerObject;
    protected float psycheDamage = 20f;
    protected bool canFollow = false;

    #endregion

    #region Start Functions

    // private void Awake()
    // {
    //     playerObject = PlayerCharacter.Instance.gameObject;
    // }

    // private void OnEnable()
    // {
    //     playerObject = PlayerCharacter.Instance.gameObject;
    // }

    #endregion

    #region Update Functions



    #endregion

    #region Base Functions

    protected void DoDamage() { }

    protected virtual void LookAtPlayer()
    {
        if (!PlayerCharacter.Instance)
            return;

        transform.LookAt(PlayerCharacter.Instance.transform.position);
    }

    protected virtual void FollowPlayer()
    {
        if (!PlayerCharacter.Instance)
            return;
    }

    #endregion
}
