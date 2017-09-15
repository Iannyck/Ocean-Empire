using System;
using FullInspector.Internal;

namespace FullInspector.Generated {
    [CustomPropertyEditor(typeof(UnityEngine.Events.UnityEvent))]
    public class Generated_UnityEngine_Events_UnityEvent_PropertyEditor : fiGenericPropertyDrawerPropertyEditor<Generated_UnityEngine_Events_UnityEvent_MonoBehaviourStorage, UnityEngine.Events.UnityEvent> {
        public override bool CanEdit(Type type) {
            return typeof(UnityEngine.Events.UnityEvent).IsAssignableFrom(type);
        }
    }
}
