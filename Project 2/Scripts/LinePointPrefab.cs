using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Level;

public class LinePointPrefab : MonoBehaviour
{
    private static LinePointPrefab instance;

    public bool isHit = false;
    public bool isMissed = false;
    public Sprite pointHitSprite;
    public Sprite pointMissedSprite;
    public Sprite pointNearMissPrite;

    private Transform linePointPrefabTransform;

    public static LinePointPrefab GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        linePointPrefabTransform = GetComponent<Transform>();

        // Next point approaching
        if (linePointPrefabTransform.position.x <= 10f)
        {
            SetNextPointTag();
        }

        // Point missed
        if (linePointPrefabTransform.position.x < 0f && isHit == false)
        {
            SetIsMissed();
            GetComponent<SpriteRenderer>().sprite = pointMissedSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Rocket")
        {
            if (!isMissed && !isHit)
            {
                Debug.Log("Nailed it!");
                Rocket.GetInstance().IncrementScore();
                SetIsHit();
                SoundsManager.PlaySound(SoundsManager.Sound.HitPoint);
            }
        }
        else
        {
            Debug.Log("Hit the future sight marker!");
            Level.GetInstance().DestroyFutureSightMarker();
        }

    }

    public void SetIsHit()
    {
        isHit = true;
        GetComponent<SpriteRenderer>().sprite = pointHitSprite;
    }

    public void SetIsMissed()
    {
        isMissed = true;
    }

    private void SetNextPointTag()
    {
        gameObject.tag = "NextPointTag";
    }
}
