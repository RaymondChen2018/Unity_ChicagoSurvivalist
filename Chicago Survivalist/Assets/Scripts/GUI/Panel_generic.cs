using UnityEngine;

public abstract class Panel_generic : MonoBehaviour {
    //Status
    protected enum STATE { OFF, ON, TRANSITION }
    protected STATE state = STATE.OFF;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Animator animator;

    /// <summary>
    /// Is this panel is a state of transitioning?
    /// </summary>
    /// <returns></returns>
    public bool isTransition()
    {
        return state == STATE.TRANSITION;
    }
    /// <summary>
    /// Is this panel fully turned on?
    /// </summary>
    /// <returns></returns>
    public bool isOn()
    {
        return state == STATE.ON;
    }
    /// <summary>
    /// Is this panel fully turned off?
    /// </summary>
    /// <returns></returns>
    public bool isOff()
    {
        return state == STATE.OFF;
    }
    /// <summary>
    /// Display this panel;
    /// If animator is not set to null, please make sure the animator has the fade effect "turnOn"
    /// </summary>
    public virtual void turnOn()
    {
        //Fade in effect
        if(animator == null)
        {
            showPanel();
        }
        //No effect, just switch on
        else
        {
            state = STATE.TRANSITION;
            animator.Play("turnOn");
        }
    }
    /// <summary>
    /// Hide this panel;
    /// If animator is not set to null, please make sure the animator has the fade effect "turnOff"
    /// </summary>
    public virtual void turnOff()
    {
        //Fade out effect etc.
        if (animator == null)
        {
            hidePanel();
        }
        //No effect, just switch off
        else
        {
            state = STATE.TRANSITION;
            animator.Play("turnOff");
        }
    }

    void showPanel()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        state = STATE.ON;
    }
    void hidePanel()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        state = STATE.OFF;
    }
}
