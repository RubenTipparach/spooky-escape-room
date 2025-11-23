using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TVController : MonoBehaviour
{
    [System.Serializable]
    public class ChannelContent
    {
        public int channelIndex;
        public Sprite sprite;
    }

    [SerializeField] private List<ChannelContent> channels = new();
    [SerializeField] private Image channelDisplay;
    [SerializeField] private Image blackoutImage;
    [SerializeField] private RawImage staticImage;
    [SerializeField] private Sprite jumpScareSprite;
    [SerializeField] private TextMeshProUGUI channelNumberText;

    private int currentChannel = 0;
    private bool isChangingChannel = false;
    private bool jumpScareTriggered = false;
    public NavigationNode assoociatedNode;
    private const int MAX_CHANNEL = 17;
    private const float BLACKOUT_DURATION = 1f;
    private const int JUMP_SCARE_CHANNEL = 17;
    private const float JUMP_SCARE_LOCK_DURATION = 3f;

    void Start()
    {
        if (blackoutImage != null)
            blackoutImage.gameObject.SetActive(false);
        UpdateChannelDisplay();
    }

    public void ChannelUp()
    {
        if (!isChangingChannel)
        {
            currentChannel++;
            if (currentChannel > MAX_CHANNEL)
                currentChannel = 0;
            ChangeChannel();
        }
    }

    public void ChannelDown()
    {
        if (!isChangingChannel)
        {
            currentChannel--;
            if (currentChannel < 0)
                currentChannel = MAX_CHANNEL;
            ChangeChannel();
        }
    }

    private void ChangeChannel()
    {
        StartCoroutine(ChannelChangeRoutine());
    }

    private IEnumerator ChannelChangeRoutine()
    {
        isChangingChannel = true;
        UpdateChannelDisplay();

        // Show blackout
        if (blackoutImage != null)
        {
            blackoutImage.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(BLACKOUT_DURATION);

        // Check if this is the jump scare channel
        if (currentChannel == JUMP_SCARE_CHANNEL && !jumpScareTriggered)
        {
            jumpScareTriggered = true;

            // Hide blackout before showing jump scare
            blackoutImage.gameObject.SetActive(false);
            staticImage.gameObject.SetActive(false);

            ShowJumpScare();

            yield return new WaitForSeconds(JUMP_SCARE_LOCK_DURATION);

            // Show blackout again before switching away from jump scare
            if (blackoutImage != null)
            {
                blackoutImage.gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(BLACKOUT_DURATION);
        }

        // Update display
        DisplayChannel();

        // Hide blackout
        if (blackoutImage != null)
        {
            blackoutImage.gameObject.SetActive(false);
        }

        isChangingChannel = false;
    }

    private void DisplayChannel()
    {
        // Clear all channel images first
        ClearAllChannels();

        // Find if this channel has content
        ChannelContent content = channels.Find(c => c.channelIndex == currentChannel);

        if (content != null && content.sprite != null)
        {
            // Display the image content
            if (channelDisplay != null)
            {
                channelDisplay.sprite = content.sprite;
                channelDisplay.gameObject.SetActive(true);
            }
        }
        else
        {
            // Show static for empty channels
            if (staticImage != null)
            {
                staticImage.gameObject.SetActive(true);
            }
        }
    }

    private void ClearAllChannels()
    {
        if (channelDisplay != null)
        {
            channelDisplay.sprite = null;
            channelDisplay.gameObject.SetActive(false);
        }

        if (staticImage != null)
        {
            staticImage.gameObject.SetActive(false);
        }
    }

    private void ShowJumpScare()
    {
        // Show jump scare image on channel display
        channelDisplay.sprite = jumpScareSprite;
        channelDisplay.gameObject.SetActive(true);
        GameManager.Instance.audioManager.PlayScream1();
        Debug.Log("Jump Scare Triggered!");
    }

    private void UpdateChannelDisplay()
    {
        if (channelNumberText != null)
        {
            channelNumberText.text = currentChannel.ToString("D2");
        }
    }

}
