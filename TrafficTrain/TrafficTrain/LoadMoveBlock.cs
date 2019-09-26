using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Move;
using TrafficTrain.DataGrafik;

namespace TrafficTrain
{
    class SettingsSegmentBlock
    {
       public  Point StartPoint { get; set; }
       public Point EndPoint { get; set; }
       public double StartKilomentr { get; set; }
       public double EndKilomentr { get; set; }
       public double Lenght { get; set; }
    }

    class SettingBlock
    {
        public string Name { get; set; }
        public string NameMove { get; set; }
        public int  StationNumber { get; set; }
        public int  StationNumberRight { get; set; }
        private List<Point> _points = new List<Point>();
        public List<Point> Points 
        {
            get
            {
                return _points;
            }
            set
            {
                _points = value;
            }
        }
        private List<Figure> _figure = new List<Figure>();
        public List<Figure> Figure
        {
            get
            {
                return _figure;
            }
            set
            {
                _figure = value;
            }
        }
    }

    class LoadMoveBlock
    {
        public List<BlockPlotProject> CreateBlock(List<LightsMoveProject> Light, double startstrage, double endstrage, ref string nameleftblock, ref string namerightblock)
        {
            try
            {
                List<BlockPlotProject> blockplots = new List<BlockPlotProject>();
                if (startstrage < endstrage)
                    Light.Sort(SortDeck);
                else Light.Sort(SortAck);
                var kilometr = Light.GroupBy(x => x.Location).ToList();
                var direction = Light.GroupBy(x => x.Landmarks).ToList();
                //
                foreach (LightsMoveProject light in Light)
                {
                    if (light.Location == startstrage && light.View == LightsMoveProject.ViewLights.input && light.Visible == LightsMoveProject.VisiblityLights.Yes)
                    {
                        if (string.IsNullOrEmpty(nameleftblock))
                            nameleftblock = string.Format("1{0}", light.NameLight);
                    }
                    //
                    if (light.Location == endstrage && light.View == LightsMoveProject.ViewLights.input && light.Visible == LightsMoveProject.VisiblityLights.Yes)
                    {
                        if (string.IsNullOrEmpty(namerightblock))
                            namerightblock = string.Format("1{0}", light.NameLight);
                    }
                }
                //разбиваем на блок участки
                for (int i = 0; i < kilometr.Count - 1; i++)
                {
                    Viewblock blockview = Viewblock.center;
                    string nameblock = string.Empty;
                    //
                    if (i == 0 || i == kilometr.Count - 2)
                    {
                        if (i == 0)
                        {
                            nameblock = nameleftblock;
                            blockview = Viewblock.left;
                        }
                        else
                        {
                            nameblock = namerightblock;
                            blockview = Viewblock.right;
                        }
                    }
                    else
                    {
                        nameblock = (blockplots.Count + 1).ToString();
                        blockview = Viewblock.center;
                    }

                    blockplots.Add(new BlockPlotProject()
                    {
                        StartKilometr = kilometr[i].Key,
                        EndKilometr = kilometr[i + 1].Key,
                        NameBlock = nameblock, 
                        Position = blockview
                    });
                }
                //
                //
                for (int i = 0; i < direction.Count; i++)
                {
                    var group = direction[i].ToList();
                    if (direction[i].Key == LightsMoveProject.LandmarksLights.top)
                    {
                        group.Reverse();
                        //
                        for (int j = 0; j < group.Count - 1; j++)
                            group[j].NamesBlock = GetBlocks(group[j + 1].Location, group[j].Location, blockplots);
                    }
                    else
                    {
                        for (int j = 0; j < group.Count - 1; j++)
                            group[j].NamesBlock = GetBlocks(group[j].Location, group[j + 1].Location, blockplots);
                    }
                }
                //
                return blockplots;
            }
            catch { return new List<BlockPlotProject>(); }
        }

        private List<string> GetBlocks(double start, double end, List<BlockPlotProject> blocks)
        {
            List<string> answer = new List<string>();
            if (start < end)
            {
                foreach (BlockPlotProject block in blocks)
                {
                    if (block.StartKilometr >= start && block.EndKilometr <= end)
                        answer.Add(block.NameBlock);
                }
            }
            else
            {
                foreach (BlockPlotProject block in blocks)
                {
                    if (block.StartKilometr <= start && block.EndKilometr >= end)
                        answer.Add(block.NameBlock);
                }
            }
            return answer;
        }

        private int SortAck(LightsMoveProject x, LightsMoveProject y)
        {
            if (x.Location > y.Location)
                return -1;
            if (x.Location < y.Location)
                return 1;
            return 0;
        }

        private int SortDeck(LightsMoveProject x, LightsMoveProject y)
        {
            if (x.Location > y.Location)
                return 1;
            if (x.Location < y.Location)
                return -1;
            return 0;
        }
    }
}
