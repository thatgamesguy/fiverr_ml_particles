using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GeneticAgent : MonoBehaviour, IComparable
{
    const int neuralNumOfInput = 7;
    const int neuralNumOfHiddenLayers = 1;
    const int neuralNumOfNeuronsInHiddenLayer = 10;
    const int neuralNumOfOutput = 2;

    public float sightRadius = 10f;
    public float moveSpeed = 100f;
    public float energy { get; set; }
    public float timeAlive { get; private set; }
    public NeuralNet neuralNetwork { get; set; }


    // Use this for initialization
    void Start()
    {
        energy = 100f;
        neuralNetwork = new NeuralNet(neuralNumOfInput, 
                                      neuralNumOfOutput, neuralNumOfHiddenLayers, 
                                      neuralNumOfNeuronsInHiddenLayer);
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;

        List<float> neuralNetInput = BuildNetworkInput();

        // Get output from neural network
        List<float> neuralNetOutput = neuralNetwork.Update(neuralNetInput);

        float x = neuralNetOutput[0];
        float y = neuralNetOutput[1];

        if (float.IsNaN(x) || float.IsNaN(y)) { return; }
        transform.position += new Vector3(x, y) * moveSpeed * Time.deltaTime;

    }

    List<float> BuildNetworkInput()
    {
        List<float> networkInput = new List<float>();

        for (int i = 0; i < neuralNumOfInput; i++)
        {
            networkInput.Add(0f);
        }

        Collider2D[] closestColliders = Physics2D.OverlapCircleAll(transform.position, sightRadius);

        if (closestColliders.Length > 0)
        {
            Collider2D closest = null;
            float closestDistance = float.MaxValue;
            foreach(var c in closestColliders)
            {
                if (c.gameObject.GetInstanceID() == gameObject.GetInstanceID()) { continue; }
                float d = Vector2.Distance(transform.position, c.transform.position);
                if(d < closestDistance)
                {
                    closestDistance = d;
                    closest = c;
                }

            }
            
            var heading = closest.transform.position - transform.position;
            var distance = heading.magnitude;
            var to = heading / distance;

            networkInput[0] = to.x;
            networkInput[1] = to.y;

            // We need to convert the distance to a number between 0 and 1 to be used as input.
            float normalisedDistance = distance / sightRadius;

            // We want a higher number to represent a closer agent so we invert the number.
            networkInput[2] = 1 - normalisedDistance;

            Vector2 pos = transform.position;

            float leftDistance = pos.x;
            float topDistance = pos.y;
            float rightDistance = Mathf.Abs(Screen.width - pos.x);
            float bottomDistance = Mathf.Abs(Screen.height - pos.y);

            networkInput[3] = 1 - (leftDistance / Screen.width);
            networkInput[4] = 1 - (rightDistance / Screen.width);
            networkInput[5] = 1 - (topDistance / Screen.height);
            networkInput[6] = 1 - (bottomDistance / Screen.height);
        }

        return networkInput;
    }


    public int CompareTo(object obj)
    {
        if (obj == null)
        {
            return 1;
        }

        var otherAgent = (GeneticAgent)obj;

        if (otherAgent.timeAlive > this.timeAlive)
        {
            return 1;
        }
        else if (otherAgent.timeAlive < this.timeAlive)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

}
