using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidGame
{
    public class ParallaxBackground : MonoBehaviour
    {
        [SerializeField] float scrollSpeed = -5;
        float spriteHeight;

        Vector2 originalPosition;

        SpriteRenderer spriteRenderer;
        SpriteRenderer extraRenderer;

        private void Start()
        {
            //set position of background
            originalPosition = transform.position;

            spriteRenderer = GetComponent<SpriteRenderer>();

            //find sprite size for looping
            spriteHeight = spriteRenderer.sprite.bounds.max.y - spriteRenderer.sprite.bounds.min.y;

            //spawn a new gameObject with a sprite renderer
            extraRenderer = new GameObject("Extra Image").AddComponent<SpriteRenderer>();
            //set new object as child of this one so they move together
            extraRenderer.transform.SetParent(transform);

            //set sprite of new object to the same as this one
            extraRenderer.sprite = spriteRenderer.sprite;
            extraRenderer.sortingOrder = spriteRenderer.sortingOrder;

            //offset the new image to exactly above or below this one
            extraRenderer.transform.position = transform.position + Vector3.up * spriteHeight * -Mathf.Sign(scrollSpeed);

        }
        private void Update()
        {
            //move based on scroll speed (other image will move with this one)
            transform.position += Vector3.up * scrollSpeed * Time.deltaTime;
            //if other image is now in center of screen, go back to original position
            if (transform.position.y > originalPosition.y + spriteHeight || transform.position.y < originalPosition.y - spriteHeight)
            {
                transform.position = originalPosition;
            }
        }
    }
}