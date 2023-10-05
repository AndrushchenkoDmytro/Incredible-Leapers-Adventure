using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private bool moveUp = true;
    [SerializeField] private float moveDistance = 2;
    [SerializeField] private float speed = 1;
    private float t = 0;
    private Vector3 startPosition = Vector3.zero;
    private Vector3 endPosition = Vector3.zero;
    [SerializeField] DoorButton doorButton;

    private void Awake()
    {
        startPosition = transform.localPosition;
        endPosition = startPosition;
        endPosition.y = moveDistance;
        doorButton.OnDoorButtonPressed += OpenDoor;
    }

    private void OnDestroy()
    {
        doorButton.OnDoorButtonPressed -= OpenDoor;
    }

    public void OpenDoor()
    {
        StartCoroutine(DoorOpening());
    }

    IEnumerator DoorOpening()
    {
        if (moveUp)
        {
            t = 0;
            while (transform.localPosition.y < endPosition.y)
            {
                transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
                t += Time.deltaTime * speed;
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            t = 1;
            while (transform.localPosition.y > endPosition.y)
            {
                transform.localPosition = Vector3.Lerp(endPosition, startPosition, t);
                t -= Time.deltaTime * speed;
                yield return new WaitForFixedUpdate();
            }
        }
        yield return new WaitForFixedUpdate();
        yield break;
    }
}
