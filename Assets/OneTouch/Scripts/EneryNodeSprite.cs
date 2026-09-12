using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EneryNodeSprite : MonoBehaviour
{
    public static SpriteRenderer currentNode;
    SpriteRenderer cSp;

    /// <summary>
    /// the UI event for click on a node.
    /// </summary>
    int state = 0;


    void Start()
    {
        GameData.getInstance().level.line.startColor = GameData.getInstance().currentColor;
        GameData.getInstance().level.line.endColor = GameData.getInstance().currentColor;
        changeState(0);
    }


    public void OnMouseDown()
    {
        if (GameData.getInstance().isfail)
            return;

        if (AudioManager.Instance)
        {
            AudioManager.Instance.TouchNode();
        }

        cSp = gameObject.GetComponent<SpriteRenderer>();


        if (currentNode == null || cSp != currentNode)
        {
            //get link name
            GameObject tnode0;
            GameObject tnode1;

            tnode0 = transform.parent.gameObject;

            if (currentNode)
            {
                tnode1 = currentNode.transform.parent.gameObject;

                int tnodeId0 = int.Parse(tnode0.name.Split("_"[0])[1]);
                int tnodeId1 = int.Parse(tnode1.name.Split("_"[0])[1]);

                //GameData.getInstance().level.reverseList.Add(new int[] { tnodeId1, tnodeId0 });

                //Debug.Log("From : " + tnodeId0 + "    To: " + tnodeId1);
                string tlinklineName = "linkLine" + "_" + Mathf.Min(tnodeId0, tnodeId1) + "_" + Mathf.Max(tnodeId0, tnodeId1);
                //Debug.Log(tlinklineName);
                GameObject tLinkLine = GameObject.Find(tlinklineName);

                if (tLinkLine)
                {

                    //last node turn to blue
                    EneryLinkSprite enerylink = tLinkLine.GetComponentInChildren<EneryLinkSprite>();

                    if (enerylink.state == 1 || enerylink.state == 2)
                    {
                        return;
                        //enerylink.changeState(2);
                    }
                    else
                    {
                        enerylink.changeState(1);
                        //light the node only when can link a new line
                        changeState(2);//turn green
                        if (currentNode)
                        {

                            currentNode.gameObject.GetComponent<EneryNodeSprite>().changeState(1);
                            //active node
                            currentNode = gameObject.GetComponentInChildren<SpriteRenderer>();//pName.Split("_"[0])[1];
                        }

                        //link a useful line

                        GameData.getInstance().nLink--;

                        if (GameData.getInstance().nLink == 0)
                        {
                            //fire event;	
                            GameData.getInstance().level.line.gameObject.SetActive(false);
                            GameData.getInstance().level.gameWin();
                        }
                        ChangeLinePosition();
                    }
                }
            }
            else
            {
                //Debug.Log("Click First Node...");
                //first node
                changeState(1);
                //active node
                currentNode = gameObject.GetComponentInChildren<SpriteRenderer>();//pName.Split("_"[0])[1];
            }
        }
    }

    void OnMouseDrag()
    {
        OnBeginDrag();
        OnDrag();
    }

    void OnMouseUp()
    {
        isShowLine = true;
        GameData.getInstance().level.line.gameObject.SetActive(false);
    }

    void ChangeLinePosition()
    {
        GameData.getInstance().level.line.SetPosition(1, transform.position);
        GameData.getInstance().level.line.SetPosition(0, transform.position);
    }

    /// <summary>
    /// Changes the color when touch a node
    /// </summary>
    /// <param name="state_">State.</param>
    public void changeState(int state_)
    {
        state = state_;
        switch (state)
        {
            case 0:
                //LeanTween.color(gameObject, new Color(1, 1, 1, 1), 0.3f);
                LeanTween.color(gameObject, GameData.getInstance().currentColor, 0f);
                break;
            case 1:
                LeanTween.color(gameObject, new Color(.2f, .6f, .8f, 1), 0.3f);
                break;
            case 2:
                LeanTween.color(gameObject, new Color(0, 1, 0, 1), 0.3f);
                break;
        }
    }


    #region Akash

    CircleCollider2D lineCollider;
    bool isShowLine = true;

    public void OnBeginDrag()
    {

        if (GameData.getInstance().isfail)
        {
            return;
        }

        if (isShowLine)
        {
            GameData.getInstance().level.line.gameObject.SetActive(true);
            lineCollider = GameData.getInstance().level.line.transform.GetChild(0).GetComponent<CircleCollider2D>();
            GameData.getInstance().level.line.SetPosition(0, transform.position);
            isShowLine = false;
        }
    }

    public void OnDrag()
    {

        if (GameData.getInstance().isfail)
        {
            return;
        }

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;

        lineCollider.gameObject.transform.position = pos;

        if (GameData.getInstance().level.line.gameObject.activeInHierarchy)
        {
            GameData.getInstance().level.line.SetPosition(1, pos);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameData.getInstance().level.line.GetPosition(0) != transform.position && collision.gameObject.CompareTag("line"))
        {
            //GameData.getInstance().level.line.SetPosition(1, transform.position);

            //GameData.getInstance().level.line.SetPosition(0, transform.position);

            OnMouseDown();
        }
    }

    #endregion /Akash

}
