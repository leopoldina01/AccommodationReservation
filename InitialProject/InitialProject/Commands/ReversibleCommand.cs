using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Commands
{
    public class ReversibleCommand : RelayCommand
    {
        private readonly Action<object?> _reverse;
        public ReversibleCommand(Action<object?> execute, Predicate<object?>? canExecute, Action<object?> reverse) : base(execute, canExecute)
        {
            _reverse = reverse;
        }

        public ReversibleCommand(Action<object?> execute, Action<object?> reverse) : base(execute)
        {
            _reverse = reverse;
        }

        public void Reverse(object? parameter)
        {
            _reverse.Invoke(parameter);
        }
    }
}
