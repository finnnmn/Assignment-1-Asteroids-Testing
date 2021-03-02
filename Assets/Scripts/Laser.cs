using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidGame {
    public class Laser : MonoBehaviour
    {
        int damage = 1;
        float speed;
        float maxPoint;
        float destroyOffset = 2;


        public void Setup(float _speed, float _direction, int _damage)
        {
            speed = _speed;
            transform.rotation = Quaternion.Euler(0, 0, _direction);
            damage = _damage;
            maxPoint = GameControl.instance.screenMaxPoint.y;
        }

        private void Update()
        {
            //move and destroy if offscreen
            transform.position += Vector3.up * speed * Time.deltaTime;
            if (transform.position.y > maxPoint + destroyOffset)
            {
                Destroy(gameObject);
            }

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //if colliding with asteroid damage it and destroy self
            if (other.gameObject.layer == LayerMask.NameToLayer("Asteroid"))
            {
                other.GetComponent<Asteroid>().Damage(damage);
                Destroy(gameObject);
            }
        }

    }
}
