// <copyright file="SuccessStoryPostulateBussinessTest.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Sergio Sierra</author>
namespace UnitTest.Backend.CoreTest.Bussiness
{
    using System.Collections.Generic;
    using Core.Bussiness;
    using Core.Bussiness.Contracts;
    using Core.Bussiness.Repositories;
    using Domain.Entities.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// Representa la clase <see cref="UnitTest.Backend.CoreTest.Bussiness.SuccessStoryPostulateTest"/>
    /// </summary>
    [TestClass]
    public class SuccessStoryPostulateBussinessTest
    {
        /// <summary>
        /// Mock para la interfaz <see cref="Core.Bussiness.Repositories.ISuccessStoryPostulateRepository"/> para simular el acceso al repositorio de postulación de casos de éxito.
        /// </summary>
        private Mock<ISuccessStoryPostulateRepository> successStoryPostulateRepository = null;

        /// <summary>
        /// Interfaz de tipo <see cref="Core.Bussiness.Contracts.ISuccessStoryPostulateBussiness"/> para acceder al negocio de postulación de casos de éxito.
        /// </summary>
        private ISuccessStoryPostulateBussiness successStoryPostulateBussiness = null;

        /// <summary>
        /// Inicializa la información necesaria para las pruebas.
        /// </summary>
        [TestInitialize]
        public void SuccessStoryPostulateTestInitialize()
        {
            this.successStoryPostulateRepository = new Mock<ISuccessStoryPostulateRepository>();
            this.successStoryPostulateBussiness = new SuccessStoryPostulateBussiness(successStoryPostulateRepository.Object);
        }

        /// <summary>
        /// Verifica si el método GetAll del respositorio de postulación de casos de éxito es llamado desde el método GetAll del negocio de postulación de casos de éxito.
        /// </summary>
        [TestMethod]
        public void Test_Verificar_Llamado_Del_Metodo_GetAll_En_Repositorio()
        {
            this.successStoryPostulateRepository.Setup(it => it.GetAll()).Verifiable();
            IList<SuccessStoryPostulate> actualSuccessStoryPostulate = this.successStoryPostulateBussiness.GetAll();
            this.successStoryPostulateRepository.Verify(it => it.GetAll());
        }

        /// <summary>
        /// Verifica el escenario donde el método GetAll del repositorio de postulación de casos de éxito no retorna ninguna postulación.
        /// </summary>
        [TestMethod]
        public void Test_Consultar_Todas_Las_Postulaciones_De_Casos_De_Exito_No_Retorna_Registros()
        {
            IList<SuccessStoryPostulate> expectedSuccessStoryPostulateList = new List<SuccessStoryPostulate>();
            this.successStoryPostulateRepository.Setup(it => it.GetAll()).Returns(expectedSuccessStoryPostulateList);
            IList<SuccessStoryPostulate> actualSuccessStoryPostulateList = this.successStoryPostulateBussiness.GetAll();
            Assert.AreEqual(0, actualSuccessStoryPostulateList.Count);
        }

        /// <summary>
        /// Verifica el escenario donde el método GetAll del repositorio de postulación de casos de éxito retorna alguna postulación.
        /// </summary>
        [TestMethod]
        public void Test_Consultar_Todas_Las_Postulaciones_De_Casos_De_Exito_Retorna_Registros()
        {
            IList<SuccessStoryPostulate> expectedSuccessStoryPostulateList = new List<SuccessStoryPostulate>();
            expectedSuccessStoryPostulateList.Add(new SuccessStoryPostulate { Id = 1 });
            this.successStoryPostulateRepository.Setup(it => it.GetAll()).Returns(expectedSuccessStoryPostulateList);
            IList<SuccessStoryPostulate> actualSuccessStoryPostulateList = this.successStoryPostulateBussiness.GetAll();
            Assert.AreEqual(1, actualSuccessStoryPostulateList.Count);
        }

        /// <summary>
        /// Verifica el escenario donde el método GetAll del repositorio de postulación de casos de éxito retorna alguna postulación y valida que sean las esperadas.
        /// </summary>
        [TestMethod]
        public void Test_Consultar_Todas_Las_Postulaciones_De_Casos_De_Exito_Validar_Que_Sean_Las_Esperadas()
        {
            IList<SuccessStoryPostulate> expectedSuccessStoryPostulateList = new List<SuccessStoryPostulate>();
            expectedSuccessStoryPostulateList.Add(new SuccessStoryPostulate { Id = 1 });
            this.successStoryPostulateRepository.Setup(it => it.GetAll()).Returns(expectedSuccessStoryPostulateList);
            IList<SuccessStoryPostulate> actualSuccessStoryPostulateList = this.successStoryPostulateBussiness.GetAll();
            Assert.AreSame(expectedSuccessStoryPostulateList, actualSuccessStoryPostulateList);
        }

