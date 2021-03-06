﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockVisual : VisualNode {


    //Not neccessarily...neccesarry because else has no condition but is technically a block visual
    public GameObject openingBlock;
    public GameObject closingBlock;

    //This should just be there
    
    VisualNode head;

    public BinaryOperationVisual condition;
    //Basically forcing state on these, now when would I reset current? I guess leave that up to develeoper
    //but that's for sure a design flaw.
    VisualNode current;
    VisualNode tail;

    public Text label;

    //Don't need attribute for tailGraphic, just part of prefab


    // Use this for initialization
    void Start() {


        this.id = label.text;


        //Set the call backs


        head = current = null;
        tail = null;
    }

    public GameObject OpeningBlock
    {
        get
        {
            return openingBlock;
        }
    }

    public GameObject ClosingBlock
    {
        get
        {
            return closingBlock;
        }
    }
   
    public override VisualNode Next{

        set
        {

            base.Next = value;

            //Everything I do is same as in base, except position is relative to closing block's position, not the node itself.
            //I may also get rid of the indent here, but that may or may not happen.
            if (next != null && closingBlock != null)
            {
                next.transform.position = closingBlock.transform.position;
                this.moveDown(next.gameObject);

            }
            //this.moveLeft(next.gameObject);


        }
        get
        {

            return base.Next;
        }
     }
	
    public VisualNode Head
    {
        get
        {
            return head;
        }
    }

    public void setHead(VisualNode visualNode)
    {
        //Can't put something that's not a value inside here
        if (visualNode is BlockVisual)
        {
            return;
        }

        if (head != null)
            visualNode.Next = head.Next;

        head = visualNode;

    }

	public void append(VisualNode node)
    {
        //Always start off at head's next

        if (head == null)
        {
            head = node;
            head.transform.position = openingBlock.transform.position;
            //Theoritcally should be working, need to make sure.
            this.moveDown(head.gameObject);
            current = head;

            return;
        }

        VisualNode curr = head;//.Next;

        Debug.Log("Attached to " + this.gameObject.name + "'s body is the node" + node.gameObject.name);

        while (curr.Next != null)
        {
           // Debug.Log("then the node" + node.gameObject.name); Yup works

            curr = curr.Next;
        }

        curr.Next = node;
        node.Next = null;

        current = head;
      

        this.moveDown(closingBlock);
       
    }


    //Have these already set, just change to nextChild to make explicit
    public VisualNode nextNode()
    {
        

        VisualNode prev = current;

        if (current != null)
        {
            current = current.Next;
        }
        else
        {
            //If null then go back to head.
            current = head;
        }

        return prev;
    }

    public VisualNode prevNode()
    {
        current = current.Prev;
        return current;
    }

    public VisualNode CurrentNode
    {
        get
        {
            return current;
        }
    }

  
    public override void delete()
    {
        VisualNode current = head;
        VisualNode prev = null;


        //Deletes everything nested under it.
        while (current != null)
        {
            //Okay this is where next is wanted to overriden, cause nested loops also recursively deleted.
            prev = current;
            current = current.Next;
            Destroy(prev);
            prev = null;
        }

        //To delete itself
        base.delete();

    }
}
