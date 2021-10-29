using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPctrl : MonoBehaviour
{
    public Animator player;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        transform.localScale = new Vector3((float)player.GetInteger("HP") * 0.01f, 1, 1);
    }
}