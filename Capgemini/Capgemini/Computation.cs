using System;
using System.Collections.Generic;
using System.Text;

namespace Capgemini
{
    class Computation
    {
        public double[] cost;
        public double[] min;
        public double[] max;
        public double goal;
        public String[] name;
        public int length;

        public Computation(int length)
        {
            this.length = length;
            this.cost = new double[length];
            this.min = new double[length];
            this.max = new double[length];
            this.name = new String[length];
        }

        // we assume the arrays c.cost, c.min and c.max have the same length c.length
        // we assume c.min[i] <= c.max[i] for all i
        // this function returns an array l of length c.length
        // the output l it is such that :
        // l[i] = 0 or c.min[i] <= l[i] <= c.max[i] for all i
        // the sum of all l[i] over i is equal to c.goal
        // the sum c.cost[i]*l[i] over i is minimal
        public static double[] Compute(Computation c)
        {
            int[] perm = GiveReorderingList(c.cost);
            List<double[]> solution = new List<double[]>();
            for (int i = 0; i < Math.Pow(2, c.length); i++)
            {
                int[] arr = ConvertBinaryToArray(i, c.length);
                for (int j = 0; j < c.length; j++)
                {
                    double total = 0;
                    double[] l = new double[c.length];
                    for (int k = 0; k < j; k++)
                    {
                        l[perm[k]] = c.max[perm[k]] * arr[k];
                        total += l[perm[k]];
                    }
                    for (int k = j + 1; k < c.length; k++)
                    {
                        l[perm[k]] = c.min[perm[k]] * arr[k];
                        total += l[perm[k]];
                    }
                    l[perm[j]] = c.goal - total;
                    if (c.min[perm[j]] <= l[perm[j]] && l[perm[j]] <= c.max[perm[j]])
                    {
                        solution.Add(l);
                    }
                }
            }
            return FindBestSolution(solution, c.cost);
        }

        // find the array arr of double in the list solution such that :
        // the sum arr[i] * cost[i] over i is minimal
        public static double[] FindBestSolution(List<double[]> solution, double[] cost)
        {
            double minValue = Double.MaxValue;
            int index = -1;
            for (int i = 0; i < solution.Count; i++)
            {
                double sum = VectorialMultiplication(cost,solution[i]);
                if (sum < minValue)
                {
                    minValue = sum;
                    index = i;
                }
            }
            return solution[index];
        }

        // we assume arr1 and arr2 have the same length
        // compute the sum arr1[i]*arr2[i] over i
        public static double VectorialMultiplication(double[] arr1, double[] arr2)
        {
            double sum = 0;
            for (int i = 0; i < arr1.Length; i++) sum += arr1[i] * arr2[i];
            return sum;
        }

        // from an int n, construct an array of int with values 0 or 1 representing n in binary
        public static int[] ConvertBinaryToArray(int n, int size)
        {
            int[] arr = new int[size];
            for (int i = 0; i < size; i++)
            {
                arr[i] = n % 2;
                n /= 2;
            }
            return arr;
        }

        // given an array arr of double, we produce an array perm of int such that :
        // arr[perm[i]] <= arr[perm[j]] for all i <= j
        public static int[] GiveReorderingList(double[] arr)
        {
            double[] copy = Copy(arr);
            int[] perm = IdentityPermutation(arr.Length);
            int j = 1;
            while (j < copy.Length)
            {
                int i = j;
                while (i > 0 && copy[i - 1] > copy[i])
                {
                    SwitchElements<double>(copy, i - 1, i);
                    SwitchElements<int>(perm, i - 1, i);
                    i--;
                }
                j++;
            }
            return perm;
        }

        // copy an array
        public static double[] Copy(double[] arr)
        {
            double[] copy = new double[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                copy[i] = arr[i];
            }
            return copy;
        }

        // produce for an integer n the array {0,1,...,n-1}
        public static int[] IdentityPermutation(int n)
        {
            int[] identity = new int[n];
            for (int i = 0; i < n; i++)
            {
                identity[i] = i;
            }
            return identity;
        }

        // switch the elements e1 and e2 of an array arr
        public static void SwitchElements<T>(T[] arr, int e1, int e2)
        {
            T temp = arr[e1];
            arr[e1] = arr[e2];
            arr[e2] = temp;
        }
    }
}
