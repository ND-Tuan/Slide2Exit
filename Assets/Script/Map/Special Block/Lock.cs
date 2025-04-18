using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ObserverPattern;

public class Lock : MonoBehaviour
{
    [Range(0, 2)]
    [SerializeField] private int lockID;

    private void Awake()
    {
        Observer.AddListener(EvenID.UnlockBlock, UnlockBlock);
    }

    private void UnlockBlock(object[] data)
    {
        if ((int)data[0] == lockID)
        {
            gameObject.SetActive(false);
        }
    }

    public void ResetLevel()
    {
        gameObject.SetActive(true);
    }

    void OnValidate()
    {
        switch (lockID)
        {
            case 0:
                GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case 1:
                GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case 2:
                GetComponent<SpriteRenderer>().color = Color.magenta;
                break;
        }
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(EvenID.UnlockBlock, UnlockBlock);
    }
}

