using System.Globalization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Color = UnityEngine.Color;
using Image = UnityEngine.UI.Image;
using UnityEngine.PlayerLoop;
public class SkillNode : MonoBehaviour, IPointerClickHandler
{
    [Serializable]
    public struct CreatePathForNextNode
    {
        public SkillConnectionType skillConnectionType;
        public SkillNode SkillNeighbor;

    }

    public List<CreatePathForNextNode> SkillNodeLinks = new();

    public bool NodeIsActive;
    public bool CanBeDeactivate;





    public LineRenderer lineRenderer;

    public SkillNodeStat Stats;
    public List<Sprite> NodeSprites = new();

    public Image NodeImage;
    public Image StatImage;
    [SerializeField]
    private bool _isStartPosition;



    public List<SkillNode> ParentNode = new();
    public List<SkillNode> ChildrenNodes = new();
    private bool neighborsNodeActivated;
    private bool SetWithHotkey = true;

    private void Awake()
    {


        FindAnyObjectByType<TreeStatController>().NodesObjects.Add(this.gameObject);

        if (SkillNodeLinks.Count != 0 && SkillNodeLinks[0].SkillNeighbor == null) return;
        if (_isStartPosition)
        {
            DrawLineToNextLink();
            return;
        }
        lineRenderer = GetComponent<LineRenderer>();
        FixImage();
        DrawLineToNextLink();
        UpdateNextLinkForParent();
        UpdateSpriteToRandom();
        UpdateStatSprite();

    }
    private void Update()
    {
        if (ParentNode.Count != 0 || ChildrenNodes.Count != 0) return;
        if (gameObject.name == "START") return;


        if (SetWithHotkey)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                BuildTreeInPlayMode();
                SetWithHotkey = false;
            }
        }
    }

    public void FixImage()
    {
        GetComponent<Image>().enabled = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    private void DrawLineToNextLink()
    {


        lineRenderer.positionCount = SkillNodeLinks.Count * 2; // Two positions (start and end) per link

        for (int i = 0; i < SkillNodeLinks.Count; i++)
        {
            CreatePathForNextNode link = SkillNodeLinks[i];

            if (link.SkillNeighbor != null)
            {
                // Get the RectTransforms of this node and the neighbor node
                RectTransform thisRect = GetComponent<RectTransform>();
                RectTransform neighborRect = link.SkillNeighbor.GetComponent<RectTransform>();

                if (thisRect != null && neighborRect != null)
                {
                    Vector3 startPos = thisRect.position;
                    Vector3 endPos = neighborRect.position;

                    // Convert positions to screen space
                    /*   Vector2 startPosScreen = RectTransformUtility.WorldToScreenPoint(Camera.main, startPos);
                      Vector2 endPosScreen = RectTransformUtility.WorldToScreenPoint(Camera.main, endPos); */

                    // Set the positions for the LineRenderer in UI canvas space
                    lineRenderer.SetPosition(i * 2, startPos);
                    lineRenderer.SetPosition(i * 2 + 1, endPos);
                }
            }
        }
    }


    public void UpdateNextLinkForParent()
    {
        foreach (var link in SkillNodeLinks)
        {
            var child = link.SkillNeighbor.gameObject.GetComponent<SkillNode>();
            child.ParentNode.Add(this);
            UpdateNeighborNodes(child, true);
        }
    }

    private void UpdateNeighborNodes(SkillNode newNeighbor, bool needAdd)
    {
        if (needAdd && !ChildrenNodes.Contains(newNeighbor))
        {
            ChildrenNodes.Add(newNeighbor);
        }
        if (!needAdd && ChildrenNodes.Contains(newNeighbor))
        {
            ChildrenNodes.Remove(newNeighbor);
        }
    }

    private void UpdateSpriteToRandom()
    {
        NodeImage.sprite = gg.ReturnRandomValue(NodeSprites);
    }

    private void UpdateStatSprite()
    {
        if (Stats != null)
        {
            StatImage.sprite = Stats.Icon;
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        /* 
               у нас три состояния клика
               1. включаем скилл
               2. отключаем скилл
               3. кликам, но ничего не происходит

               1 и 2 состояние доступно только если нод активирован или активирован его сосед
                */

        ClickNode();
    }

    public void ClickNode()
    {

        neighborsNodeActivated = false;

        foreach (var closeNode in ParentNode)
        {
            if (closeNode.NodeIsActive)
            {
                neighborsNodeActivated = true;
                // если соседний нод активирован, то мы можем использовать этот нод

            }

        }


        if (NodeIsActive)
        {

            if (neighborsNodeActivated)
            {
                // для выключения нода мы должны быть уверенны, что 

                int nodeCanDeacite = 0;
                foreach (var children in ChildrenNodes)
                {
                    if (!children.NodeIsActive)
                    {
                        nodeCanDeacite++;
                    }
                }
                if (ChildrenNodes.Count == nodeCanDeacite || ChildrenNodes.Count == 0)
                {
                    DeactivateNode();

                }

            }

        }
        else if (neighborsNodeActivated)
        {

            ActivateNode();

        }



    }

    public void DeactivateNode()
    {
        Debug.Log(" деактиавация");

        NodeIsActive = !NodeIsActive;


        foreach (var parent in ParentNode)
        {
            parent.lineRenderer.material.color = Color.red;
        }

        ApplyStats(NodeIsActive);
    }
    public void ActivateNode()
    {
        Debug.Log(" активаяция");

        NodeIsActive = !NodeIsActive;
        foreach (var parent in ParentNode)
        {
            parent.lineRenderer.material.color = Color.green;

        }
        ApplyStats(NodeIsActive);
    }


    private void ApplyStats(bool addRemoveStats)
    {
        /* FindAnyObjectByType<PlayerStat>().UpdateSelfStat(Stats, addRemoveStats); */
    }



    [ContextMenu("ResetLinkWithParentNode")]
    private void ResetParentNode()
    {
        foreach (var node in ParentNode)
        {
            node.lineRenderer.positionCount = 0;

        }
        ParentNode.Clear();

    }

    [ContextMenu("AddPathToThisNode")]
    private void BuildTreeInPlayMode()
    {
        ParentNode.Clear();
        var tree = FindAnyObjectByType<TreeStatController>();
        var parent = tree.GetSecondLastNode().GetComponent<SkillNode>();
        /*   ParentNode.Add(tree.GetSecondLastNode().GetComponent<SkillNode>()); */


        CreatePathForNextNode node = new CreatePathForNextNode
        {
            skillConnectionType = SkillConnectionType.Forward, // Set the value for skillConnectionType
            SkillNeighbor = this, // Set the value for SkillNeighbor
        };
        parent.SkillNodeLinks.Add(node);
        parent.DrawLineToNextLink();
        parent.UpdateNextLinkForParent();
        SetWithHotkey = true;

    }


}
