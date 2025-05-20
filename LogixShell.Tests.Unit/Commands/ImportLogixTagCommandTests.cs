using L5Sharp.Core;
using LogixShell;
using Shouldly;

namespace LogixCli.Tests.cmd;

[TestFixture]
public class ImportLogixTagCommandTests
{
    [Test]
    public void Invoke_WithInvalidPath_ShouldThrowException()
    {
        var command = new ImportLogixTagCommand { FilePath = [Known.FakeFile] };

        Should.Throw<FileNotFoundException>(() => command.Invoke().Cast<Exception>().ToList());
    }

    [Test]
    public void Invoke_WithValidAndInvalidPath_ShouldThrowException()
    {
        var command = new ImportLogixTagCommand { FilePath = [Known.TestFile, Known.FakeFile] };

        Should.Throw<FileNotFoundException>(() => command.Invoke().Cast<Exception>().ToList());
    }

    [Test]
    public void Invoke_WithValidPath_ShouldProcessTags()
    {
        var command = new ImportLogixTagCommand { FilePath = [Known.TestFile] };

        var output = command.Invoke<Tag>().ToList();

        output.ShouldNotBeEmpty();
    }

    [Test]
    public void Invoke_WithNameFilter_ShouldFilterByName()
    {
        var command = new ImportLogixTagCommand
        {
            FilePath = [Known.TestFile],
            Name = "Test*"
        };

        var output = command.Invoke<Tag>().ToList();

        output.ShouldNotBeEmpty();
        output.ForEach(x => x.Name.ShouldContain("Test"));
    }

    [Test]
    public void Invoke_WithScopeFilter_ShouldFilterByScope()
    {
        var command = new ImportLogixTagCommand
        {
            FilePath = [Known.TestFile],
            Scope = "MainProgram"
        };

        var output = command.Invoke<Tag>().ToList();

        output.ShouldNotBeEmpty();
    }

    [Test]
    public void Invoke_WithDataTypeFilter_ShouldFilterByDataType()
    {
        var command = new ImportLogixTagCommand
        {
            FilePath = [Known.TestFile],
            DataType = "DINT"
        };

        var output = command.Invoke<Tag>().ToList();

        output.ShouldNotBeEmpty();
    }
}