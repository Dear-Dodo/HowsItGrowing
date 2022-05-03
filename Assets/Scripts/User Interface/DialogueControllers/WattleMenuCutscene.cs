using UnityEngine;

namespace DialogueControllers
{
    public class WattleMenuCutscene : MonoBehaviour
    {
        [SerializeField]
        private Conversation dialogue;

        public void RunDialogue()
        {
            UserInterfaceStack.Instance.Show("DialogueUI");
            DialogueSystem.Instance.Play(dialogue,false);

            DialogueSystem.Instance.onDialogueClose.AddListener(() =>
            {
                DialogueSystem.Instance.gameObject.SetActive(false);
                UserInterfaceStack.Instance.Show("Start_Game_Menu", true);
            });
        }
    }
}