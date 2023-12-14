using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlGuidePanel : BaseUIElement
{
    [Header("CONTROL GUIDE PANEL")]
    [SerializeField] protected List<CanvasGroup> listGuidePanel = new List<CanvasGroup>();

    protected int currentIndex = 0;

    //protected override void LoadComponent()
    //{
    //    base.LoadComponent();
    //    if (this.listGuidePanel.Count == 0)
    //    {
    //        CanvasGroup[] canvasGroups = GetComponentsInChildren<CanvasGroup>();
    //        if (canvasGroups.Length > 1)
    //        {
    //            for (int i = 1; i < canvasGroups.Length; i++) //Skip CanvasGroup component in this gameobject
    //            {
    //                this.listGuidePanel.Add(canvasGroups[i]);
    //            }
    //        }
    //    }
    //}

    public override void Show(object data)
    {
        base.Show(data);

        if (data is int)
        {
            int newData = (int)data;
            this.SwitchIndexGuide(newData);
        }
        else
        {
            for (int i = 0; i < this.listGuidePanel.Count; i++)
            {
                this.listGuidePanel[i].alpha = i == 0 ? 1 : 0;
            }
        }
    }

    protected virtual void SwitchGuideOffset(int offset)
    {
        if (this.listGuidePanel.Count == 0) return;

        this.currentIndex += offset;
        this.SwitchIndexGuide(this.currentIndex);
    }

    protected virtual void SwitchIndexGuide(int index)
    {
        this.currentIndex = index;
        if (this.currentIndex >= this.listGuidePanel.Count)
            this.currentIndex = 0;
        else if (this.currentIndex < 0)
            this.currentIndex = this.listGuidePanel.Count - 1;

        for (int i = 0; i < this.listGuidePanel.Count; i++)
        {
            this.listGuidePanel[i].alpha = this.currentIndex == i ? 1 : 0;
        }
    }

    public virtual void OnClickNextButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_BTN_BOOK_01_PAGE_TURN_14);
        }

        this.SwitchGuideOffset(1);
    }

    public virtual void OnClickPrevButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_BTN_BOOK_01_PAGE_TURN_14);
        }

        this.SwitchGuideOffset(-1);
    }
}
