﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;

// This class demonstrates the creation of "empty"
// specs.. technically speaking, this is a normal
// context class in MSpec, with the exception that
// none of the Then delegate members are assigned
// an anon method, so they're empty. That being said
// they will be parsed by the runner and still add
// to the test count, but show up as "unimplemented"
// in any reports.
//
// This functionality is entirely optional, as far as
// patterns go, but this is useful for documentating specs
// of some component of the software prior to it's creation,
// ie the UI prior to being designed. This allows
// the implementation team to put expectations down in
// code and have a place to come back to later, when
// the documented functionality is implemented (or
// intra-implementation, even). 

namespace Machine.Specifications.Example
{
  [Subject("Recent Account Activity Summary page")]
  public class when_a_customer_first_views_the_account_summary_page
  {
    Then should_display_all_account_transactions_for_the_past_thirty_days;
    Then should_display_debit_amounts_in_red_text;
    Then should_display_deposit_amounts_in_black_text;
  }
}