namespace SandboxSamples.TimeSeriesCorrelation;

/*This application defines two time series as arrays of double values, 
 * then calculates the cross-correlation between the two series using the CrossCorrelate method. 
 * The MaxIndex method is used to find the index of the maximum cross-correlation value, 
 * which is then displayed along with the cross-correlation values.
Note that this is just one example of how cross-correlation can be calculated, 
and the specific approach used may vary depending on the requirements of the analysis.*/

public class CrossCorrelation
{
    public static void Start()
    {
        double[] series1 = new double[] { 1, 2, 3, 4, 5 };
        double[] series2 = new double[] { 5, 4, 3, 2, 1 };

        // Calculate cross-correlation between the two series
        double[] crossCorrelation = CrossCorrelate(series1, series2);

        // Find the maximum cross-correlation value and its index
        int maxIndex = MaxIndex(crossCorrelation);
        double maxValue = crossCorrelation[maxIndex];

        // Print results
        Console.WriteLine("Cross-Correlation:");
        for (int i = 0; i < crossCorrelation.Length; i++)
        {
            Console.WriteLine("Index " + i + ": " + crossCorrelation[i]);
        }
        Console.WriteLine("\nMaximum Cross-Correlation Value: " + maxValue);
        Console.WriteLine("Maximum Cross-Correlation Index: " + maxIndex);
    }

    static double[] CrossCorrelate(double[] series1, double[] series2)
    {
        int n = series1.Length;
        int m = series2.Length;
        int maxLag = n - 1;
        double[] crossCorrelation = new double[2 * maxLag + 1];

        for (int lag = -maxLag; lag <= maxLag; lag++)
        {
            double sum = 0;
            for (int i = 0; i < n; i++)
            {
                int j = i + lag;
                if (j >= 0 && j < m)
                {
                    sum += series1[i] * series2[j];
                }
            }
            crossCorrelation[lag + maxLag] = sum;
        }
        return crossCorrelation;
    }

    // Find index of maximum value
    static int MaxIndex(double[] array)
    {
        int maxIndex = 0;
        double maxValue = array[0];
        for (int i = 1; i < array.Length; i++)
        {
            if (array[i] > maxValue)
            {
                maxIndex = i;
                maxValue = array[i];
            }
        }
        return maxIndex;
    }
}

