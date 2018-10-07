using System;

namespace Chat.Client.Contracts
{
    public interface IDispatcher
    {
        void Invoke(Action action);
    }
}
