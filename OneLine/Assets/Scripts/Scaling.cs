using System.Collections;
using System.Collections.Generic;
using Unity;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Scaling
{
    // Resolución de referencia = 720x1280
    int[] refResolution = new int[] { 720, 1280 };
    int[] currResolution;

    // Valor para cambiar entre unidades de Unity y Pixeles
    int unityUds;

    public Scaling(int[] res, int camSize)
    {
        currResolution = res;

        unityUds = res[1] / (2 * camSize);
    }

    /// <summary>
    /// Función que hace el escalado de un rectangulo para que entre en la pantalla
    /// </summary>
    public Vector3 ScaleToScreen(Vector3 sizeInUnits, Vector3 scale)
    {
        Vector3 temp = sizeInUnits;

        temp.x *= unityUds;
        temp.y *= unityUds;

        // Anchura 
        if (temp.x > currResolution[0])
        {
            temp.x = currResolution[0];

            temp.y = (temp.x * sizeInUnits.y) / sizeInUnits.x;
        }

        // Altura
        if (temp.y > currResolution[1])
        {
            temp.y = currResolution[1];

            temp.x = (temp.y * sizeInUnits.x) / sizeInUnits.y;
        }

        // Traducción a unidades de Unity
        temp.x = temp.x / unityUds;
        temp.y = temp.y / unityUds;

        Vector3 nScale;

        nScale = resizeObjectScale(sizeInUnits, temp, scale);

        return nScale; 
    }

    /// <summary>
    /// Función para escalar un rectángulo en base a uno de referencia. 
    /// </summary>
    /// <param name="srcDims">El rectángulo que queremos escalar</param>
    /// <param name="refDims">El rectángulo que vamos a tener como referencia</param>
    /// <returns>Las dimensiones escaladas del rectángulo</returns>
    public int[] ScaleWithReference(int[] srcDims, int[] refDims)
    {
        int[] temp = null;

        return temp;
    }

    /// <summary>
    /// Convierte las dimensiones de píxeles a unidades de Unity. TODO: Hacer reversible
    /// </summary>
    /// <param name="pixels"></param>
    /// <returns></returns>
    public int[] ConvertToUnity(int[] pixels)
    {
        int[] temp = null;

        return temp;
    }

    Vector3 resizeObjectScale(Vector3 origUnits, Vector3 currUnits, Vector3 scale)
    {
        Vector3 scalated = new Vector3();

        scalated.x = (currUnits.x * scale.x) / origUnits.x;
        scalated.y = (currUnits.y * scale.y) / origUnits.y;

        return scalated;
    }

    public int UnityUds()
    {
        return unityUds;
    }

    float ResizeX(float x)
    {
        return (x * currResolution[0]) / refResolution[0];
    }

    float ResizeY(float y)
    {
        return (y * currResolution[1]) / refResolution[1];
    }
}
