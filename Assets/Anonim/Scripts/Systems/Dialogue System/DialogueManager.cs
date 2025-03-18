using TMPro;
using UnityEngine.UI;
using Anonim.Systems.EventSystem;
using UnityEngine;
using System.Collections.Generic;

namespace Anonim.Systems.DialogueSystem
{
    public class DialogueManager : Singleton<DialogueManager>
    {
        [SerializeField] private GameObject _dialoguePanel;
        public TMP_Text NpcText;
        public Button[] ChoiceButtons;
        public Language Language;
        public string CurrentDialogueName;
        private DialogueSO _currentDialogue;
        private DialogueNode _currentNode;
        private const string _dialogueScriptableObjectsFolderPath = "Scriptable Objects/Dialogue System/";
        private const string _dialogueSuffix = "_Dialogue_SO";
        [SerializeField] private DialogueSO _exampleDialogue;

        private void OnEnable()
        {
            EventManager.Instance.CurrentDialogueChanged.AddListener(UpdateCurrentDialogue);
            EventManager.Instance.LanguageChanged.AddListener(UpdateLanguage);

            StartDialogue(_exampleDialogue);
        }

        private void OnDisable()
        {
            EventManager.Instance.CurrentDialogueChanged.RemoveListener(UpdateCurrentDialogue);
            EventManager.Instance.LanguageChanged.RemoveListener(UpdateLanguage);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_currentDialogue is not null && _currentDialogue.Language != Language)
            {
                UpdateLanguage(Language);
            }
        }
#endif

        private void UpdateCurrentDialogue(string currentDialogueName)
        {
            _currentDialogue = Resources.Load<DialogueSO>(_dialogueScriptableObjectsFolderPath + Language.ToString() + "/" + currentDialogueName + _dialogueSuffix);

            if (_currentDialogue == null)
            {
                Debug.LogError($"Dialogue not found at path: {_dialogueScriptableObjectsFolderPath}{Language.ToString()}{_dialogueSuffix}. Please make sure the dialogue file exists and the path is correct.");
                return;
            }
        }

        private void UpdateLanguage(Language language)
        {
            Language = language;
            UpdateCurrentDialogue(CurrentDialogueName);
        }

        private void ChangeNode(int id)
        {
            foreach (DialogueNode dialogueNode in _currentDialogue.Nodes)
            {
                if (dialogueNode.Id == id)
                {
                    _currentNode = dialogueNode;
                    return;
                }
            }
        }

        public void StartDialogue(DialogueSO dialogue, int nodeId = 0)
        {
            _currentDialogue = dialogue;
            ChangeNode(nodeId);
            if (NpcText is not null)
            {
                DisplayNode();
            }
        }

        void DisplayNode()
        {
            _dialoguePanel.SetActive(true);
            NpcText.text = _currentNode.NpcText;
            for (int i = 0; i < ChoiceButtons.Length; i++)
            {
                if (_currentNode.Choices.Length <= i)
                {
                    ChoiceButtons[i].gameObject.SetActive(false);
                    continue;
                }

                DialogueChoice choice = _currentNode.Choices[i];

                ChoiceButtons[i].GetComponentInChildren<TMP_Text>().text = choice.ChoiceText;
                ChoiceButtons[i].gameObject.SetActive(true);
                int index = i;
                ChoiceButtons[i].onClick.RemoveAllListeners();
                ChoiceButtons[i].onClick.AddListener(() => ChooseOption(index));
            }
        }

        public void CloseDialogue()
        {
            _currentDialogue = null;
            _dialoguePanel.SetActive(false);
        }

        void ChooseOption(int index)
        {
            if (_currentNode.Choices[index].NextNodeId == -1)
            {
                CloseDialogue();
                return;
            }

            ChangeNode(_currentNode.Choices[index].NextNodeId);
            DisplayNode();
        }
    }
}

