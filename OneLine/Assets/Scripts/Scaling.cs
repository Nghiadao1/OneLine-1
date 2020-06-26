using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public class Scaling
{
    // Resolución de referencia = 720x1280
    int[] refResolution;
    int[] currResolution;

    public Scaling(int[] res)
    {
        currResolution = res; 
    }

    /// <summary>
    /// Función que hace el escalado de un rectangulo para que entre en la pantalla
    /// </summary>
    public int[] ScaleToScreen(int[] dimensions)
    {
        int[] temp = dimensions;



        return temp; 
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
    /// Convierte las dimensiones de píxeles a unidades de Unity.
    /// </summary>
    /// <param name="pixels"></param>
    /// <returns></returns>
    public int[] ConvertToUnity(int[] pixels)
    {
        int[] temp = null;

        return temp;
    }

    int ResizeX(int x)
    {
        return (x * currResolution[0]) / refResolution[0];
    }

    int ResizeY(int y)
    {
        return (y * currResolution[1]) / refResolution[1];
    }
}
