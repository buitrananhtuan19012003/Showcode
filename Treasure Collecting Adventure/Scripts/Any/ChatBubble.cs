using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatBubble : MonoBehaviour
{
    [SerializeField] private Vector2 padding = new Vector2(4f, 4f);

    private Animator animator;
    private SpriteRenderer backgroundSprite;
    private TMP_Text text;

    private void Awake()
    {
        this.animator = GetComponent<Animator>();
        this.backgroundSprite = GetComponentInChildren<SpriteRenderer>();
        this.text = GetComponentInChildren<TMP_Text>();
    }

    public void Setup(string text, float timeExist)
    {
        this.text.SetText(text);
        this.text.ForceMeshUpdate();

        Vector2 textSize = this.text.GetRenderedValues(false);
        Vector2 offset = new Vector2(-this.padding.x / 2f, 0f);
        this.backgroundSprite.size = textSize + this.padding;
        this.backgroundSprite.transform.localPosition = new Vector2(this.backgroundSprite.size.x / 2, 0f) + offset;

        this.animator.SetTrigger("FadeIn");

        Destroy(gameObject, timeExist);
    }
}
