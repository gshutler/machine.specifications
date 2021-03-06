﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machine.Specifications.FailingExample
{
  [Subject("Scott Bellware")]
  public class at_any_given_moment
  {
    Then will_fail = ()=>
    {
      throw new Exception("hi scott, love you, miss you.");
    };
  }
  
  [Tags("example")]
  public class context_with_multiple_establish_clauses
  {
    Given foo = () => { };
    Given bar = () => { };

    Then should = () => { };
  }
  
}
