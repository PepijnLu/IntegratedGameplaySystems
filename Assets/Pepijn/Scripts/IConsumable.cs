using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConsumable
{
    void Consume(Vector3Int _cellPosition);
}
