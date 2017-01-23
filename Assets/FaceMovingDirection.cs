using UnityEngine;

public class FaceMovingDirection : MonoBehaviour
{
	public Sprite norm,up;
	Vector3 vect;
	void Start()
	{
		vect = transform.localScale;
	}
    void Update()
    {
        Vector2 vel = GetComponent<Rigidbody2D>().velocity;
		float angle = (Mathf.Atan2 (vel.y, vel.x) / Mathf.PI * 180+360)%360;
		transform.rotation = Quaternion.Euler(0f, 0f, angle);
		if ((angle > 46 && angle < 134) || (angle > 226 && angle < 314))
			GetComponent<SpriteRenderer> ().sprite=up;
		else
			GetComponent<SpriteRenderer> ().sprite=norm;
		if ((angle > 134) && (angle < 226))
			transform.localScale = new Vector3 (vect.x,-vect.y,vect.z);
        Vector2 pos = transform.position;
        if (pos.x < -3 || pos.y < -3 || pos.x > 3 || pos.y > 3)
            Destroy(gameObject);
    }
}
