using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidGame
{

    public class GameControl : MonoBehaviour
    {
        public static GameControl instance;
        [SerializeField] public Vector2 screenMinPoint, screenMaxPoint;

        
        bool isPlaying = false;
        public bool IsPlaying() => isPlaying;      
        int score;

        [Header("Gameplay")]
        [SerializeField] float startSpawnRate = 2f;
        [SerializeField] float minSpawnRate = 0.5f;
        [SerializeField] float spawnRateDecreasePerAsteroid = 0.01f;
        float spawnRate;

        [Header("Prefabs")]
        [SerializeField] GameObject playerPrefab;
        [SerializeField] public GameObject[] asteroidPrefabs = new GameObject[3];

        public MenuControl menuControl;

        public List<GameObject> asteroids = new List<GameObject>();
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                menuControl = FindObjectOfType<MenuControl>();
            }
        }


        public void StartGame()
        {
            SpawnPlayer();
            StartSpawningAsteroids();
        }

        public Player SpawnPlayer()
        {
            ResetScore();
            Player player = Instantiate(playerPrefab, this.transform).GetComponent<Player>();
            isPlaying = true;
            return player;
        }

        public void StartSpawningAsteroids()
        {
            spawnRate = startSpawnRate;
            StartCoroutine(SpawnAsteroids());
        }

        public void EndGame()
        {
            isPlaying = false;
            foreach (GameObject asteroid in asteroids)
            {
                Destroy(asteroid);
            }
            asteroids = new List<GameObject>();
            menuControl.GameOver();
        }

        #region asteroid spawning
        IEnumerator SpawnAsteroids()
        {
            while (isPlaying)
            {
                SpawnRandomAsteroids();
                yield return new WaitForSeconds(Random.Range(minSpawnRate, spawnRate));
                if (spawnRate > minSpawnRate)
                    spawnRate -= spawnRateDecreasePerAsteroid;
            }
        }

        void SpawnRandomAsteroids()
        {
            int random = Random.Range(0, 3);
            GameObject spawnedAsteroid = asteroidPrefabs[random];
            int numberToSpawn = 3 - random;

            while (numberToSpawn > 0)
            {
                SpawnAsteroid(spawnedAsteroid);
                numberToSpawn--;
            }
        }

        public GameObject SpawnAsteroid(GameObject _asteroidPrefab)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(screenMinPoint.x, screenMaxPoint.x), screenMaxPoint.y);
            return Instantiate(_asteroidPrefab, spawnPosition, Quaternion.identity);
        }

        public void AddScore(int _score)
        {
            score += _score;
            menuControl.UpdateScoreText(score);
        }

        public void ResetScore()
        {
            score = 0;
            menuControl.UpdateScoreText(score);
        }

        public int GetScore() => score;

        #endregion
    }
}