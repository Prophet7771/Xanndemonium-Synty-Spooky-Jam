using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class QuestSystem : MonoBehaviour
{
    #region Variables

    #region Singleton

    public static QuestSystem Instance { get; private set; }

    #endregion

    public List<Interactable> questsItems;

    #endregion

    #region Start Functions

    private void Awake()
    {
        // Check if an instance of the Player already exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy this instance if one already exists
            return;
        }

        Instance = this; // Assign the correct instance of the player
        DontDestroyOnLoad(gameObject); // Make sure it exists throughout all scenes
    }

    #endregion

    #region Update Functions



    #endregion

    #region Base Functions

    public void CompleteQuest(Interactable questItem)
    {
        if (FL.QuestLibrary.CheckQuestPreReq(questItem))
        {
            questItem.quest.QuestCompleted = true;
            questItem.quest.CompleteEvent?.Invoke();
        }
    }

    #endregion

    #region Other Classes

    [Serializable]
    public class QuestItem
    {
        public string QuestName;
        public bool QuestCompleted;
        public List<Interactable> preReqQuests;
        public UnityEvent CompleteEvent;
    }

    #endregion
}
