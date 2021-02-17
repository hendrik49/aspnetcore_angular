using System.Collections.Generic;
using System;

namespace angular_netcore.Models
{
  public class Tasks
  {
    public int id { get; set; }
    public string name { get; set; }
    public DateTime when { get; set; }
    public bool status { get; set; }
  }

}