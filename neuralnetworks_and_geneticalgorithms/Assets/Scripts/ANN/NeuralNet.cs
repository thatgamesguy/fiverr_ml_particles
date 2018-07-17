using System;
using System.Collections.Generic;

/// <summary>
/// Contains a list of layers: represents the whole neural net
/// </summary>
public class NeuralNet
{
    private int m_NumOfInput; //Number of inputs for each neuron
    public int numOfInput { get { return m_NumOfInput; } }

    private int m_NumOfOutput; //Number of outputs of each neuron
    public int numOfOutput { get { return m_NumOfOutput; } }

    private int m_NumHiddenLayers; //Number of hidden layers
    public int numOfHiddenLayers { get { return m_NumHiddenLayers; } }

    private int m_NumOfNeuronsPerHiddenLayer; //Number of neurons per hidden layer
    public int numOfNeuronsPerHiddenLayer { get { return m_NumOfNeuronsPerHiddenLayer; } }

    private List<NeuronLayer> m_Layers; //List containing layers
    private List<float> m_Weights = new List<float>();

    public List<int> splitPoints { get; private set; }

    public NeuralNet(int numOfInput, int numOfOutput, int numOfHiddenLayers, int numOfNeuronsPerHiddenLayer)
    {
        m_NumOfInput = numOfInput;
        m_NumOfOutput = numOfOutput;
        m_NumHiddenLayers = numOfHiddenLayers;
        m_NumOfNeuronsPerHiddenLayer = numOfNeuronsPerHiddenLayer;

        m_Layers = new List<NeuronLayer>();

        CreateNeuralNet();

        CalculateSplitPoints();

    }

    private void CalculateSplitPoints()
    {
        splitPoints = new List<int>();

        int weightCounter = 0;

        // Each layer
        for (int i = 0; i < m_Layers.Count; i++)
        {
            // Eeach neuron
            for (int j = 0; j < m_Layers[i].numNeurons; j++)
            {
                // Each weight
                for (int k = 0; k < m_Layers[i].neurons[j].numInputs; k++)
                {
                    weightCounter++;
                }

                splitPoints.Add(weightCounter - 1);
            }
        }
    }

    public void CreateNeuralNet()
    {
        // Create first layer
        m_Layers.Add(new NeuronLayer(numOfNeuronsPerHiddenLayer, numOfInput));

        // Create any other subsequent hidden layers
        for (int i = 0; i < numOfHiddenLayers; i++)
        {
            // Input from first hidden layer
            m_Layers.Add(new NeuronLayer(numOfNeuronsPerHiddenLayer,
                                         numOfNeuronsPerHiddenLayer));
        }

        // Output layer
        // Input from subsequent or first hidden layer
        m_Layers.Add(new NeuronLayer(numOfOutput, numOfNeuronsPerHiddenLayer));


    }
	public List<float> Update(params float[] inputs)
	{
		List<float> inputList = new List<float>();
		inputList.AddRange(inputs);
		return Update (inputList);
	}

    //Receives input and returns output: performs caluclations for neural net
	public List<float> Update(List<float> inputList)
    {
        //Output from each layer
        List<float> outputs = new List<float>();

        int weightCount = 0;

        //Return empty if not corrent number of inputs
        if (inputList.Count != m_NumOfInput)
        {
            Console.WriteLine("NeuralNet|Update|Size of inputs list not equal number of inputs");
            return outputs;
        }

        //Each layer
        for (int i = 0; i < m_Layers.Count; i++)
        {
            if (i > 0)
            {
                //Clear input and add output from previous layer
                inputList.Clear();
                inputList.AddRange(outputs);

                outputs.Clear();

                weightCount = 0;
            }



            for (int j = 0; j < m_Layers[i].numNeurons; ++j)
            {
                float netInput = 0.0f;

                int numInputs = m_Layers[i].neurons[j].numInputs;

                //Each weight
                for (int k = 0; k < numInputs - 1; ++k)
                {
                    //Sum the weights x inputs
                    netInput += m_Layers[i].neurons[j].weight[k] *
                        inputList[weightCount++];
                }

                //Add in the bias
                netInput += m_Layers[i].neurons[j].weight[numInputs - 1] *
                    Utilities.instance.bias;

                

                //Store result in output
                outputs.Add(netInput);

                weightCount = 0;
            }
        }

        return outputs;
    }

