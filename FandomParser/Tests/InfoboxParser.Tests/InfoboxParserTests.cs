﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FandomParser.Core;
using Moq;
using Xunit;
using System.IO;
using System.Reflection;
using FandomParser.Core.Presets.Models;

namespace InfoboxParser.Tests
{
    public class InfoboxParserTests
    {
        private static readonly ICommons mockedCommons;

        private static readonly string testDataSchnapps_Distillery;
        private static readonly string testDataBakery;
        private static readonly string testDataCannery;
        private static readonly string testDataPoliceStation;
        private static readonly string testDataBrickFactory;
        private static readonly string testDataChapel;
        private static readonly string testDataHospital;
        private static readonly string testDataMarketplace;

        static InfoboxParserTests()
        {
            mockedCommons = Commons.Instance;

            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            testDataSchnapps_Distillery = File.ReadAllText(Path.Combine(basePath, "Testdata", "Schnapps_Distillery.infobox"));
            testDataBakery = File.ReadAllText(Path.Combine(basePath, "Testdata", "Bakery.infobox"));
            testDataCannery = File.ReadAllText(Path.Combine(basePath, "Testdata", "Cannery.infobox"));
            testDataPoliceStation = File.ReadAllText(Path.Combine(basePath, "Testdata", "Police_Station.infobox"));
            testDataBrickFactory = File.ReadAllText(Path.Combine(basePath, "Testdata", "Brick_Factory.infobox"));
            testDataChapel = File.ReadAllText(Path.Combine(basePath, "Testdata", "Chapel.infobox"));
            testDataHospital = File.ReadAllText(Path.Combine(basePath, "Testdata", "Hospital.infobox"));
            testDataMarketplace = File.ReadAllText(Path.Combine(basePath, "Testdata", "Marketplace.infobox"));
        }

        public static TheoryData<string, BuildingType> BuildingTypeTestData
        {
            get
            {
                return new TheoryData<string, BuildingType>
                {
                    { testDataSchnapps_Distillery, BuildingType.Production },
                    { testDataBakery, BuildingType.Production },
                    { testDataCannery, BuildingType.Production },
                    { testDataPoliceStation, BuildingType.Institution },
                    { testDataBrickFactory, BuildingType.Production },
                    { testDataChapel, BuildingType.PublicService },
                    { testDataHospital, BuildingType.Institution },
                    { testDataMarketplace, BuildingType.PublicService },
                };
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("")]
        public void GetInfobox_InputIsNullOrWhiteSpace_ShouldReturnNull(string input)
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox(input);

            // Assert
            Assert.Null(result);
        }

        #region BuildingType tests

        [Fact]
        public void GetInfobox_InputContainsNoBuildingType_ShouldReturnUnknown()
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox("dummy");

            // Assert
            Assert.Equal(BuildingType.Unknown, result[0].Type);
        }

        [Theory]
        [InlineData("|Building Type = something special")]
        [InlineData("|Building Type (OW) = something special")]
        [InlineData("|Building Type (NW) = something special")]
        public void GetInfobox_InputContainsUnknownBuildingType_ShouldReturnUnknown(string input)
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox(input);

