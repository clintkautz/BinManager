using System;
using BinManager.Models;
using BinManager.Utilities.Enums;
using BinManager.ViewModels;

namespace BinManager.Utilities
{
    public static class VolumeMath
    {
        public static double CalculateVolume(BinViewModel binViewModel)
        {
            double volume = 0;
            double d;
            //calculate
            //switch (binViewModel.BinType)
            //{
            //    case 0:                    
            //        double CribLength = double.TryParse(binViewModel.CribLength, out d)? d : 0;
            //        double CribWidth = double.TryParse(binViewModel.CribWidth, out d) ? d : 0;
            //        double GrainHeight = double.TryParse(binViewModel.GrainHeight, out d) ? d : 0;

            //        volume = FlatBinGrainVolume(CribLength, CribWidth, GrainHeight);                    
            //        break;                
            //    case 1:
            //        double RectangleLength = double.TryParse(binViewModel.RectangleLength, out d) ? d : 0;
            //        double RectangleWidth = double.TryParse(binViewModel.RectangleWidth, out d) ? d : 0;
            //        double HopperHeight = double.TryParse(binViewModel.HopperHeight, out d) ? d : 0;
            //        double ChuteLength = double.TryParse(binViewModel.ChuteLength, out d) ? d : 0;
            //        GrainHeight = double.TryParse(binViewModel.GrainHeight, out d) ? d : 0;

            //        volume = GravityBinGrainVolume(RectangleLength, RectangleWidth, HopperHeight, ChuteLength, GrainHeight);
            //        break;
            //    case 2:
            //        double NumberOfSides = double.TryParse(binViewModel.NumberOfSides, out d) ? d : 0;
            //        double SideWidth = double.TryParse(binViewModel.SideWidth, out d) ? d : 0;
            //        GrainHeight = double.TryParse(binViewModel.GrainHeight, out d) ? d : 0;
            //        double GrainConeHeight = double.TryParse(binViewModel.GrainConeHeight, out d) ? d : 0;

            //        volume = PolygonBinGrainVolume(NumberOfSides, SideWidth,
            //                                       GrainHeight, GrainConeHeight);                    
            //        break;
            //    case 3:
            //        double Radius = double.TryParse(binViewModel.SideWidth, out d) ? d : 0;
            //        double WallHeight = double.TryParse(binViewModel.WallHeight, out d) ? d : 0;
            //        double RoofHeight = double.TryParse(binViewModel.RoofHeight, out d) ? d : 0;
            //        HopperHeight = double.TryParse(binViewModel.HopperHeight, out d) ? d : 0;
            //        GrainHeight = double.TryParse(binViewModel.GrainHeight, out d) ? d : 0;
            //        GrainConeHeight = double.TryParse(binViewModel.GrainConeHeight, out d) ? d : 0;

            //        volume = RoundBinGrainVolume(Radius, WallHeight, RoofHeight,
            //                                     HopperHeight, GrainHeight, GrainConeHeight);                    
            //        break;
            //    default:
            //        volume = 0;
            //        break;
            //}

            return volume;
        }
        ////////// 2-D Volumes //////////

        public static double RegularPolygonArea(double numberOfSides, double sideLength)
        {
            return (sideLength * sideLength * numberOfSides)
                 / (4 * Math.Tan(Math.PI / numberOfSides));
            //apothem = (sideLength) / (2*tan(180/numberOfSides))
            //area = (numberOfSides*apothem*sideLength) / (2)
            //area = (sideLength*sideLength*numberOfSides) / (4*tan(180/numberOfSides))
        }

        public static double RectangleArea(double length, double width)
        {
            return length * width;
        }

        public static double TriangleArea(double triangleBase, double triangleHeight)
        {
            return (triangleBase * triangleHeight) / 2;
        }

        public static double CircleArea(double radius)
        {
            return Math.PI * radius * radius;
        }



        ////////// 3-D Volumes //////////
        ///// Prisms /////
        public static double RegularPolygonPrismVolume(double numberOfSides, double sideLength, double prismHeight)
        {
            return RegularPolygonArea(numberOfSides, sideLength) * prismHeight;
        }

        public static double RectangularPrismVolume(double length, double width, double prismHeight)
        {
            return RectangleArea(length, width) * prismHeight;
        }

        public static double TrianglePrismVolume(double triangleBase, double triangleHeight, double depth)
        {
            return TriangleArea(triangleBase, triangleHeight) * depth;
        }

        public static double CylinderVolume(double radius, double height)
        {
            return CircleArea(radius) * height;
        }

        ///// Cones /////
        public static double RegularPyramidVolume(double numberOfSides, double sideLength, double pyramidHeight)
        {
            return (RegularPolygonArea(numberOfSides, sideLength) * pyramidHeight) / 3;
        }

        public static double GrainReguarPolygonPyramidVolume(double numberOfSides, double sideLength, double pyramidHeight)
        {
            return (RegularPolygonArea(numberOfSides, sideLength) * pyramidHeight) * 0.2618;
        }

