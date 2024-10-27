using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FL
{
    public static class FunctionLibrary { }

    public static class MouseHandler
    {
        public static void ToggleCursor(bool value)
        {
            if (value)
            {
                Cursor.lockState = CursorLockMode.None; // Unlock the cursor
                Cursor.visible = true; // Show the cursor again
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
                Cursor.visible = false; // Hide the cursor
            }
        }
    }

    public static class QuestLibrary
    {
        public static bool CheckQuestPreReq(Interactable questItem)
        {
            int linkCount = 0;

            foreach (var item in questItem.quest.preReqQuests)
            {
                if (item.quest.QuestCompleted)
                {
                    linkCount++;
                }
            }

            return linkCount == questItem.quest.preReqQuests.Count;
        }
    }

    public enum SoundEnemyType
    {
        Burst,
        Moving,
        Delayd,
        SlowRelease,
    }
}
