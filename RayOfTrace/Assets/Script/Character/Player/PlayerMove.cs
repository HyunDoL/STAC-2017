﻿using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Transform playerTrans;

    private void Awake()
    {
        playerTrans = InGameManager.Instance.PlayerDataContainer_readonly.PlayerTrans;
    }

    public void Move(Vector2 direction)
    {
        playerTrans.Translate(direction * speed * Time.deltaTime);

        if(!(direction.x > 0 && playerTrans.localScale.x > 0) && !(direction.x < 0 && playerTrans.localScale.x < 0))
        {
            playerTrans.localScale = new Vector3(playerTrans.localScale.x * -1,
                                                             playerTrans.localScale.y,
                                                             playerTrans.localScale.z);
        }
    }
}