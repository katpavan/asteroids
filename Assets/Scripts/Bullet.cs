using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 500.0f; 
    public float maxLifetime = 10.0f; 
    private Rigidbody2D _rigidbody;
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>(); //this grabs the rigid body 2d component from the player game object
    }

    public void Project(Vector2 direction) 
    { 
        _rigidbody.AddForce(direction * this.speed); 
        Destroy(this.gameObject, this.maxLifetime);
    }
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        Destroy(this.gameObject);
    }
}
