using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using FinalTask.Wrappers;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalTask
{
    class MainViewViewModel
    {
        private ExternalCommandData _commandData;
        private Document doc;

        public List<LevelWrapper> Levels { get; set; }
        public bool ByLevels { get; set; }
        public DelegateCommand NumberingCommand { get; }
        public DelegateCommand SelectAllCommand { get; }
        public bool ThroughLevels { get; set; }
        public string Error { get; internal set; } = "";
        public int InitialNum { get; set; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            doc = commandData.Application.ActiveUIDocument.Document;
            Levels = SelectionUtils.SelectAllElements(commandData, typeof(Level))
                                    .OfType<Level>()
                                    .Where(x => x.GetDependentElements(new ElementCategoryFilter(BuiltInCategory.OST_Rooms)).Count != 0)
                                    .Select(x => new LevelWrapper(x))
                                    .OrderBy(x => x.Level.Elevation)
                                    .ToList();
            if (Levels.Count == 0) 
                Error = "Отсутствует хотя бы один уровень с помещениями";

            for (int i = 0; i < Levels.Count; i++)
            {
                Levels[i].Num = i + 1;
            }
            ByLevels = true;
            InitialNum = 1;
            NumberingCommand = new DelegateCommand(OnNumberingCommand);
            SelectAllCommand = new DelegateCommand(OnSelectAllCommand);
        }

        private void OnSelectAllCommand()
        {
            for (int i = 0; i < Levels.Count; i++)
            {
                Levels[i].IsSelected = true;
            }
        }

        private void OnNumberingCommand()
        {
            var selectedLevels = Levels.Where(x => x.IsSelected).ToList();
            if (selectedLevels.Count == 0) return;
            RaiseCloseRequest();
            InitialNum--;
            using (var ts = new Transaction(doc, "Room numbering"))
            {
                ts.Start();
                foreach (var level in selectedLevels)
                {

                    List<Room> rooms = new FilteredElementCollector(doc)
                                            .OfClass(typeof(SpatialElement))
                                            .OfType<Room>()
                                            .Where(x => x.LevelId == level.Id)
                                            .OrderByDescending(x => x.get_BoundingBox(null).Min.Y)
                                            .ThenBy(x => x.get_BoundingBox(null).Min.X)
                                            .ToList();

                    for (int i = 0; i < rooms.Count; i++)
                    {
                        if (ByLevels)
                        {
                            rooms[i].get_Parameter(BuiltInParameter.ROOM_NUMBER).Set($"{level.Num}.{i + 1}");
                        }
                        else
                        {
                            InitialNum++;
                            rooms[i].get_Parameter(BuiltInParameter.ROOM_NUMBER).Set(InitialNum.ToString());
                        }
                    }
                }
                ts.Commit();
            }
        }

        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }

    }
}
