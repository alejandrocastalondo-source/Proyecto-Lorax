// Cada instancia es un NODO de la lista enlazada
using UnityEngine;

public class PatrolNode : MonoBehaviour
{
    [Header("Siguiente nodo")]
    public PatrolNode nextNode;
    // nextNode es la referencia al siguiente elemento de la cadena.
    // Si nextNode == null, el recorrido termina (lista lineal).
    // Si nextNode apunta al primer nodo, forma un ciclo (lista circular).

}