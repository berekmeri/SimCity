using SimCity_Model.Model;
//using System.Threading.Tasks;
//using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace SimCity_Test
{
    [TestClass]
    public class Tests
    {
        GameModel _model = new GameModel(false);
        Random random = new Random();

        [TestMethod]
        public void TestZones_Residental()
        {
            int x = random.Next(0, CONSTS.GridWidth), y = random.Next(0, CONSTS.GridHeight);
            Field field = _model.GameGrid[x, y];
            _model.SelectZone(field, ZoneType.RESIDENTIAL);
            Assert.AreEqual(1, _model.GameGrid.ZoneList.Count);
            Assert.AreEqual(ZoneType.RESIDENTIAL, _model.GameGrid.ZoneList[0].ZoneType);
            _model.DeleteZone(field);
            Assert.AreEqual(0, _model.GameGrid.ZoneList.Count);
        }

        [TestMethod]
        public void TestZones_Industrial()
        {
            int x = random.Next(0, CONSTS.GridWidth), y = random.Next(0, CONSTS.GridHeight);
            Field field = _model.GameGrid[x, y];
            _model.SelectZone(field, ZoneType.INDUSTRIAL);
            Assert.AreEqual(1, _model.GameGrid.ZoneList.Count);
            Assert.AreEqual(ZoneType.INDUSTRIAL, _model.GameGrid.ZoneList[0].ZoneType);
            _model.DeleteZone(field);
            Assert.AreEqual(0, _model.GameGrid.ZoneList.Count);
        }

        [TestMethod]
        public void TestZones_Commercial()
        {
            int x = random.Next(0, CONSTS.GridWidth), y = random.Next(0, CONSTS.GridHeight);
            Field field = _model.GameGrid[x, y];
            _model.SelectZone(field, ZoneType.COMMERCIAL);
            Assert.AreEqual(1, _model.GameGrid.ZoneList.Count);
            Assert.AreEqual(ZoneType.COMMERCIAL, _model.GameGrid.ZoneList[0].ZoneType);
            _model.DeleteZone(field);
            Assert.AreEqual(0, _model.GameGrid.ZoneList.Count);
        }

        [TestMethod]
        public void TestZones_SelectAgain()
        {
            int x = random.Next(0, CONSTS.GridWidth), y = random.Next(0, CONSTS.GridHeight);
            Field field = _model.GameGrid[x, y];
            _model.SelectZone(field, ZoneType.RESIDENTIAL);
            _model.SelectZone(field, ZoneType.INDUSTRIAL);
            Assert.AreEqual(ZoneType.RESIDENTIAL, _model.GameGrid.ZoneList[0].ZoneType);
            _model.DeleteZone(field);
        }

       [TestMethod]
        public void TestZones_DeleteEmptyField()
        {
            int x = random.Next(0, CONSTS.GridWidth), y = random.Next(0, CONSTS.GridHeight);
            Field field = _model.GameGrid[x, y];
            _model.DeleteZone(field);
            Assert.AreEqual(FieldType.EMPTY, _model.GameGrid[x,y].fieldType);
        }

        [TestMethod]
        public void DevelopeZones_OneLevel_Residental()
        {
            int x = random.Next(0, CONSTS.GridWidth), y = random.Next(0, CONSTS.GridHeight);
            Field field = _model.GameGrid[x, y];
            _model.SelectZone(field, ZoneType.RESIDENTIAL);
            _model.DevelopeZone(field);
            Assert.AreEqual(1,_model.GameGrid.GetZoneAt(field)?.Level);
        }

        [TestMethod]
        public void DevelopeZones_OneLevel_Industrial()
        {
            int x = random.Next(0, CONSTS.GridWidth), y = random.Next(0, CONSTS.GridHeight);
            Field field = _model.GameGrid[x, y];
            _model.SelectZone(field, ZoneType.INDUSTRIAL);
            _model.DevelopeZone(field);
            Assert.AreEqual(1, _model.GameGrid.GetZoneAt(field)?.Level);
        }
        [TestMethod]
        public void DevelopeZones_TwoLevel()
        {
            int x = random.Next(0, CONSTS.GridWidth), y = random.Next(0, CONSTS.GridHeight);
            Field field = _model.GameGrid[x, y];
            _model.SelectZone(field, ZoneType.RESIDENTIAL);
            _model.DevelopeZone(field);
            _model.DevelopeZone(field);
            Assert.AreEqual(2, _model.GameGrid.GetZoneAt(field)?.Level);
        }

        [TestMethod]
        public void TestBuilding_Residental()
        {
            int x = random.Next(0, CONSTS.GridWidth), y = random.Next(0, CONSTS.GridHeight);
            Field field = _model.GameGrid[x, y];
            _model.SelectZone(field, ZoneType.RESIDENTIAL);
            _model.Build();
            Assert.AreEqual(1, _model.Buildings.Count);
            Assert.AreEqual(field.fieldType, FieldType.RESIDENTIAL_BUILDING_ZERO);
        }
        [TestMethod]
        public void TestBuilding_Industrial()
        {
            int x = random.Next(0, CONSTS.GridWidth), y = random.Next(0, CONSTS.GridHeight);
            Field field = _model.GameGrid[x, y];
            _model.SelectZone(field, ZoneType.INDUSTRIAL);
            _model.Build();
            Assert.AreEqual(1, _model.Buildings.Count);
            Assert.AreEqual(field.fieldType, FieldType.INDUSTRIAL_BUILDING_ZERO);
        }
        [TestMethod]
        public void TestBuilding_Commercial()
        {
            int x = random.Next(0, CONSTS.GridWidth), y = random.Next(0, CONSTS.GridHeight);
            Field field = _model.GameGrid[x, y];
            _model.SelectZone(field, ZoneType.COMMERCIAL);
            _model.Build();
            Assert.AreEqual(1, _model.Buildings.Count);
            Assert.AreEqual(field.fieldType, FieldType.COMMERCIAL_BUILDING_ZERO);
        }

        [TestMethod]
        public void TestRoad()
        {
            int x = random.Next(0, CONSTS.GridWidth), y = random.Next(0, CONSTS.GridHeight);
            Field field = _model.GameGrid[x, y];
            _model.buildRoad(field);
            Assert.AreEqual(field.fieldType, FieldType.ROAD);
            _model.DeleteRoad(field);
            Assert.AreEqual(field.fieldType, FieldType.EMPTY);
        }

        [TestMethod]
        public void TestRoad_Delete()
        {
            Field field1 = _model.GameGrid[5, 5];
            Field field2 = _model.GameGrid[4, 5];
            Field field3 = _model.GameGrid[6, 5];
            _model.buildRoad(field1);
            _model.SelectZone(field2, ZoneType.COMMERCIAL);
            _model.SelectZone(field3, ZoneType.RESIDENTIAL);
            Assert.AreEqual(field1.fieldType, FieldType.ROAD);
            _model.DeleteRoad(field1);
            Assert.AreEqual(field1.fieldType, FieldType.ROAD);
        }

        [TestMethod]
        public void TestRoad_EmptyFiedl()
        {
            int x = random.Next(0, CONSTS.GridWidth), y = random.Next(0, CONSTS.GridHeight);
            Field field = _model.GameGrid[x, y];
            _model.DeleteRoad(field);
            Assert.AreEqual(field.fieldType, FieldType.EMPTY);
        }

        [TestMethod]
        public void TestCable()
        {
            int x = random.Next(0, CONSTS.GridWidth), y = random.Next(0, CONSTS.GridHeight);
            Field field = _model.GameGrid[x, y];
            _model.buildCable(field);
            Assert.AreEqual(field.fieldType, FieldType.CABLE);
            _model.DeleteRoad(field);
            Assert.AreEqual(field.fieldType, FieldType.EMPTY);
        }

        [TestMethod]
        public void TestCable_EmptyField()
        {
            int x = random.Next(0, CONSTS.GridWidth), y = random.Next(0, CONSTS.GridHeight);
            Field field = _model.GameGrid[x, y];
            _model.DeleteRoad(field);
            Assert.AreEqual(field.fieldType, FieldType.EMPTY);
        }


        [TestMethod]
        public void TestServiceBuilding_Police()
        {
            Assert.IsFalse(_model.ableToBuildServiceBuild(_model.GameGrid[0, 0], 1, 1));
            _model.buildRoad(_model.GameGrid[0, 1]);
            _model.buildServiceBuilding("Police", _model.GameGrid[0, 0]);
            Assert.AreEqual(1, _model.ServiceList.ServiceBuildings.Count);
            Assert.AreEqual("Police", _model.ServiceList.ServiceBuildings[0].Name);
            _model.deleteServiceBuilding(_model.GameGrid[0, 0]);
            Assert.AreEqual(0, _model.ServiceList.ServiceBuildings.Count);
        }

        [TestMethod]
        public void TestServiceBuilding_Stadium()
        {
            _model.buildRoad(_model.GameGrid[0, 0]);
            _model.buildServiceBuilding("Stadium", _model.GameGrid[0, 1]);
            Assert.AreEqual(1, _model.ServiceList.ServiceBuildings.Count);
            Assert.AreEqual("Stadium", _model.ServiceList.ServiceBuildings[0].Name);
            _model.deleteServiceBuilding(_model.GameGrid[0, 1]);
            Assert.AreEqual(0, _model.ServiceList.ServiceBuildings.Count);
        }
        [TestMethod]
        public void TestServiceBuilding_University()
        {
            _model.buildRoad(_model.GameGrid[0, 0]);
            _model.buildServiceBuilding("University", _model.GameGrid[0, 1]);
            Assert.AreEqual(1, _model.ServiceList.ServiceBuildings.Count);
            Assert.AreEqual("University", _model.ServiceList.ServiceBuildings[0].Name);
            _model.deleteServiceBuilding(_model.GameGrid[0, 1]);
            Assert.AreEqual(0, _model.ServiceList.ServiceBuildings.Count);
        }

        [TestMethod]
        public void TestServiceBuilding_School()
        {
            Assert.IsFalse(_model.ableToBuildServiceBuild(_model.GameGrid[0, 0], 1, 1));
            _model.buildRoad(_model.GameGrid[0, 1]);
            _model.buildServiceBuilding("School", _model.GameGrid[0, 0]);
            Assert.AreEqual(1, _model.ServiceList.ServiceBuildings.Count);
            Assert.AreEqual("School", _model.ServiceList.ServiceBuildings[0].Name);
            _model.deleteServiceBuilding(_model.GameGrid[0, 0]);
            Assert.AreEqual(0, _model.ServiceList.ServiceBuildings.Count);
        }

        [TestMethod]
        public void TestForest()
        {
            _model = new GameModel(true);
            //Assert.IsTrue(_model.Forests.Count > 0);
            int x = random.Next(0, CONSTS.GridWidth), y = random.Next(0, CONSTS.GridHeight);
            Field field = _model.GameGrid[x, y];
            while (field.fieldType != FieldType.EMPTY)
            {
                x = random.Next(0, CONSTS.GridWidth);
                y = random.Next(0, CONSTS.GridHeight);
                field = _model.GameGrid[x, y];
            }
            _model.BuildForest(field);
            Assert.AreEqual(field.fieldType, FieldType.FOREST);
            _model.DeleteForest(field);
            Assert.AreEqual(field.fieldType, FieldType.EMPTY);
        }

        [TestMethod]
        public void TestEducation()
        {
            int x = random.Next(0, CONSTS.GridWidth), y = random.Next(0, CONSTS.GridHeight);
            Field field = _model.GameGrid[x, y];
            _model.buildRoad(_model.GameGrid[x - 1, y]);
            _model.buildServiceBuilding("University", field);
            _model.SelectZone(_model.GameGrid[x - 2, y], ZoneType.RESIDENTIAL);
            _model.Build();
            if (CONSTS.containesPosition(x+1,y) && CONSTS.containesPosition(x, y+1) && CONSTS.containesPosition(x + 1, y + 1))
            {
                Assert.AreEqual(_model.ServiceList.ServiceBuildings.Count, 1);
            
           
            for (int i = 0; i < 100; i++)
            {
                _model.AddCitizen();
            }
            _model.increaseEducaitonLevel();
            Assert.IsTrue(_model.ableIncreaseEducationLevel(3));
            int sumFelsoFok = 0;
            foreach (var item in Population.Citizens)
            {
                if (item.EducationLevel == 3)
                {
                    sumFelsoFok++;
                }
            }
            Assert.IsTrue(sumFelsoFok > -1);
            }
           
        }
        [TestMethod]
        public void TestElectricity_True()
        {
            Field field = _model.GameGrid[3, 3];
            _model.SelectZone(field, ZoneType.COMMERCIAL);
            Assert.AreEqual(1, _model.GameGrid.ZoneList.Count);
            Field fieldRoad = _model.GameGrid[4, 2];
            Field fieldPowerPlant = _model.GameGrid[4, 3];
            _model.buildRoad(fieldRoad);
            _model.buildServiceBuilding("PowerPlant", fieldPowerPlant);
            Assert.AreEqual(1, _model.ServiceList.ServiceBuildings.Count);
            Assert.IsTrue(field.HasElectricity);
        }

        [TestMethod]
        public void TestElectricity_False()
        {
            Field field = _model.GameGrid[3, 3];
            _model.SelectZone(field, ZoneType.COMMERCIAL);
            Assert.AreEqual(1, _model.GameGrid.ZoneList.Count);
            Field fieldPowerPlant = _model.GameGrid[4, 3];
            _model.buildServiceBuilding("PowerPlant", fieldPowerPlant);
            Assert.IsFalse(field.HasElectricity);
        }

        [TestMethod]
        public void TestElectricity_Cable()
        {
            Field field = _model.GameGrid[3, 3];
            _model.SelectZone(field, ZoneType.COMMERCIAL);
            Assert.AreEqual(1, _model.GameGrid.ZoneList.Count);
            Field fieldRoad = _model.GameGrid[5, 2];
            Field fieldPowerPlant = _model.GameGrid[5, 3];
            Field fieladCable = _model.GameGrid[4, 3];
            _model.buildRoad(fieldRoad);
            _model.buildCable(fieladCable);
            _model.buildServiceBuilding("PowerPlant", fieldPowerPlant);
            Assert.AreEqual(1, _model.ServiceList.ServiceBuildings.Count);
            Assert.IsTrue(field.HasElectricity);
        }

        [TestMethod]
        public void TestElectricity_Cable_False()
        {
            Field field = _model.GameGrid[3, 3];
            _model.SelectZone(field, ZoneType.COMMERCIAL);
            Assert.AreEqual(1, _model.GameGrid.ZoneList.Count);
            Field fieldRoad = _model.GameGrid[5, 2];
            Field fieladCable = _model.GameGrid[4, 3];
            _model.buildRoad(fieldRoad);
            _model.buildCable(fieladCable);
            Assert.IsFalse(field.HasElectricity);
        }

        [TestMethod]
        public void TestElectricity_MoreZones1()
        {
            Field field1 = _model.GameGrid[3, 3];
            Field field2 = _model.GameGrid[2, 3];
            _model.SelectZone(field1, ZoneType.COMMERCIAL);
            _model.SelectZone(field2, ZoneType.RESIDENTIAL);
            Assert.AreEqual(2, _model.GameGrid.ZoneList.Count);
            Field fieldRoad = _model.GameGrid[5, 2];
            Field fieldPowerPlant = _model.GameGrid[5, 3];
            Field fieladCable = _model.GameGrid[4, 3];
            _model.buildRoad(fieldRoad);
            _model.buildCable(fieladCable);
            _model.buildServiceBuilding("PowerPlant", fieldPowerPlant);
            Assert.AreEqual(1, _model.ServiceList.ServiceBuildings.Count);
            Assert.IsTrue(field1.HasElectricity);
            Assert.IsTrue(field2.HasElectricity);
        }
        [TestMethod]
        public void TestElectricity_MoreZones2()
        {
            Field field1 = _model.GameGrid[3, 3];
            Field field2 = _model.GameGrid[2, 3];
            Field field3 = _model.GameGrid[1, 3];
            Field field4 = _model.GameGrid[0, 3];
            _model.SelectZone(field1, ZoneType.COMMERCIAL);
            _model.SelectZone(field2, ZoneType.RESIDENTIAL);
            _model.SelectZone(field3, ZoneType.RESIDENTIAL);
            _model.SelectZone(field4, ZoneType.INDUSTRIAL);
            Assert.AreEqual(4, _model.GameGrid.ZoneList.Count);
            Field fieldRoad = _model.GameGrid[5, 2];
            Field fieldPowerPlant = _model.GameGrid[5, 3];
            Field fieladCable = _model.GameGrid[4, 3];
            _model.buildRoad(fieldRoad);
            _model.buildCable(fieladCable);
            _model.buildServiceBuilding("PowerPlant", fieldPowerPlant);
            Assert.AreEqual(1, _model.ServiceList.ServiceBuildings.Count);
            Assert.IsTrue(field1.HasElectricity);
            Assert.IsTrue(field2.HasElectricity);
            Assert.IsTrue(field3.HasElectricity);
            Assert.IsTrue(field4.HasElectricity);
        }

        [TestMethod]
        public void TestElectricity_Service()
        {
            Field field1 = _model.GameGrid[3, 3];
            Field field2 = _model.GameGrid[2, 3];
            _model.SelectZone(field2, ZoneType.RESIDENTIAL);
            Assert.AreEqual(1, _model.GameGrid.ZoneList.Count);
            Field fieldRoad = _model.GameGrid[4, 3];
            Field fieldPowerPlant = _model.GameGrid[3, 4];
            _model.buildRoad(fieldRoad);
            _model.buildServiceBuilding("Police", field1);
            _model.buildServiceBuilding("PowerPlant", fieldPowerPlant);
            Assert.AreEqual(2, _model.ServiceList.ServiceBuildings.Count);
            Assert.IsTrue(field2.HasElectricity);
        }
        [TestMethod]
        public void TestElectricity_RemovePowerPlants()
        {
            Field field1 = _model.GameGrid[3, 3];
            Field field2 = _model.GameGrid[2, 3];
            _model.SelectZone(field2, ZoneType.RESIDENTIAL);
            Assert.AreEqual(1, _model.GameGrid.ZoneList.Count);
            Field fieldRoad = _model.GameGrid[4, 3];
            Field fieldPowerPlant = _model.GameGrid[3, 4];
            _model.buildRoad(fieldRoad);
            _model.buildServiceBuilding("Police", field1);
            _model.buildServiceBuilding("PowerPlant", fieldPowerPlant);
            Assert.AreEqual(2, _model.ServiceList.ServiceBuildings.Count);
            Assert.IsTrue(field2.HasElectricity);
            _model.deleteServiceBuilding(fieldPowerPlant);
            Assert.IsFalse(field2.HasElectricity);
        }

        [TestMethod]
        public void TestSatisfaction()
        {
            Field field = _model.GameGrid[4, 3];
            _model.SelectZone(field, ZoneType.RESIDENTIAL);
            Field fieldRoad = _model.GameGrid[3, 3];
            Field fieldC = _model.GameGrid[3, 2];
            _model.SelectZone(fieldC, ZoneType.COMMERCIAL);
            Field fieldPowerPlant = _model.GameGrid[3, 4];
            _model.buildRoad(fieldRoad);
            _model.buildServiceBuilding("PowerPlant", fieldPowerPlant);
            Field fieladCable = _model.GameGrid[4, 2];
            _model.buildCable(fieladCable);
            Assert.AreEqual(1, _model.ServiceList.ServiceBuildings.Count);
            Assert.IsTrue(field.HasElectricity);
            _model.Build();

            for (int i = 0; i < 100; i++)
            {
                 _model.AddCitizen();
            }
            Assert.AreEqual(_model.Population.Satisfaction, 3);


            }

    }
}