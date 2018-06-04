using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace CCC.Gameplay
{
    public class Bullet : MonoBehaviour
    {
        [Tooltip("Time before the bullet disappear.")]
        public float timeAlive = 4;
        public float speed;
        public LayerMask mask;

        private static List<Bullet> inactiveBullets = new List<Bullet>();
        private float timer = 0;
        private UnityAction hitCallback;

        /// <summary>
        /// Tire un projectile dans une direction à partir d'une position. Lors de la collision, un UnityAction peut être appelé
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="position"></param>
        /// <param name="bulletprefab"></param>
        /// <param name="hitCallback"></param>
        public static void Shoot(Vector3 direction, Vector3 position, Bullet bulletprefab, UnityAction hitCallback = null)
        {
            if (bulletprefab.gameObject == null) return;

            Bullet launchedBullet;
            if (inactiveBullets.Count == 0)     //Create new bullet
            {
                launchedBullet = Instantiate(bulletprefab.gameObject).GetComponent<Bullet>();
            }
            else                                //Use existing bullet
            {
                launchedBullet = inactiveBullets[0];
                inactiveBullets.RemoveAt(0);
            }
            launchedBullet.Init(direction, position, hitCallback);
        }

        public void Init(Vector3 direction, Vector3 position, UnityAction hitCallback = null)
        {
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.position = position;

            this.hitCallback = hitCallback;

            gameObject.SetActive(true);
        }

        void Update()
        {
            //Raycast
            float distance = speed * Time.deltaTime;
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit = new RaycastHit();
            Physics.Raycast(ray, out hit, distance, mask);
            if (hit.collider != null)
            {
                Hit(hit.collider);
            }

            //Move
            transform.position += transform.forward * distance;

            //Decrease remaining life span
            if (timer >= timeAlive)
            {
                gameObject.SetActive(false);
                inactiveBullets.Add(this);
                timer = 0;
                return;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }

        void Hit(Collider col)
        {
            if (col != null && hitCallback != null) hitCallback.Invoke();

            gameObject.SetActive(false);
            inactiveBullets.Add(this);
        }
    }
}
