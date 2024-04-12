using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public GameObject hsuB;
    public GameObject noKeys;
    public GameObject player;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("hsuB contact");
        if (GetComponent<PlayerController>().item >= 4)
        {
            hsuB.SetActive(false);
        }
        else
        {
            Debug.Log("No keys");
            noKeys.SetActive(true);
            Invoke("setBadScreen", 3.0f);
        }
    }

    void setBadScreen()
    {
        noKeys.SetActive(false);
    }

}
