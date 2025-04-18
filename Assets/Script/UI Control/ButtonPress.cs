using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPress : MonoBehaviour
{
    private Button button;
    [SerializeField] private Image Icon;
    [SerializeField] private AudioClip buttonPressSound;


    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonPress);
    }

    private void OnButtonPress()
    {
        
    }

}
