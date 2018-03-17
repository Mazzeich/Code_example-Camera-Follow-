using UnityEngine;

/// <summary>
/// Скрипт следования камеры за персонажем
/// </summary>
public class CameraFollow : MonoBehaviour
{
    /// <summary>
    /// Коэффициент амортизации
    /// </summary>
    [Tooltip("Коэффициент амортизации")]
    public float damping = 1.4f;

    /// <summary>
    /// Вектор смещения относительно объекта
    /// </summary>
    public Vector2 offset = new Vector2(1.4f, 0.1f);

    /// <summary>
    /// Игрок
    /// </summary>
    private GameObject characterObject;
    private Transform characterTransform;
    private Rigidbody2D characterRB;
    private Vector3 spawnPosition;

    void Awake()
    {
        offset = new Vector2(offset.x, offset.y);
        FindCharacter();
        if (!characterTransform)
            Debug.LogError("Camera: Player not found");
        spawnPosition = new Vector3(
            characterTransform.position.x,
            characterTransform.position.y,
            transform.position.z);
    }

    void Update()
    {
        Vector3 target;
        if (!characterObject)
        {
            Debug.Log("Camera: characterObject not found");
            target = spawnPosition;
        }
        else
            target = new Vector3(
                characterTransform.position.x + offset.x * characterRB.velocity.x,
                characterTransform.position.y + offset.y * characterRB.velocity.y,
                transform.position.z);
        transform.position = Vector3.Lerp(transform.position, target, damping * Time.deltaTime);
    }

    /// <summary>
    /// Поиск персонажа и связанных с ним объектов
    /// </summary>
    public void FindCharacter()
    {
        characterObject = GameObject.FindGameObjectWithTag("Player");
        characterTransform = characterObject.transform;
        characterRB = characterObject.GetComponent<Rigidbody2D>();

        MoveScript move = characterObject.GetComponent<MoveScript>();

        // Начальная установка камеры
        if (move.physics.facing == FacingDirection.Left)
            transform.position = new Vector3(
                characterTransform.position.x - offset.x, 
                characterTransform.position.y + offset.y, 
                transform.position.z);
        else
            transform.position = new Vector3(
                characterTransform.position.x + offset.x, 
                characterTransform.position.y + offset.y, 
                transform.position.z);
    }
}
