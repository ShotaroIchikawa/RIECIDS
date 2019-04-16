using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StrokeInputField : MonoBehaviour , IPointerUpHandler, IPointerDownHandler, IDragHandler{

    private List<Stroke> strokes;

    public Material material;
    public Material endMaterial;

    private Color lineColor = Color.green;

    public void OnPointerDown(PointerEventData ed)
    {
        if(ed.button == 0)
        {
            if (image.enabled == true)
            {
                GameObject lineObject = new GameObject() as GameObject;
                LineRenderer line = lineObject.AddComponent<LineRenderer>();
                line.material = Instantiate(material);
                line.material.SetColor("_EmissionColor", lineColor);

                GameObject endLineObject = new GameObject() as GameObject;
                LineRenderer endLine = endLineObject.AddComponent<LineRenderer>();
                endLine.material = Instantiate(endMaterial);
                endLine.material.SetColor("_EmissionColor", Color.grey);

                Stroke stroke = lineObject.AddComponent<Stroke>();
                stroke.photos.AddRange(PhotoManager.Instance.photos);
                stroke.myLineRenderer = line;
                stroke.myEndLineRenderer = endLine;
                stroke.inputID = ed.pointerId;
                strokes.Add(stroke);

                #region 色変え
                if (lineColor == Color.red)
                {
                    lineColor = Color.blue;
                }
                else if (lineColor == Color.blue)
                {
                    lineColor = Color.green;
                }
                else if (lineColor == Color.green)
                {
                    lineColor = Color.red;
                }
                #endregion
            }
        }
    }


    public void OnDrag(PointerEventData ed)
    {
        if(ed.button == 0)
        {
            if (image.enabled == true)
            {
                foreach (Stroke a in strokes)
                {
                    if (ed.pointerId == a.inputID)
                    {
                        Vector2 pos = Camera.main.ScreenToWorldPoint(ed.position);
                        a.AddStroke(pos);
                        a.RenderVenn();
                        a.RenderEndLine();
                    }
                }           
            }
        }
    }

    public void OnPointerUp(PointerEventData ed)
    {
        if(ed.button == 0)
        {
            if (image.enabled == true)
            {
                for (int i = strokes.Count - 1; i > -1; --i)
                {
                    if (ed.pointerId == strokes[i].inputID)
                    {
                        
                        if (strokes[i].Strokes.Count > 0)
                        {
                            strokes[i].RenderEnd();
                            Destroy(strokes[i].myEndLineRenderer.gameObject);
                            StrokeManager.Instance.strokes.Add(strokes[i]);
                            StrokeManager.Instance.CreateKeywordField(strokes[i]);
                        }
                        else
                        {
                            Destroy(strokes[i].myLineRenderer.gameObject);
                            Destroy(strokes[i].myEndLineRenderer.gameObject);
                        }

                        strokes.Remove(strokes[i]);
                    }
                }
            }
        }       
    }


    Image image;
    // Use this for initialization
    void Start()
    {
        strokes = new List<Stroke>();
        image = GetComponent<Image>();
        image.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if((SystemManager.pointingState & SystemManager.PointingState.VENN) == SystemManager.PointingState.VENN)
        {
            image.enabled = true;
        }
        else
        {
            image.enabled = false;
        }

    }
}
