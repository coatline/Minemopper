using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSetup : MonoBehaviour
{
    [SerializeField] TMP_InputField lWidth;
    [SerializeField] TMP_InputField lHeight;
    [SerializeField] TMP_InputField lMines;
    [SerializeField] Animator warning;

    private void Start()
    {
        lWidth.text = LD.levelSize.x.ToString();
        lHeight.text = LD.levelSize.y.ToString();
        lMines.text = LD.minePercentage.ToString();
    }

    public void TryPlay()
    {
        if (lWidth.text.Length == 0 || lHeight.text.Length == 0 || lMines.text.Length == 0)
        {
            warning.Play("Fade Out");
            return;
        }

        LD.levelSize.x = int.Parse(lWidth.text);
        LD.levelSize.y = int.Parse(lHeight.text);
        LD.minePercentage = int.Parse(lMines.text);

        SceneManager.LoadScene(1);
    }
}
