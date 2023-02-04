using System.Collections;
using System.Collections.Generic;
using Gameplay.Commands.GameLogic;
using Gameplay.Commands.HoverPreviewCommands;
using Gameplay.Commands.TileBehaviourCommands;
using UnityEngine;

namespace Gameplay.Commands
{
    public class CommandBuffer: MonoBehaviour
    {
        private Queue<IGameCommand> _toExecute;

        private List<IActionPreviewCommand> _lastHoverCommands;

        private void Awake()
        {
            _toExecute = new Queue<IGameCommand>();
            _lastHoverCommands = new List<IActionPreviewCommand>();
        }

        /// <summary>
        /// This class is endlessly running on the game scene. Waits for GameCommands to be enqueued and
        /// then executes them until the queue is empty.
        /// </summary>
        private void Start()
        {
            StartCoroutine(CheckForCommandsToExecute());
        }

        public void EnqueueCommand(IGameCommand command)
        {
            // Debug.Log("Command queued       IN: "+ command.GetType().Name);
            _toExecute.Enqueue(command);
        }

        public void ClearQueue()
        {
            _toExecute.Clear();
        }

        public void EnqueueBehaviourCommands(List<ITileActionCommand> commands)
        {
            for (int i = 0; i < commands.Count; i++)
                EnqueueCommand(commands[i]);
        }
        
        public void EnqueueVisualEffectCommands(List<IActionPreviewCommand> commands)
        {
            _lastHoverCommands = commands;
            for (int i = 0; i < commands.Count; i++)
                EnqueueCommand(commands[i]);
        }

        private IEnumerator CheckForCommandsToExecute()
        {
            while (this != null)
            {
                if (_toExecute.Count > 0)
                {
                    IGameCommand command = _toExecute.Dequeue();
                    // yield return new WaitForSeconds(command.GetDelay());
                    // Debug.Log("Command executed     OUT: "+ command.GetType().Name);
                    command.Execute();
                    yield return new WaitForSeconds(command.GetDelay());
                }
                yield return null;
            }
        }

        public bool IsQueueEmpty()
        {
            return _toExecute.Count < 1;
        }
        
        
    }
}