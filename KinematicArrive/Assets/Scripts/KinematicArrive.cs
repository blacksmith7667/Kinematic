using UnityEngine;

public class KinematicArrive : MonoBehaviour {
    private Vector3 target;            
    public float maxSpeed = 10f;       
    public float radius = 0.5f;        
    public float timeToTarget = 0.5f;  

    private void Update() {
        
        if (Input.GetMouseButtonDown(0)) {
            SetTargetPosition();
        }

        KinematicSteeringOutput steering = GetSteering();

        if (steering != null) {
           
            transform.position += steering.velocity * Time.deltaTime;

            if (steering.velocity.magnitude > 0) {
                Quaternion targetRotation = Quaternion.LookRotation(steering.velocity.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
    }

   
    void SetTargetPosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            
            target = new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z);  
        }
    }

    class KinematicSteeringOutput {
        public Vector3 velocity;
        public float rotation;
    }

    KinematicSteeringOutput GetSteering() {
        KinematicSteeringOutput steering = new KinematicSteeringOutput();
        steering.velocity = target - transform.position;

        if (steering.velocity.magnitude < radius) {
            return null;  
        }

        steering.velocity /= timeToTarget;

        if (steering.velocity.magnitude > maxSpeed) {
            steering.velocity.Normalize();
            steering.velocity *= maxSpeed;
        }

       
        steering.rotation = 0;
        return steering;
    }
}
