using UnityEngine;
using System.Collections;


public class Utilities : MonoBehaviour
{
    [Header("Neural Network")]
    public float
        bias = -1f;
    public float activationResponse = 1f;

    private System.Random rand = new System.Random();

    private static Utilities _instance;
    public static Utilities instance
    {
        get
        {
            if (!_instance)
            {
                _instance = GameObject.FindObjectOfType<Utilities>();
            }

            return _instance;
        }
    }

    //Used to initialise ANN with random weights and initial positiom of agents
    public float RandomMinMax(double min, double max)
    {
        return (float)(rand.NextDouble() * (max - min) + min);
    }

    public float[] RandomInputBuilder(NeuralNetController controller)
    {
        var retFloat = new float[controller.numOfInput];

        for (int i = 0; i < controller.numOfInput; i++)
        {
            retFloat[i] = Random.value;
        }

        return retFloat;

    }

    public void PrettyPrint(IList list)
    {
        string print = "";

        for (int i = 0; i < list.Count; i++)
        {

            print += list[i] + ((i < list.Count - 1) ? ", " : "");
        }

        Debug.Log(print);
    }
}

