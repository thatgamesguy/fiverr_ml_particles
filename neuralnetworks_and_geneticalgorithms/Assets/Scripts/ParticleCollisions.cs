using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisions : MonoBehaviour
{

    private GeneticAgent agent;

	private void Awake()
	{
        agent = GetComponent<GeneticAgent>();
	}

    /*
	private void Update()
	{
        if(Physics2D.OverlapCircle(transform.position, 10f))
        {
            agent.energy = 0f;
        }
	}
*/

	void OnTriggerEnter2D(Collider2D collision)
	{
        agent.energy = 0f;
	}
	
}