        /// <summary>
        /// Verifica si el método GetById del respositorio de postulación de casos éxitos es llamado desde el método GetAll del negocio de postulación de casos de éxito.
        /// </summary>
        [TestMethod]
        public void Test_Verificar_Llamado_Del_Metodo_GetById_En_Repositorio()
        {
            this.successStoryPostulateRepository.Setup(it => it.GetById(It.IsAny<int>(), It.IsAny<int>())).Verifiable();
            SuccessStoryPostulate actualSuccessStoryPostulate = this.successStoryPostulateBussiness.GetById(0, 0);
            this.successStoryPostulateRepository.Verify(it => it.GetById(It.IsAny<int>(), It.IsAny<int>()));
        }

        /// <summary>
        /// Verifica el escenario donde el método GetById del repositorio de postulación de casos de éxito no retorna ninguna postulación.
        /// </summary>
        [TestMethod]
        public void Test_Consultar_Una_Postulacion_De_Casos_De_Exito_No_Retorna_Registro()
        {
            SuccessStoryPostulate expectedSuccessStoryPostulate = new SuccessStoryPostulate();
            this.successStoryPostulateRepository.Setup(it => it.GetById(It.IsAny<int>(), It.IsAny<int>())).Returns(expectedSuccessStoryPostulate);
            SuccessStoryPostulate actualSuccessStoryPostulate = this.successStoryPostulateBussiness.GetById(0, 0);
            Assert.AreSame(expectedSuccessStoryPostulate, actualSuccessStoryPostulate);
        }

        /// <summary>
        /// Verifica el escenario donde el método GetById del repositorio de postulación de casos de éxito retorna alguna postulación.
        /// </summary>
        [TestMethod]
        public void Test_Consultar_Una_Postulacion_De_Casos_De_Exito_Retorna_Registro()
        {
            SuccessStoryPostulate expectedSuccessStoryPostulate = new SuccessStoryPostulate { Id = 1 };
            this.successStoryPostulateRepository.Setup(it => it.GetById(It.IsAny<int>(), It.IsAny<int>())).Returns(expectedSuccessStoryPostulate);
            SuccessStoryPostulate actualSuccessStoryPostulate = this.successStoryPostulateBussiness.GetById(0, 0);
            Assert.AreEqual(1, actualSuccessStoryPostulate.Id);
        }

        /// <summary>
        /// Verifica el escenario donde el método GetById del repositorio de postulación de casos de éxito retorna una postulación y valida que sea la esperada.
        /// </summary>
        [TestMethod]
        public void Test_Consultar_Una_Postulacion_De_Casos_De_Exito_Retorna_Registro_Valida_Que_Sea_El_Esperado()
        {
            SuccessStoryPostulate expectedSuccessStoryPostulate = new SuccessStoryPostulate { Id = 1 };
            this.successStoryPostulateRepository.Setup(it => it.GetById(It.IsAny<int>(), It.IsAny<int>())).Returns(expectedSuccessStoryPostulate);
            SuccessStoryPostulate actualSuccessStoryPostulate = this.successStoryPostulateBussiness.GetById(0, 0);
            Assert.AreSame(expectedSuccessStoryPostulate, actualSuccessStoryPostulate);
        }

        /// <summary>
        /// Verifica si el método Save del respositorio de postulación de casos éxitos es llamado desde el método Save del negocio de postulación de casos de éxito.
        /// </summary>
        [TestMethod]
        public void Test_Verificar_Llamado_Del_Metodo_Save_En_Repositorio()
        {
            this.successStoryPostulateRepository.Setup(it => it.Save(It.IsAny<SuccessStoryPostulate>())).Verifiable();
            bool actual = this.successStoryPostulateBussiness.Save(new SuccessStoryPostulate());
            this.successStoryPostulateRepository.Verify(it => it.Save(It.IsAny<SuccessStoryPostulate>()));
        }

        /// <summary>
        /// Verifica si una postulación de caso de éxito no se guardó en BD.
        /// </summary>
        [TestMethod]
        public void Test_Guardar_Postulacion_De_Un_Caso_De_Exito_No_Se_Realiza()
        {
            this.successStoryPostulateRepository.Setup(it => it.Save(It.IsAny<SuccessStoryPostulate>())).Returns(false);
            Assert.IsFalse(this.successStoryPostulateBussiness.Save(new SuccessStoryPostulate()));
        }

        /// <summary>
        /// Verifica si una postulación de caso de éxito se guardo correctamente.
        /// </summary>
        [TestMethod]
        public void Test_Guardar_Postulacion_De_Un_Caso_De_Exito_Se_Realiza_Correctamente()
        {
            this.successStoryPostulateRepository.Setup(it => it.Save(It.IsAny<SuccessStoryPostulate>())).Returns(true);
            Assert.IsTrue(this.successStoryPostulateBussiness.Save(new SuccessStoryPostulate()));
        }

        #region update
        /// <summary>
        /// Verifica si el método Save del respositorio de postulación de casos éxitos es llamado desde el método Save del negocio de postulación de casos de éxito.
        /// </summary>
        [TestMethod]
        public void Test_Verificar_Llamado_Del_Metodo_Update_En_Repositorio()
        {
            this.successStoryPostulateRepository.Setup(it => it.Update(It.IsAny<SuccessStoryPostulate>())).Verifiable();
            bool actual = this.successStoryPostulateBussiness.Update(new SuccessStoryPostulate());
            this.successStoryPostulateRepository.Verify(it => it.Update(It.IsAny<SuccessStoryPostulate>()));
        }

