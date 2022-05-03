/*
 * Author: Alex Manning
 * Date: 3/11/2021
 * Folder Location: Assets/Scripts/WaratahChallenge
 */

using UnityEngine;

public class Seed : MonoBehaviour
{
    public float FallSpeed;         //Speed multiplier for seeds falling
    public float Torque;            //Torque multiplier for seeds
    public Spawner Spawner;         //Spawner object
    private Rigidbody rigidbody;    //Seed rigidbody cache
    private Renderer renderer;      //Seed renderer cache

    //cache components
    private void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        renderer = gameObject.GetComponent<Renderer>();
    }

    private void Update()
    {
        //destroy if offscreen
        if (!renderer.isVisible && gameObject.transform.position.y < -8)
        {
            Spawner.subtractPeanaltyTime();
            Destroy(this.gameObject);
        }
        // Game Running Safety Check (james)

        if (!Spawner.Instance.Running)
            Destroy(this.gameObject);
    }

    //phsyics update
    private void FixedUpdate()
    {
        rigidbody.AddForce(Physics.gravity * FallSpeed);
        rigidbody.AddTorque(0f, Torque, 0f, ForceMode.Force);
    }

    /// <summary>
    /// Adds a force to the seed
    /// </summary>
    /// <param name="force">Force to apply to the seed</param>
    public void push(Vector3 force)
    {
        if (rigidbody != null)
        {
            rigidbody.AddForce(force, ForceMode.Impulse);
        }
    }
}