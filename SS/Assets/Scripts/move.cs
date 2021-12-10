using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    float speed;
    float acceleration;
    Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        speed = 0;
        acceleration = 0;
        target = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + speed * Time.deltaTime + ((float)1 /(float)2) * Mathf.Pow(Time.deltaTime, 2) * acceleration );
        //gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + speed * Time.deltaTime );
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, speed *Time.deltaTime);
        //speed = acceleration * Time.deltaTime + speed;
    }
    public void setSpeed(float speed )
    {
        this.speed = speed;
    }
    public void setAcceleration(float acceleration)
    {
        //print("acceleration now is " +acceleration);
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