            // Assert
            Assert.Equal(BuildingType.Unknown, result[0].Type);
        }

        [Theory]
        [InlineData("|Building Type = Production", BuildingType.Production)]
        [InlineData("|Building Type = Public Service", BuildingType.PublicService)]
        [InlineData("|Building Type = Residence", BuildingType.Residence)]
        [InlineData("|Building Type = Infrastructure", BuildingType.Infrastructure)]
        [InlineData("|Building Type = Institution", BuildingType.Institution)]
        [InlineData("|Building Type = Ornament", BuildingType.Ornament)]
        [InlineData("|Building Type = Monument", BuildingType.Monument)]
        [InlineData("|Building Type = Administration", BuildingType.Administration)]
        [InlineData("|Building Type = Harbour", BuildingType.Harbour)]
        [InlineData("|Building Type = Street", BuildingType.Street)]
        [InlineData("|Building Type = something special", BuildingType.Unknown)]
        [MemberData(nameof(BuildingTypeTestData))]
        public void GetInfobox_InputContainsBuildingType_ShouldReturnCorrectValue(string input, BuildingType expectedType)
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox(input);

            // Assert
            Assert.Equal(expectedType, result[0].Type);
        }

        [Theory]
        [InlineData("|Building Type (OW) = Production", BuildingType.Production)]
        [InlineData("|Building Type (OW) = Public Service", BuildingType.PublicService)]
        [InlineData("|Building Type (OW) = Residence", BuildingType.Residence)]
        [InlineData("|Building Type (OW) = Infrastructure", BuildingType.Infrastructure)]
        [InlineData("|Building Type (OW) = Institution", BuildingType.Institution)]
        [InlineData("|Building Type (OW) = Ornament", BuildingType.Ornament)]
        [InlineData("|Building Type (OW) = Monument", BuildingType.Monument)]
        [InlineData("|Building Type (OW) = Administration", BuildingType.Administration)]
        [InlineData("|Building Type (OW) = Harbour", BuildingType.Harbour)]
        [InlineData("|Building Type (OW) = Street", BuildingType.Street)]
        [InlineData("|Building Type (OW) = something special", BuildingType.Unknown)]
        public void GetInfobox_InputContainsBuildingTypeOldWorld_ShouldReturnCorrectValue(string input, BuildingType expectedType)
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox(input);

            // Assert
            Assert.Equal(expectedType, result[0].Type);
        }

        [Theory]
        [InlineData("|Building Type (NW) = Production", BuildingType.Production)]
        [InlineData("|Building Type (NW) = Public Service", BuildingType.PublicService)]
        [InlineData("|Building Type (NW) = Residence", BuildingType.Residence)]
        [InlineData("|Building Type (NW) = Infrastructure", BuildingType.Infrastructure)]
        [InlineData("|Building Type (NW) = Institution", BuildingType.Institution)]
        [InlineData("|Building Type (NW) = Ornament", BuildingType.Ornament)]
        [InlineData("|Building Type (NW) = Monument", BuildingType.Monument)]
        [InlineData("|Building Type (NW) = Administration", BuildingType.Administration)]
        [InlineData("|Building Type (NW) = Harbour", BuildingType.Harbour)]
        [InlineData("|Building Type (NW) = Street", BuildingType.Street)]
        [InlineData("|Building Type (NW) = something special", BuildingType.Unknown)]
        public void GetInfobox_InputContainsBuildingTypeNewWorld_ShouldReturnCorrectValue(string input, BuildingType expectedType)
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox(input);

            // Assert
            Assert.Equal(expectedType, result[0].Type);
        }

        [Theory]
        [InlineData("|Building Type     = Production", BuildingType.Production)]
        [InlineData("|Building Type =   Public Service", BuildingType.PublicService)]
        [InlineData("|Building Type (OW)     =   Ornament", BuildingType.Ornament)]
        [InlineData("|Building Type (NW) =  Harbour", BuildingType.Harbour)]
        public void GetInfobox_InputContainsBuildingTypeAndWhiteSpace_ShouldReturnCorrectValue(string input, BuildingType expectedType)
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox(input);

            // Assert
            Assert.Equal(expectedType, result[0].Type);
        }

        [Theory]
        [InlineData("|Building Type = Production}}", BuildingType.Production)]
        [InlineData("|Building Type = Public Service}}", BuildingType.PublicService)]
        [InlineData("|Building Type (OW)     =   Ornament   }}", BuildingType.Ornament)]
        [InlineData("|Building Type (NW) = Harbour}}", BuildingType.Harbour)]
        public void GetInfobox_InputContainsBuildingTypeAndTemplateEnd_ShouldReturnCorrectValue(string input, BuildingType expectedType)
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox(input);

            // Assert
            Assert.Equal(expectedType, result[0].Type);
        }

        [Theory]
        [InlineData("|Building Type = production", BuildingType.Production)]
        [InlineData("|Building Type = Public service", BuildingType.PublicService)]
        [InlineData("|Building Type (OW) = ornaMent", BuildingType.Ornament)]
        [InlineData("|Building Type (NW) = harbouR", BuildingType.Harbour)]
        public void GetInfobox_InputContainsBuildingTypeDifferentCasing_ShouldReturnCorrectValue(string input, BuildingType expectedType)
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox(input);

            // Assert
            Assert.Equal(expectedType, result[0].Type);
        }

        #endregion

        #region BuildingName tests

        [Fact]
        public void GetInfobox_InputContainsNoTitle_ShouldReturnEmpty()
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox("dummy");

            // Assert
            Assert.Equal(string.Empty, result[0].Name);
        }

        [Theory]
        [InlineData("|Title = Marketplace", "Marketplace")]
        [InlineData("|Title = Chapel", "Chapel")]
        [InlineData("|Title = Cannery", "Cannery")]
        [InlineData("|Title = Police Station", "Police Station")]
        [InlineData("|Title = Straight Path", "Straight Path")]
        [InlineData("|Title = Small Palm Tree", "Small Palm Tree")]
        [InlineData("|Title = Coffee Plantation", "Coffee Plantation")]
        [InlineData("|Title = Bomb­ín Weaver", "Bomb­ín Weaver")]
        public void GetInfobox_InputContainsTitle_ShouldReturnCorrectValue(string input, string expectedType)
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox(input);

            // Assert
            Assert.Equal(expectedType, result[0].Name);
        }

        [Theory]
        [InlineData("|Title     = Marketplace", "Marketplace")]
        [InlineData("|Title =    Chapel", "Chapel")]
        [InlineData("|Title     =   Cannery", "Cannery")]
        [InlineData("|Title  =  Police Station  ", "Police Station")]
        [InlineData("|Title =   Straight Path   ", "Straight Path")]
        [InlineData("|Title     = Small Palm Tree   ", "Small Palm Tree")]
        [InlineData("|Title     =   Coffee Plantation   ", "Coffee Plantation")]
        [InlineData("|Title = Bomb­ín Weaver", "Bomb­ín Weaver")]
        public void GetInfobox_InputContainsTitleAndWhiteSpace_ShouldReturnCorrectValue(string input, string expectedType)
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox(input);

            // Assert
            Assert.Equal(expectedType, result[0].Name);
        }

        [Theory]
        [InlineData("|Title  =   Cannery   }}  ", "Cannery")]
        [InlineData("|Title = Police Station}}  ", "Police Station")]
        [InlineData("|Title = Straight Path }}", "Straight Path")]
        [InlineData("|Title = Small Palm Tree}} ", "Small Palm Tree")]
        [InlineData("|Title = Coffee Plantation}}", "Coffee Plantation")]
        [InlineData("|Title = Bomb­ín Weaver    }}", "Bomb­ín Weaver")]
        public void GetInfobox_InputContainsTitleAndTemplateEnd_ShouldReturnCorrectValue(string input, string expectedType)
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox(input);

            // Assert
            Assert.Equal(expectedType, result[0].Name);
        }

        [Theory]
        [InlineData("|Title = Bombin Weaver", "Bomb­ín Weaver")]
        public void GetInfobox_InputContainsSpecialTitle_ShouldReturnCorrectValue(string input, string expectedType)
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox(input);

            // Assert
            Assert.Equal(expectedType, result[0].Name);
        }

        #endregion

        #region ProductionInfo tests

        [Fact]
        public void GetInfobox_InputContainsNoProductionInfo_ShouldReturnNull()
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox("dummy");

            // Assert
            Assert.Null(result[0].ProductionInfos);
        }

        [Fact]
        public void GetInfobox_InputIsTemplateForBothWorlds_ShouldReturnNull()
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox("{{Infobox Buildings Old and New World");

            // Assert
            Assert.Null(result[0].ProductionInfos);
        }

        [Fact]
        public void GetInfobox_InputContainsProductionAmount_ShouldReturnCorrectValue()
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox(testDataSchnapps_Distillery);

            // Assert
            Assert.Equal(2, result[0].ProductionInfos.EndProduct.Amount);
        }

        [Fact]
        public void GetInfobox_InputContainsProductionAmountAndWhiteSpace_ShouldReturnCorrectValue()
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox(testDataBakery);

            // Assert
            Assert.Equal(1, result[0].ProductionInfos.EndProduct.Amount);
        }

        [Fact]
        public void GetInfobox_InputContainsProductionAmountElectricity_ShouldReturnCorrectValue()
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox(testDataSchnapps_Distillery);

            // Assert
            Assert.Equal(4, result[0].ProductionInfos.EndProduct.AmountElectricity);
        }

        [Fact]
        public void GetInfobox_InputContainsProductionAmountElectricityAndWhiteSpace_ShouldReturnCorrectValue()
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox(testDataBakery);

            // Assert
            Assert.Equal(2, result[0].ProductionInfos.EndProduct.AmountElectricity);
        }

        [Fact]
        public void GetInfobox_InputContainsProductionIcon_ShouldReturnCorrectValue()
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox(testDataSchnapps_Distillery);

            // Assert
            Assert.Equal("Schnapps.png", result[0].ProductionInfos.EndProduct.Icon);
        }

        [Fact]
        public void GetInfobox_InputContainsProductionIconAndWhiteSpace_ShouldReturnCorrectValue()
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox(testDataBakery);

            // Assert
            Assert.Equal("Bread.png", result[0].ProductionInfos.EndProduct.Icon);
        }

        [Fact]
        public void GetInfobox_InputContainsOneInput_ShouldReturnOneInput()
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox(testDataSchnapps_Distillery);

            // Assert
            Assert.Single(result[0].ProductionInfos.InputProducts);
        }

        [Fact]
        public void GetInfobox_InputContainsOneInputAndWhiteSpace_ShouldReturnOneInput()
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox(testDataBakery);

            // Assert
            Assert.Single(result[0].ProductionInfos.InputProducts);
        }

        [Fact]
        public void GetInfobox_InputContainsTwoInput_ShouldReturnTwoInput()
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox(testDataCannery);

            // Assert
            Assert.Equal(2, result[0].ProductionInfos.InputProducts.Count);
        }

        #endregion

        #region specific buildings tests

        [Fact]
        public void GetInfobox_InputIsCannery_ShouldReturnCorrectResult()
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox(testDataCannery);

            // Assert
            Assert.Equal("Cannery", result[0].Name);
            Assert.Equal(BuildingType.Production, result[0].Type);
            Assert.Equal(0.667, result[0].ProductionInfos.EndProduct.Amount);
            Assert.Equal(1.333, result[0].ProductionInfos.EndProduct.AmountElectricity);
            Assert.Equal("Canned_food.png", result[0].ProductionInfos.EndProduct.Icon);
            Assert.Equal(2, result[0].ProductionInfos.InputProducts.Count);
            Assert.Equal(0.667, result[0].ProductionInfos.InputProducts[0].Amount);
            Assert.Equal(1.333, result[0].ProductionInfos.InputProducts[0].AmountElectricity);
            Assert.Equal("Goulash.png", result[0].ProductionInfos.InputProducts[0].Icon);
            Assert.Equal(0.667, result[0].ProductionInfos.InputProducts[1].Amount);
            Assert.Equal(1.333, result[0].ProductionInfos.InputProducts[1].AmountElectricity);
            Assert.Equal("Iron.png", result[0].ProductionInfos.InputProducts[1].Icon);

            Assert.Equal(2, result[0].SupplyInfos.SupplyEntries.Count);
            Assert.Equal(65, result[0].SupplyInfos.SupplyEntries[0].Amount);
            Assert.Equal(130, result[0].SupplyInfos.SupplyEntries[0].AmountElectricity);
            Assert.Equal("Artisans", result[0].SupplyInfos.SupplyEntries[0].Type);
            Assert.Equal(32.5, result[0].SupplyInfos.SupplyEntries[1].Amount);
            Assert.Equal(65, result[0].SupplyInfos.SupplyEntries[1].AmountElectricity);
            Assert.Equal("Engineers", result[0].SupplyInfos.SupplyEntries[1].Type);

            Assert.Single(result[0].UnlockInfos.UnlockConditions);
            Assert.Equal(1, result[0].UnlockInfos.UnlockConditions[0].Amount);
            Assert.Equal("Artisans", result[0].UnlockInfos.UnlockConditions[0].Type);
        }

        [Fact]
        public void GetInfobox_InputIsBakery_ShouldReturnCorrectResult()
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox(testDataBakery);

            // Assert
            Assert.Equal("Bakery", result[0].Name);
            Assert.Equal(BuildingType.Production, result[0].Type);
            Assert.Equal(1, result[0].ProductionInfos.EndProduct.Amount);
            Assert.Equal(2, result[0].ProductionInfos.EndProduct.AmountElectricity);
            Assert.Equal("Bread.png", result[0].ProductionInfos.EndProduct.Icon);
            Assert.Single(result[0].ProductionInfos.InputProducts);
            Assert.Equal(1, result[0].ProductionInfos.InputProducts[0].Amount);
            Assert.Equal(2, result[0].ProductionInfos.InputProducts[0].AmountElectricity);
            Assert.Equal("Flour.png", result[0].ProductionInfos.InputProducts[0].Icon);

            Assert.Equal(2, result[0].SupplyInfos.SupplyEntries.Count);
            Assert.Equal(55, result[0].SupplyInfos.SupplyEntries[0].Amount);
            Assert.Equal(110, result[0].SupplyInfos.SupplyEntries[0].AmountElectricity);
            Assert.Equal("Workers", result[0].SupplyInfos.SupplyEntries[0].Type);
            Assert.Equal(27.5, result[0].SupplyInfos.SupplyEntries[1].Amount);
            Assert.Equal(55, result[0].SupplyInfos.SupplyEntries[1].AmountElectricity);
            Assert.Equal("Artisans", result[0].SupplyInfos.SupplyEntries[1].Type);

            Assert.Single(result[0].UnlockInfos.UnlockConditions);
            Assert.Equal(150, result[0].UnlockInfos.UnlockConditions[0].Amount);
            Assert.Equal("Workers", result[0].UnlockInfos.UnlockConditions[0].Type);
        }

        [Fact]
        public void GetInfobox_InputIsPoliceStation_ShouldReturnCorrectResult()
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox(testDataPoliceStation);

            // Assert
            //Old World
            Assert.Equal("Police Station", result[0].Name);
            Assert.Equal(BuildingType.Institution, result[0].Type);
            Assert.NotNull(result[0].ProductionInfos);
            Assert.NotNull(result[0].ProductionInfos.EndProduct);
            Assert.Empty(result[0].ProductionInfos.InputProducts);
            Assert.NotNull(result[0].SupplyInfos);
            Assert.Empty(result[0].SupplyInfos.SupplyEntries);

            Assert.Equal(WorldRegion.OldWorld, result[0].Region);
            Assert.Single(result[0].UnlockInfos.UnlockConditions);
            Assert.Equal(500, result[0].UnlockInfos.UnlockConditions[0].Amount);
            Assert.Equal("Workers", result[0].UnlockInfos.UnlockConditions[0].Type);

            //New World
            Assert.Equal("Police Station", result[1].Name);
            Assert.Equal(BuildingType.Institution, result[1].Type);
            Assert.NotNull(result[1].ProductionInfos);
            Assert.NotNull(result[1].ProductionInfos.EndProduct);
            Assert.Empty(result[1].ProductionInfos.InputProducts);
            Assert.NotNull(result[1].SupplyInfos);
            Assert.Empty(result[1].SupplyInfos.SupplyEntries);

            Assert.Equal(WorldRegion.NewWorld, result[1].Region);
            Assert.Single(result[1].UnlockInfos.UnlockConditions);
            Assert.Equal(0, result[1].UnlockInfos.UnlockConditions[0].Amount);
            Assert.Equal("Jornaleros", result[1].UnlockInfos.UnlockConditions[0].Type);
        }

        [Fact]
        public void GetInfobox_InputIsBrickFactory_ShouldReturnCorrectResult()
        {
            // Arrange
            var parser = new InfoboxParser(mockedCommons);

            // Act
            var result = parser.GetInfobox(testDataBrickFactory);

            // Assert
            //Old World
            Assert.Equal("Brick Factory", result[0].Name);
            Assert.Equal(BuildingType.Production, result[0].Type);
            Assert.NotNull(result[0].ProductionInfos);
            Assert.NotNull(result[0].ProductionInfos.EndProduct);
            Assert.Equal(1, result[0].ProductionInfos.EndProduct.Amount);
            Assert.Equal(2, result[0].ProductionInfos.EndProduct.AmountElectricity);
            Assert.Equal("Bricks.png", result[0].ProductionInfos.EndProduct.Icon);
            Assert.Single(result[0].ProductionInfos.InputProducts);
            Assert.Equal(1, result[0].ProductionInfos.InputProducts[0].Amount);
            Assert.Equal(2, result[0].ProductionInfos.InputProducts[0].AmountElectricity);
            Assert.Equal("Clay.png", result[0].ProductionInfos.InputProducts[0].Icon);
            Assert.NotNull(result[0].SupplyInfos);
            Assert.Empty(result[0].SupplyInfos.SupplyEntries);
            Assert.Equal(WorldRegion.OldWorld, result[0].Region);
            Assert.Single(result[0].UnlockInfos.UnlockConditions);
            Assert.Equal(1, result[0].UnlockInfos.UnlockConditions[0].Amount);
            Assert.Equal("Workers", result[0].UnlockInfos.UnlockConditions[0].Type);

            //New World
            Assert.Equal("Brick Factory", result[1].Name);
            Assert.Equal(BuildingType.Production, result[1].Type);
            Assert.NotNull(result[1].ProductionInfos);
            Assert.NotNull(result[1].ProductionInfos.EndProduct);
            Assert.Equal(1, result[1].ProductionInfos.EndProduct.Amount);
            Assert.Equal(0, result[1].ProductionInfos.EndProduct.AmountElectricity);
            Assert.Equal("Bricks.png", result[1].ProductionInfos.EndProduct.Icon);
            Assert.Single(result[1].ProductionInfos.InputProducts);
            Assert.Equal(1, result[1].ProductionInfos.InputProducts[0].Amount);
            Assert.Equal(0, result[1].ProductionInfos.InputProducts[0].AmountElectricity);
            Assert.Equal("Clay.png", result[1].ProductionInfos.InputProducts[0].Icon);
            Assert.NotNull(result[1].SupplyInfos);
            Assert.Empty(result[1].SupplyInfos.SupplyEntries);
            Assert.Equal(WorldRegion.NewWorld, result[1].Region);
            Assert.Single(result[1].UnlockInfos.UnlockConditions);
            Assert.Equal(1, result[1].UnlockInfos.UnlockConditions[0].Amount);
            Assert.Equal("Obreros", result[1].UnlockInfos.UnlockConditions[0].Type);
        }

        #endregion
    }
}