        /// <summary>
        /// Verifica si una postulación de caso de éxito no se guardó en BD.
        /// </summary>
        [TestMethod]
        public void Test_Update_Postulacion_De_Un_Caso_De_Exito_No_Se_Realiza()
        {
            this.successStoryPostulateRepository.Setup(it => it.Update(It.IsAny<SuccessStoryPostulate>())).Returns(false);
            Assert.IsFalse(this.successStoryPostulateBussiness.Update(new SuccessStoryPostulate()));
        }

        /// <summary>
        /// Verifica si una postulación de caso de éxito se guardo correctamente.
        /// </summary>
        [TestMethod]
        public void Test_Update_Postulacion_De_Un_Caso_De_Exito_Se_Realiza_Correctamente()
        {
            this.successStoryPostulateRepository.Setup(it => it.Update(It.IsAny<SuccessStoryPostulate>())).Returns(true);
            Assert.IsTrue(this.successStoryPostulateBussiness.Update(new SuccessStoryPostulate()));
        }
        #endregion
        
        /// <summary>
        /// Verifica si el método GetPaging del respositorio de postulación de casos de éxito es llamado desde el método GetAll del negocio de postulación de casos de éxito.
        /// </summary>
        [TestMethod]
        public void Test_Verificar_Llamado_Del_Metodo_GetPaging_En_Repositorio()
        {
            int total = 0;
            this.successStoryPostulateRepository.Setup(it => it.GetPaging(1, 1, out total, 1)).Verifiable();
            IList<SuccessStoryPostulatePaging> actualSuccessStoryPostulate = this.successStoryPostulateBussiness.GetPaging(1, 1, out total, 1);
            this.successStoryPostulateRepository.Verify(it => it.GetPaging(1, 1, out total, 1));
        }

        /// <summary>
        /// Verifica el escenario donde el método GetPaging del repositorio de postulación de casos de éxito no retorna ninguna postulación.
        /// </summary>
        [TestMethod]
        public void Test_Consultar_Todas_Las_Postulaciones_GetPaging_De_Casos_De_Exito_No_Retorna_Registros()
        {
            int total = 0;
            IList<SuccessStoryPostulatePaging> expectedSuccessStoryPostulateList = new List<SuccessStoryPostulatePaging>();
            this.successStoryPostulateRepository.Setup(it => it.GetPaging(1, 0, out total, 1)).Returns(expectedSuccessStoryPostulateList);
            IList<SuccessStoryPostulatePaging> actualSuccessStoryPostulateList = this.successStoryPostulateBussiness.GetPaging(1, 0, out total, 1);
            Assert.AreEqual(0, actualSuccessStoryPostulateList.Count);
        }

        /// <summary>
        /// Verifica el escenario donde el método GetPaging del repositorio de postulación de casos de éxito retorna alguna postulación.
        /// </summary>
        [TestMethod]
        public void Test_Consultar_Todas_Las_Postulaciones_GetPaging_De_Casos_De_Exito_Retorna_Registros()
        {
            int total = 0;
            IList<SuccessStoryPostulatePaging> expectedSuccessStoryPostulateList = new List<SuccessStoryPostulatePaging>();
            expectedSuccessStoryPostulateList.Add(new SuccessStoryPostulatePaging { Id = 1 });
            this.successStoryPostulateRepository.Setup(it => it.GetPaging(1, 1, out total, 1)).Returns(expectedSuccessStoryPostulateList);
            IList<SuccessStoryPostulatePaging> actualSuccessStoryPostulateList = this.successStoryPostulateBussiness.GetPaging(1, 1, out total, 1);
            Assert.AreEqual(1, actualSuccessStoryPostulateList.Count);
        }

        /// <summary>
        /// Verifica el escenario donde el método GetPaging del repositorio de postulación de casos de éxito retorna alguna postulación y valida que sean las esperadas.
        /// </summary>
        [TestMethod]
        public void Test_Consultar_Todas_Las_Postulaciones_GetPaging_De_Casos_De_Exito_Validar_Que_Sean_Las_Esperadas()
        {
            int total = 0;
            IList<SuccessStoryPostulatePaging> expectedSuccessStoryPostulateList = new List<SuccessStoryPostulatePaging>();
            expectedSuccessStoryPostulateList.Add(new SuccessStoryPostulatePaging { Id = 1 });
            this.successStoryPostulateRepository.Setup(it => it.GetPaging(1, 1, out total, 1)).Returns(expectedSuccessStoryPostulateList);
            IList<SuccessStoryPostulatePaging> actualSuccessStoryPostulateList = this.successStoryPostulateBussiness.GetPaging(1, 1, out total, 1);
            Assert.AreSame(expectedSuccessStoryPostulateList, actualSuccessStoryPostulateList);
        }
    }
}
