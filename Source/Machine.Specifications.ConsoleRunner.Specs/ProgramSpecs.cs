using System.IO;
using System.Reflection;
using Machine.Specifications.ConsoleRunner.Properties;

namespace Machine.Specifications.ConsoleRunner.Specs
{
  // TODO: Add Subject count
  // TODO: Add Tag and filter by tag
  // TODO: Add awesome client side reporting stuff

  [Subject("Console runner")]
  public class when_arguments_are_not_provided
    : ConsoleRunnerSpecs 
  {
    When of = ()=>
      program.Run(new string[] {});

    Then should_print_usage_statement = ()=>
      console.Lines.ShouldContain(Resources.UsageStatement);
  }

  [Subject("Console runner")]
  public class when_running_a_specification_assembly
    : ConsoleRunnerSpecs 
  {
    When of = ()=>
      program.Run(new [] {GetPath("Machine.Specifications.Example.dll")});

    Then should_write_the_assembly_name = ()=>
      console.ShouldContainLineWith("Machine.Specifications.Example");

    Then should_write_the_specifications = ()=>
      console.Lines.ShouldContain(
        "» should debit the from account by the amount transferred", 
        "» should credit the to account by the amount transferred", 
        "» should not allow the transfer");

    Then should_write_the_contexts = ()=>
      console.Lines.ShouldContain(
        "Account Funds transfer, when transferring between two accounts",
        "Account Funds transfer, when transferring an amount larger than the balance of the from account"
        );

    Then should_write_the_count_of_contexts = ()=>
      console.ShouldContainLineWith("Contexts: 3");

    Then should_write_the_count_of_specifications = ()=>
      console.ShouldContainLineWith("Specifications: 6");
  }

  [Subject("Console runner")]
  public class when_specifying_a_missing_assembly_on_the_command_line
    : ConsoleRunnerSpecs 
  {
    const string missingAssemblyName = "Some.Missing.Assembly.dll";
    public static ExitCode exitCode;

    When of = ()=>
      exitCode = program.Run(new string[] {missingAssemblyName});

    Then should_output_an_error_message_with_the_name_of_the_missing_assembly = ()=>
      console.Lines.ShouldContain(string.Format(Resources.MissingAssemblyError, missingAssemblyName));

    Then should_return_the_Error_exit_code = ()=>
      exitCode.ShouldEqual(ExitCode.Error);
  }

  [Subject("Console runner")]
  public class when_a_specification_fails : ConsoleRunnerSpecs
  {
    public static ExitCode exitCode;
    const string assemblyWithFailingSpecification = "Machine.Specifications.FailingExample";
    const string failingSpecificationName = "should fail";

    When of = ()=>
      exitCode = program.Run(new string[] {GetPath(assemblyWithFailingSpecification + ".dll")});

    Then should_write_the_failure = ()=>
      console.ShouldContainLineWith("Exception");

    Then should_write_the_count_of_failed_specifications = ()=>
      console.ShouldContainLineWith("1 failed");

    Then should_return_the_Failure_exit_code = ()=>
      exitCode.ShouldEqual(ExitCode.Failure);
  }

  [Subject("Console runner")]
  public class when_a_specification_fails_and_silent_is_set : ConsoleRunnerSpecs
  {
    public static ExitCode exitCode;
    const string assemblyWithFailingSpecification = "Machine.Specifications.FailingExample";
    const string failingSpecificationName = "should fail";

    When of = ()=>
      exitCode = program.Run(new string[] { GetPath(assemblyWithFailingSpecification + ".dll"), "-s"});

    Then should_write_the_count_of_failed_specifications = ()=>
      console.ShouldContainLineWith("1 failed");

    Then should_return_the_Failure_exit_code = ()=>
      exitCode.ShouldEqual(ExitCode.Failure);
  }

  [Subject("Console runner")]
  public class when_specifying_an_include_filter : ConsoleRunnerSpecs
  {
    When of = ()=>
      program.Run(new [] { GetPath("Machine.Specifications.Example.dll"), "--include", "failure"});

    Then should_execute_specs_with_the_included_tag = () =>
      console.ShouldContainLineWith(
        "Account Funds transfer, when transferring an amount larger than the balance of the from account");

    Then should_not_execute_specs_that_are_not_included = () =>
      console.ShouldNotContainLineWith("Account Funds transfer, when transferring between two accounts");
  }

  public class ConsoleRunnerSpecs
  {
    public static Program program;
    public static FakeConsole console;

    Given context = ()=>
    {
      console = new FakeConsole();
      program = new Program(console);
    };

    protected static string GetPath(string path)
    {
      return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), path);
    }
  }
}