    //Gets weights from network
    public List<float> GetWeights()
    {
        if(m_Weights.Count == 0)
        {
            CalculateWeights();
        }
      
        return m_Weights;
    }

    //Gets number of weights
    public int GetNumberOfWeights()
    {
        if(m_Weights.Count == 0)
        {
            CalculateWeights();
        }

        return m_Weights.Count;
    }

    //Sets weights for network (initially set to random values)
    public void SetWeights(List<float> weights)
    {

 
        //Used to cycle through received weights
        int weightCount = 0;

        //Each layer
        for (int i = 0; i < m_Layers.Count; ++i)
        {
            //Each neuron
            for (int j = 0; j < m_Layers[i].numNeurons; ++j)
            {
                //Each weight
                for (int k = 0; k < m_Layers[i].neurons[j].numInputs; ++k)
                {
                    m_Layers[i].neurons[j].weight[k] = weights[weightCount++];
                }
            }
        }

        m_Weights = weights;
    }

    //S shaped output
    public float Sigmoid(float netInput)
    {
        return (float)(1 / (1 + Math.Exp(-netInput / Utilities.instance.activationResponse)));
    }

    
    public override bool Equals(object obj)
    {
        if(!(obj is NeuralNet))
        {
            return false;
        }

        if(obj == this)
        {
            return true;
        }

        NeuralNet other = (NeuralNet)obj;

        var otherWeights = other.GetWeights();
        var thisWeights = GetWeights();

        if(otherWeights.Count != thisWeights.Count)
        {
            return false;
        }

        for(int i = 0; i < thisWeights.Count; i++)
        {
            if(thisWeights[i] != otherWeights[i])
            {
                return false;
            }
        }

        return true;

        /*
        return (other.m_NumOfInput == this.m_NumOfInput && other.m_NumOfOutput == this.m_NumOfOutput
         && other.m_NumHiddenLayers == this.m_NumHiddenLayers
         && other.m_NumOfNeuronsPerHiddenLayer == this.m_NumOfNeuronsPerHiddenLayer);
         */
    }

    public override int GetHashCode()
    {
        List<float> weights = GetWeights();

        return new HashCodeBuilder().
                        Add(numOfInput).
                        Add(numOfOutput).
                        Add(numOfHiddenLayers).
                        Add(numOfNeuronsPerHiddenLayer).
                        Add(weights[0]).
                        Add(weights[1]).
                        Add(weights[2]).
                        Add(weights[3]).
                        GetHashCode();
    }

    public void Mutate()
    {
        int numOfWeights = GetNumberOfWeights();

        int numOfSwaps = UnityEngine.Random.Range(0, numOfWeights / 2);

        List<float> weights = GetWeights();

        for (int i = 0; i < numOfSwaps; i++)
        {
            int pos1 = UnityEngine.Random.Range(0, numOfWeights);
            int pos2 = pos1;

            while (pos1 == pos2)
            {
                pos2 = UnityEngine.Random.Range(0, numOfWeights);
            }

            float temp = weights[pos1];
            weights[pos1] = weights[pos2];
            weights[pos2] = temp;
        }

        SetWeights(weights);
    }

    private int GetHash(int value)
    {
        return 31 + value; 
    }

    private void CalculateWeights()
    {
        m_Weights.Clear();

        //Each layer
        for (int i = 0; i < m_Layers.Count; ++i)
        {
            //Each neuron
            for (int j = 0; j < m_Layers[i].numNeurons; ++j)
            {
                //Each weight
                for (int k = 0; k < m_Layers[i].neurons[j].numInputs; ++k)
                {
                    m_Weights.Add(m_Layers[i].neurons[j].weight[k]);
                }
            }
        }

    }
}

public sealed class HashCodeBuilder
{
    private int hash = 17;

    public HashCodeBuilder Add(int value)
    {
        unchecked
        {
            hash = hash * 31 + value; 
        }
        return this;
    }

    public HashCodeBuilder Add(object value)
    {
        return Add(value != null ? value.GetHashCode() : 0);
    }

    public HashCodeBuilder Add(float value)
    {
        return Add(value.GetHashCode());
    }

    public HashCodeBuilder Add(double value)
    {
        return Add(value.GetHashCode());
    }

    public override int GetHashCode()
    {
        return hash;
    }
}

