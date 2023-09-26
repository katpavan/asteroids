using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Sprite[] sprites;
    public float size = 1.0f;
    public float minSize = 0.5f;
    public float maxSize = 1.5f;
    public float maxLifetime = 30.0f;

    public float movementSpeed = 5.0f;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //randomize asteroid image
        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

        //randomize rotation
        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);

        //randomize scale
        this.transform.localScale = Vector3.one *  this.size; 
        _rigidbody.mass = this.size * 2.0f;
    }

    public void SetTrajectory(Vector2 direction)
    {
        // The asteroid only needs a force to be added once since they have no
        // drag to make them stop moving
        _rigidbody.AddForce(direction * movementSpeed);
        Destroy(this.gameObject, maxLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Check if the asteroid is large enough to split in half
            // (both parts must be greater than the minimum size)
            if ((size * 0.5f) >= minSize)
            {
                CreateSplit();
                CreateSplit();
            }

            // GameManager.Instance.OnAsteroidDestroyed(this);
            FindObjectOfType<GameManager>().OnAsteroidDestroyed(this);
            
            // Destroy the current asteroid since it is either replaced by two
            // new asteroids or small enough to be destroyed by the bullet
            Destroy(gameObject);
        }
    } 

    //function is returning an Asteroid so we have to label the return as such
    private Asteroid CreateSplit(){
        // Set the new asteroid poistion to be the same as the current asteroid
        // but with a slight offset so they do not spawn inside each other
        Vector2 position = transform.position;
        position += Random.insideUnitCircle * 0.5f;

        // Create the new asteroid at half the size of the current
        Asteroid half = Instantiate(this, position, transform.rotation);
        half.size = size * 0.5f;

        // Set a random trajectory
        half.SetTrajectory(Random.insideUnitCircle.normalized * this.movementSpeed);

        return half;
    }
}
