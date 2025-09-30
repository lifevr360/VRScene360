using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    
    public GameObject Map3DObject;
    public GameObject InfoPanel;
    public Image infoPanelDisplayImage;
    public List<Sprite> imageList;

    void Start()
    {
        InfoPanel.SetActive(false);
        if (imageList.Count > 0)
        {
            infoPanelDisplayImage.sprite = imageList[0];
        }
    }

    public void OnMapButtonClick()
    {
        if (Map3DObject != null)
        {
            Map3DObject.SetActive(!Map3DObject.activeSelf);
        }
    }

    public void OnInfoButtonClick()
    {
        if (InfoPanel != null)
        {
            InfoPanel.SetActive(!InfoPanel.activeSelf);
        }
    }

    public void OnExitButtonClick()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void PopulateInfopanel(int index)
    {
        if (index >= 0 && index < imageList.Count)
        {
            infoPanelDisplayImage.sprite = imageList[index];
        }
        else
        {
            Debug.LogWarning("Invalid image index: " + index);
        }
    }

}
