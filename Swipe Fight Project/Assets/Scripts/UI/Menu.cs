using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject opponent;
    [SerializeField] GameObject inputManager;

    public void StartMatch(int difficulty)
    {
        player.SetActive(true);
        opponent.SetActive(true);
        opponent.GetComponent<AIController>().difficulty = difficulty;
        inputManager.SetActive(true);

        gameObject.SetActive(false);
    }
}
