using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalTask.Wrappers
{
    public class LevelWrapper
    {
        public LevelWrapper(Level level)
        {
            Level = level;
            Name = level.Name;
            Id = level.Id;
        }

        public bool IsSelected { get; set; }
        public Level Level { get; }
        public string Name { get; }
        public ElementId Id { get; }
        public int Num { get; set; }
    }
}
