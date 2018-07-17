using UnityEngine;

[RequireComponent (typeof(NeuralNetController), typeof(Utilities))]
public class ANNTest : MonoBehaviour
{
	private NeuralNetController _netController;
	
	void Start ()
	{
		_netController = GetComponent<NeuralNetController> ();
	}

	void Update ()
	{
        // Create random input (this wouldn't be random in a real application).
        var neuralNetInput = Utilities.instance.RandomInputBuilder(_netController);

        // Runs the input throught the network and receives output.
        var neuralNetworkOutput = _netController.UpdateNeuralNet(neuralNetInput);

        // Print to console.
        Utilities.instance.PrettyPrint (neuralNetworkOutput);
	}
}
