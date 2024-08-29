using L5Sharp.Core;

namespace L5Shell.Console.Services;

public interface IProjectManager
{
    void LoadProject(string file);
    void SaveProject(string file);
    void SaveChanges(L5X project);
    bool TryGetProject(out L5X project);
}