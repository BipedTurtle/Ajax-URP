using TMPro;
using UnityEngine;

namespace QuestSystem
{
    public class QuestButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tmp;

        public void DisplayInfo(string questName, string npcName)
        {
            this.tmp.text = $"{questName}   (by {npcName})";
        }
    }
}