using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public Asteroid asteroidPrefab;
    public float spawnRate = 2.0f;
    public float spawnDistance = 5.0f;
    public int spawnAmount = 2;
    public float trajectoryVariance = 15.0f; //15 degrees and it should cover a decent amount because it's away from the main screen


    // Start is called before the first frame update
    private void Start()
    {
     InvokeRepeating(nameof(Spawn), this.spawnRate, this.spawnRate);   
    }

    private void Spawn()
    {
        for (int i = 0; i < this.spawnAmount; i++)
        {
           
            //Random.insideUnitCircle is a random point on the edge or in a circle
            //we want asteroids to spawn at the edge of the circle never inside the circle
            //Random.insideUnitCircle.normalized normalized sets the magnitude to be 1 and this will do a random point on the edge of the circle
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * this.spawnDistance;

            /*
            without this, our asteroids will always be relative to the origin  
            */
            Vector3 spawnPoint = this.transform.position + spawnDirection;
            
            // we want our asteroids to go towards the inside
            // Calculate a random variance in the asteroid's rotation which will
            // cause its trajectory to change
            float variance = Random.Range(-trajectoryVariance, trajectoryVariance);
            //with rotations we usually represent that with Quaternion
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);
            //Vector3.forward is 0,0,1

            Asteroid asteroid = Instantiate(this.asteroidPrefab, spawnPoint, rotation);
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);
            asteroid.SetTrajectory(rotation * -spawnDirection);
            
        }
    }
}
