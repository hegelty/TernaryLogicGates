using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour, IPointerClickHandler
{
    public Button github, email;
    public GameObject settingPanel;

    void Start()
    {
        github.onClick.AddListener(() =>
        {
            Application.OpenURL("https://github.com/hegelty/TernerayLogicGates");
        });
        email.onClick.AddListener(() =>
        {
            Application.OpenURL("mailto:skxodid0305@gmail.com");
        });
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if(settingPanel.activeSelf)
            {
                settingPanel.SetActive(false);
            }
            else
            {
                settingPanel.SetActive(true);
            }
        }
    }
}
