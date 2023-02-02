using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MouseOrbitSample : MonoBehaviour
{
    public bool mouseRotateActive = false;
    public Transform Target;
    public bool IsRotate = false;
    public float Distance = 2;
    public float XSpeed = 250;
    public float YSpeed = 120;
    public float YMinLimit = -20;
    public float YMaxLimit = 80;

    private float xRotation = 0.0f;
    private float yRotation = 0.0f;

    private bool isRefreshTarget = false;
    private Vector3 rotatePoint;
    private Vector3 newDirection;
    private Vector3 newCameraPosition;
    private Quaternion tempRotation;
    private float cameraSpeed = 1.5f;
    
    

    Vector3 tempPoint;
    float refreshPointPer = 0;
    float time;
    float tempDistance;



    void Start()
    {
        rotatePoint = Target.position;
        Vector3 angles = transform.eulerAngles;
        xRotation = 0;
        yRotation = 0;

        RefreshCamera(Target, 2, 3);
    }

    public void RefreshCamera(Transform target,float distance,float speed)
    {
        cameraSpeed = speed;
        newDirection = target.position ;
        newCameraPosition = target.position + target.forward * distance;
        Distance = distance;
        tempPoint = new Vector3( transform.position.x,transform.position.y,transform.position.z);
        tempRotation = transform.rotation;
       
        time = Time.time;
        tempDistance = Vector3.Distance(tempPoint, newCameraPosition);
        if (tempDistance > 0.01f)
        {
            isRefreshTarget = true;
        }
    }


    private void LateUpdate()
    {
        {
            if (mouseRotateActive)
            {
                if (Target)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        IsRotate = true;
                    }
                    if (Input.GetMouseButtonUp(1))
                    {
                        IsRotate = false;
                    }
                        if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
                        {
                            Distance += 0.1f;
                            SolveUpdateCamera(0, 0);

                        }
                        if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
                        {
                            Distance -= 0.1f;
                            SolveUpdateCamera(0, 0);
                        }

                    if (IsRotate)
                    {
                        SolveUpdateCamera(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                    }
                }
            }

            if (isRefreshTarget)
            {
                float distCovered = (Time.time - time) * cameraSpeed;
                refreshPointPer = distCovered / tempDistance;

                transform.position = Vector3.Lerp(tempPoint, newCameraPosition, refreshPointPer);
                transform.rotation = Quaternion.Slerp(tempRotation, Quaternion.LookRotation(newDirection - transform.position), refreshPointPer);

                if (refreshPointPer >= 0.999f)
                {
                    refreshPointPer = 0;
                    isRefreshTarget = false;
                    rotatePoint = newDirection;
                    xRotation = transform.rotation.eulerAngles.y;
                    yRotation = transform.rotation.eulerAngles.x;
                    if (yRotation > 180)
                    {
                        yRotation = 360f - yRotation;
                    }
                }
            }

        }
    }


    private void SolveUpdateCamera(float xValue, float yValue)
    {
        xRotation += xValue * XSpeed * 0.02f;
        yRotation -= yValue * YSpeed * 0.02f;

        yRotation = ClampAngle(yRotation, YMinLimit, YMaxLimit);

        var rotation = Quaternion.Euler(yRotation, xRotation, 0);
        var position = rotation * new Vector3(0.0f, 0.0f, -Distance) + rotatePoint;

        transform.rotation = rotation;
        transform.position = position;
    }


    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

}




    



