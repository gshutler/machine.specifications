using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Machine.Specifications.Model;
using Machine.Specifications.Utility;

namespace Machine.Specifications.Factories
{
  public class BehaviorFactory
  {
    readonly SpecificationFactory _specificationFactory;

    public BehaviorFactory()
    {
      _specificationFactory = new SpecificationFactory();
    }

    public Behavior CreateBehaviorFrom(FieldInfo behaviorField, Context context)
    {
      Type behaviorType = behaviorField.FieldType.GetGenericArguments().First();

      if(!behaviorType.HasAttribute<BehaviorsAttribute>())
      {
        throw new SpecificationUsageException("Behaviors require the BehaviorsAttribute on the type containing the Specifications. Attribute is missing from " + behaviorType.FullName);
      }

      object behaviorInstance = Activator.CreateInstance(behaviorType);

      if (behaviorType.GetPrivateFieldsOfType<Given>().Any())
      {
        throw new SpecificationUsageException("You cannot have Establishs on Behaviors. Given found in " + behaviorType.FullName);
      }
      
      if (behaviorType.GetPrivateFieldsOfType<When>().Any())
      {
        throw new SpecificationUsageException("You cannot have Becauses on Behaviors. When found in " + behaviorType.FullName);
      }

      if (behaviorType.GetPrivateFieldsWith(typeof(Behaves_like<>)).Any())
      {
        throw new SpecificationUsageException("You cannot nest Behaviors. Nested Behaviors found in " + behaviorType.FullName);
      }

      var isIgnored = behaviorField.HasAttribute<IgnoreAttribute>() ||
                      behaviorInstance.GetType().HasAttribute<IgnoreAttribute>();
      var behavior = new Behavior(behaviorInstance, context, isIgnored);

      IEnumerable<FieldInfo> itFieldInfos = behaviorType.GetPrivateFieldsOfType<Then>();
      CreateBehaviorSpecifications(itFieldInfos, behavior);

      return behavior;
    }

    void CreateBehaviorSpecifications(IEnumerable<FieldInfo> itFieldInfos,
                                      Behavior behavior)
    {
      foreach (var itFieldInfo in itFieldInfos)
      {
        Specification specification = _specificationFactory.CreateSpecificationFromBehavior(behavior,
                                                                                            itFieldInfo);
        behavior.AddSpecification(specification);
      }
    }
  }
}