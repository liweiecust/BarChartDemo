using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp4
{
    interface ISort<T> where T : DataPoint
    {
        void QuickSort(T[] array, int left, int right); //Quick sort through big pile heap

        void Swap(T A, T B); // Swap elements


    }
}
