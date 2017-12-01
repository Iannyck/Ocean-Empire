using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace CCC.Utility
{
    public class TextSplitter : MonoBehaviour
    {
        static char separator = '.';
        public static List<string> Split(string text, Text textUI, Vector2 area, char forceSeparation = '\0')
        {
            List<string> texts = new List<string>();
            Font font = textUI.font;
            int fontSize = textUI.fontSize;
            FontStyle fontStyle = textUI.fontStyle;

            font.RequestCharactersInTexture(text, fontSize, fontStyle);

            int lineSize = Mathf.RoundToInt(((float)font.lineHeight / (float)font.fontSize) * fontSize * textUI.lineSpacing);

            int lastSpace = 1;
            int lastPotentialSeparator = 0;
            int lastSeparator = -1;
            List<int> newLineInserts = new List<int>();

            float currentX = 0;
            float currentY = lineSize;

            bool hasMetALetter = false;

            for (int i = 0; i < text.Length; i++)
            {
                CharacterInfo info;
                char character = text[i];

                if (character == '\n')
                {
                    if (hasMetALetter) // Ce 'if' elimine les \n en debut de page
                    {
                        currentY += lineSize;
                        currentX = 0;
                    }
                    else lastSeparator = i;
                }
                else
                {
                    bool result = font.GetCharacterInfo(text[i], out info, fontSize, fontStyle);

                    if (!result) { Debug.LogError("Error while loading character into font."); return null; }
                    if (info.advance * 2 > area.x) { Debug.LogError("Area is too small for the font size."); return null; }

                    if (character == ' ') lastSpace = i;
                    else if (character == separator || character == forceSeparation) lastPotentialSeparator = i;
                    else hasMetALetter = true;

                    currentX += info.advance;
                }

                if (currentX > area.x) // deborde en x
                {
                    newLineInserts.Add(lastSpace + 1);
                    currentX = 0;
                    currentY += lineSize;

                    i = lastSpace+1;
                }

                if (currentY > area.y || character == forceSeparation) // deborde en y
                {
                    if (lastPotentialSeparator <= lastSeparator) lastPotentialSeparator = i; // Au cas ou il n'y aurait eu aucun '.' depuis le dernier paragraph

                    i = lastPotentialSeparator;

                    //Clear les 'newLinesInserts' qui sont coupé sur la prochaine page
                    for(int u=0; u < newLineInserts.Count; u++)
                    {
                        if (newLineInserts[u] > i)
                        {
                            newLineInserts.RemoveAt(u);
                            u--;
                        }
                    }

                    int length = i - lastSeparator;
                    if (character == forceSeparation) length -= 1;

                    if (length > 0)
                    {
                        char[] array = new char[length];
                        text.CopyTo(lastSeparator + 1, array, 0, length);

                        string aText = new string(array);

                        texts.Add(aText);
                    }

                    lastSeparator = lastPotentialSeparator;
                    currentY = lineSize;
                    currentX = 0;
                    hasMetALetter = false;
                }
            }

            //Ajoute les \n dans le texte pour couper sur ligne
            for (int u = 0; u < newLineInserts.Count; u++)
            {
                int realIndex = newLineInserts[u] + u;
                text = text.Insert(realIndex, "\n");
                if (realIndex <= lastSeparator)
                {
                    lastSeparator++;
                }
            }

            int lastLength = text.Length - lastSeparator - 1;

            if (lastLength > 0)
            {
                char[] array = new char[lastLength];
                text.CopyTo(lastSeparator + 1, array, 0, lastLength);

                string aText = new string(array);

                texts.Add(aText);
            }

            return texts;
        }
    }
}
