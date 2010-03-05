using System.Linq;
using Machine.Specifications.Factories;
using Machine.Specifications.Model;

namespace Machine.Specifications.Specs.Factories
{
  [Subject(typeof(ContextFactory))]
  public class when_creating_a_context_with_a_concern
  {
    static Context newContext;

    Given context = ()=>
    {
      var factory = new ContextFactory();
      newContext = factory.CreateContextFrom(new context_with_subject());
    };

    Then should_capture_the_concerns_type = ()=>
      newContext.Subject.Type.ShouldEqual(typeof(int));

    Then should_capture_the_concerns_description = ()=>
      newContext.Subject.Description.ShouldEqual("Some description");
  }

  [Subject(typeof(ContextFactory))]
  public class when_creating_a_context_with_tags
  {
    static Context newContext;

    Given context = ()=>
    {
      var factory = new ContextFactory();
      newContext = factory.CreateContextFrom(new context_with_tags());
    };

    Then should_capture_the_tags = () =>
      newContext.Tags.ShouldContainOnly(new Tag(tag2.example), new Tag(tag2.some_other_tag), new Tag(tag2.one_more_tag));
  }

  [Subject(typeof(ContextFactory))]
  public class when_creating_a_context_with_duplicate_tags
  {
    static Context newContext;

    Given context = ()=>
    {
      var factory = new ContextFactory();
      newContext = factory.CreateContextFrom(new context_with_duplicate_tags());
    };

    Then should_capture_the_tags_once = ()=>
      newContext.Tags.Count().ShouldEqual(1);
  }
}
