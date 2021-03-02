using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AsteroidGame;

namespace Tests
{
    public class TestScript
    {

        GameControl gameControl;
        Player player;

        //Asteroid moves down (Assert.Less)
        [UnityTest, Order(0)]
        public IEnumerator AsteroidMovesDown()
        {

            //create instance of game
            GameObject game = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/BaseGame"));
            gameControl = game.GetComponentInChildren<GameControl>();

            yield return null;

            //close menu
            gameControl.menuControl.CloseMenus();

            yield return new WaitForSeconds(1);

            //spawn an asteroid and get its y position
            GameObject asteroid = gameControl.SpawnAsteroid(gameControl.asteroidPrefabs[1]);

            float initialYPos = asteroid.transform.position.y;

            yield return new WaitForSeconds(1);

            //check if the new y position after a second is less than the original y
            Assert.Less(asteroid.transform.position.y, initialYPos);

            //destroy the asteroid
            Object.Destroy(asteroid);

            yield return null;
        }


        //Laser moves up (Assert.Greater)
        [UnityTest]
        public IEnumerator LaserMovesUp()
        {
            //spawn the player
            player = gameControl.SpawnPlayer();

            player.transform.position = player.startPoint;

            //make the player shoot and get laser y position
            Laser laser = player.Shoot();

            float initialYPos = laser.transform.position.y;

            yield return new WaitForSeconds(0.5f);

            //check if the new y position after a second is greater than the original y
            Assert.Greater(laser.transform.position.y, initialYPos);

            //destroy the laser and player
            Object.Destroy(laser.gameObject);
            Object.Destroy(player.gameObject);

            yield return null;
        }


        //Laser destroys offscreen (Assert.IsNull)
        [UnityTest]

        public IEnumerator LaserNullOffscreen()
        {
            //spawn the player
            player = gameControl.SpawnPlayer();
            player.transform.position = player.startPoint;

            //make the player shoot a laser
            Laser laser = player.Shoot();

            //wait for laser to be offscreen
            yield return new WaitUntil(() => laser.transform.position.y > gameControl.screenMaxPoint.y);

            //give laser time to be destroyed
            yield return new WaitForSeconds(1);

            //check if laser was destroyed
            UnityEngine.Assertions.Assert.IsNull(laser);

            //Destroy the player
            Object.Destroy(player.gameObject);

            yield return null;


        }

        //game ends when player collides with asteroid (Assert.IsFalse)
        [UnityTest]
        public IEnumerator GameOverOnCollision()
        {

            //spawn the player
            player = gameControl.SpawnPlayer();
            player.transform.position = player.startPoint;

            GameObject asteroid = gameControl.SpawnAsteroid(gameControl.asteroidPrefabs[1]);
            yield return new WaitForSeconds(1);

            //spawn an asteroid on the player
            asteroid.transform.position = player.transform.position;

            yield return new WaitForSeconds(0.5f);

            //check that the game is no longer playing
            Assert.IsFalse(gameControl.IsPlaying());

            Object.Destroy(asteroid);

            if (player)
                Object.Destroy(player);

            //close menu
            gameControl.menuControl.CloseMenus();

        }

        //the score is reset on a new game (Assert.AreEqual)
        [UnityTest]
        public IEnumerator NewGameResetsScore()
        {
            //set the score to the player
            gameControl.AddScore(100);

            yield return new WaitForSeconds(1);

            //start the game by spawning the player
            gameControl.SpawnPlayer();

            yield return new WaitForSeconds(1);

            //check if the score is now zero
            Assert.AreEqual(gameControl.GetScore(), 0);

            Object.Destroy(player);

        }
        


        

    }
}
