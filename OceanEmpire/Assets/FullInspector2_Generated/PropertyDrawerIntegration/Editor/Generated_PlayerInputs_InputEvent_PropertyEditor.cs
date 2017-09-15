using System;
using FullInspector.Internal;

namespace FullInspector.Generated {
    [CustomPropertyEditor(typeof(PlayerInputs.InputEvent))]
    public class Generated_PlayerInputs_InputEvent_PropertyEditor : fiGenericPropertyDrawerPropertyEditor<Generated_PlayerInputs_InputEvent_MonoBehaviourStorage, PlayerInputs.InputEvent> {
        public override bool CanEdit(Type type) {
            return typeof(PlayerInputs.InputEvent).IsAssignableFrom(type);
        }
    }
}
