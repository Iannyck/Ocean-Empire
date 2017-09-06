using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.Utility
{
    public interface ILotteryItem
    {
        float Weight();
    }

    // Structure de données permettant de choisir un élément ayant une chance d'être pigé
    // parmis un lot d'éléments ayant également leur propre chance d'être pigé (weight)
    public class Lottery
    {
        public Lottery() { }
        public Lottery(ILotteryItem[] items)
        {
            list.AddRange(items);
        }
        private class LotteryItem : ILotteryItem
        {
            // Constructeur d'un élément qui va faire parti du lot
            public LotteryItem(object obj, float weight)
            {
                this.obj = obj;
                this.weight = weight;
            }
            public object obj = null;
            public float weight = 1;

            public float Weight()
            {
                return weight;
            }
        }

        ArrayList list = new ArrayList(); // Liste des objets du lot

        public void Add(ILotteryItem item)
        {
            list.Add(item);
        }

        public object At(int i)
        {
            return list[i];
        }

        public void RemoveAt(int i)
        {
            list.RemoveAt(i);
        }

        /// <summary>
        /// Ajout d'un objet dans le lot en fonction de sa chance d'être sélectionné
        /// </summary>
        public void Add(object item, float weight)
        {
            Add(new LotteryItem(item, weight));
        }

        /// <summary>
        /// Nombre d'éléments dans le lot
        /// </summary>
        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        /// <summary>
        /// Sélection d'un élément de facon aléatoire en fonction de leurs chance d'être pigé
        /// </summary>
        public object Pick()
        {
            int bidon;
            return Pick(out bidon);
        }

        /// <summary>
        /// Sélection d'un élément de facon aléatoire en fonction de leurs chance d'être pigé
        /// </summary>
        public object Pick(out int index)
        {
            index = -1;

            if (list.Count <= 0)
            {
                Debug.LogError("No lottery item to pick from. Add some before picking.");
                return null;
            }

            float totalWeight = 0;
            foreach (ILotteryItem item in list)
            {
                totalWeight += item.Weight();
            }

            index = 0;

            float ticket = Random.Range(0, totalWeight);
            float currentWeight = 0;
            foreach (ILotteryItem item in list)
            {
                currentWeight += item.Weight();
                if (ticket < currentWeight)
                {
                    if (item is LotteryItem)
                        return (item as LotteryItem).obj;          //Devrais toujours return ici
                    else
                        return item;
                }
                index++;
            }

            Debug.LogError("Error in lotery.");
            return null;
        }
    }
}
