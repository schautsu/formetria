using UnityEngine;

public class BoxesShapeController : MonoBehaviour
{
    private Vector2 initialPosition, _mousePosition;
    private float deltaX, deltaY;

    public Transform shapePlace;
    public bool locked;
    public static int count, attempts;

    private void Start()
    {
        initialPosition = transform.position;
        locked = false;
        count = 0;
        attempts = 0;
    }

    private void OnMouseDown()
    {
        if (BoxesGameController.Instance.canClick && !locked)
        {
            deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
            deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
        }
    }

    private void OnMouseDrag()
    {
        if (BoxesGameController.Instance.canClick && !locked)
        {
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(_mousePosition.x - deltaX, _mousePosition.y - deltaY);   
        }
    }

    private void OnMouseUp()
    {
        if (BoxesGameController.Instance.canClick && !locked)
        {
            attempts++;
            BoxesGameController.Instance.attemptsText.text = "Jogadas: " + attempts;

            if (Mathf.Abs(transform.position.x - shapePlace.position.x) <= 3f && Mathf.Abs(transform.position.y - shapePlace.position.y) <= 3f)
            {
                transform.position = new Vector3(shapePlace.position.x, shapePlace.position.y, 0f);
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                locked = true; 
                count++;

                if (count == BoxesGameController.Instance.numberOfShapes)
                {  
                    BoxesGameController.Instance.finishAttemptsText.text = "Jogadas: " + attempts;
                    count = 0;
                    attempts = 0;
                    BoxesGameController.Instance.Finish();
                }
            }
            else transform.position = new Vector2(initialPosition.x, initialPosition.y);
        }  
    }
}
