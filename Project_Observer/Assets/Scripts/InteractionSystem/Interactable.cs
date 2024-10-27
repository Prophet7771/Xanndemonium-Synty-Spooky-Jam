using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    #region Variables

    [Header("Interaction")]
    [SerializeField]
    string interactionMessage = "Press 'E' to interact";

    [Header("VFX")]
    [SerializeField]
    Outline outline;

    PlayerInput inputAction;

    [Space(10)]
    public QuestSystem.QuestItem quest;

    #endregion

    #region Getters & Setters

    public string GetInteractMsg
    {
        get { return interactionMessage; }
    }

    #endregion

    #region Unity Events

    public delegate void OnInteract();
    public OnInteract onInteract;

    #endregion

    #region Event Handlers

    private void Awake() { }

    private void OnEnable() { }

    private void OnDisable()
    {
        if (inputAction == null)
            return;
    }

    #endregion

    #region Basic Functions

    public void Interact()
    {
        QuestSystem.Instance.CompleteQuest(this);

        if (!FL.QuestLibrary.CheckQuestPreReq(this))
            return;

        gameObject.SetActive(false);
        PlayerCharacter.Instance.ClearInteractMessage();
        // Invoke("DestroySelf", 2f);
    }

    private void DestroySelf() => Destroy(gameObject);

    #endregion

    #region Hover Visuals

    public void HoverVisual(bool val)
    {
        outline.OutlineMode = Outline.Mode.OutlineAll;

        if (val)
            outline.OutlineColor = new Color(0, 255, 0, 255);
        else
            outline.OutlineColor = new Color(255, 255, 255, 255);
    }

    public void HoverVisual(bool val, bool canInteract)
    {
        outline.OutlineMode = Outline.Mode.OutlineAll;

        if (val)
        {
            outline.OutlineColor = new Color(0, 255, 0, 255);
        }
        else
            outline.OutlineColor = new Color(255, 255, 255, 255);
    }

    #endregion
}
