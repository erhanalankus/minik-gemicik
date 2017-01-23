using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GMScript : MonoBehaviour
{
    public GameObject Ship;
    public Transform WavePrefab;
    public LayerMask WaveSpawn;
    public GameObject[] Squares, Clouds;
    public float sharkAttackTime, sharkBubbleTime, waveSpeed, stormDuration, sharkCooldown, waveCooldown, stormCooldown;
    public bool isGameOver = false;
    public GameObject EndGameBanners;
    public Text WhoWonBanner;
	public Animator anim;

    List<GameObject> squaresToSendSharksTo;
    float currentSharkCooldown, currentWaveCooldown, currentStormCooldown;
    public enum EndGameCondition
    {
        ShipWasEaten, ShipSank, ShipWon
    }

    void Start()
    {
        GetComponent<CountdownScript>().StartCountdown();
        squaresToSendSharksTo = new List<GameObject>();
        EndGameBanners.SetActive(false);
    }

    void Update()
    {
        currentSharkCooldown -= Time.deltaTime;
        currentWaveCooldown -= Time.deltaTime;
        currentStormCooldown -= Time.deltaTime;
        if (currentSharkCooldown < 0f)
        {
            GetComponent<AttackCommunicatorScript>().ClearSharkAttackIndicators();
        }
        if (currentWaveCooldown < 0f)
        {
            GetComponent<AttackCommunicatorScript>().ClearWaveAttackIndicators();
        }
        if (currentStormCooldown < 0f)
        {
            GetComponent<AttackCommunicatorScript>().ClearStormAttackIndicator();
        }
		if (Input.GetKeyDown(KeyCode.Return)) //TODO Press Enter to restart instead of "0" or change the UI directive.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SendSharksTo(List<int> buttonsPressedForSharkAttack)
    {
        currentSharkCooldown = sharkCooldown;
        squaresToSendSharksTo.Clear();
        foreach (var item in buttonsPressedForSharkAttack)
        {
            squaresToSendSharksTo.Add(Squares[item - 1]);
        }
        squaresToSendSharksTo = squaresToSendSharksTo.Distinct().ToList();
        foreach (GameObject item in squaresToSendSharksTo)
        {
            item.GetComponent<AudioSource>().Play();
        }

        foreach (GameObject item in squaresToSendSharksTo)
        {
            AnimateBubble(item);
        }
        Invoke("ActivateSquareColliders", sharkBubbleTime);
        Invoke("DeactivateSquareColliders", sharkAttackTime+0.01f);
    }

    private void ActivateSquareColliders()
    {
        foreach (GameObject item in squaresToSendSharksTo)
        {
            item.GetComponent<Collider2D>().enabled = true;
            AnimateShark(item);
        }
    }

    private void DeactivateSquareColliders()
    {
        foreach (GameObject item in squaresToSendSharksTo)
        {
            item.GetComponent<Collider2D>().enabled = false;
            StopShark(item);
        }
    }

    private void AnimateBubble(GameObject item)
    {
		item.GetComponent<Animator> ().StopPlayback ();
        item.GetComponent<Animator>().SetTrigger("Bubble");
    }

    private void AnimateShark(GameObject item)
    {
        item.GetComponent<Animator>().speed = 1 / (sharkAttackTime - sharkBubbleTime);
        item.GetComponent<Animator>().SetTrigger("Attack");
    }

	private void StopShark(GameObject item)
	{
		item.GetComponent<Animator>().speed = 1f;
		//item.GetComponent<Animator> ().StartPlayback ();
		item.GetComponent<Animator>().Play("IdleSea",0,(float)anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
	}
    public void SendWave(List<int> waveDirectionPoints)
    {
        currentWaveCooldown = waveCooldown;
        if (waveDirectionPoints[0] == waveDirectionPoints[1])
        {
            if (waveDirectionPoints[0] == 5)
                waveDirectionPoints[0] = 6;
            else
                waveDirectionPoints[1] = 5;
        }
        Vector2 fr = Squares[waveDirectionPoints[0] - 1].transform.position, to = Squares[waveDirectionPoints[1] - 1].transform.position;
        RaycastHit2D RH = Physics2D.Raycast(to, fr - to, 10f, WaveSpawn);
        Vector3 spawnPos = RH.point;
        GameObject spawnedWave = (GameObject)Instantiate(WavePrefab.gameObject, spawnPos, Quaternion.identity);
        spawnedWave.GetComponent<Rigidbody2D>().velocity = (to - fr).normalized * waveSpeed;
    }

    public void SendStorm(List<int> cloudID)
    {
        currentStormCooldown = stormCooldown;
        ActivateCloud(cloudID[0] - 1);
        StartCoroutine("DeactivateCloud", cloudID[0] - 1);
    }

    public void ActivateCloud(int cloud, bool playSound = true)
    {
        Clouds[cloud].GetComponent<SpriteRenderer>().enabled = true;
        Clouds[cloud].GetComponent<Collider2D>().enabled = true;
        if (playSound)
            Clouds[cloud].GetComponent<AudioSource>().Play();
    }

    public IEnumerator DeactivateCloud(int cloud)
    {
        float totalTime = 0, timeDelta = 0.01f;
        while (totalTime < stormDuration)
        {
            totalTime += Mathf.Max(timeDelta, Time.deltaTime);
            ActivateCloud(cloud, false);
            yield return new WaitForSeconds(timeDelta);
        }
        Clouds[cloud].GetComponent<SpriteRenderer>().enabled = false;
        Clouds[cloud].GetComponent<Collider2D>().enabled = false;
    }

    public void EndGame(EndGameCondition endGameCondition)
    {
        isGameOver = true;
        GetComponent<CountdownScript>().StopCountdown();
        Ship.GetComponent<AudioSource>().clip = Ship.GetComponent<ShipManager>().MenuTheme;
        Ship.GetComponent<AudioSource>().Play();

        Ship.GetComponent<Collider2D>().enabled = false;
        Ship.GetComponent<ShipManager>().StopTheShip();
        Invoke("ZoomInOnShip", 2f);

        switch (endGameCondition)
        {
            case EndGameCondition.ShipWasEaten:
                Ship.GetComponent<ShipManager>().SinkTheShip();
                WhoWonBanner.text = "ATTACKER WON";
                break;
            case EndGameCondition.ShipSank:
                Ship.GetComponent<ShipManager>().SinkTheShip();
                WhoWonBanner.text = "ATTACKER WON";
                break;
            case EndGameCondition.ShipWon:
                WhoWonBanner.text = "TINY SHIP WON";
                break;
            default:
                break;
        }
        Invoke("ShowEndGameBanners", 1.5f);
    }

    public void ShowEndGameBanners()
    {
        EndGameBanners.SetActive(true);
    }

    private void ZoomInOnShip()
    {
        Ship.transform.localScale += new Vector3(0.9f, 0.9f);
        Ship.transform.position = new Vector3(0, 0);
    }

    public float GetNormalizedAttackCooldown(int attackType)
    {
        if (attackType == 0)
            return Mathf.Max(currentSharkCooldown, 0f) / sharkCooldown;
        if (attackType == 1)
            return Mathf.Max(currentWaveCooldown, 0f) / waveCooldown;
        if (attackType == 2)
            return Mathf.Max(currentStormCooldown, 0f) / stormCooldown;
        return 0f;
    }

    public bool CanPerformAttack(int attackType)
    {
        if (attackType == 0)
        {
            if (currentSharkCooldown < 0f)
            {
                return true;
            }
        }
        if (attackType == 1)
        {
            if (currentWaveCooldown < 0f)
            {
                return true;
            }
        }
        if (attackType == 2)
        {
            if (currentStormCooldown < 0f)
            {
                return true;
            }
        }
        return false;
    }
}
