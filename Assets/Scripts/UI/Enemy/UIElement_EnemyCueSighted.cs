using UnityEngine;
using UnityEngine.UI;

public class UIElement_EnemyCueSighted : UIElement_FillingBar
{
    [SerializeField] private View_EnemyCueSightedBar _enemyCueSightedBar;

    protected override void OnStart()
    {
        base.OnStart();
        _enemyCueSightedBar = GetComponentInParent<View_EnemyCueSightedBar>();
    }

    override public void SetFilling(float _fillingNormalised)
    {
        base.SetFilling(_fillingNormalised);

        if (_fillingNormalised <= 0)
        {
            _enemyCueSightedBar.OnHide();
        }
        else if (_fillingNormalised > 0 && !transform.parent.gameObject.activeSelf)
        {
            _enemyCueSightedBar.OnShow();
        }
    }
}
