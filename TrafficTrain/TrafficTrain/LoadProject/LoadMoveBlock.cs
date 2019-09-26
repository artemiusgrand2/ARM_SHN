using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using SCADA.Common.Enums;
using SCADA.Common.SaveElement;
using SCADA.Common.Strage.SaveElement;

namespace TrafficTrain
{
    class SettingsSegmentBlock
    {
        public Point StartPoint { get; set; }
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
        /// <summary>
        /// Координата начала участка
        /// </summary>
        public double LocationStart { get; set; }
        /// <summary>
        /// Координата окончания участка
        /// </summary>
        public double LocationEnd { get; set; }
        //
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
        public List<BlockPlotProject> CreateBlock(List<LightsProject> Light, double startstrage, double endstrage, ref string nameleftblock, ref string namerightblock, Dictionary<int, int> stationNotmanager)
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
                int index = 0;
                foreach (LightsProject light in Light)
                {
                    if (light.Location == startstrage && light.View == ViewLights.input && light.Visible == VisiblityLights.Yes)
                    {
                        if (string.IsNullOrEmpty(nameleftblock))
                            nameleftblock = string.Format("1{0}", light.Name);
                    }
                    //
                    if (light.Location == endstrage && light.View == ViewLights.input && light.Visible == VisiblityLights.Yes)
                    {
                        if (string.IsNullOrEmpty(namerightblock))
                            namerightblock = string.Format("1{0}", light.Name);
                    }

                    light.LocationNext = GetLocationNextLight(index, light, Light);
                    index++;
                }
                //разбиваем на блок участки
                for (int i = 0; i < kilometr.Count - 1; i++)
                {
                    ViewBlock blockview = ViewBlock.center;
                    string nameblock = string.Empty;
                    //
                    if (i == 0 || i == kilometr.Count - 2)
                    {
                        if (i == 0)
                        {
                            nameblock = nameleftblock;
                            blockview = ViewBlock.left;
                        }
                        else
                        {
                            nameblock = namerightblock;
                            blockview = ViewBlock.right;
                        }
                    }
                    else
                    {
                        nameblock = (blockplots.Count + 1).ToString();
                        blockview = ViewBlock.center;
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
                    if (direction[i].Key == LandmarksLights.top)
                    {
                        group.Reverse();
                        //
                        for (int j = 0; j < group.Count - 1; j++)
                        {
                            group[j].NamesBlock = GetBlocks(group[j].LocationNext, group[j].Location, blockplots);
                        }
                        //
                        FindSectionArriveLeave(group, stationNotmanager);
                    }
                    else
                    {
                        for (int j = 0; j < group.Count - 1; j++)
                        {
                            group[j].NamesBlock = GetBlocks(group[j].Location, group[j].LocationNext, blockplots);
                        }
                        //
                        FindSectionArriveLeave(group, stationNotmanager);
                    }
                }
                //
                return blockplots;
            }
            catch { return new List<BlockPlotProject>(); }
        }

        private double GetLocationNextLight(int index, LightsProject light, List<LightsProject> Lights)
        {
            if (light.View == ViewLights.anadromous)
            {
                if (light.Landmarks == LandmarksLights.bottom)
                {
                    for (int i = index+1; i < Lights.Count; i++)
                    {
                        if (Lights[i].Landmarks == light.Landmarks && Lights[i].Location != light.Location)
                            return Lights[i].Location;
                    }
                }
                else
                {
                    for (int i = index - 1; i >= 0; i--)
                    {
                        if (Lights[i].Landmarks == light.Landmarks && Lights[i].Location != light.Location)
                            return Lights[i].Location;
                    }
                }
               
            }
            return -1;
        }

        /// <summary>
        /// находим участки приближения и отправления
        /// </summary>
        /// <param name="light">перечень светофоров</param>
        /// <param name="stationmanager">перечень диспетчерских станций</param>
        private void FindSectionArriveLeave(List<LightsProject> light, Dictionary<int, int> stationmanager)
        {
            if (light.Count >= 2)
            {
                if (light[light.Count - 1].View == ViewLights.input)
                {
                    light[light.Count - 2].SectionArrive = true;
                    light[light.Count - 2].StationInputLight = light[light.Count - 1].Station;
                    light[light.Count - 2].NameInputLight = light[light.Count - 1].Name;
                    if (!stationmanager.ContainsKey(light[light.Count - 2].Station))
                        light[light.Count - 2].NotDispatcherStation = true;
                    foreach (StateElement imp in light[light.Count - 1].Impulses)
                    {
                        if (imp.Name == Viewmode.signal)
                        {
                            light[light.Count - 2].ImpulsInputLight = imp.Impuls;
                            break;
                        }
                    }
                }
                //
                light[0].SectionLeave = true;
            }
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

        private int SortAck(LightsProject x, LightsProject y)
        {
            if (x.Location > y.Location)
                return -1;
            if (x.Location < y.Location)
                return 1;
            return 0;
        }

        private int SortDeck(LightsProject x, LightsProject y)
        {
            if (x.Location > y.Location)
                return 1;
            if (x.Location < y.Location)
                return -1;
            return 0;
        }
    }
}
