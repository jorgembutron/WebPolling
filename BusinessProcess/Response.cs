using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessProcess
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public string Key { get; set; }

        public T Item { get; set; }
    }
}
