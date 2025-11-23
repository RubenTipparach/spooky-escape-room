using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PicturePuzzle : Puzzle
{
    public Paintings[] paintings;
    public int[] correctPaintingOrder = { 4, 2, 1, 3 };
    public Color selectedColor = Color.yellow;
    public Color originalColor = Color.white;

    public float swapAnimationDuration = 0.5f;
    public float swapVerticalDistance = .8f;

    private int selectedIndex = -1;
    private bool isAnimating = false;
    private Coroutine currentSwapCoroutine = null;
    private Dictionary<Paintings, Vector3> paintingStartPositions = new Dictionary<Paintings, Vector3>();

    public List<Button> paintingButtons;

    public NavigationNode unlockedNode;

    void Start()
    {
        InitializePaintings();
    }

    void OnDisable()
    {
        // Clean up animation if the puzzle is disabled while animating
        if (isAnimating && currentSwapCoroutine != null)
        {
            StopCoroutine(currentSwapCoroutine);

            // Restore paintings to their intended positions
            foreach (var kvp in paintingStartPositions)
            {
                if (kvp.Key != null)
                {
                    kvp.Key.transform.position = kvp.Value;
                }
            }
            paintingStartPositions.Clear();

            isAnimating = false;
        }

        // Reset selection and UI state
        DeselectPainting();
        ShowAllPaintingButtons();
    }

    private void InitializePaintings()
    {
        // Set initial positions for all paintings
        for (int i = 0; i < paintings.Length; i++)
        {
            if (paintings[i] != null)
            {
                paintings[i].gameObject.SetActive(true);
                // Can add position initialization here if needed
            }
        }
    }

    public void SelectPainting(int paintingIndex)
    {
        if (isAnimating)
            return;

        if (paintingIndex < 0 || paintingIndex >= paintings.Length)
        {
            Debug.LogError($"Invalid painting index: {paintingIndex}");
            return;
        }

        // If clicking the same painting, deselect it
        if (selectedIndex == paintingIndex)
        {
            DeselectPainting();
            return;
        }

        // If a painting is already selected, swap with the new one
        if (selectedIndex != -1)
        {
            SwapPaintings(selectedIndex, paintingIndex);
        }
        else
        {
            // Select this painting
            selectedIndex = paintingIndex;
            SetPaintingButtonColor(selectedIndex);
        }
    }

    private void DeselectPainting()
    {
        if (selectedIndex != -1)
        {
            ResetPaintingButtonColor(selectedIndex);
            selectedIndex = -1;
        }
    }

    private void SwapPaintings(int firstIndex, int secondIndex)
    {
        if (currentSwapCoroutine != null)
        {
            StopCoroutine(currentSwapCoroutine);
        }

        // Store start positions before animation
        Paintings firstPainting = paintings[firstIndex];
        Paintings secondPainting = paintings[secondIndex];
        paintingStartPositions[firstPainting] = firstPainting.transform.position;
        paintingStartPositions[secondPainting] = secondPainting.transform.position;

        GameManager.Instance.audioManager.PlayPaintingSound();
        currentSwapCoroutine = StartCoroutine(SwapAnimationRoutine(firstIndex, secondIndex));
    }

    private IEnumerator SwapAnimationRoutine(int firstIndex, int secondIndex)
    {
        isAnimating = true;
        HideAllPaintingButtons();

        Paintings firstPainting = paintings[firstIndex];
        Paintings secondPainting = paintings[secondIndex];

        Vector3 firstStartPos = firstPainting.transform.position;
        Vector3 secondStartPos = secondPainting.transform.position;

        Vector3 firstUpPos = firstStartPos + Vector3.up * swapVerticalDistance;
        Vector3 secondDownPos = secondStartPos - Vector3.up * swapVerticalDistance;

        float elapsedTime = 0f;

        // Animate paintings moving up/down (only Y/Z axis)
        while (elapsedTime < swapAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / swapAnimationDuration;

            Vector3 firstNewPos = Vector3.Lerp(firstStartPos, firstUpPos, progress);
            firstNewPos.x = firstStartPos.x;
            firstPainting.transform.position = firstNewPos;

            Vector3 secondNewPos = Vector3.Lerp(secondStartPos, secondDownPos, progress);
            secondNewPos.x = secondStartPos.x;
            secondPainting.transform.position = secondNewPos;

            yield return null;
        }

        // Ensure exact positions (preserve X)
        firstPainting.transform.position = new Vector3(firstStartPos.x, firstUpPos.y, firstUpPos.z);
        secondPainting.transform.position = new Vector3(secondStartPos.x, secondDownPos.y, secondDownPos.z);

        // Swap positions in the array
        (paintings[firstIndex], paintings[secondIndex]) = (paintings[secondIndex], paintings[firstIndex]);

        elapsedTime = 0f;

        // Animate paintings moving horizontally to swap positions (only Z axis)
        while (elapsedTime < swapAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / swapAnimationDuration;

            Vector3 firstNewPos = firstUpPos;
            firstNewPos.z = Mathf.Lerp(firstUpPos.z, secondStartPos.z, progress);
            firstNewPos.x = firstStartPos.x;
            firstPainting.transform.position = firstNewPos;

            Vector3 secondNewPos = secondDownPos;
            secondNewPos.z = Mathf.Lerp(secondDownPos.z, firstStartPos.z, progress);
            secondNewPos.x = secondStartPos.x;
            secondPainting.transform.position = secondNewPos;

            yield return null;
        }

        // Ensure exact final positions (preserve X)
        firstPainting.transform.position = new Vector3(firstStartPos.x, firstUpPos.y, secondStartPos.z);
        secondPainting.transform.position = new Vector3(secondStartPos.x, secondDownPos.y, firstStartPos.z);

        elapsedTime = 0f;

        // Animate paintings moving back down to final positions (only Y axis)
        while (elapsedTime < swapAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / swapAnimationDuration;

            Vector3 firstNewPos = firstPainting.transform.position;
            firstNewPos.y = Mathf.Lerp(firstUpPos.y, secondStartPos.y, progress);
            firstNewPos.x = firstStartPos.x;
            firstPainting.transform.position = firstNewPos;

            Vector3 secondNewPos = secondPainting.transform.position;
            secondNewPos.y = Mathf.Lerp(secondDownPos.y, firstStartPos.y, progress);
            secondNewPos.x = secondStartPos.x;
            secondPainting.transform.position = secondNewPos;

            yield return null;
        }

        // Complete the animation
        CompleteSwapAnimation(firstPainting, secondPainting, firstStartPos, secondStartPos, secondStartPos.z, firstStartPos.z);
    }

    private void CompleteSwapAnimation(Paintings firstPainting, Paintings secondPainting,
        Vector3 firstStartPos, Vector3 secondStartPos, float firstFinalZ, float secondFinalZ)
    {
        // Ensure exact final positions (preserve X)
        firstPainting.transform.position = new Vector3(firstStartPos.x, secondStartPos.y, firstFinalZ);
        secondPainting.transform.position = new Vector3(secondStartPos.x, firstStartPos.y, secondFinalZ);

        // Reset selection
        DeselectPainting();
        ShowAllPaintingButtons();
        isAnimating = false;
        currentSwapCoroutine = null;

        // Check if puzzle is solved
        CheckPuzzleSolved();
    }

    private void SetPaintingButtonColor(int paintingIndex)
    {
        if (paintingIndex >= 0 && paintingIndex < paintingButtons.Count)
        {
            Button button = paintingButtons[paintingIndex];
            Image buttonImage = button.GetComponent<Image>();

            //ColorBlock colors = button.colors;
            buttonImage.color = selectedColor;
            //button.colors = colors;
            Debug.Log($"Painting {paintingIndex} selected");
        }
    }

    private void ResetPaintingButtonColor(int paintingIndex)
    {
        if (paintingIndex >= 0 && paintingIndex < paintingButtons.Count)
        {
            Button button = paintingButtons[paintingIndex];
            Image buttonImage = button.GetComponent<Image>();

            buttonImage.color = originalColor;
            Debug.Log($"Painting {paintingIndex} deselected");
        }
    }

    private void HideAllPaintingButtons()
    {
        foreach (Button button in paintingButtons)
        {
            if (button != null)
            {
                button.gameObject.SetActive(false);
            }
        }
    }

    private void ShowAllPaintingButtons()
    {
        foreach (Button button in paintingButtons)
        {
            if (button != null)
            {
                button.gameObject.SetActive(true);
            }
        }
    }

    private void CheckPuzzleSolved()
    {
        // Print current painting order
        string currentOrder = $"[{Time.time:F3}] Current order: ";
        for (int i = 0; i < paintings.Length; i++)
        {
            currentOrder += paintings[i].paintingID + " ";
        }
        Debug.Log(currentOrder);

        // Print expected order
        string expectedOrder = $"[{Time.time:F3}] Expected order: ";
        for (int i = 0; i < correctPaintingOrder.Length; i++)
        {
            expectedOrder += correctPaintingOrder[i] + " ";
        }
        Debug.Log(expectedOrder);

        // Check if current painting order matches the correct order
        for (int i = 0; i < paintings.Length; i++)
        {
            if (paintings[i].paintingID != correctPaintingOrder[i])
            {
                return; // Puzzle not solved
            }
        }

        // Puzzle solved!
        Debug.Log("Picture puzzle solved!");
        UnlockNode();
    }

    private void UnlockNode()
    {
        GameManager.Instance.UnlockNode(unlockedNode);
        GameManager.Instance.uiManager.SetDoorUnlockedMessage("Bedroom 2");
        SolvePuzzle();
    }

}
