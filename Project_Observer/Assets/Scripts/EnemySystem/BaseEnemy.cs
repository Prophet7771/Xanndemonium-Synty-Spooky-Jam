using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    #region Variables

    [Header("Parent Data"), SerializeField]
    // protected GameObject playerObject;
    protected float psycheDamage = 2f;
    protected float psycheDamageRange = 30f;
    protected bool canFollow = false;
    public float maxDistanceFromPlayer = 30f;
    protected float currDistanceFromPlayer;

    public bool enemyDead = false;

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

    protected void DamageSanity()
    {
        PlayerCharacter.Instance.DamageSanity(psycheDamage);
    }

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

    protected virtual async void StartSanityDrain(int delay = 0)
    {
        await Task.Delay(delay);

        PlayerCharacter.Instance.StartSanityDrain(psycheDamage);
    }

    protected void StopSanityDrain() => PlayerCharacter.Instance.StopSanityDrain();

    protected void DistanceCheck()
    {
        currDistanceFromPlayer = Vector3.Distance(
            transform.position,
            PlayerCharacter.Instance.transform.position
        );

        if (currDistanceFromPlayer > maxDistanceFromPlayer)
        {
            StopSanityDrain();
            Debug.Log("Enemy Out Of Range!");
            gameObject.SetActive(false);
            enemyDead = true;
            // Destroy(gameObject);
        }
    }

    #endregion
}
