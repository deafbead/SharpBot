using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Util.Collections
{
    class FixedSizeQueue<T> : Queue<T>
    {
        private readonly int _capacity;

        public FixedSizeQueue(int capacity) : base(capacity)
        {
            _capacity = capacity;
        }

        public new void Enqueue(T item)
        {
            if (this.Count > 0 && this.Count + 1 > _capacity)
            {
                Dequeue();
            }
            base.Enqueue(item);
        }
    }
}
