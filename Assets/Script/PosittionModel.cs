using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionModel
{
    public PositionModel(string username, string posittionX, string posittionY, string posittionZ)
    {
        this.username = username;
        this.positionX = posittionX;
        this.positionY = posittionY;
        this.positionZ = posittionZ;
    }

    public string username { get; set; }
    public string positionX { get; set; }
    public string positionY { get; set; }
    public string positionZ { get; set; }
} 
