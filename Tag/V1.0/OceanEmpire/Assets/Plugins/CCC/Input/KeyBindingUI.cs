using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;
using UnityEngine.UI;
using FullInspector;

namespace CCC.Input
{
    public class KeyBindingUI : BaseBehavior
    {
        [InspectorHeader("Settings")]
        public bool showOnStart;
        public float scrollSensitivity = 1;
        public float maxScrollerVelocity = 100;
        [InspectorHeader("UI")]
        public HorizontalLayoutGroup headersLayoutGroup;
        public KeyBindingEntry entryPrefab;
        public KeyBindingHeader headerPrefab;
        public GameObject categoryPrefab;
        public ScrollRect scroller;
        public KeyBindingBinder binder;

        private List<InputMapping> mappings;
        private int currentMappingIndex = -1;

        private List<KeyBindingHeader> headers = new List<KeyBindingHeader>();
        private List<KeyBindingEntry> entries = new List<KeyBindingEntry>();
        private List<GameObject> categories = new List<GameObject>();

        private List<InputMapping> dirtiedMappings = new List<InputMapping>();

        void Start()
        {
            headerPrefab.onClick.AddListener(OnHeaderClick);
            entryPrefab.onClick.AddListener(OnEntryClicked);

            headers.Add(headerPrefab);
            entries.Add(entryPrefab);
            categories.Add(categoryPrefab);

            MasterManager.Sync(Init);
        }

        void Init()
        {
            mappings = InputManager.ToList();
            ShowHeaders();
            if (showOnStart)
                ShowMapping(0);
        }

        void OnHeaderClick(KeyBindingHeader header)
        {
            ShowMapping(header.Index);
        }

        void OnEntryClicked(KeyBindingEntry entry)
        {
            KeyCombination combination = null;
            if (entry.pressedKey == 0)
                combination = entry.key.Value.primary;
            else if (entry.pressedKey == 1)
                combination = entry.key.Value.secondary;

            binder.Rebind(combination, mappings[entry.mappingIndex], OnFinishRebind);
        }

        void OnFinishRebind(KeyCombination a, KeyCombination b)
        {
            Dirty(mappings[currentMappingIndex]);

            int i = 0;
            foreach (KeyValuePair<string, InputKey> key in mappings[currentMappingIndex].keys)
            {
                if (key.Value.primary == a || key.Value.secondary == a || key.Value.primary == b || key.Value.secondary == b)
                    entries[i].Rewrite();
                i++;
            }
        }

        void ShowHeaders()
        {
            for (int i = 0; i < mappings.Count; i++)
            {
                InputMapping mapping = mappings[i];

                if (headers.Count > i)
                {
                    //Fill old header
                    headers[i].Fill(mapping.displayName, i);
                }
                else
                {
                    //New Header

                    //Spawn
                    KeyBindingHeader header = Instantiate(headerPrefab.gameObject).GetComponent<KeyBindingHeader>();
                    header.transform.SetParent(headerPrefab.transform.parent);
                    header.transform.localScale = Vector3.one;

                    //Listener
                    header.onClick.AddListener(OnHeaderClick);

                    //List
                    headers.Add(header);

                    //Fill with data
                    header.Fill(mapping.displayName, i);
                }
            }
        }

