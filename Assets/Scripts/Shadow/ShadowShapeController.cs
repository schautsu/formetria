using UnityEngine;

public class ShadowShapeController : MonoBehaviour
{
    private Vector2 initialPosition, _mousePosition;
    private float deltaX, deltaY;

    public Transform shapePlace;
    public string shapeName;
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
        if (ShadowGameController.Instance.canClick && !locked)
        {
            ShadowGameController.Instance.shapeNameText.text = shapeName;

            deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
            deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
        }
    }

    private void OnMouseDrag()
    {
        if (ShadowGameController.Instance.canClick && !locked)
        {
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(_mousePosition.x - deltaX, _mousePosition.y - deltaY);   
        }
    }

    private void OnMouseUp()
    {
        if (ShadowGameController.Instance.canClick && !locked)
        {
            attempts++;
            ShadowGameController.Instance.attemptsText.text = "Jogadas: " + attempts;

            if (Mathf.Abs(transform.position.x - shapePlace.position.x) <= 2f && Mathf.Abs(transform.position.y - shapePlace.position.y) <= 2f)
            {
                transform.position = new Vector3(shapePlace.position.x, shapePlace.position.y, 0f);
                locked = true; 
                count++;

                if (count == ShadowGameController.Instance.numberOfShapes)
                {  
                    ShadowGameController.Instance.finishAttemptsText.text = "Jogadas: " + attempts;
                    count = 0;
                    attempts = 0;
                    ShadowGameController.Instance.Finish();
                }
            }
            else transform.position = new Vector2(initialPosition.x, initialPosition.y);
        }
    }
}
