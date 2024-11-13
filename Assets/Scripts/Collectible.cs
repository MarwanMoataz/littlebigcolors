using UnityEngine;

public class Collectible : MonoBehaviour
{
    public Color collectibleColor;
    public float floatAmplitude = 0.5f; // Controls the height of the float
    public float floatSpeed = 1f; // Controls the speed of the float
    private Vector3 startPosition;

    private void Start()
    {
  
        GetComponent<Renderer>().material.color = collectibleColor;
        startPosition = transform.position;

    }
    private void Update()
    {
        // Calculate new position for floating effect
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            GameManager.instance.CollectColor(collectibleColor);
        }
    }

}
