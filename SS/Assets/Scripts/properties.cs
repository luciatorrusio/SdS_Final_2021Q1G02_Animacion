using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class properties : MonoBehaviour
{
    [SerializeField]private GameObject textobject;
    // Start is called before the first frame update
    public void setID(int id)
    {
        textobject.GetComponent<TextMesh>().text = id.ToString();
    }
    public void setColor(float r, float g, float b )
    {

    }
}
