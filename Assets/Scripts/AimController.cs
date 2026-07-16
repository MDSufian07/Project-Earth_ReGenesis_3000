using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AimController : MonoBehaviour
{
    
    public Rig aimRig;
    public float speed = 8f;

    void Update()
    {
        float targetWeight = (Input.GetMouseButton(1) || Input.GetMouseButton(0)) ? 1f : 0f;

        aimRig.weight = Mathf.Lerp(aimRig.weight, targetWeight, Time.deltaTime * speed);
        
        
    }
}