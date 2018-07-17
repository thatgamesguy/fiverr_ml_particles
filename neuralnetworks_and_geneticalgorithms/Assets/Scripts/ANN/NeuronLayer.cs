using System.Collections.Generic;

//Contains a list of neurons: represents a layer
class NeuronLayer
{
    /// <summary>
    /// Number of neurons in layer
    /// </summary>
    public int numNeurons
    {
        get; private set;
    }

    /// <summary>
    /// List of neurons in layer
    /// </summary>
    public List<Neuron> neurons
    {
        get; private set;
    }

    public NeuronLayer(int numOfNeurons, int numOfInput)
    {
        this.numNeurons = numOfNeurons;
        neurons = new List<Neuron>();

        //Adds neurons to neuron list
        for (int i = 0; i < numOfNeurons; ++i)
        {
            neurons.Add(new Neuron(numOfInput));
        }
    }
}

