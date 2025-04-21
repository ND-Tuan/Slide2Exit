using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ObserverPattern;
using System;

public class Teleporter : MonoBehaviour
{
    private ParticleSystem[] teleportParticles;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Range(0, 5)]
    [SerializeField] private int TeleporterID;
    [SerializeField] private bool isTeleporting = false;

    void  Awake()
    {
        Observer.AddListener(EvenID.Teleport, TeleportPlayer);
    }

    void Start()
    {   
        teleportParticles = GetComponentsInChildren<ParticleSystem>();

        ParticleSystem.MainModule mainModule;
        foreach (ParticleSystem particle in teleportParticles)
        {
            mainModule = particle.main;
            mainModule.startColor = spriteRenderer.color;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTeleporting)
        {
            Observer.PostEvent(EvenID.Teleport, new object[] {TeleporterID, other.gameObject, this.gameObject});
            isTeleporting = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isTeleporting = false;
        }
    }

    private void TeleportPlayer(object[] data)
    {
        int teleporterID = (int)data[0];
        GameObject teleporter = (GameObject)data[2];
       
        if (teleporterID == TeleporterID && teleporter != this.gameObject)
        {
            isTeleporting = true;

            //await Task.Delay(10); // Simulate teleportation delay

            GameObject player = (GameObject)data[1];
            player.transform.position = transform.position;

        }
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(EvenID.Teleport, TeleportPlayer);
    }

    private void OnValidate()
    {
        ColorChanger.ChangeColor(spriteRenderer, TeleporterID);
    }
}
