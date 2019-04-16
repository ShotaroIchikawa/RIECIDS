using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    Texture2D texture;
    SpriteRenderer spriteRenderer;
    public GameObject camera;
    public Sprite BackgroundSprite;

    AutoDisplayAdjuster auto;
	// Use this for initialization
	void Start () {
        BackgroundSprite = Sprite.Create(Texture2D.whiteTexture, new UnityEngine.Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 100);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        auto = camera.GetComponent<AutoDisplayAdjuster>();
    }
	

	// Update is called once per frame
	void Update () {
        if (AutoDisplayAdjuster.screenChange)
        {
            AdjustScreen();
        }
	}

    public void AdjustScreen()
    {
        //spriteのスケールをワールド座標系で取得
        float width = spriteRenderer.sprite.bounds.size.x;
        float height = spriteRenderer.sprite.bounds.size.y;


        float wx = auto.TopRight().x - auto.BottomLeft().x;
        float wy = auto.TopRight().y - auto.BottomLeft().y;
        // カメラの外枠のスケールをワールド座標系で取得
        float worldScreenHeight = Camera.main.orthographicSize * 2f;

        Vector3 camPos = Camera.main.transform.position;
        float x = Screen.width;
        float y = Screen.height;
        
        float worldScreenWidth = (worldScreenHeight / y) * x;
        transform.localScale = new Vector2(worldScreenWidth / width, worldScreenHeight / height);
        transform.position = new Vector3(auto.TopRight().x - wx/2, auto.TopRight().y - wy/2, 25);
    }
}
