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
    public Vector2 ScaleToFitKeepingAspectRatio(Vector2 srcDims, Vector2 refDims)
    {
        Vector2 temp = srcDims;

        // Comprobamos el ancho
        if(temp.x > refDims.x || temp.x < refDims.x)
        {
            temp.x = refDims.x;

            temp.y = (temp.x * srcDims.y) / srcDims.x;
        }

        // Comprobamos el alto
        if(temp.y > refDims.y)
        {
            if(temp != srcDims)
            {
                temp = srcDims;
            }

            temp.y = refDims.y;

            temp.x = (temp.y * srcDims.x) / srcDims.y;
        }

        return temp;
    }

    // Dos rectángulos -> Como es siempre, las dimensiones nuevas

    public Vector3 resizeObjectScale(Vector3 origUnits, Vector3 currUnits, Vector3 scale)
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

    public float ResizeX(float x)
    {
        float temp = (x * currResolution.x) / refResolution.x;

        return temp;
    }

    public float ResizeY(float y)
    {
        float temp = (y * currResolution.y) / refResolution.y;

        return temp;
    }
}