        public static double RectanglePyramidVolume(double length, double width, double pyramidHeight)
        {
            return (RectangleArea(length, width) * pyramidHeight) / 3;
        }

        public static double ConeVolume(double radius, double coneHeight)
        {
            return (CircleArea(radius) * coneHeight) / 3;
        }

        public static double GrainConeVolume(double radius, double coneHeight)
        {
            return (CircleArea(radius) * coneHeight) * 0.2618;
        }



        ////////// Bin Volume Helpers //////////
        /// finds the radius of a cross section of a cone at a specific height
        public static double RoofRadius(double radius, double totalConeHeight, double heightFromBase)
        {
            return radius * ((totalConeHeight - heightFromBase) / totalConeHeight);
        }

        public static double HopperRadius(double radius, double totalConeHeight, double heightFromApex)
        {
            return radius * (heightFromApex / totalConeHeight);
        }



        ////////// Bin Volumes //////////
        public static double RoundBinVolume(double radius, double wallHeight, double roofHeight, double hopperHeight)
        {
            return ConeVolume(radius, roofHeight)
                + CylinderVolume(radius, wallHeight)
                + ConeVolume(radius, hopperHeight);
        }
        public static double RoundBinGrainVolume(double radius, double wallHeight, double roofHeight, double hopperHeight, double totalGrainHeight, double coneHeight)
        {
            
            if (totalGrainHeight > hopperHeight + wallHeight + roofHeight)
            {
                return -2;
            }
            //if normal calculations

            if (hopperHeight >= 0)
            {
                if(totalGrainHeight >= hopperHeight)
                {
                    if(totalGrainHeight <= hopperHeight + wallHeight)

                    {
                        //wall
                        return GrainConeVolume(radius, coneHeight)
                            + CylinderVolume(radius, totalGrainHeight - hopperHeight)
                            + ConeVolume(radius, hopperHeight);
                    }
                    //roof
                    double roofRadius = RoofRadius(radius, roofHeight, totalGrainHeight - wallHeight - hopperHeight);
                    return RoundBinVolume(radius, wallHeight, roofHeight, hopperHeight)
                        - ConeVolume(roofRadius, roofHeight - (totalGrainHeight - wallHeight - hopperHeight))
                        + GrainConeVolume(roofRadius, coneHeight);
                }
                //hopper
                double hopperRadius = HopperRadius(radius, hopperHeight, totalGrainHeight);
                return ConeVolume(hopperRadius, totalGrainHeight)
                    + GrainConeVolume(hopperRadius, coneHeight);
            }
            else //hopper points up into bin, grain height measured as if no hopper
            {
                if (totalGrainHeight >= hopperHeight)
                {
                    //only recurses once
                    return RoundBinGrainVolume(radius, wallHeight, roofHeight, 0, totalGrainHeight, coneHeight)
                        - ConeVolume(radius, hopperHeight);
                }
                //grain height < hopper height
                double coneDeductible = ConeVolume(radius, hopperHeight)
                    - ConeVolume(RoofRadius(radius, hopperHeight, totalGrainHeight), (hopperHeight - totalGrainHeight));

                return RoundBinGrainVolume(radius, wallHeight, roofHeight, 0, totalGrainHeight, coneHeight)
                    - coneDeductible;
            }
        }

        public static double GravityBinVolume(double length, double width, double rectangleHeight, double hopperHeight, double chuteLength)
        {
            return RectangularPrismVolume(length, width, rectangleHeight)
                + RectanglePyramidVolume((length - chuteLength), width, hopperHeight)
                + TrianglePrismVolume(width, hopperHeight, chuteLength);
        }
        public static double GravityBinGrainVolume(double length, double width, double hopperHeight, double chuteLength, double grainHeight)
        {
            if(grainHeight >= hopperHeight)
            {
                return RectangularPrismVolume(length, width, (grainHeight-hopperHeight))
                    + RectanglePyramidVolume((length - chuteLength), width, hopperHeight)
                    + TrianglePrismVolume(width, hopperHeight, chuteLength);
            }
            double heightRatio = grainHeight / hopperHeight;
            double smallPyramidLength = (length - chuteLength) * heightRatio;
            double smallWidth = width * heightRatio;
            return RectanglePyramidVolume(smallPyramidLength, smallWidth, grainHeight)
                + TrianglePrismVolume(smallWidth, grainHeight, chuteLength);
        }

        public static double PolygonBinVolume(double numberOfSides, double sideLength, double wallHeight)
        {
            return RegularPolygonPrismVolume(numberOfSides, sideLength, wallHeight);
        }
        public static double PolygonBinGrainVolume(double numberOfSides, double sideLength, double coneHeight, double grainHeight)
        {
            return GrainReguarPolygonPyramidVolume(numberOfSides, sideLength, coneHeight)
                + RegularPolygonPrismVolume(numberOfSides, sideLength, grainHeight);
        }

        public static double FlatBinArea(double length, double width)
        {
            return RectangleArea(length, width);
        }
        public static double FlatBinGrainVolume(double length, double width, double grainDepth)
        {
            return RectangleArea(length, width) * grainDepth;
        }
    }
}

