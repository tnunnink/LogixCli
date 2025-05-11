using LogixCli.Services;
using NUnit.Framework.Legacy;
using Spectre.Console.Testing;

namespace LogixCli.Tests;

[TestFixture]
public class ProjectManagerTests
{
    private const string TestFile = @"C:\Users\tnunn\Documents\L5X\Test.L5X";
    private const string TestStageFile = @"C:\Users\tnunn\AppData\Roaming\L5Shell\Test.L5X";
    private const string FakeFile = @"C:\Users\tnunn\Documents\Test.L5X";

    [TearDown]
    public void Teardown()
    {
        if (File.Exists(TestStageFile)) return;
        File.Delete(TestStageFile);
    }

    [Test]
    public void LoadProject_ValidFile_ShouldHaveFileExistingInStagingFolder()
    {
        var console = new TestConsole();
        var manager = new ProjectManager(console);

        manager.LoadProject(TestFile);

        FileAssert.Exists(TestStageFile);
    }

    [Test]
    public Task LoadProject_ValidFile_ShouldHaveVerifiedOutput()
    {
        var console = new TestConsole();
        var manager = new ProjectManager(console);

        manager.LoadProject(TestFile);

        return Verify(console.Output);
    }

    [Test]
    public Task LoadProject_InvalidFile_ShouldReportFileDoesNotExist()
    {
        var console = new TestConsole();
        var manager = new ProjectManager(console);

        manager.LoadProject(FakeFile);

        return Verify(console.Output);
    }
}