using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    public GameObject scrollbar;
    private float scroll_pos = 0;
    private float[] pos;

    void Update()
    {
        // Initialize the positions array
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f); // Calculate the distance between each menu item

        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i; // Properly set each position incrementally
        }

        // Check for user input
        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                // Snap the scrollbar to the nearest position
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }

        // Scale menu items based on proximity to the focused position
        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                // Focused item scales up
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
            }
            else
            {
                // Other items scale down
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(0.8f, 0.8f), 0.1f);
            }
        }
    }
}
