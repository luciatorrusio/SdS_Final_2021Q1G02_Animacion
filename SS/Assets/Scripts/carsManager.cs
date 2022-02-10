using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class carsManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject car;
    [SerializeField] private GameObject parentCar;
   
    [SerializeField] private GameObject road;
    [SerializeField] private GameObject parentRoad;
    private int index;
    [SerializeField] private float dif_x;
    [SerializeField] private float dif_z;
    [SerializeField] private float difcar_y;
    private int num_carriles;
    private const int NUM_LARGO = 2000;
    public const int FIRSTCAR_INDEX = 2;
    enum Properties { ID, RADIUS, POSZ,CARRIL,VELX, VELY, COLOR_R, COLOR_G, COLOR_B};
    enum lines { CAR_NUM, PROPERTIES_NAMES, FIRSTCAR_INDEX};
    Dictionary<int, carType> cars;
    Dictionary<int, carType> cars2;
    private class carType
    {
        GameObject obj;
        float speed;
        float acceleration;
        float posCarril;
        float posz;
        public carType()
        {

        }
        public void setobj(GameObject obj)
        {
            this.obj = obj;
        }
        public void setSpeed(float speed)
        {
            this.speed = speed;
        }
        public void setAcceleration(float acceleration)
        {
            this.acceleration = acceleration;
        }
        public void setPosCarril(float posCarril)
        {
            this.posCarril = posCarril;
        }
        public void setPosz(float posz)
        {
            this.posz = posz;
        }

        public GameObject getobj()
        {
            return this.obj;
        }
        public float getSpeed()
        {
            return this.speed;
        }
        public float getAcceleration()
        {
            return this.acceleration;
        }
        public float getPosCarril()
        {
            return this.posCarril ;
        }
        public float getPosz()
        {
            return this.posz;
        }

    }

    void Start()
    {
        // cars representa la lista los autos ahora
        // donde el key es el id y el value es toda la info del auto
        cars = new Dictionary<int, carType>();
        // cars2 representa la lista de los autos en el proximo segundo
        // donde el key es el id y el value es toda la info del auto
        cars2 = new Dictionary<int, carType>();

        // distancia en la que tienen que estar los autos para estar uno al lado del otro en diferentes carriles 
        dif_x = 5.5f;
        // ditantcia en la que tienen que estar los autos para estar pegados en el mimso carril
        dif_z = 7.8f;
        // lo arriba que esta el auto
        difcar_y = 0.35f;
        getStreetSize();
        createStreet();
        index = 1;
        setInitialPos();
        setNextPos();
        InvokeRepeating("moveCars", 1f, 1f);  //1s delay, repeat every 1s TODO: esta bien el delay?
    }
    private void getStreetSize()
    {
        string path = "Assets/output/simulation_l5_q_0.3_d_0.55_lp_5_0.xyz";
        StreamReader reader = new StreamReader(path);
        reader.ReadLine(); // cantidad de autos
        reader.ReadLine(); // descripcion de cada coulmna
        string line_filas = reader.ReadLine(); // primer auto
        string[] car = line_filas.Split(new string[] { "\t" }, System.StringSplitOptions.RemoveEmptyEntries);
        int id = int.Parse(car[(int)Properties.ID]);
        while(id >= 0) // descripcion de carriles cuando el id es negativo
        {
            line_filas = reader.ReadLine();
            car = line_filas.Split(new string[] { "\t" }, System.StringSplitOptions.RemoveEmptyEntries);
            id = int.Parse(car[(int)Properties.ID]);
        }
        num_carriles = int.Parse(car[(int)Properties.CARRIL]);
        reader.Close();
    }
    private void setInitialPos()
    {
        string path = "Assets/output/simulation_l5_q_0.3_d_0.55_lp_5_" + index.ToString() + ".xyz";
        
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        reader.ReadLine();  // cantidad de autos
        reader.ReadLine();  // descripcion de coulmnas
        string line;
        string[] car_properties;
        int id;
        int carril;
        int posz;
        float speed;
        line = reader.ReadLine(); //primer auto
        car_properties = line.Split(new string[] { "\t" }, System.StringSplitOptions.RemoveEmptyEntries);
        id = int.Parse(car_properties[(int)Properties.ID]);
        while (id >= 0)
        {
            GameObject curr_carobj;
            carType curr_car = new carType();
            carril = int.Parse(car_properties[(int)Properties.CARRIL]);
            posz = int.Parse(car_properties[(int)Properties.POSZ]);
            speed = int.Parse(car_properties[(int)Properties.VELX]);
            curr_carobj = Instantiate(car, new Vector3(carril * dif_x, difcar_y, posz * dif_z), car.transform.rotation);
            curr_carobj.transform.parent = parentCar.transform;
            curr_carobj.GetComponent<move>().setSpeed(speed * dif_z);
            curr_carobj.GetComponent<properties>().setID(id);
            curr_car.setobj(curr_carobj);
            curr_car.setSpeed(speed * dif_z);
            curr_car.setPosCarril(carril*dif_x);
            curr_car.setPosz(posz*dif_x);
            cars.Add(id, curr_car);
            
            line = reader.ReadLine();
            car_properties = line.Split(new string[] { "\t" }, System.StringSplitOptions.RemoveEmptyEntries);
            id = int.Parse(car_properties[(int)Properties.ID]);

        }
        reader.Close();

        index++;
    }

    private void setNextPos()
    {
        string path = "Assets/output/simulation_l5_q_0.3_d_0.55_lp_5_" + index.ToString() + ".xyz";
        //print(path);
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        reader.ReadLine();
        reader.ReadLine();
        string line;
        string[] car_properties;
        int id;
        int carril;
        int posz;
        float speed;
        
        
        line = reader.ReadLine(); //leo el primer auto
        car_properties = line.Split(new string[] { "\t" }, System.StringSplitOptions.RemoveEmptyEntries);
        id = int.Parse(car_properties[(int)Properties.ID]);
        while (id >= 0)
        {
            carType curr_car = new carType();
            carril = int.Parse(car_properties[(int)Properties.CARRIL]);
            posz = int.Parse(car_properties[(int)Properties.POSZ]);
            speed = int.Parse(car_properties[(int)Properties.VELX]);
            
            curr_car.setSpeed(speed * dif_z);
            curr_car.setPosCarril(carril * dif_x);
            curr_car.setPosz(posz * dif_z);
            
            cars2.Add(id, curr_car);
            if (cars.ContainsKey(id))
            {
                carType curr_car2;
                cars.TryGetValue(id, out curr_car2);

                float v = Mathf.Sqrt( Mathf.Pow( (curr_car.getPosz() - curr_car2.getPosz()),2 )+ Mathf.Pow( (curr_car.getPosCarril() - curr_car2.getPosCarril()), 2));
                curr_car2.setSpeed(v);

                GameObject c = curr_car2.getobj();

                c.GetComponent<move>().setSpeed(v);
                c.GetComponent<move>().setNextPos(curr_car.getPosCarril(), curr_car.getPosz());

            }

            line = reader.ReadLine();
            car_properties = line.Split(new string[] { "\t" }, System.StringSplitOptions.RemoveEmptyEntries);
            id = int.Parse(car_properties[(int)Properties.ID]);

        }
        reader.Close();

        index++;
    }

    private void createStreet()
    {
        GameObject curr_street;
        print("create street");
        print("num carriles: " + num_carriles);
        for(int x=0; x<num_carriles; x++)
        {
            curr_street = Instantiate(road,new Vector3(x*dif_x, 0 , 1000), road.transform.rotation);
            curr_street.transform.localScale = new Vector3(1, 1, 500);
            curr_street.transform.parent = parentRoad.transform;
        }
    }
   
    
    private  void moveCars()
    {
        // borramos todos los autos que ya no estan
        foreach(KeyValuePair<int, carType> c in cars)
        {
            if (!cars2.ContainsKey(c.Key))
            {
                Destroy(c.Value.getobj());
            }
        }

        foreach(KeyValuePair<int, carType>  c in cars2)
        {
            GameObject curr_carobj;
            carType cart;
            if (cars.ContainsKey(c.Key))
            {
                // movemos el auto
                cars.TryGetValue(c.Key, out cart);
                curr_carobj =  cart.getobj();

                curr_carobj.GetComponent<move>().setPos(c.Value.getPosCarril(), difcar_y, c.Value.getPosz());
                cart.setPosCarril(c.Value.getPosCarril());
                cart.setPosz(c.Value.getPosz());
            }
            else
            {
                //creamos un nuevo auto
                curr_carobj = Instantiate(car, new Vector3(c.Value.getPosCarril(), difcar_y, c.Value.getPosz()), car.transform.rotation);
                curr_carobj.transform.parent = parentCar.transform;
                curr_carobj.GetComponent<properties>().setID(c.Key);
                c.Value.setobj(curr_carobj);
                cars.Add(c.Key, c.Value);
               
            }
        }
        // limpio cars2
        cars2 = new Dictionary<int, carType>();

        setNextPos();

    }

}
