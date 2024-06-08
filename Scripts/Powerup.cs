using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private float _speed = 3f;
    [SerializeField]
    private int _powerupID;

    [SerializeField]
    private AudioClip _clip;

    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.SetTripleShot(true);
                        break;
                    case 1:
                        player.SetSpeed(13);
                        break;
                    case 2:
                        player.SetShield(true);
                        break;
                    default:
                        break;
                }
            }
            AudioSource.PlayClipAtPoint(_clip,transform.position);
            Destroy(this.gameObject);

        }
    }
}
