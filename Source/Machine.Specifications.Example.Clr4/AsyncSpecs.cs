﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Machine.Specifications.Example.Clr4
{
  public class AsyncWorker
  {
    public Task<string> DoWorkAsync()
    {
      return Task.Factory.StartNew(() =>
      {
        Thread.Sleep(TimeSpan.FromMilliseconds(500));
        return "done";
      });
    }
  }

  class when_using_tasks_to_do_async_work
  {
    static AsyncWorker Worker;
    static string Result;

    Given context = () => { Worker = new AsyncWorker(); };

    When of = () => { Result = Worker.DoWorkAsync().Await(); };

    Then should_wait_for_the_async_work_to_complete =
      () => Result.ShouldEqual("done");
  }
}