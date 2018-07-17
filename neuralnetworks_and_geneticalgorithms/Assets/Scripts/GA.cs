using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GA : MonoBehaviour
{
    public GameObject agentPrefab;
    public float mutationChance = 0.1f;
    public int maxPoolSize = 40;

    private float totalFitnessScore = 0;
    private List<GeneticAgent> agents = new List<GeneticAgent>();
    private List<GeneticAgent> pool = new List<GeneticAgent>();

    private void Start()
    {
        const int initialAgentCount = 200;

        for (int i = 0; i < initialAgentCount; i++)
        {
            var agent = AddAgentToSimulation();
            agents.Add(agent);
        }
    }


    private GeneticAgent AddAgentToSimulation()
    {
        /*
        const sf::Vector2u windowSize = window.GetSize();

        const int minX = 50;
        const int minY = 50;

        const int maxX = windowSize.x - minX;
        const int maxY = windowSize.y - minY;

        const std::string spritePreName = "ufo";
        const std::string spritePostName = ".png";


        std::shared_ptr<Object> ufo = std::make_shared<Object>();

        ufo->AddComponent<C_DamageOnWallHit>();

        auto sight = ufo->AddComponent<C_AgentSight>();
        sight->SetObjectCollection(&agents);

        ufo->AddComponent<C_AgentCollision>();

        ufo->AddComponent<C_Velocity>();
        auto agent = ufo->AddComponent<C_GeneticAgent>();
        agent->SetObjectCollection(&agents);
        agent->SetWindowSize(windowSize);

        auto sprite = ufo->AddComponent<C_Sprite>();
        sprite->SetTextureAllocator(&textureAllocator);
        const std::string ufoCount = std::to_string(1 + (std::rand() % (4 - 1 + 1))); // Selects a random ufo sprite.
        sprite->Load(workingDir.Get() + spritePreName + ufoCount + spritePostName);
        sf::FloatRect spriteRect = sprite->GetSpriteRect();
        sprite->SetCenter(spriteRect.width * 0.5f, spriteRect.height * 0.5f);

        const int randX = minX + (std::rand() % (maxX - minX + 1));
        const int randY = minY + (std::rand() % (maxY - minY + 1));
        ufo->transform->SetPosition(randX, randY);

        auto screenWrapAround = ufo->AddComponent<C_ScreenWrapAround>();
        screenWrapAround->SetScreenSize(windowSize);
        screenWrapAround->SetSpriteHalfSize({ spriteRect.width * 0.5f, spriteRect.height * 0.5f});

        agents.Add(ufo);

        return agent;
        */

        GameObject agent = Instantiate(agentPrefab);

        Vector2 pos  = Camera.main.ScreenToWorldPoint(
            new Vector2(Random.Range(0f, Screen.width), Random.Range(0f, Screen.height)));

        agent.transform.position = pos;
        agent.transform.SetParent(transform);
        return agent.GetComponent<GeneticAgent>();
    }

    /*
    bool :PoolSort(std::shared_ptr<C_GeneticAgent> a, std::shared_ptr<C_GeneticAgent> b)
    {
        return a->GetTimeAlive() > b->GetTimeAlive();
    }
    */


    void Update()
    {
        for (int i = agents.Count - 1; i >= 0; i--)
        {
            GeneticAgent o = agents[i];
            var geneticAgent = o.GetComponent<GeneticAgent>();

            if (geneticAgent.energy <= 0)
            {
                AddToPool(o);
              
                CalculateTotalFitness();

                if (pool.Count > 1)
                {
                    int parentOne = FitnessProportionateSelection();
                    int parentTwo = FitnessProportionateSelection();


                    GeneticAgent agent = CreateAgentFromCrossOver(pool[parentOne], pool[parentTwo]);


                    float mutation = Random.value;
                    if (mutation < mutationChance)
                    {
                        agent.neuralNetwork.Mutate();
                    }


                    agents.Add(agent);
                }

                agents.RemoveAt(i);
                o.gameObject.SetActive(false);
            }
        }
    }

    void AddToPool(GeneticAgent agent)
    {
        pool.Add(agent);
        pool.Sort();

        pool.Sort();

        if (pool.Count > maxPoolSize)
        {
            GeneticAgent a = pool[pool.Count - 1];
            Destroy(a.gameObject);
            pool.RemoveAt(pool.Count - 1);
        }
    }

    void CalculateTotalFitness()
    {
        totalFitnessScore = 0;

        for (int i = 0; i < pool.Count; i++)
        {
            totalFitnessScore += pool[i].timeAlive;
        }
    }

    int FitnessProportionateSelection()
    {
        float randomSlice = Random.Range(0f, totalFitnessScore);

        float fitnessTotal = 0;

        for (int i = 0; i < pool.Count; i++)
        {

            fitnessTotal += pool[i].timeAlive;

            if (fitnessTotal >= randomSlice)
            {
                return i;
            }
        }

        return -1;
    }

    GeneticAgent CreateAgentFromCrossOver(GeneticAgent parentOne, GeneticAgent parentTwo)
    {
        NeuralNet neuralNetwork = parentOne.neuralNetwork;

        if (parentOne != parentTwo)
        {
            neuralNetwork = CreateNeuralNetworkFromCrossOver(parentOne.neuralNetwork, parentTwo.neuralNetwork);
        }

        var agent = AddAgentToSimulation();
        agent.neuralNetwork = neuralNetwork;

        return agent;
    }

    NeuralNet CreateNeuralNetworkFromCrossOver(NeuralNet networkOne, NeuralNet networkTwo)
    {
        List<float> parentOneWeights = networkOne.GetWeights();
        List<float> parentTwoWeights = networkTwo.GetWeights();

        List<int> crossoverPoints = networkOne.splitPoints;

        int crossOverIndex = Random.Range(0, crossoverPoints.Count - 1);
        int crossOverPoint = crossoverPoints[crossOverIndex];

        List<float> newWeights = new List<float>();

        for (int i = 0; i < crossOverPoint; i++)
        {
            newWeights.Add(parentOneWeights[i]);
        }

        for (int i = crossOverPoint; i < parentOneWeights.Count; i++)
        {
            newWeights.Add(parentTwoWeights[i]);
        }

        int numOfInput = networkOne.numOfInput;
        int numOfHiddenLayers = networkOne.numOfHiddenLayers;
        int numOfNeuronsInHiddenLayer = networkOne.numOfNeuronsPerHiddenLayer;
        int numOfOutput = networkOne.numOfOutput;

        NeuralNet neuralNet = 
            new NeuralNet(numOfInput, numOfOutput, numOfHiddenLayers, numOfNeuronsInHiddenLayer);
        neuralNet.SetWeights(newWeights);

        return neuralNet;
    }





}