        public void ShowMapping(int index)
        {
            //Highlight header
            if (currentMappingIndex >= 0)
                headers[currentMappingIndex].Highlight(false);
            currentMappingIndex = index;
            headers[currentMappingIndex].Highlight(true);


            List<string> categorieNames = new List<string>() { "Other" };
            List<List<Transform>> categoryItems = new List<List<Transform>>() { new List<Transform>() };

            //Display all entries
            InputMapping mapping = mappings[index];

            int i = 0;
            foreach (KeyValuePair<string, InputKey> key in mapping.keys)
            {
                if (entries.Count > i)
                {
                    entries[i].gameObject.SetActive(true);
                    entries[i].Fill(key, currentMappingIndex);
                }
                else
                {
                    KeyBindingEntry entry = Instantiate(entryPrefab.gameObject).GetComponent<KeyBindingEntry>();
                    entry.transform.SetParent(entryPrefab.transform.parent);
                    entry.transform.localScale = Vector3.one;

                    //Listener
                    entry.onClick.AddListener(OnEntryClicked);

                    //List
                    entries.Add(entry);

                    //Fill with data
                    entry.Fill(key, currentMappingIndex);
                }

                if (!string.IsNullOrEmpty(key.Value.displayCategory))
                {
                    int ind = -1;
                    if (!categorieNames.Contains(key.Value.displayCategory))
                    {
                        ind = categorieNames.Count;
                        categorieNames.Add(key.Value.displayCategory);
                        categoryItems.Add(new List<Transform>());
                    }
                    else
                        ind = categorieNames.IndexOf(key.Value.displayCategory);

                    categoryItems[ind].Add(entries[i].transform);
                }
                else
                {
                    categoryItems[0].Add(entries[i].transform);
                }


                i++;
            }

            //Deactivate the rest
            for (int u = i; u < entries.Count; u++)
            {
                entries[u].gameObject.SetActive(false);
            }


            int y = 0;
            if (categorieNames.Count > 1)
            {
                //Category: other
                if (categoryItems[0].Count > 0)
                {
                    for (int f = categoryItems[0].Count - 1; f >= 0; f--)
                    {
                        categoryItems[0][f].SetAsFirstSibling();
                    }
                    categories[0].transform.SetAsFirstSibling();
                    categories[0].GetComponentInChildren<Text>().text = categorieNames[0];
                    categories[0].SetActive(true);
                    y++;
                }

                //All categories
                for (int v = categorieNames.Count - 1; v > 0; v--)
                {

                    for (int f = categoryItems[v].Count -1; f >= 0; f--)
                    {
                        categoryItems[v][f].SetAsFirstSibling();
                    }

                    if (categories.Count > y)
                    {
                        categories[y].transform.SetAsFirstSibling();
                        categories[y].GetComponentInChildren<Text>().text = categorieNames[v];
                        categories[y].SetActive(true);
                    }
                    else
                    {
                        GameObject categoryObj = Instantiate(categoryPrefab);
                        categoryObj.transform.SetParent(categoryPrefab.transform.parent);
                        categoryObj.transform.localScale = Vector3.one;
                        categoryObj.transform.SetAsFirstSibling();

                        //List
                        categories.Add(categoryObj);

                        //Fill
                        categoryObj.GetComponentInChildren<Text>().text = categorieNames[v];
                    }

                    y++;
                }
            }

            //Deactivate the rest
            for (int u = y; u < categories.Count; u++)
            {
                categories[u].gameObject.SetActive(false);
            }
        }

        void Update()
        {
            //Scrolling
            float scroll = UnityEngine.Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity * -100;

            if (scroll != 0)
            {
                Vector2 speed = scroller.velocity + Vector2.up * scroll;
                scroller.velocity = Vector2.ClampMagnitude(speed, maxScrollerVelocity);
            }
        }

        /// <summary>
        /// Adds the mapping to the list of dirtied mappings (if it's not already there)
        /// </summary>
        void Dirty(InputMapping mapping)
        {
            foreach (InputMapping dirtied in dirtiedMappings)
            {
                if (dirtied == mapping)
                    return;
            }
            dirtiedMappings.Add(mapping);
        }

        public void LoadDefaultsForCurrentMapping()
        {
            //Load
            mappings[currentMappingIndex].LoadDefaults();

            //Dirty
            Dirty(mappings[currentMappingIndex]);

            //Show
            ShowMapping(currentMappingIndex);
        }

        public void Confirm()
        {
            //Save all dirtied
            foreach (InputMapping mapping in dirtiedMappings)
            {
                mapping.Save();
            }
            dirtiedMappings.Clear();
        }

        public void Cancel()
        {
            //Load back all dirtied
            foreach (InputMapping mapping in dirtiedMappings)
            {
                mapping.Load();
            }
            
            //Clear list
            dirtiedMappings.Clear();

            //Show
            ShowMapping(currentMappingIndex);
        }
    }

}