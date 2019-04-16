using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using System.IO;

public class SetBackgroundImage : MonoBehaviour {

    public GameObject background;
    public GameObject FileMenu;
    public Sprite chosenBG;
    public void OnButtonClick()
    {
        if ((SystemManager.systemState & SystemManager.SystemState.MAIN) == SystemManager.SystemState.MAIN)
        {
            var extentions = new[] {
            new ExtensionFilter("Image Files","jpg","png"),
        };
            string[] path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extentions, true);
            if (path.Length > 0)
            {
                var data = File.ReadAllBytes(path[0]);
                Texture2D tex = new Texture2D(1, 1);
                tex.LoadImage(data, true);
                UnityEngine.Rect rec = new UnityEngine.Rect(0, 0, tex.width, tex.height);
                chosenBG = Sprite.Create(tex, rec, new Vector2(0.5f, 0.5f), 100);
                background.GetComponent<SpriteRenderer>().sprite = chosenBG;
                background.GetComponent<Background>().AdjustScreen();
                if (background.activeSelf == false)
                {
                    background.SetActive(true);
                }
            }
        }
        FileMenu.GetComponent<OpenMenu>().OnButtonClick();
    }
}
