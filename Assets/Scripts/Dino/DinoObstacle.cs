using UnityEngine;

public class DinoObstacle : MonoBehaviour
{
    private float leftEdge;
    
    public int id;

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 2f;
    }
    private void Update()
    {
        transform.position += Vector3.left * DinoGameController.Instance.gameSpeed * Time.deltaTime;

        if (transform.position.x < leftEdge)
        {
            if (id == DinoGameController.Instance.idCollect)
            {
                DinoGameController.Instance.GameOver();
            }
            Destroy(gameObject);
        }
    }
}
