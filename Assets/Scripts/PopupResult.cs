using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupResult : MonoBehaviour
{
    [SerializeField] private GameObject objWin;
    [SerializeField] private GameObject objDefeat;
    [SerializeField] private TextMeshProUGUI txtPoint;
    [SerializeField] private Button btnRestart;

    void Start()
    {
        btnRestart.onClick.AddListener(OnClickRestart);
    }

    public void Show(bool isWin, int point)
    {
        objWin.SetActive(isWin);
        objDefeat.SetActive(!isWin);
        txtPoint.text = point.ToString();
        DOVirtual.DelayedCall(2.5f, () =>
        {
            gameObject.SetActive(true);
        });
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnClickRestart()
    {
        GameControl.Ins.RestartGame();
    }
}
