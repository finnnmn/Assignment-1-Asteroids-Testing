using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidGame
{
    public class Asteroid : MonoBehaviour
    {
        [SerializeField] int size = 3;
        [SerializeField] int health;
        [SerializeField] int score;
        [SerializeField] float speed;
        [SerializeField] float speedOffset = 1f;
        [SerializeField] Vector2 rotationRange = new Vector2(0.3f, 1f);
        [SerializeField] int splitCount = 2;

        [SerializeField] GameObject breakParticles;

        Vector2 direction;
        float rotationSpeed = 1;
        public bool fromSplit;
        float minPoint;
        private void Start()
        {
            SetupAsteroid();
        }

        void SetupAsteroid()
        {
            //direction is completelt random if asteroid comes from another broken one
            if (fromSplit)
            {
                direction = new Vector2(Random.Range(-1f, 1f), -1);
            }
            else
            {
                //if on the right side of the screen point left and vice versa
                if (transform.position.x > (GameControl.instance.screenMaxPoint.x - GameControl.instance.screenMinPoint.x) / 2)
                {
                    direction = new Vector2(Random.Range(-1f, 0.1f), -1);
                }
                else
                {
                    direction = new Vector2(Random.Range(-0.1f, 1f), -1);
                }
            }

            //randomise speed
            speed += Random.Range(-speedOffset, speedOffset);
            speed = Mathf.Max(0.1f, speed);

            //randomise rotation
            transform.Rotate(Vector3.forward, Random.Range(0, 360));
            rotationSpeed = Random.Range(rotationRange.x, rotationRange.y) / size;
            int randomDirection = Random.Range(0, 2) * 2 - 1;
            rotationSpeed *= randomDirection;

            //set boundary
            minPoint = GameControl.instance.screenMinPoint.y;
            //add self to asteroid list
            GameControl.instance.asteroids.Add(gameObject);

        }

        private void Update()
        {
            Movement();
        }

        void Movement()
        {
            //move and rotate
            transform.position += (Vector3)direction * speed * Time.deltaTime;
            transform.Rotate(Vector3.forward, rotationSpeed);

            //destroy if off boundary
            if (transform.position.y < minPoint - size)
            {
                GameControl.instance.asteroids.Remove(gameObject);
                Destroy(gameObject);
            }

        }

        public void Damage(int _damage)
        {
            //take damage, destroy if on 0 health
            health -= _damage;
            if (health <= 0)
            {
                Destroy();
            }
        }

        private void Destroy()
        {
            //decrease size
            size -= 1;
            //if not smallest size, split
            if (size > 0)
            {
                Split();
            }
            //break particles
            if (breakParticles)
                Instantiate(breakParticles, transform.position, Quaternion.identity);

            //add score
            GameControl.instance.AddScore(score);
            //remove from asteroid list and destroy
            GameControl.instance.asteroids.Remove(gameObject);
            Destroy(gameObject);
        }

        void Split()
        {
            //spawn number of asteroids equal to splitCount
            for (int i = 0; i < splitCount; i++)
            {
                Asteroid newAsteroid = Instantiate(GameControl.instance.asteroidPrefabs[size - 1], transform.position, Quaternion.identity).GetComponent<Asteroid>();
                newAsteroid.fromSplit = true;
            }

        }

    }
}
