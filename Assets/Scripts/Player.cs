using UnityEngine;

public class Player : MonoBehaviour
{
    public Bullet bulletPrefab;
    public float turnSpeed = 0.1f;
    public float thrustSpeed = 1.0f;
    private Rigidbody2D _rigidbody;
    private bool _thrusting; 

    private float _turnDirection;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>(); //this grabs the rigid body 2d component from the player game object
    }

    // Update is called once per frame
    void Update()
    {
        _thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _turnDirection = 1.0f;
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _turnDirection = -1.0f;
        }else 
        {
            _turnDirection = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

    }

    private void FixedUpdate()
    {
        if (_thrusting)
        {
            //public void AddForce(Vector3 force, ForceMode mode = ForceMode.Force);
            //force	Force vector in world coordinates.
            //mode	Type of force to apply.
            //Adds a force to the Rigidbody.
            //Force is applied continuously along the direction of the force vector. Specifying the ForceMode mode allows the type of force to be changed to an Acceleration, Impulse or Velocity Change.
            _rigidbody.AddForce(this.transform.up * this.thrustSpeed);
        }

        if (_turnDirection != 0.0f)
        {
            _rigidbody.AddTorque(_turnDirection * this.turnSpeed);
        }
    }

    public void Shoot(){
        Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation);
        bullet.Project(this.transform.up);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = 0f;

            this.gameObject.SetActive(false);

            //we can't invoke here because the gameObject is off. So we have to make a game manager
            
            //GameManager.Instance.OnPlayerDeath(this); //you can do this if you make the GameManager script a singleton

            //really slow function
            //really bad to use this inside an update
            //it looks through every game object in the game
            FindObjectOfType<GameManager>().OnPlayerDeath();
        }
    }
}
