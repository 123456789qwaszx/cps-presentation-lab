using System.Collections;
using System.Collections.Generic;

public sealed class CompositeCommand : ISequenceCommand
{
    private readonly List<ISequenceCommand> _commands;
    private readonly bool _wait;

    public CompositeCommand(IEnumerable<ISequenceCommand> commands, bool waitForCompletion = true)
    {
        _commands = new List<ISequenceCommand>(commands ?? new ISequenceCommand[0]);
        _wait = waitForCompletion;
    }

    public bool WaitForCompletion => _wait;

    public IEnumerator Execute(NodePlayScope api)
    {
        if (_commands.Count == 0) yield break;

        for (int i = 0; i < _commands.Count; i++)
        {
            var cmd = _commands[i];
            if (cmd == null) continue;

            // SequencePlayer가 WaitForCompletion을 보고 분기하긴 하지만,
            // Composite는 내부적으로 순서를 보장해야 하므로 여기서 직접 실행한다.
            var e = cmd.Execute(api);
            if (e == null) continue;

            while (true)
            {
                bool moved = e.MoveNext();
                if (!moved) break;
                yield return e.Current;
            }
        }
    }
}