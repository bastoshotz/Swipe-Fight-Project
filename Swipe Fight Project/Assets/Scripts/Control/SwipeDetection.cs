using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{

    [SerializeField] private float minimunDistance = .2f;
    [SerializeField] private float maximumTime = 1f;
    [SerializeField][Range(0f, 1f)] private float directionThreshold = .5f;
    [SerializeField] private GameObject trail;

    private InputManager inputManager;
    private Fighter player;

    private Vector2 startPosition;
    private Vector2 endPosition;
    private float startTime;
    private float endTime;
    private Dictionary<Vector2, string> cardinals = new Dictionary<Vector2, string>();

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        player = FindObjectsOfType<Fighter>().Where(f => f.tag == "Player").FirstOrDefault();

        InitiateCardinals();
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
    }
    private void InitiateCardinals()
    {
        cardinals.Add(new Vector2(0, 1), "north");
        cardinals.Add(new Vector2(0, -1), "south");
        cardinals.Add(new Vector2(1, 0), "west");
        cardinals.Add(new Vector2(-1, 0), "east");

        cardinals.Add(new Vector2(1, 1), "northWest");
        cardinals.Add(new Vector2(-1, 1), "northEast");
        cardinals.Add(new Vector2(1, -1), "southWest");
        cardinals.Add(new Vector2(-1, -1), "southEast");
    }

    private void SwipeStart(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;

        trail.SetActive(true);
        trail.transform.position = position;
        StartCoroutine("Trail");
    }

    private void SwipeEnd(Vector2 position, float time)
    {
        endPosition = position;
        endTime = time;

        trail.SetActive(false);
        StopCoroutine("Trail");

        DetectSwipe();
    }

    private IEnumerator Trail()
    {
        while (true)
        {
            trail.transform.position = inputManager.PrimaryPosition();
            yield return null;
        }
    }

    private void DetectSwipe()
    {
        if (Vector3.Distance(startPosition, endPosition) >= minimunDistance &&
            (endTime - startTime) <= maximumTime)
        {
            Vector3 direction = endPosition - startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
        }
    }

    private void SwipeDirection(Vector2 direction)
    {
        Vector2 dir = Vector2.zero;

        if (Vector2.Dot(Vector2.up, direction) > directionThreshold) dir.y = 1;
        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold) dir.y = -1;

        if (Vector2.Dot(Vector2.left, direction) > directionThreshold) dir.x = -1;
        else if (Vector2.Dot(Vector2.right, direction) > directionThreshold) dir.x = 1;

        cardinals.TryGetValue(dir, out var cardinal);
        player.ExecuteCommand(cardinal);
    }

}
