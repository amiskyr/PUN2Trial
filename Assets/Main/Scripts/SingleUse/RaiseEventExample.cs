﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class RaiseEventExample : MonoBehaviourPun
{
    private SpriteRenderer _spriteRenderer;

    private const byte COLOR_CHANGE_EVENT = 0;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (base.photonView.IsMine && Input.GetKeyDown(KeyCode.Space))
            ChangeColor();
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }
    
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
        if(obj.Code == COLOR_CHANGE_EVENT)
        {
            object[] receivedData = (object[])obj.CustomData;
            float r = (float)receivedData[0];
            float g = (float)receivedData[1];
            float b = (float)receivedData[2];
            _spriteRenderer.color = new Color(r, g, b, 1f);
        }
    }

    private void ChangeColor()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);

        _spriteRenderer.color = new Color(r, g, b, 1f);

        object[] data = new object[] { r, g, b };

        PhotonNetwork.RaiseEvent(COLOR_CHANGE_EVENT, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }
}
