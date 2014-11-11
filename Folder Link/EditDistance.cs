using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Folder_Link
{
    class EditDistance
    {
        //Wagner-Fisher algorithm: http://en.wikipedia.org/wiki/Wagner%E2%80%93Fischer_algorithm
        static public int Compare(string s1, string s2)
        {
            int[,] matrix = new int[s1.Length + 1, s2.Length + 1];

            for (int i = 0; i < s1.Length + 1; i++)
                matrix[i, 0] = i;
            for (int j = 0; j < s2.Length + 1; j++)
                matrix[0, j] = j;

            for (int j = 0; j < s2.Length; j++)
            {
                for (int i = 0; i < s1.Length; i++)
                {
                    if (s1[i] == s2[j])
                        matrix[i+1 , j+1] = matrix[i , j];
                    else
                    {
                        matrix[i + 1, j + 1] = new List<int>() { 
                            matrix[i , j+1 ]+1,
                            matrix[i+1 , j] +1,
                            matrix[i , j] +1
                        }.Min();
                    }
                }
            }

            return matrix[s1.Length, s2.Length];
        }

    }
}
