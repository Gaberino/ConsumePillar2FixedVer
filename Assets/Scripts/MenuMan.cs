using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pixelplacement;
using UnityEngine.SceneManagement;

public class MenuMan : MonoBehaviour
{
    public RawImage fadeImage;
    public bool canStart = false;
    public int gameSceneIndex;
    // Start is called before the first frame update
    void Start()
    {
        fadeImage.color = Color.black;
        Tween.Color(fadeImage, Color.clear, 1f, 0f, Tween.EaseLinear, Tween.LoopType.None, null, () => { canStart = true; });
    }


    void OnConsume()
    {
        if (canStart)
        {
            canStart = false;
            Tween.Color(fadeImage, Color.black, 1f, 0f, Tween.EaseLinear, Tween.LoopType.None, null,
                () => { SceneManager.LoadScene(gameSceneIndex); });
        }
    }

    void OnExit()
    {
        Application.Quit();
    }
}
