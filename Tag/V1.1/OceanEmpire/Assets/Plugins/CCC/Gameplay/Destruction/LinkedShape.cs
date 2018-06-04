using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.Gameplay.Destruction
{
    [System.Serializable]
    public class LinkedShape
    {
        public int index;
        public List<int> neighborsIndex = new List<int>();
        public List<float> distances = new List<float>();
        [System.NonSerialized]
        public List<LinkedShape> neighbors = new List<LinkedShape>();
        public Shape shape;

        private float cost;

        public LinkedShape(Shape shape)
        {
            this.shape = shape;
        }

        public void ValidateNeighbors(List<LinkedShape> validNeighbors)
        {
            for (int i = 0; i < neighbors.Count; i++)
            {
                if (!validNeighbors.Contains(neighbors[i]))
                {
                    //Remove this from neighbor
                    int neighborIndex = neighbors[i].neighbors.IndexOf(this);
                    neighbors[i].neighbors.RemoveAt(neighborIndex);
                    neighbors[i].distances.RemoveAt(neighborIndex);

                    //Remove neighbor from this
                    neighbors.RemoveAt(i);
                    distances.RemoveAt(i);
                    i--;
                }
            }
        }

        public void DrawNeighborLinks_editor(List<LinkedShape> completeList, List<LinkedShape> exclude)
        {
            foreach (int neighbor in neighborsIndex)
            {
                if (!exclude.Contains(completeList[neighbor]))
                {
                    Debug.DrawLine(shape.tr.position, completeList[neighbor].shape.tr.position, Color.red);
                }
            }
        }
        public void DrawNeighborLinks(List<LinkedShape> exclude)
        {
            foreach (LinkedShape neighbor in neighbors)
            {
                if (!exclude.Contains(neighbor))
                {
                    Debug.DrawLine(shape.tr.position, neighbor.shape.tr.position, Color.red);
                }
            }
        }

        public void AddToPickedList(List<LinkedShape> picked, List<LinkedShape> potentials, Vector3 costDir, Vector3 originImpact)
        {
            picked.Add(this);
            foreach (LinkedShape neighbor in neighbors)
            {
                if (picked.Contains(neighbor))
                    continue;

                //Cost evaluation
                Vector3 dir = neighbor.shape.tr.position - originImpact;
                float alignment = Vector3.Dot(dir.normalized, costDir);
                float relativeCost = Mathf.Min(1.1f - alignment, 1);// va varier entre 0.1 et 1
                float realCost = this.cost + relativeCost;

                //Add to list OR adjust cost
                if (potentials.Contains(neighbor))
                {
                    neighbor.cost = Mathf.Min(neighbor.cost, realCost);
                }
                else
                {
                    neighbor.cost = realCost;
                    potentials.Add(neighbor);
                }
            }
        }


        public List<List<LinkedShape>> FormGroupsWithNeighbors(float propagate, Vector3 propagationDirection, int maxGroupSize, Vector3 originPoint)
        {
            List<LinkedShape> picked = new List<LinkedShape>();
            List<LinkedShape> potentials = new List<LinkedShape>();
            propagationDirection.Normalize();
            //propagate *= 0.6f;

            this.cost = 0;
            potentials.Add(this);

            bool hasPicked = true;
            while (hasPicked)
            {
                hasPicked = false;

                LinkedShape recorder = null;
                float record = propagate;

                //Trouve le potentiel noeud le moins cher. Les noeuds avec un coup plus grand que 'propagate' ne seront jamais choisient
                foreach (LinkedShape item in potentials)
                {
                    if (item.cost <= record)
                    {
                        record = item.cost;
                        recorder = item;
                    }
                }

                //Ajoue d'un nouveau noeud
                if (recorder != null)
                {
                    potentials.Remove(recorder);
                    recorder.AddToPickedList(picked, potentials, propagationDirection, originPoint);
                    hasPicked = true;
                }
            }

            List<List<LinkedShape>> groupList = new List<List<LinkedShape>>();

            foreach (LinkedShape item in picked)
            {
                groupList.Add(new List<LinkedShape>(1) { item });
            }

            return groupList;
        }
    }
}