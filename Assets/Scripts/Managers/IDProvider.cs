using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IDProvider
{
    private static int IDCounter = 0;

    public static int GetID()
    {
        IDCounter++;
        return IDCounter;
    }
}