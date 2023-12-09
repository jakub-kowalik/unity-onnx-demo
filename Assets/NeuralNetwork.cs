using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.UI;

public class NeuralNetwork : MonoBehaviour
{
    IWorker worker;
    TMP_Text myText;
    float period = 0f;

    [SerializeField]
    public NNModel modelFile;
    [SerializeField]
    public string OUTPUT_NAME;
    [SerializeField]
    public string INPUT_NAME;


    // Start is called before the first frame update
    void Start()
    {
        var model = ModelLoader.Load(modelFile);
        myText = this.GetComponent<TMP_Text>();
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
    }

    // Update is called once per frame
    void Update()
    {
        if (period > 1f) {
            period = 0f;
            var inputTensor = new Tensor(1, 1, 1, 10);

            for (int i = 0; i < 10; i++)
            {
                inputTensor[0, 0, 0, i] = Random.Range(0f, 1f);
            }

            var inputs = new Dictionary<string, Tensor> {
                { INPUT_NAME, inputTensor }
            };
            worker.Execute(inputs);
            Tensor outputTensor = worker.PeekOutput(OUTPUT_NAME);

            // display input tensor
            string inputString = "";
            for (int i = 0; i < 10; i++)
            {
                inputString += inputTensor[0, 0, 0, i] + " ";
            }
            myText.text = "input: " + inputString + "\noutput: " + outputTensor[0, 0, 0, 0].ToString();

            inputTensor.Dispose();
            outputTensor.Dispose();
            inputs.Clear();
        }
        period += UnityEngine.Time.deltaTime;

    }
}
