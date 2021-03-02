using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidGame
{
    public class Player : MonoBehaviour
    {
        [Header("Positioning")]
        [SerializeField] public Vector2 startPoint;
        [SerializeField] [Range(0, 0.5f)] float borderClampOffset = 0.4f;
        Vector2 minPoint, maxPoint;

        [Header("Stats")]
        [SerializeField] float speed = 10;
        [SerializeField] int attackDamage = 1;
        [SerializeField] [Range(0.1f, 1)] float reloadTime = 0.25f;
        [SerializeField] float bulletSpeed = 20;

        [Header("Prefabs")]
        [SerializeField] GameObject bulletPrefab;

        

        bool reloading;


        private void Start()
        {
            SetUpPositioning();
        }
        private void Update()
        {
            PlayerMovement();
            PlayerShooting();
        }

        #region position/movement
        private void SetUpPositioning()
        {
            //set up boundaries
            minPoint = GameControl.instance.screenMinPoint;
            minPoint += new Vector2(borderClampOffset, borderClampOffset);

            maxPoint = GameControl.instance.screenMaxPoint;
            maxPoint -= new Vector2(borderClampOffset, borderClampOffset);

            //go to start position
            transform.position = startPoint;
        }

        private void PlayerMovement()
        {
            //get player input and normalize
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            input.Normalize();

            //move player
            Vector2 move = input * speed * Time.deltaTime;
            transform.position += (Vector3)move;

            //clamp within boundaries
            transform.position = new Vector2(Mathf.Clamp(transform.position.x, minPoint.x, maxPoint.x), Mathf.Clamp(transform.position.y, minPoint.y, maxPoint.y));
        }
        #endregion

        #region shooting
        private void PlayerShooting()
        {
            //get player input
            if (Input.GetKey(KeyCode.Space))
            {
                //shoot if not reloading
                if (reloading == false)
                {
                    StartCoroutine(ShootRoutine());
                }
            }
        }

        IEnumerator ShootRoutine()
        {
            Shoot();

            //stop reloading after time
            reloading = true;
            yield return new WaitForSeconds(reloadTime);
            reloading = false;
        }

        public Laser Shoot()
        {
            //spawn a laser and set it up
            Laser laser = Instantiate(bulletPrefab, transform).GetComponent<Laser>();
            laser.transform.SetParent(null);
            laser.Setup(bulletSpeed, 0, attackDamage);
            return laser;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            //if colliding with asteroid
            if (other.gameObject.layer == LayerMask.NameToLayer("Asteroid"))
            {
                //end the game and destroy
                GameControl.instance.EndGame();
                Destroy(gameObject);
            }
        }

        #endregion
    }
}
