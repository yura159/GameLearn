using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slider : MonoBehaviour
{
    public Transform Right;
    public Transform Wrong;

    private Collider2D collider;
    private Vector3 startPos;
    private bool answer = false;
    void Start()
    {
        collider = GetComponent<Collider2D>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var position = new Vector2();
        var input = InputCrossPlatform.Instance;
        if (input.IsCursorClickOnCanvas(out position) && !answer)
        {
            if (collider.OverlapPoint(position))
            {
                var x = position.x;
                var minX = Wrong.position.x;
                var maxX = Right.position.x;
                var posX = Mathf.Min(Mathf.Max(x, minX), maxX);
                transform.position = new Vector3(posX, transform.position.y, transform.position.z);
            }
        }
        else
        {
            var distToWrong = Vector2.Distance(Wrong.position, transform.position);
            var distToRight = Vector2.Distance(Right.position, transform.position);
            var minDistanse = 100;
            if (distToWrong < minDistanse)
                SetWrong();
            else if (distToRight < minDistanse)
                SetRight();
            else transform.position = startPos;
        }
    }

    private void SetWrong()
    {
        transform.position = Wrong.position;
        var manager = transform.parent.GetComponent<SliderManager>();
        manager.Result(false);
        answer = true;

    }
    private void SetRight()
    {
        transform.position = Right.position;
        var manager = transform.parent.GetComponent<SliderManager>();
        manager.Result(true);
        answer = true;
    }
}
