﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;

    [SerializeField]
    private PlayerListing _playerListing;

    private List<PlayerListing> _listings = new List<PlayerListing>();

    private void Awake()
    {
        GetCurrentRoomPlayers();
    }

    /// <summary>
    /// creating a list of all the players currently in room
    /// </summary>
    private void GetCurrentRoomPlayers()
    {
        // perform error check to make sure we are in a room
        
        // iterating through the Dictionary<int, Player> to add them to the _playerListing list
        foreach (KeyValuePair<int, Player> playerinfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerinfo.Value);
        }
    }

    /// <summary>
    /// Adding palyer to the local player list(_playerListting)
    /// </summary>
    /// <param name="player"></param>
    private void AddPlayerListing(Player player)
    {
        // instantiating list item in scroll view as a child of content
        PlayerListing listing = Instantiate(_playerListing, _content);

        if (listing != null)
        {
            // setting the values of list item after instantiating it
            listing.SetPlayerInfo(player);
            _listings.Add(listing);
        }

    }

    // PUN Callback method for player entering room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // adding player to the list
        AddPlayerListing(newPlayer);
    }

    // PUN callback method
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // locate the index for the room in the list through name which has been deleted
        int index = _listings.FindIndex(x => x.Player == otherPlayer);

        // if we find the room item being removed in the local list then destroy its gameObject in the UI
        if (index != -1)
        {
            Destroy(_listings[index].gameObject);
            _listings.RemoveAt(index);
        }
    }
}
