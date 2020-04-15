using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Object bettle;
    GameObject refToPlayer;

    public GameObject PlayerPos {
        set { refToPlayer = value; }
    }

    // Update is called once per frame
    void Update()
    {
        if (refToPlayer != null)
        {
            Vector3 pos = this.transform.position;
            pos.x = refToPlayer.transform.position.x;
            pos.z = refToPlayer.transform.position.z;
            Quaternion rot = refToPlayer.transform.rotation;
            this.transform.position = pos;
            this.transform.rotation = rot;
        }
    }

    public void CreatePlayer()
    {
        Instantiate(bettle);
    }
}
