using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class HighScoreTracker : MonoBehaviour
{
    private int highscore;
    private TextMeshProUGUI highscoretext;

    private void Start()
    {
        highscoretext = GetComponent<TMPro.TextMeshProUGUI>();
    }

}
