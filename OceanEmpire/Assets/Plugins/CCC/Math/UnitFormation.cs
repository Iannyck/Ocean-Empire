using UnityEngine;
using System.Collections;

namespace CCC.Utility
{
    public class UnitFormation
    {
        /// <summary>
        /// Return a vector3 offset from the center of a formation group.
        /// </summary>
        /// <param name="index">The index of the unit</param>
        /// <param name="firstLayerCount">The amount of unit on the first layer of formation</param>
        /// <param name="firstLayerRadius">The radius of the first layer.</param>
        /// <param name="randomness">The amount of randomness in the final position. Best left between 0 and 0,3f.</param>
        /// <returns></returns>
        public static Vector3 GetCircularOffset(int index, int firstLayerCount = 6, float firstLayerRadius = 5, float randomness = 0)
        {
            if (firstLayerCount <= 0 || firstLayerRadius <= 0)
            {
                Debug.LogError("Error making formation. firstLevelCount and firstLevelRadius must be > 0.");
                return Vector3.zero;
            }
            int etage = 1;
            //Determine l'etage
            int nm = 0;
            for (int i = 1; i < index; i++)
            {
                nm += i * firstLayerCount;
                if (nm > index)
                {
                    etage = i;
                    break;
                }
            }
            

            float distance = etage * firstLayerRadius;
            float x = 0, z = 1;
            float newX, newZ;
            float angle;

            int membreSurEtage = etage * firstLayerCount;
            angle = (index % membreSurEtage) * (Mathf.PI * 2 / membreSurEtage);

            angle *= Random.Range(1 - randomness, 1 + randomness);
            distance *= Random.Range(1 - randomness, 1 + randomness);

            newX = x * Mathf.Cos(angle) - z * Mathf.Sin(angle);
            newZ = x * Mathf.Sin(angle) + z * Mathf.Cos(angle);

            return new Vector3(newX, 0, newZ) * distance;
        }
    }
}
