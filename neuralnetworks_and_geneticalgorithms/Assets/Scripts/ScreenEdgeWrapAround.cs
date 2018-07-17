using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEdgeWrapAround : MonoBehaviour
{
    private GeneticAgent agent;
    // Use this for initialization
    void Start()
    {
        agent = GetComponent<GeneticAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        //Vector2 newPos = transform.position;

        if(screenPos.x < 0)
        {
            agent.energy = 0f;
           // newPos.x = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, screenPos.y)).x;
        }
        else if(screenPos.x > Screen.width)
        {
            agent.energy = 0f;
            //newPos.x = Camera.main.ScreenToWorldPoint(new Vector2(0, screenPos.y)).x;
        }

        if (screenPos.y < 0)
        {
            agent.energy = 0f;
            //newPos.y = Camera.main.ScreenToWorldPoint(new Vector2(screenPos.x, Screen.height)).y;
        }
        else if (screenPos.y > Screen.height)
        {
            agent.energy = 0f;
            //newPos.y = Camera.main.ScreenToWorldPoint(new Vector2(screenPos.x, 0)).y;
        }


        //transform.position = newPos;
    }
}
