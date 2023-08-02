using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPatrol : MonoBehaviour
{
    [SerializeField] Vector2 direction;
    [SerializeField] float speed = 2;
    [SerializeField] float maxDistance = 2;
    [SerializeField] Transform trickStartPosition;

    [Header("Patrol Line Config")]
    [SerializeField] Material patrolLineMaterial;
    [SerializeField] float patrolLineWidth = 0.1f;
    [SerializeField] Color patrolLineColor;

    Vector2 startPosition;

    void Start()
    {
        startPosition = transform.position;
        PaintPatrolLine();

        if(trickStartPosition != null)
        {
            transform.position = trickStartPosition.position;
        }
    }

    private void PaintPatrolLine()
    {
        GameObject patrolLine = new GameObject("patrolLine");
        LineRenderer line = patrolLine.AddComponent<LineRenderer>();
        line.startColor = patrolLineColor;
        line.endColor = patrolLineColor;
        line.startWidth = patrolLineWidth;
        line.endWidth = patrolLineWidth;
        line.SetPosition(0, startPosition + direction * maxDistance);
        line.SetPosition(1, startPosition - direction * maxDistance);
        line.material = patrolLineMaterial;
        line.numCapVertices = 3;
        patrolLine.transform.SetParent(transform);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction.normalized * Time.deltaTime * speed);
        var distance = Vector2.Distance(startPosition, transform.position);
        if (distance >= maxDistance)
        {
            transform.position = startPosition + (direction.normalized * maxDistance);
            direction *= -1;
        }
    }
}
