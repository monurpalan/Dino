using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float destroyOffset = 2f;
    private float screenLeftEdge;

    private void Start()
    {
        InitializeScreenBoundary();
    }

    private void Update()
    {
        MoveObstacle();
        CheckIfOutOfBounds();
    }

    private void InitializeScreenBoundary()
    {
        // Ekranın sol kenarını (x=0) dünya koordinatlarına çevirir ve ofseti ekler
        Vector3 screenLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        screenLeftEdge = screenLeft.x - destroyOffset;
    }

    private void MoveObstacle()
    {
        // Engeli, oyun hızına bağlı olarak sola doğru hareket ettirir
        float speed = GameManager.instance.gameSpeed * Time.deltaTime;
        transform.Translate(Vector3.left * speed);
    }

    private void CheckIfOutOfBounds()
    {
        // Engel, ekranın sol kenarını geçerse yok edilir
        if (transform.position.x < screenLeftEdge)
        {
            Destroy(gameObject);
        }
    }
}