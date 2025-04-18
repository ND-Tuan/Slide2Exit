using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class StageInfo : MonoBehaviour
{
    public int StageIndex => _stageIndex;
    public string StageName => _stageName;
    public Color MainColor => _mainColor;
    public CanvasGroup CanvasGroup => _canvasGroup;

    [SerializeField] private int _stageIndex;
    [SerializeField] private string _stageName;
    [SerializeField] private Color _mainColor;
     private CanvasGroup _canvasGroup;


    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
}
