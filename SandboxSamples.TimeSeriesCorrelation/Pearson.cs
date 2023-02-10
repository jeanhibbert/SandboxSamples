namespace SandboxSamples.TimeSeriesCorrelation;
/*This application defines two time series as arrays of double values, 
 * then calculates Pearson's correlation coefficient between the two series using the PearsonCorrelation method. 
 * The result is displayed in the console.

Note that this is just one example of how Pearson's correlation coefficient can be calculated, 
and the specific approach used may vary depending on the requirements of the analysis.*/

internal class Pearson
{

    public static void Start()
    {
        // Define two time series
        double[] series1 = new double[] { 1, 2, 3, 4, 5 };
        double[] series2 = new double[] { 5, 4, 3, 2, 1 };

        // Calculate Pearson's correlation coefficient between the two series
        double correlation = PearsonCorrelation(series1, series2);

        // Print result
        Console.WriteLine("Pearson's Correlation Coefficient: " + correlation);
    }

    // Pearson's correlation coefficient calculation
    static double PearsonCorrelation(double[] series1, double[] series2)
    {
        int n = series1.Length;
        double sum1 = 0;
        double sum2 = 0;
        double sum1Squared = 0;
        double sum2Squared = 0;
        double sumProducts = 0;

        for (int i = 0; i < n; i++)
        {
            sum1 += series1[i];
            sum2 += series2[i];
            sum1Squared += series1[i] * series1[i];
            sum2Squared += series2[i] * series2[i];
            sumProducts += series1[i] * series2[i];
        }

        double numerator = sumProducts - (sum1 * sum2 / n);
        double denominator = Math.Sqrt((sum1Squared - sum1 * sum1 / n) * (sum2Squared - sum2 * sum2 / n));

        return numerator / denominator;
    }
}

