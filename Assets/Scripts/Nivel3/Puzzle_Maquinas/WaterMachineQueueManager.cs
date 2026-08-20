using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMachineQueueManager : MonoBehaviour
{
    public static WaterMachineQueueManager Instance;


    // ESTRUCTURA DE DATOS: COLA (Queue<WaterMachineStation>)
    private Queue<WaterMachineStation> queue =
        new Queue<WaterMachineStation>();

    private List<WaterMachineType> processedOrder =
        new List<WaterMachineType>();

    [Header("Todas las estaciones")]
    public List<WaterMachineStation> allStations =
        new List<WaterMachineStation>();


    //  Orden correcto del proceso real de potabilización
    [Header("Orden correcto")]
    public List<WaterMachineType> correctOrder =
        new List<WaterMachineType>()
    {
        WaterMachineType.Desarenadores,
        WaterMachineType.Floculadores,
        WaterMachineType.Decantadores,
        WaterMachineType.FiltrosArenaCarbon,
        WaterMachineType.BombasDosificadoras
    };

    private bool processing = false;

    void Awake()
    {
        Instance = this;
    }

    public void EnqueueStation(
        WaterMachineStation station
    )
    {
        queue.Enqueue(station);

        Debug.Log(
            "Encolado: " + station.GetMachineType()
        );

        //  Cuando se activan las 5
        if (queue.Count == 5 && !processing)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    IEnumerator ProcessQueue()
    {
        processing = true;

        processedOrder.Clear();

        while (queue.Count > 0)
        {
            WaterMachineStation station =
                queue.Dequeue();

            processedOrder.Add(
                station.GetMachineType()
            );

            Debug.Log(
                "Procesando: " +
                station.GetMachineType()
            );

            yield return new WaitForSeconds(0.4f);
        }

        ValidateOrder();

        processing = false;
    }

    void ValidateOrder()
    {
        bool correct = true;

        for (int i = 0; i < correctOrder.Count; i++)
        {
            if (
                processedOrder[i] !=
                correctOrder[i]
            )
            {
                correct = false;
                break;
            }
        }

        if (correct)
        {
            Debug.Log("PUZZLE COMPLETADO");

            PuzzleCompletionManager.Instance
                .MarkMachinePuzzleComplete();
        }
        else
        {
            Debug.Log(
                "ORDEN INCORRECTO - REINICIANDO"
            );

            ResetAllStations();
        }
    }

    void ResetAllStations()
    {
        foreach (
            WaterMachineStation station
            in allStations
        )
        {
            station.TurnOff();
        }

        processedOrder.Clear();

        queue.Clear();
    }
}