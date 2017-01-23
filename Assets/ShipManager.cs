using UnityEngine;

public class ShipManager : MonoBehaviour
{
    public GMScript GM;
    public float stormMoveSpeed = 0.8f;
    public float moveSpeed = 3f;
    public bool shipCanMove = true;
	public Sprite sunkSprite,horizontal,vertical,topright,topleft,botright,botleft;
    public AudioClip GameTheme;
    public AudioClip MenuTheme;
    //bool inCloud=false;
    float currentSpeed;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = moveSpeed;
    }

    void Update()
    {
        if (shipCanMove)
        {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized * currentSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
        currentSpeed = moveSpeed;
		if (shipCanMove)
			AnimateShip ();
    }
	void AnimateShip()
	{
		float X=Input.GetAxis("Horizontal"), Y=Input.GetAxis("Vertical"),limit=0.1f;
		if (Y > limit)
		{
			if (X > limit)
				GetComponent<SpriteRenderer> ().sprite = topright;
			else if (X < -limit)
					GetComponent<SpriteRenderer> ().sprite = topleft;
				else
					GetComponent<SpriteRenderer> ().sprite = vertical;
		} else if (Y < -limit)
		{
			if (X > limit)
				GetComponent<SpriteRenderer> ().sprite = botright;
			else if (X < -limit)
				GetComponent<SpriteRenderer> ().sprite = botleft;
			else
				GetComponent<SpriteRenderer> ().sprite = vertical;
		}
		else
			GetComponent<SpriteRenderer> ().sprite = horizontal;
	}
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "cloud")
            currentSpeed = stormMoveSpeed;
        else
            GM.EndGame(GMScript.EndGameCondition.ShipWasEaten);
    }

    public void SinkTheShip()
    {
        GetComponent<SpriteRenderer>().sprite = sunkSprite;
    }

    public void StopTheShip()
    {
        shipCanMove = false;
    }
}
