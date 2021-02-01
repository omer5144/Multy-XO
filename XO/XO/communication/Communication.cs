using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XO.communication
{
    public interface Communication
    {
        void Open();

        string Send(string msg);

        string Recv();

        void Close();

    }
}
