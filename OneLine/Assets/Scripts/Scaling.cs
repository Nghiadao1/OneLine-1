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
    Vector2 refResolution;
    Vector2 currResolution;

    // Valor para cambiar entre unidades de Unity y Pixeles
    float unityUds;

    public Scaling(Vector2 res, Vector2 refRes, int camSize)
    {
        currResolution = res;
        refResolution = refRes;

        unityUds = res.y / (2 * camSize);
    }

    /// <summary>
    /// Función que hace el escalado de un rectangulo para que entre en la pantalla
    /// </summary>
    public Vector3 ScaleToFitScreen(Vector3 sizeInUnits, Vector3 scale)
    {
        Vector3 temp = sizeInUnits;

        temp.x *= unityUds;
        temp.y *= unityUds;

        temp.x = currResolution.x;

        temp.y = (temp.x * sizeInUnits.y) / sizeInUnits.x;

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
    public Vector2 ScaleKeepingAspectRatio(Vector2 srcDims)
    {
        Vector2 temp;

        temp.x = ResizeX(srcDims.x);
        temp.y = (temp.x * srcDims.y) / srcDims.x;


        return temp;
    }

    // Dos rectángulos -> Como es siempre, las dimensiones nuevas

    Vector3 resizeObjectScale(Vector3 origUnits, Vector3 currUnits, Vector3 scale)
    {
        Vector3 scalated = new Vector3();

        scalated.x = (currUnits.x * scale.x) / origUnits.x;
        scalated.y = (currUnits.y * scale.y) / origUnits.y;

        return scalated;
    }

    public float UnityUds()
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
