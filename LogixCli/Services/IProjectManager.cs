using L5Sharp.Core;

namespace LogixCli.Services;

public interface IProjectManager
{
    /// <summary>
    /// Loads the specified project into the known staging directory to be accessed by further commands.
    /// </summary>
    /// <param name="file">The full path to the L5X file to load.</param>
    void LoadProject(string file);

    /// <summary>
    /// Saves the staged L5X to the specified output file.
    /// </summary>
    /// <param name="file">The path to save the staged L5X file to.</param>
    void SaveProject(string file);

    /// <summary>
    /// Saves the changes made to the staged L5X project.
    /// </summary>
    /// <param name="project">The staged L5X project to save.</param>
    void SaveChanges(L5X project);

    /// <summary>
    /// Attempts to retrieve the staged L5X project, if it exists.
    /// </summary>
    /// <param name="project">The retrieved L5X project.</param>
    /// <returns><c>true</c> if the project was successfully retrieved; otherwise, <c>false</c>.</returns>
    bool TryGetProject(out L5X project);
}