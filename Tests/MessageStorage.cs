using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    public class MessageStorage
    {
        private readonly String[] _messages;

        public MessageStorage(string[] messages)
        {
            _messages = messages;
        }

        public string[] Messages
        {
            get { return _messages; }
        }
    }
}
