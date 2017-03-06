// <copyright file="CityBussinessTest.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Sergio Sierra</author>
namespace UnitTest.Backend.CoreTest.Bussiness
{
    using System.Collections.Generic;
    using Core.Bussiness;
    using Core.Bussiness.Contracts;
    using Core.Bussiness.Repositories;
    using Domain.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Representa la clase <see cref="UnitTest.Backend.CoreTest.Bussiness.CityBussinessTest"/>
    /// </summary>
    [TestClass]
    public class CityBussinessTest
    {
        /// <summary>
        /// Mock para la interfaz <see cref="Core.Bussiness.Repositories.ICityRepository"/> para simular el acceso al repositorio de ciudades.
        /// </summary>
        private Mock<ICityRepository> cityRepository = null;

        /// <summary>
        /// Interfaz de tipo <see cref="Core.Bussiness.Contracts.ICityBussiness"/> para acceder al negocio de las ciudades.
        /// </summary>
        private ICityBussiness cityBussiness = null;

        /// <summary>
        /// Inicializa la información necesaria para las pruebas.
        /// </summary>
        [TestInitialize]
        public void CityBussinessTestInitialize()
        {
            this.cityRepository = new Mock<ICityRepository>();
            this.cityBussiness = new CityBussiness(cityRepository.Object);
        }

        /// <summary>
        /// Verifica si el método GetByCountry del respositorio de ciudades es llamado desde el método GetAll del negocio de ciudades.
        /// </summary>
        [TestMethod]
        public void Test_Verificar_Llamado_Del_Metodo_GetByCountry_En_Repositorio()
        {
            this.cityRepository.Setup(it => it.GetByCountry(It.IsAny<int>())).Verifiable();
            IList<City> actualCategories = this.cityBussiness.GetByCountry(0);
            this.cityRepository.Verify(it => it.GetByCountry(It.IsAny<int>()));
        }

        /// <summary>
        /// Verifica el escenario donde el método GetByCountry del repositorio de ciudadess no retorna ninguna ciudada.
        /// </summary>
        [TestMethod]
        public void Test_Consultar_Todas_Las_Ciudades_No_Retorna_Registros()
        {
            IList<City> expectedCityList = new List<City>();
            this.cityRepository.Setup(it => it.GetByCountry(It.IsAny<int>())).Returns(expectedCityList);
            IList<City> actualCityList = this.cityBussiness.GetByCountry(0);
            Assert.AreEqual(0, actualCityList.Count);
        }

        /// <summary>
        /// Verifica el escenario donde el método GetByCountry del repositorio de ciudades retorna alguna ciudad.
        /// </summary>
        [TestMethod]
        public void Test_Consultar_Todas_Las_Ciudades_Retorna_Registros()
        {
            IList<City> expectedCityList = new List<City>();
            expectedCityList.Add(new City { CityID = 1 });
            this.cityRepository.Setup(it => it.GetByCountry(It.IsAny<int>())).Returns(expectedCityList);
            IList<City> actualCityList = this.cityBussiness.GetByCountry(0);
            Assert.AreEqual(1, actualCityList.Count);
        }

        /// <summary>
        /// Verifica el escenario donde el método GetByCountry del repositorio de ciudades retorna alguna ciudad y valida que sean las esperadas.
        /// </summary>
        [TestMethod]
        public void Test_Consultar_Todas_Las_Ciudades_Validar_Que_Sean_Las_Esperadas()
        {
            IList<City> expectedCityList = new List<City>();
            expectedCityList.Add(new City { CityID = 1 });
            this.cityRepository.Setup(it => it.GetByCountry(It.IsAny<int>())).Returns(expectedCityList);
            IList<City> actualCityList = this.cityBussiness.GetByCountry(0);
            Assert.AreSame(expectedCityList, actualCityList);
        }
    }
}
