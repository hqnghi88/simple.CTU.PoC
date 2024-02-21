using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiceBag : MonoBehaviour
{

    [SerializeField] private float destroyTime = 1;
    [SerializeField] private Vector3 moveVector = new Vector3(0, 0, 0);

    [SerializeField] private float moveSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        var step = moveSpeed * Time.deltaTime;
        Vector3 targetPosition = transform.position + moveVector;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }
}
