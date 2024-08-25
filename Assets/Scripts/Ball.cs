using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 startMousePosition;
    private Vector2 currentMousePosition;
    private Vector2 swipeDirection;
    public float forceMultiplier = 10f;
    public float maxForce = 15f;
    public int predictionSteps = 30;
    public float predictionStepTime = 0.1f;

    public LineRenderer aimLine;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        aimLine.enabled = false;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startMousePosition = touch.position;
                aimLine.enabled = true;
            }

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                currentMousePosition = touch.position;
                swipeDirection = (startMousePosition - currentMousePosition);
                UpdateAimLine();
            }

            if (touch.phase == TouchPhase.Ended)
            {
                aimLine.enabled = false;
                ApplyForce();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                startMousePosition = Input.mousePosition;
                aimLine.enabled = true;
            }

            if (Input.GetMouseButton(0))
            {
                currentMousePosition = Input.mousePosition;
                swipeDirection = (startMousePosition - currentMousePosition);
                UpdateAimLine();
            }

            if (Input.GetMouseButtonUp(0))
            {
                aimLine.enabled = false;
                ApplyForce();
            }
        }
    }

    void UpdateAimLine()
    {
        Vector3 startLinePosition = transform.position;
        Vector3 force = new Vector3(swipeDirection.x, swipeDirection.y, 0).normalized * swipeDirection.magnitude * 0.005f * forceMultiplier;
        force = Vector3.ClampMagnitude(force, maxForce);

        aimLine.positionCount = predictionSteps;

        Vector3[] predictedPositions = PredictTrajectory(transform.position, force);

        for (int i = 0; i < predictionSteps; i++)
        {
            aimLine.SetPosition(i, predictedPositions[i]);
        }
    }

    Vector3[] PredictTrajectory(Vector3 startPosition, Vector3 initialForce)
    {
        Vector3[] trajectory = new Vector3[predictionSteps];
        Vector3 currentPosition = startPosition;
        Vector3 velocity = initialForce / rb.mass;

        for (int i = 0; i < predictionSteps; i++)
        {
            trajectory[i] = currentPosition;
            currentPosition += velocity * predictionStepTime;
            velocity += Physics.gravity * predictionStepTime;
        }

        return trajectory;
    }

    void ApplyForce()
    {
        Vector3 force = new Vector3(swipeDirection.x, swipeDirection.y, 0).normalized * swipeDirection.magnitude * 0.005f * forceMultiplier;
        force = Vector3.ClampMagnitude(force, maxForce);
        rb.AddForce(force, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Goal>() != null)
        {
            Game.PushMessage(this, "+1", 1f);
        }
    }
}
