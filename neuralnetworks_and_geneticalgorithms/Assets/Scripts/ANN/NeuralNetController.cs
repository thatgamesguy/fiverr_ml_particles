using UnityEngine;
using System.Collections.Generic;


public class NeuralNetController : MonoBehaviour
{
    public int numOfInput;
    public int numOfOutput;
    public int numOfHiddenLayers;
    public int numOfNeuronsPerHiddenLayer;

    private NeuralNet _neuralNet;

    void Awake()
    {
        _neuralNet = new NeuralNet(numOfInput, numOfOutput, numOfHiddenLayers, numOfNeuronsPerHiddenLayer);
    }

    // Update is called once per frame
    public List<float> UpdateNeuralNet(params float[] input)
    {
        if (_neuralNet == null)
            return null;

        return _neuralNet.Update(input);
    }
}

