using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObserverPattern;

public class Key : MonoBehaviour, IResetLevel
{
    [Range(0, 2)]
    [SerializeField] private int lockID;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Observer.PostEvent(EvenID.UnlockBlock, lockID);
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
}
