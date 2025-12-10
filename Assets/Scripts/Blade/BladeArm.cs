using UnityEngine;

public class BladeArm : MonoBehaviour
{
    private HingeJoint2D hinge;
    private JointMotor2D motor;

    void Start()
    {
        hinge = GetComponent<HingeJoint2D>();
        motor = hinge.motor;
    }

    void Update()
    {

        if (hinge.jointAngle >= hinge.limits.max - 1)
        {
            motor.motorSpeed = -Mathf.Abs(motor.motorSpeed);
            hinge.motor = motor;
        }

        else if (hinge.jointAngle <= hinge.limits.min + 1)
        {
            motor.motorSpeed = Mathf.Abs(motor.motorSpeed);
            hinge.motor = motor;
        }
    }
}