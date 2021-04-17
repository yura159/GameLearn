using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Connection : MonoBehaviour
{ 
    void Start()
    {
        var x = new FileStream(@"Assets\Resources\Sprites\Vector.png", FileMode.Open);

    }
}
