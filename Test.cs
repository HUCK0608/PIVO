using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    private float _upHeight = 0f;

    [SerializeField]
    private float _moveSpeed = 0f;

    [SerializeField]
    private float _nexProcessTime = 1f;

    [SerializeField]
    private float _minRandomTime = 0f, _maxRandomTime = 1f;

    private List<List<Transform>> _lists = new List<List<Transform>>();

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            List<Transform> t = new List<Transform>();
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
                t.Add(transform.GetChild(i).GetChild(j));

            _lists.Add(t);

            foreach (Transform obj in _lists[i])
                obj.position -= Vector3.up * _upHeight;
        }

        StartCoroutine(MainLogic());
    }

    private IEnumerator MainLogic()
    {
        int currentFloor = 0;

        while(!currentFloor.Equals(transform.childCount))
        {
            StartCoroutine(UpLogic(currentFloor));
            currentFloor++;
            yield return new WaitForSeconds(_nexProcessTime);
        }
    }

    private IEnumerator UpLogic(int floor)
    {
        foreach (Transform obj in _lists[floor])
        {
            Debug.Log(obj.name);
            StartCoroutine(UpTransform(obj));
            yield return new WaitForSeconds(Random.Range(_minRandomTime, _maxRandomTime));
        }
    }

    private IEnumerator UpTransform(Transform a)
    {
        Vector3 finalPoint = a.position + Vector3.up * _upHeight;
        Debug.Log(a.position);
        Debug.Log(finalPoint);

        while(true)
        {
            a.position = Vector3.MoveTowards(a.position, finalPoint, _moveSpeed * Time.deltaTime);

            if (transform.position.Equals(finalPoint))
                break;

            yield return null;
        }
    }
}
