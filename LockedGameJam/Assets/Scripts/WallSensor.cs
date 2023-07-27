using UnityEngine;

public class WallSensor : MonoBehaviour
{
#if UNITY_ANDROID
    enum Side
    {
        Up, Down, Left, Right
    }

    [SerializeField] private Side side;
    [SerializeField] private CharacterMovement character;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Wall"))
        {
            switch (side)
            {
                case Side.Up:
                    character.SetUp(false);
                    break;
                case Side.Down:
                    character.SetDown(false);
                    break;
                case Side.Right:
                    character.SetRight(false);
                    break;
                case Side.Left:
                    character.SetLeft(false);
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            switch (side)
            {
                case Side.Up:
                    character.SetUp(true);
                    break;
                case Side.Down:
                    character.SetDown(true);
                    break;
                case Side.Right:
                    character.SetRight(true);
                    break;
                case Side.Left:
                    character.SetLeft(true);
                    break;
            }
        }
    }
#endif
}
