using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

namespace CCC.UI.Animation
{
    /// <summary>
    /// Animation to show a new road available like a treasure map. Define your little dot appearance and it will spawn from point A to B
    /// </summary>
    public class RoadMapPoint : MonoBehaviour
    {
        // Countainer
        public GameObject closestParent;

        // Structure de la map
        public RoadMapPoint nextPoint;

        // On Complete Event
        Action onComplete;

        // Animation de la map
        public GameObject dotSpritePrefab;

        // Parameters
        public float roadIntensity = 50; // How close are we following the animation curve ?

        public AnimationCurve curve; // The curve that modify the straight line between point A to B
        [Header("                                          Curve From 0 to 0 with variation")] // TODO : Fix display of this header
        public float timeBetweenDots = 1;
        public float dotDensity = 50; // How much dots there will be during the road
        public int screenWidthToMatch = 1920; // The width of the screen you would like to compare (not the actual screen width)

        // Variables
        private int dotCount = 0;
        private Vector3 vectorDeplacement;
        private float pathLenght;
        private Vector3 vectorBetween;

        [HideInInspector]
        public List<GameObject> dotList = new List<GameObject>(); // Access this to get all dots position

        void Start()
        {
            roadIntensity = (roadIntensity * Screen.width) / screenWidthToMatch;
        }

        /// <summary>
        /// Call this function to start the road animation
        /// </summary>
        /// <param name="onComplete">Callback executed when the road is finished</param>
        public void StartRoad(Action onComplete)
        {
            // If there isn't a next dot
            if (nextPoint == null)
                return;

            if (onComplete != null)
                this.onComplete = onComplete;

            Vector2 adjustFactor;

            adjustFactor.x = Screen.width / 1920;
            adjustFactor.y = Screen.height / 1080;

            // Calculate data for dots anims
            vectorBetween = nextPoint.transform.position - transform.position;
            pathLenght = vectorBetween.magnitude;
            vectorDeplacement = vectorBetween / Mathf.Floor(pathLenght / dotDensity);

            // Add first dot
            dotList.Add(Instantiate(dotSpritePrefab, transform.position, Quaternion.identity, closestParent.transform));
            dotCount++;

            // Keep doing the road after a delay
            StartCoroutine(MakeRoad());
        }

        IEnumerator MakeRoad()
        {
            yield return new WaitForSeconds(timeBetweenDots);
            dotList.Add(Instantiate(dotSpritePrefab, ApplyCurveOnVecPos((transform.position + (vectorDeplacement * dotCount))), Quaternion.identity, closestParent.transform));
            dotCount++;
            Vector2 positionCurrentDot = dotList[dotList.Count - 1].GetComponent<RectTransform>().localPosition;
            Vector2 positionPointSuivant = nextPoint.GetComponent<RectTransform>().localPosition;
            if (AreClose(positionCurrentDot, positionPointSuivant, 10))
            {
                onComplete.Invoke();
                yield break;
            }
            // Keep doing the road after a delay
            StartCoroutine(MakeRoad());
        }

        Vector3 ApplyCurveOnVecPos(Vector2 currentPos)
        {
            Vector3 currentPosV3 = new Vector3(currentPos.x, currentPos.y, 0); // Position actuel
            float lenghtFromStart = (currentPosV3 - transform.position).magnitude;
            Vector3 perpendicularVec = Vector3.Cross(vectorBetween, Vector3.forward).normalized;
            Vector3 modifyingFactor = -1 * (perpendicularVec * (curve.Evaluate(lenghtFromStart / pathLenght) * roadIntensity));
            return currentPosV3 + modifyingFactor;
        }

        bool AreClose(Vector2 obj1, Vector2 obj2, float minDistance = 10)
        {
            return ((obj1.x < obj2.x + minDistance && obj1.x > obj2.x - minDistance) &&
                (obj1.y < obj2.y + minDistance && obj1.y > obj2.y - minDistance));
        }
    }
}
