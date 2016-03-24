using System;
using System.Threading.Tasks;

namespace WinService
{
    internal interface IProcessor
    {
        Guid Id { get; }

        Task Process();
    }
}