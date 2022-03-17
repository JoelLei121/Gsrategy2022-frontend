using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    Animator animator;
    float velocityX;
    public float velocityZ;
    int VelocityXHash;
    int VelocityZHash;
    public float acceleration = 2.0f;
    public float deceleration = 1.0f;

    public float maxWalkVelocity = 0.5f;
    public float maxRunVelocity = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        VelocityXHash = Animator.StringToHash("VelocityX");
        VelocityZHash = Animator.StringToHash("VelocityZ");
    }

    // Update is called once per frame
    // -0.5 0 0.5 1
    void Update()
    {
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        float currentMaxVelocity = runPressed ? maxRunVelocity : maxWalkVelocity;

        if (forwardPressed && velocityZ < currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
        }
        
        if (!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }

        if(forwardPressed && runPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ = currentMaxVelocity;
        }
        else if(forwardPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * deceleration;
            if(forwardPressed && velocityZ > currentMaxVelocity && velocityZ < (currentMaxVelocity + 0.05f))
            {
                velocityZ = currentMaxVelocity;
            }
        }
        else if(forwardPressed && velocityZ < currentMaxVelocity && velocityZ > (currentMaxVelocity - 0.05f))
        {
            velocityZ = currentMaxVelocity;
        }

        if (!forwardPressed && velocityZ < 0.0f)
        {
            velocityZ = 0;
        }

        animator.SetFloat(VelocityXHash, velocityX);
        animator.SetFloat(VelocityZHash, velocityZ);
    }
}
