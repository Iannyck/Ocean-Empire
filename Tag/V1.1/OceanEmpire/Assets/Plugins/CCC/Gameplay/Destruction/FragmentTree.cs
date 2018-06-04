using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CCC.Gameplay.Destruction
{
    [ExecuteInEditMode(), RequireComponent(typeof(Rigidbody))]
    public class FragmentTree : MonoBehaviour
    {
        public bool debugDraw = false;
        public MeshRenderer intactVisual;
        public FragmentTree newChunkPrefab;
        public List<LinkedShape> linkedShapes;
        [Tooltip("Affecte la force minimal requise pour briser quoi que se soit."), Range(1, 100)]
        public float hardness = 1;
        [Tooltip("Affecte la propagation d'impacte. Plus grande => se propage moins."), Range(0, 30)]
        public float elasticity = 0;

        [HideInInspector]
        private FragmentTree original = null;

        private bool relinkTree = true; // DOIS ÊTRE À TRUE
        private float massPerBlock;
        private Vector3 wasVel;
        private Rigidbody myRb;

        void Start()
        {
            myRb = GetComponent<Rigidbody>();
            if (Application.isPlaying && relinkTree)
            {
                InitTreeLinks();
                massPerBlock = myRb.mass / linkedShapes.Count;
            }
        }

        void Update()
        {
            if (debugDraw && Application.isEditor)
                DrawTree();
            if (Application.isPlaying)
                wasVel = myRb.velocity;
        }

        void InitTreeLinks()
        {
            original = this;
            foreach (LinkedShape linkedShape in linkedShapes)
            {
                linkedShape.neighbors = new List<LinkedShape>(linkedShape.neighborsIndex.Count);
                foreach (int neighbor in linkedShape.neighborsIndex)
                {
                    linkedShape.neighbors.Add(linkedShapes[neighbor]);
                }
            }
        }

        //Retourne une listes des régions déconnectés, n'inclu pas la région mère
        List<List<LinkedShape>> FindSeperatedRegions()
        {
            List<LinkedShape> unpicked = new List<LinkedShape>(linkedShapes);
            List<List<LinkedShape>> newGroups = new List<List<LinkedShape>>();

            bool isMotherRegion = true;

            while (unpicked.Count > 0)
            {
                List<LinkedShape> group = new List<LinkedShape>();
                List<LinkedShape> specials = new List<LinkedShape>(unpicked[0].neighbors);
                group.Add(unpicked[0]);
                unpicked.RemoveAt(0);

                while (specials.Count > 0)
                {
                    group.Add(specials[0]);
                    unpicked.Remove(specials[0]);
                    foreach (LinkedShape item in specials[0].neighbors)
                    {
                        if (!specials.Contains(item) && unpicked.Contains(item))
                            specials.Add(item);
                    }
                    specials.RemoveAt(0);
                }

                if (isMotherRegion)
                    isMotherRegion = false;
                else
                    newGroups.Add(group);
            }

            return newGroups;
        }

        void Break(LinkedShape shape, Vector3 point, Vector3 force, float quantitéDeMouvement, bool addVelocity)
        {
            if (linkedShapes.Count <= 1)
                return;

            if (intactVisual != null)
                intactVisual.enabled = false;

            //double time1 = EditorApplication.timeSinceStartup;
            Vector3 dir = force.normalized;

            float propagation = Mathf.Max(0, 0.025f * force.magnitude / (0.5f + elasticity) / massPerBlock);

            List<List<LinkedShape>> groups = shape.FormGroupsWithNeighbors(propagation, force.normalized, Mathf.Max(linkedShapes.Count / 2, 1), point);


            //Validate neighbors
            int totalInGroups = 0;
            for (int i = 0; i < groups.Count; i++)
            {
                totalInGroups += groups[i].Count;
                foreach (LinkedShape item in groups[i])
                {
                    item.ValidateNeighbors(groups[i]);
                }
            }
            int skipLastGroup = linkedShapes.Count > totalInGroups ? 0 : 1;

            //Remove groups from list
            for (int i = 0; i < groups.Count - skipLastGroup; i++)
            {
                List<LinkedShape> group = groups[i];
                foreach (LinkedShape item in group)
                {
                    linkedShapes.Remove(item);
                }
            }

            //Parcourt de l'arbre pour séparé les région qui ne sont plus en contact
            List<List<LinkedShape>> newGroups = FindSeperatedRegions();
            for (int i = 0; i < newGroups.Count; i++)
            {
                totalInGroups += newGroups[i].Count;
                foreach (LinkedShape item in newGroups[i])
                {
                    item.ValidateNeighbors(newGroups[i]);
                }
            }
            //Remove newGroups from list
            for (int i = 0; i < newGroups.Count; i++)
            {
                foreach (LinkedShape item in newGroups[i])
                {
                    linkedShapes.Remove(item);
                }
            }
            //Ajoute les nouveaux groupes à la liste
            groups.AddRange(newGroups);

            //Kinematic settings
            bool resetKinematic = false;
            if (myRb.isKinematic)
            {
                resetKinematic = true;
                myRb.isKinematic = false;
            }
            Vector3 oldMassCenter = myRb.worldCenterOfMass;


            float velocityChange = quantitéDeMouvement / (totalInGroups * massPerBlock);

            //Nouveaux blocka
            for (int i = 0; i < groups.Count - skipLastGroup; i++)
            {
                List<LinkedShape> group = groups[i];
                FragmentTree newTree = Instantiate(newChunkPrefab.gameObject).GetComponent<FragmentTree>();
                newTree.linkedShapes = group;
                newTree.hardness = hardness;
                newTree.debugDraw = debugDraw;
                newTree.relinkTree = false;
                newTree.newChunkPrefab = newChunkPrefab;
                newTree.elasticity = elasticity;
                newTree.original = original;
                newTree.massPerBlock = massPerBlock;

                Rigidbody rb = newTree.GetComponent<Rigidbody>();
                rb.velocity = myRb.velocity;
                rb.angularVelocity = myRb.angularVelocity;
                rb.mass = newTree.linkedShapes.Count * massPerBlock;
                rb.drag = myRb.drag;
                rb.angularDrag = myRb.angularDrag;
                rb.ResetCenterOfMass();

                foreach (LinkedShape item in group)
                {
                    item.shape.tr.SetParent(newTree.transform, true);
                }

                Vector3 dist = rb.worldCenterOfMass - point;
                float alignment = Vector3.Dot(dist.normalized, dir); //de 0 à 1
                                                                     //float forceRatio = Mathf.Max(1.5f + 0.5f*alignment, 0.1f); // REMOVE
                float distanceFactor = Mathf.Pow(1 / (dist.magnitude + 1), elasticity); // de 0 à 1

                Vector3 addVel = dir * velocityChange * alignment * distanceFactor * 2;
                //print("remains: " + alignment * distanceFactor + "   vel: " + addVel);

                if (addVelocity)
                    rb.AddForceAtPosition(addVel, point, ForceMode.VelocityChange);
                //rb.AddExplosionForce(force.magnitude*forceRatio, point, 10, 0);
            }

            //Reset mass settings
            myRb.ResetCenterOfMass();
            myRb.mass = linkedShapes.Count * massPerBlock;

            if (resetKinematic)
            {
                Vector3 newCenterOfMass = myRb.worldCenterOfMass;

                if (oldMassCenter.y < newCenterOfMass.y)
                    myRb.isKinematic = false;
                else
                    myRb.isKinematic = true;
            }
        }

        public void DrawTree()
        {
            if (linkedShapes == null || linkedShapes.Count <= 1)
                return;

            List<LinkedShape> drawn = new List<LinkedShape>();
            foreach (LinkedShape linkedShape in linkedShapes)
            {
                if (Application.isPlaying)
                    linkedShape.DrawNeighborLinks(drawn);
                else
                    linkedShape.DrawNeighborLinks_editor(linkedShapes, drawn);
                drawn.Add(linkedShape);
            }
        }

        void ClearTree()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.GetComponent<FixedJoint>();
            }
        }

        public void BuildTree()
        {
            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            linkedShapes = new List<LinkedShape>(meshFilters.Length);

            int u = 0;
            //Cree les shapes
            for (int i = 0; i < meshFilters.Length; i++)
            {
                if (meshFilters[i].GetComponent<BoxCollider>() == null && meshFilters[i].GetComponent<MeshCollider>() == null)
                    continue;

                linkedShapes.Add(new LinkedShape(
                    new Shape(
                        meshFilters[i].sharedMesh,
                        meshFilters[i].GetComponent<MeshRenderer>(),
                        meshFilters[i].transform
                        )));
                linkedShapes[u].index = u;
                linkedShapes[u].shape.tr.gameObject.name = "Block " + u;
                u++;
            }


            //Verifie leurs intersection et ajoute les neighbors - attention!!! θ(n^2)
            foreach (LinkedShape linkedShape in linkedShapes)
            {
                int i = 0;
                foreach (LinkedShape other in linkedShapes)
                {
                    if (other != linkedShape && linkedShape.shape.Intersects(other.shape))
                    {
                        linkedShape.neighborsIndex.Add(i);
                        linkedShape.distances.Add((linkedShape.shape.tr.position - other.shape.tr.position).magnitude);
                    }
                    i++;
                }
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.impulse.magnitude < hardness * massPerBlock || linkedShapes.Count <= 1)
                return;

            //Ignore la collision avec sois meme
            if (collision.transform != null && collision.transform.GetComponent<FragmentTree>() != null)
                if (collision.transform.GetComponent<FragmentTree>().original == original)
                    return;

            //Trouve la piece qui s'est fait frappé
            LinkedShape hitShape = null;
            foreach (LinkedShape shape in linkedShapes)
            {
                if (shape.shape.tr == collision.contacts[0].thisCollider.transform)
                {
                    hitShape = shape;
                    break;
                }
            }
            if (hitShape == null)
                return;


            float quantitéDeMouvement = 0;
            if (collision.rigidbody != null)
                quantitéDeMouvement = collision.relativeVelocity.magnitude * collision.rigidbody.mass;
            else
                quantitéDeMouvement = collision.relativeVelocity.magnitude * myRb.mass;

            //Pour fix le 'je tombe sur le sol et je me fait propulsé dans les airs'
            Vector3 otherVel = wasVel + collision.relativeVelocity;
            bool addVelocity = myRb.velocity.magnitude < otherVel.magnitude;

            Break(hitShape, collision.contacts[0].point, -collision.impulse, quantitéDeMouvement, addVelocity);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(FragmentTree))]
    public class FragmentTreeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Application.isPlaying && GUILayout.Button("Build Tree"))
            {
                (target as FragmentTree).BuildTree();
            }
        }
    }
#endif
}