using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class NotepadLimit : MonoBehaviour {
    public int ligneMax = 3;
    private InputField field;

    void Start()
    {
        field = GetComponent<InputField>();
        field.onValueChanged.AddListener(OnValueChange);
    }

    void OnValueChange(string value)
    {
        bool change = false;
        int ligneCount = 1;
        for(int i=0; i<value.Length; i++)
        {
            if(value[i] == '\n')
            {
                ligneCount++;
                if(ligneCount > ligneMax)
                {
                    value = value.Remove(i);
                    change = true;
                    break;
                }
            }
        }

        if (change)
            field.text = value;
    }
}
