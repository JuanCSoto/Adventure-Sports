// <copyright file="CountryBussinessTest.cs" company="Intergrupo">
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
    /// Representa la clase <see cref="UnitTest.Backend.CoreTest.Bussiness.CountryBussinessTest"/>
    /// </summary>
    [TestClass]
    public class CountryBussinessTest
    {
        /// <summary>
        /// Mock para la interfaz <see cref="Core.Bussiness.Repositories.ICountryRepository"/> para simular el acceso al repositorio de países.
        /// </summary>
        private Mock<ICountryRepository> countryRepository = null;

        /// <summary>
        /// Interfaz de tipo <see cref="Core.Bussiness.Contracts.ICountryBussiness"/> para acceder al negocio de países.
        /// </summary>
        private ICountryBussiness countryBussiness = null;

        /// <summary>
        /// Inicializa la información necesaria para las pruebas.
        /// </summary>
        [TestInitialize]
        public void CountryBussinessTestInitialize()
        {
            this.countryRepository = new Mock<ICountryRepository>();
            this.countryBussiness = new CountryBussiness(countryRepository.Object);
        }

        /// <summary>
        /// Verifica si el método GetAll del respositorio de países es llamado desde el método GetAll del negocio de países.
        /// </summary>
        [TestMethod]
        public void Test_Verificar_Llamado_Del_Metodo_GetAll_En_Repositorio()
        {
            this.countryRepository.Setup(it => it.GetAll()).Verifiable();
            IList<Country> actualCategories = this.countryBussiness.GetAll();
            this.countryRepository.Verify(it => it.GetAll());
        }

        /// <summary>
        /// Verifica el escenario donde el método GetAll del repositorio de países no retorna ningun país.
        /// </summary>
        [TestMethod]
        public void Test_Consultar_Todos_Los_Paises_No_Retorna_Registros()
        {
            IList<Country> expectedCountryList = new List<Country>();
            this.countryRepository.Setup(it => it.GetAll()).Returns(expectedCountryList);
            IList<Country> actualCategoriesList = this.countryBussiness.GetAll();
            Assert.AreEqual(0, actualCategoriesList.Count);
        }

        /// <summary>
        /// Verifica el escenario donde el método GetAll del repositorio de países retorna algun país.
        /// </summary>
        [TestMethod]
        public void Test_Consultar_Todos_Los_Paises_Retorna_Registros()
        {
            IList<Country> expectedCountryList = new List<Country>();
            expectedCountryList.Add(new Country { CountryID = 1 });
            this.countryRepository.Setup(it => it.GetAll()).Returns(expectedCountryList);
            IList<Country> actualCategoriesList = this.countryBussiness.GetAll();
            Assert.AreEqual(1, actualCategoriesList.Count);
        }

        /// <summary>
        /// Verifica el escenario donde el método GetAll del repositorio de países retorna algun país y valida que sean los esperados.
        /// </summary>
        [TestMethod]
        public void Test_Consultar_Todos_Los_Paises_Validar_Que_Sean_Los_Esperados()
        {
            IList<Country> expectedCountryList = new List<Country>();
            expectedCountryList.Add(new Country { CountryID = 1 });
            this.countryRepository.Setup(it => it.GetAll()).Returns(expectedCountryList);
            IList<Country> actualCategoriesList = this.countryBussiness.GetAll();
            Assert.AreSame(expectedCountryList, actualCategoriesList);
        }
    }
}
