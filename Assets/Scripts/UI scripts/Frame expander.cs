using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FrameExpander : MonoBehaviour, IPointerClickHandler
{
    private LayoutElement layoutElement;
    public float collapsedHeight = 114.7721f;
    public float expandedHeight = 498.21f;
    private bool isExpanded = false;

    void Awake()
    {
        layoutElement = GetComponent<LayoutElement>();
        // Set initial state
        layoutElement.preferredHeight = collapsedHeight;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isExpanded = !isExpanded;
        
        // Toggle the height
        layoutElement.preferredHeight = isExpanded ? expandedHeight : collapsedHeight;
        
        // If your frame is inside a Vertical Layout Group, 
        // this will automatically push other items down!
    }
}
