using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    float speed;
    float acceleration;
    Vector3 target;

    void Start()
    {
        speed = 0;
        acceleration = 0;
        target = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // el ultimo parametro es "distancia que recorre en un segundo * Time.deltaTime"
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, speed *Time.deltaTime);
    }
    public void setSpeed(float speed )
    {
        this.speed = speed;
    }
    public void setAcceleration(float acceleration)
    {
        this.acceleration = acceleration;
    }

    public void setPos(float x, float y,float z)
    {
        gameObject.transform.position = new Vector3(x,y,z);
    }
    
    // x = carril; z = where en el carril
    public void setNextPos(float x, float z)
    {
        target.x = x;
        target.z = z;
    }
}
