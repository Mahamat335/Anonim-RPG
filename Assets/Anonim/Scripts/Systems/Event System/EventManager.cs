using Sigtrap.Relays;
using Anonim.Systems.DialogueSystem;
using Anonim.Systems.StatSystem;

namespace Anonim.Systems.EventSystem
{
    public class EventManager : Singleton<EventManager>
    {
        #region Dialogue System Events
        public Relay<string> CurrentDialogueChanged { get; private set; } = new();
        public Relay<Language> LanguageChanged { get; private set; } = new();
        #endregion

        #region Input Events
        public InputEvent PlayerMovementInput;
        #endregion

        #region Stat System Events
        public Relay<Stat> StatModifierAdded { get; private set; } = new();
        #endregion
    }
}

