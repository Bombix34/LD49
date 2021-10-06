using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRenderingLayer : MonoBehaviour
{
    public List<ParticleSystemRenderer> particles;

    public void SetSortingOrder(int newSortingOrder)
    {
        foreach(var particle in particles)
        {
            particle.sortingOrder = newSortingOrder;
        }
    }
}
