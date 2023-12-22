using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimCity_Model.Model
{
    public class CONSTS
    {
        public static readonly int GridWidth = 40;
        public static readonly int GridHeight = 30;

        public static readonly int VisibleGridWidthSmall = 8;
        public static readonly int VisibleGridHeightSmall = 6;

        public static readonly int VisibleGridWidthNormal = 12;
        public static readonly int VisibleGridHeightNormal = 9;
               
        public static readonly int VisibleGridWidthMedium = 24;
        public static readonly int VisibleGridHeightMedium = 18;

        public static readonly int StadiumRadius = 4;
        public static readonly int PoliceRadius = 5;

        public static readonly int IncreaseSatisfactionStadium = 6;
        public static readonly int IncreaseSatisfactionPolice = 7;

        public static readonly int StadiumOperationCost = 200;
        public static readonly int PoliceOperationCost = 50;
        public static readonly int SchoolOperationCost = 100;
        public static readonly int UniversityOperationCost = 200;
        public static readonly int PowerPlantOperationCost = 200;

        public static readonly int ForestOperationCost = 15;

        public static readonly int StadiumBuiltPrice = 400;
        public static readonly int PoliceBuiltPrice = 100;
        public static readonly int SchoolBuiltPrice = 200;
        public static readonly int UniversityBuiltPrice = 400;
        public static readonly int PowerPlantBuiltPrice = 400;

        public static readonly int ForestBuiltPrice = 30;

        public static readonly int RoadBuiltPrice = 10;

        public static readonly int DevelopeZoneFirstLevel = 100;
        public static readonly int DevelopeZoneSecondLevel = 150;


        public static bool containesPosition(int i, int j)
        {
            return i < CONSTS.GridWidth &&
                   i >= 0 &&
                   j < CONSTS.GridHeight &&
                   j >= 0;
        }

        public static readonly int PopulationThreshold = 60;
    }
}
