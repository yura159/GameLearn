using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMove : MonoBehaviour
{
    public static ItemMove current;
    public List<GameObject> boxes;

    private BoxCollider2D collider;
    private Vector3 startPos;
    private Transform startParent;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        startPos = transform.position;
        startParent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        var position = new Vector2();
        var input = InputCrossPlatform.Instance;
        if (input.IsCursorClickOnCanvas(out position))
        {
            if (collider.OverlapPoint(position) && (current == null || current == this))
            {
                transform.position = new Vector3(position.x, position.y, transform.position.z);
                current = this;
            }
        }
        else
        {
            current = null;
            foreach(var box in boxes)
            {
                var distToBox = Vector2.Distance(box.transform.position, transform.position);
                if(distToBox <= 50 && (box.transform.childCount == 0 || box.transform.GetChild(0) == transform))
                {
                    transform.position = box.transform.position;
                    transform.SetParent(box.transform);
                    return;
                }
            }
            transform.position = startPos;
            transform.SetParent(startParent);
        }
    }
}
