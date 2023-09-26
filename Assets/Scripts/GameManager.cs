using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // public static GameManager Instance { get; private set; }

    public int score = 0; //{ get; private set; }
    public float respawnTime = 3.0f;
    public int lives = 3; //{ get; private set; }
    public ParticleSystem explosion;
    [SerializeField] private Player player;
    public float respawnInvulnerabilityTime = 3.0f;
    // [SerializeField] private ParticleSystem explosionEffect;
    // [SerializeField] private GameObject gameOverUI;
    // [SerializeField] private Text scoreText;
    // [SerializeField] private Text livesText;

    public void OnAsteroidDestroyed(Asteroid asteroid)
    {
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();

        //increase score based on the size of the asteroid you shoot. smaller is harder. so smaller gets more points.
        if (asteroid.size < 0.7f) {
           score = score + 100; // small asteroid
        } else if (asteroid.size < 1.4f) {
           score = score + 50; // medium asteroid
        } else {
           score = score + 25; // large asteroid
        }

    }

    public void OnPlayerDeath()
    {
        this.player.gameObject.SetActive(false);
        
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();

        this.lives--;

        if (this.lives <= 0)
        {
            GameOver();
        } else
        {
            Invoke(nameof(Respawn), this.respawnTime);
        }
    }

    private void Respawn()
    {
        //what if you respawn into an asteroid
        this.player.transform.position = Vector3.zero;

        //better to do the following in an OnEnable and OnDisable lifecycle function
        //we should give temporary invulnerability 
        //we do this by changing the player's layer to IgnoreCollisions, and we made it so that layer had no collisions on it
        this.player.gameObject.layer = LayerMask.NameToLayer("IgnoreCollisions");
        this.player.gameObject.SetActive(true);

        //turn collisions back on after respawnInvulnerabilityTime
        //this needs to be invoked on the game manager object
            //not the player, because the TurnOncollisions function exists here and not in the player script
        Invoke(nameof(TurnOnCollisions), this.respawnInvulnerabilityTime);
    }

    private void TurnOnCollisions()
    {
        this.player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void GameOver()
    {
        this.lives = 3;
        this.score = 0;
        Invoke(nameof(Respawn), this.respawnTime);
    }
}