using UnityEngine;
using UnityEngine.InputSystem;

namespace Anonim.Systems.EventSystem
{
    [CreateAssetMenu(fileName = "InputEvent", menuName = "Scriptable Objects/Scriptable Events/InputEvent [CallbackContext]")]
    public class InputEvent : ScriptableEvent<InputAction.CallbackContext> { }
}
