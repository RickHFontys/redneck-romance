using UnityEngine;
using System.Collections;

public class HandController : MonoBehaviour
{
    [Header("Hand References")]
    public SpriteRenderer leftHand;
    public SpriteRenderer rightHand;

    [Header("Left Hand Sprites")]
    public Sprite neutralLeft;
    public Sprite happyLeft;
    public Sprite angryLeft;
    public Sprite flirtyLeft;
    public Sprite nervousLeft;
    public Sprite slamLeft;

    [Header("Right Hand Sprites")]
    public Sprite neutralRight;
    public Sprite happyRight;
    public Sprite angryRight;
    public Sprite flirtyRight;
    public Sprite nervousRight;
    public Sprite slamRight;

    [Header("Animation Settings")]
    public float bobAmplitude = 0.02f;
    public float bobSpeed = 2f;

    private Vector3 leftStartPos;
    private Vector3 rightStartPos;
    private float bobTimer;

    private void Awake()
    {
        leftStartPos = leftHand.transform.localPosition;
        rightStartPos = rightHand.transform.localPosition;
    }

    private void Update()
    {
        AnimateBob();
    }

    /// <summary>
    /// Call this when a response is selected
    /// </summary>
    public void SetHands(ResponseOption response)
    {
        leftHand.sprite = GetLeftSprite(response.leftHandReaction);
        rightHand.sprite = GetRightSprite(response.rightHandReaction);

        // Optional slam animation
        if (response.leftHandReaction == HandState.Slam || response.rightHandReaction == HandState.Slam)
        {
            StartCoroutine(SlamAnimation());
        }
    }

    private Sprite GetLeftSprite(HandState state)
    {
        return state switch
        {
            HandState.Happy => happyLeft,
            HandState.Angry => angryLeft,
            HandState.Flirty => flirtyLeft,
            HandState.Nervous => nervousLeft,
            HandState.Slam => slamLeft,
            _ => neutralLeft
        };
    }

    private Sprite GetRightSprite(HandState state)
    {
        return state switch
        {
            HandState.Happy => happyRight,
            HandState.Angry => angryRight,
            HandState.Flirty => flirtyRight,
            HandState.Nervous => nervousRight,
            HandState.Slam => slamRight,
            _ => neutralRight
        };
    }

    private void AnimateBob()
    {
        bobTimer += Time.deltaTime * bobSpeed;
        float offset = Mathf.Sin(bobTimer) * bobAmplitude;

        leftHand.transform.localPosition = leftStartPos + new Vector3(0, offset, 0);
        rightHand.transform.localPosition = rightStartPos + new Vector3(0, offset, 0);
    }

    private IEnumerator SlamAnimation()
    {
        Vector3 originalL = leftHand.transform.localPosition;
        Vector3 originalR = rightHand.transform.localPosition;

        // Quick down motion
        leftHand.transform.localPosition += new Vector3(0, -0.1f, 0);
        rightHand.transform.localPosition += new Vector3(0, -0.1f, 0);
        yield return new WaitForSeconds(0.05f);

        // Return to original
        leftHand.transform.localPosition = originalL;
        rightHand.transform.localPosition = originalR;
    }
}
