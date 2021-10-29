using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentHP : MonoBehaviour
{
    public Player player;
    private RectTransform rectTransform;
    public Vector2 offset;
    public Vector2 multiplier;
    private HPctrl HPctrl;

    // Start is called before the first frame update
    private void Start()
    {
        HPctrl = GetComponentInChildren<HPctrl>();
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (player.inEnemyRange)
        {
            HPctrl.player = player.opponent.GetComponent<Animator>();
            rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(player.opponent.position);
            float x = rectTransform.anchoredPosition.x * multiplier.x + offset.x;
            float y = rectTransform.anchoredPosition.y * multiplier.y + offset.y;
            rectTransform.anchoredPosition = new Vector2(x, y);
        }
        else
            rectTransform.anchoredPosition = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
    }
}