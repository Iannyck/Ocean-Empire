using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;

/// <summary>
/// Structure de données pour la bulle au dessu d'un objet (Utile pour les discussions textuels)
/// </summary>
public class Bubble : MonoBehaviour
{
    private float currentOffsetX;
    private float currentOffsetZ;

    private MonoBehaviour target;

    public GameObject myText;
    public CCC.Utility.RandomAudioCliptList audioclips;

    private float time;

    void Start()
    {
        if (audioclips.GetList().Count > 0)
        {
            GetComponent<AudioSource>().clip = audioclips.Pick();
            GetComponent<AudioSource>().Play();
        }
    }

    public void Init()
    {
        Destroy(gameObject, time);
    }

    void Update()
    {
        transform.position = new Vector3((target.transform.position.x + currentOffsetX), transform.position.y, (target.transform.position.z + currentOffsetZ));
    }

    public void SetValues(float offsetX, float offsetZ, MonoBehaviour target, float time)
    {
        currentOffsetX = offsetX;
        currentOffsetZ = offsetZ;
        this.target = target;
        this.time = time;
    }
}

