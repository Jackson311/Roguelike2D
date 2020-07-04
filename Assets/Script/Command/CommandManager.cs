

using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Script.Command
{
    
    
    public class CommandManager
    {
        private static CommandManager _Instance;
        private List<ICommand> _commands = new List<ICommand>();
        private int _currentCommandIndex = -1;
        
        private CommandManager()
        {
            _Instance = this;
        }

        public static CommandManager Instance()
        {
            if (_Instance == null)
                _Instance = new CommandManager();
            return _Instance;
        }

        public void ExecuteCommand(ICommand command)
        {
            ++_currentCommandIndex;
            _commands.Add(command);
            command.DoCommand();
        }

        public void DoCommand()
        {
            if(_currentCommandIndex >= _commands.Count-1)
                return;

            ++_currentCommandIndex;
            _commands[_currentCommandIndex].DoCommand();
        }

        public void UnDoCommand()
        {
            if(_currentCommandIndex < 0)
                return;
            
            _commands[_currentCommandIndex].UnDoCommand();
            --_currentCommandIndex;
        }
    }
}