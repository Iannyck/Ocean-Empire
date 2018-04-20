using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class IntroScript : MonoBehaviour {

    public Image uqacLogo;
    public float animDuration;

    public SceneInfo tutorialMap;
    public SceneInfo shack;

    public DataSaver tutorialSaver;

    private const string firstRunKey = "";

	void Start ()
    {
        tutorialSaver.LoadAsync(delegate ()
        {
            var sq = DOTween.Sequence();
            sq.Append(uqacLogo.DOFade(1, animDuration));
            sq.Append(uqacLogo.DOFade(0, animDuration));
            sq.OnComplete(delegate ()
            {
                // First run
                if (tutorialSaver.GetInt(firstRunKey, 0) == 0)
                {
                    tutorialSaver.SetInt(firstRunKey, 1);
                    tutorialSaver.Save();
                    Scenes.Load(tutorialMap);
                }
                else
                {
                    Scenes.Load(shack);
                }
            });
        });
    }
}
