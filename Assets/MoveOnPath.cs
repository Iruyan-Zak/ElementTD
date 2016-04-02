using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using System.Linq;
using System.Collections;

public class MoveOnPath : MonoBehaviour
{
    Vector3 distination;

    [SerializeField]
    float timeToRotateSec;

    [SerializeField]
    float speed;

    const string groundLayerName = "Ground";

    // Use this for initialization
    void Awake()
    {
        distination = transform.position;
        RaycastHit hit;

        this.OnMouseUpAsObservable()
            .SkipUntil(this.OnMouseDownAsObservable())
            .Select(unit => Input.mousePosition)
            .Repeat()
            .Subscribe(v =>
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(v), out hit, Mathf.Infinity, LayerMask.GetMask(groundLayerName)))
                {
                    distination = hit.point;

                    var theta1 = 90 - transform.rotation.eulerAngles.y;
                    var movingDirection = new Vector2(distination.x - transform.position.x, distination.z - transform.position.z).normalized;
                    var theta2 = Mathf.Atan2(movingDirection.y, movingDirection.x) * Mathf.Rad2Deg;

                    var angle = theta2 - theta1;
                    if (angle > 180) angle -= 360;
                    if (angle < -180) angle += 360;

                    var omega = angle / timeToRotateSec;
                    this.UpdateAsObservable()
                        .TakeUntil(Observable.Timer(TimeSpan.FromSeconds(timeToRotateSec)))
                        .Subscribe(unit =>
                        {
                            transform.Rotate(Vector3.down, omega * Time.deltaTime);
                        });
                }
            });
    }

    void FixedUpdate()
    {
        var direction = distination - transform.position;
        if (direction.magnitude > speed / 2)
        {
            GetComponent<CharacterController>().Move(direction.normalized * speed);

        }
    }
}
