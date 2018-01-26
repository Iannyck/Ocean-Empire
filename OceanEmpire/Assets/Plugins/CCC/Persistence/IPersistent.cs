using System;

namespace CCC.Persistence
{
    public interface IPersistent
    {
        void Init(Action onComplete);

        /// <summary>
        /// Permet la duplication avant l'initialisation. Ex: On voudrait probablement dupliquer un prefab avant son initialization.
        /// Alors qu'avec un scriptableObject, on ne veut peut-etre pas le dupliquer.
        /// </summary>
        UnityEngine.Object DuplicationBehavior();
    }
}