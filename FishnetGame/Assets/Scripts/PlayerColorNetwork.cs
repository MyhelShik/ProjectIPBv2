// Modified Code
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;

public class PlayerColorNetwork : NetworkBehaviour
{
    public GameObject body;
    public Color endColor;  // Change the type to Color

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
        {
            GetComponent<PlayerColorNetwork>().enabled = false;
        }
    }

    private void Update()
    {
        if (base.IsOwner && Input.GetKeyDown(KeyCode.F))
        {
            ChangeColorServer(gameObject, endColor);
        }
    }

    [ServerRpc]
    public void ChangeColorServer(GameObject player, Color color)  // Change parameter type to Color
    {
        ChangeColor(player, color);
    }

    [ObserversRpc]
    public void ChangeColor(GameObject player, Color color)
    {
        player.GetComponent<PlayerColorNetwork>().body.GetComponent<Renderer>().material.color = color;
    }
}
