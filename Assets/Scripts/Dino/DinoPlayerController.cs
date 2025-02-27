using UnityEngine;
using UnityEngine.EventSystems;

public class DinoPlayerController : MonoBehaviour
{
    private CharacterController character;
    private Vector3 direction;

    public float gravity = 9.81f * 2f;
    public float jumpForce = 8f;

    private void Awake()
    {
        character = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        direction = Vector3.zero;
    }

    private void Update()
    {
        direction += Vector3.down * gravity * Time.deltaTime;

        if (character.isGrounded)
        {
            direction = Vector3.down;

            if (SimpleInput.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                direction = Vector3.up * jumpForce;
            }
        }
        character.Move(direction * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            if (other.GetComponent<DinoObstacle>().id != DinoGameController.Instance.idCollect)
            {
                DinoGameController.Instance.GameOver();
            }
            else Destroy(other.gameObject);
        }
    }
}
