using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp4.ViewModel
{
    public class DataViewModel : DependencyObject, ISort<DataPoint>
    {
        public DataViewModel()
        {
            collection = new List<DataPoint>
                {
                     new DataPoint("A",5), new DataPoint("A", 4),new DataPoint("A", 3),new DataPoint("A", 19),
                     new DataPoint("A", 1),new DataPoint("A",11),new DataPoint("A",15),new DataPoint("A",6)

                };

            DisorderCommand = new CommandBase();
            DisorderCommand.ExecuteCommand += new Action<object>(ReOrder);
        }

        public List<DataPoint> collection { get; set; }

       

        public CommandBase DisorderCommand
        {
            get; set;
        }
        public void QuickSort(DataPoint[] array, int left, int right)
        {
            int i, j, Key;

            Key = array[left].Value;
            i = left;                              //3, 52, 5, 2, 1, 6, 3, 7, 3, 78, 32, 12, 54, 25, 11, 23
            j = right;                             //1, 52, 5, 2, 3, 6, 3, 7, 3, 78, 32, 12, 54, 25, 11, 23
            while (i != j)                           //1, 3, 5, 2, 52, 6, 3, 7, 3, 78, 32, 12, 54, 25, 11, 23
            {                                      //1, 2, 5, 3, 52, 6, 3, 7, 3, 78, 32, 12, 54, 25, 11, 23
                while (array[j].Value >= Key && i < j)      //1, 2, 3, 5, 52, 6, 3, 7, 3, 78, 32, 12, 54, 25, 11, 23
                {
                    j--;

                }
                if (i < j)
                {
                    Swap(array[i], array[j]);
                }
                else
                    break;
                while (array[i].Value < Key && i < j)
                {
                    i++;

                }
                if (i < j)
                {
                    Swap(array[i], array[j]);
                }
                else
                    break;
            }
            if (j >= 1 && j <= right - 2)
            {
                QuickSort(array, left, i - 1);
                QuickSort(array, i + 1, right);
            }
            else if (j == 0)
            {
                QuickSort(array, i + 1, right);
            }
            else if (j == right - 1)
            {
                QuickSort(array, left, i - 1);
            }
        }

        public void Swap(DataPoint A, DataPoint B)
        {
            var temp = A;
            A = B;
            B = temp;
            Thread.Sleep(300);
        }

        public void ReOrder(object obj)
        {
            collection = ReOrderList(collection);

        }
        public List<DataPoint> ReOrderList(List<DataPoint> list)
        {
            List<DataPoint> newList = new List<DataPoint>();

            Random RAN = new Random();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                int index = RAN.Next(0, list.Count);
                newList.Add(list[index]);
                list.RemoveAt(index);
            }
            return newList;

        }



    }
}
